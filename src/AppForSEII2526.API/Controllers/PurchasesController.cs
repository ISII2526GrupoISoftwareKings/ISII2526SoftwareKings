using AppForSEII2526.API.DTOs.PurchaseDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PurchasesController> _logger;

        public PurchasesController(ApplicationDbContext context, ILogger<PurchasesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseForDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPurchase(int id)
        {
            var purchase = await _context.Purchases
                .Where(p => p.Id == id)
                .Include(p => p.PurchaseItems)
                    .ThenInclude(pi => pi.Item)
                        .ThenInclude(i => i.Brand)
                .Include(p => p.PaymentMethod)
                    .ThenInclude(pm => pm.User)
                .Select(p => new PurchaseForDetailDTO(
                    p.Id,
                    p.TotalPrice,
                    p.City,
                    p.Country,
                    p.Street,
                    p.Description,
                    p.Date,
                    p.PaymentMethod,
                    p.PurchaseItems.Select(pi => new PurchaseItemDTO(
                        pi.ItemId,
                        pi.Item.Name,
                        pi.Item.Brand.Name,
                        pi.AmountBought,
                        pi.Price
                    )).ToList()
                ))
                .FirstOrDefaultAsync();

            if (purchase == null)
            {
                _logger.LogError($"Purchase with id {id} not found.");
                return NotFound();
            }

            if (purchase.PaymentMethod == null
                || purchase.PaymentMethod.User == null
                || string.IsNullOrEmpty(purchase.PaymentMethod.User.Name))
            {
                _logger.LogWarning($"Purchase {id} has missing payment details.");
                return BadRequest("Missing mandatory payment information.");
            }

            foreach (var item in purchase.PurchaseItems)
            {
                var dbItem = await _context.Items.FindAsync(item.ItemId);
                if (dbItem == null)
                {
                    _logger.LogWarning($"Item {item.ItemId} not found in inventory for Purchase {id}.");
                    return BadRequest($"Item {item.ItemId} not found in inventory.");
                }

                if (item.AmountBought > dbItem.QuantityAvailableForPurchase)
                {
                    _logger.LogWarning($"Purchase {id}: requested quantity ({item.AmountBought}) exceeds available ({dbItem.QuantityAvailableForPurchase}) for item {item.ItemId}.");
                    return BadRequest($"Requested quantity for item '{dbItem.Name}' exceeds available stock.");
                }
            }

            return Ok(purchase);
        }
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(PurchaseForDetailDTO), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreatePurchase(PurchaseForCreateDTO purchaseForCreate)
        {
            //any validation defined in PurchaseForCreateDTO is checked before running the method so they don't have to be checked again
            if (purchaseForCreate.PaymentMethod == null)
                ModelState.AddModelError("PaymentMethod", "Error! You must select a payment method");

            if (string.IsNullOrEmpty(purchaseForCreate.City) || string.IsNullOrEmpty(purchaseForCreate.Country) || string.IsNullOrEmpty(purchaseForCreate.Street))
                ModelState.AddModelError("Address", "Error! City, Country and Street are mandatory");

            if (purchaseForCreate.PurchaseItems.Count == 0)
                ModelState.AddModelError("PurchaseItems", "Error! You must include at least one item to purchase");

            //we must check that all the items to be purchased exist in the database
            var itemIds = purchaseForCreate.PurchaseItems.Select(pi => pi.ItemId).ToList<int>();

            var items = await _context.Items
                .Where(i => itemIds.Contains(i.Id))
                .ToListAsync();

            //we must provide purchase with the info to be saved in the database
            Purchase purchase = new Purchase(
                purchaseForCreate.City,
                purchaseForCreate.Country,
                DateTime.Now,
                purchaseForCreate.Street,
                purchaseForCreate.TotalPrice,
                purchaseForCreate.PaymentMethod,
                purchaseForCreate.Description
            );

            foreach (var pi in purchaseForCreate.PurchaseItems)
            {
                var item = items.FirstOrDefault(i => i.Id == pi.ItemId);

                //we must check that there is enough quantity to be purchased in the database
                if (item == null)
                {
                    ModelState.AddModelError("PurchaseItems", $"Error! Item with id '{pi.ItemId}' does not exist");
                }
                else if (pi.AmountBought > item.QuantityAvailableForPurchase)
                {
                    ModelState.AddModelError("PurchaseItems", $"Error! Requested quantity for item '{item.Name}' exceeds available stock");
                }
                else
                {
                    // purchase does not exist in the database yet, so we must relate purchaseitem to the object purchase
                    purchase.PurchaseItems.Add(new PurchaseItem
                    {
                        ItemId = item.Id,
                        Item = item,
                        AmountBought = pi.AmountBought,
                        Price = pi.Price
                    });
                    pi.Price = item.PurchasePrice;
                }
            }

            //if there is any problem because of the available quantity of items or because any item does not exist
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(purchase);

            try
            {
                //we store in the database both purchase and its purchaseitems
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(DateTime.Now + ":" + ex.Message);
                return Conflict("Error: " + ex.Message);
            }

            //prepare the DTO to return to the user
            var purchaseDetail = new PurchaseForDetailDTO(
                purchase.Id,
                purchase.TotalPrice,
                purchase.City,
                purchase.Country,
                purchase.Street,
                purchase.Description,
                purchase.Date,
                purchase.PaymentMethod,
                purchaseForCreate.PurchaseItems
            );

            return CreatedAtAction("GetPurchase", new { id = purchase.Id }, purchaseDetail);
        }
    }
}
