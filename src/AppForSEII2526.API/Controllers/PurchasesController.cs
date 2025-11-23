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
            // Practica SSDD: uso del logger para registrar la inicialización del servicio:
            _logger.LogInformation("PurchasesController initialized");
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseForDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPurchase(int id)
        {
            _logger.LogInformation("Received request to get purchase. id={PurchaseId}", id);

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
                    new PaymentMethodDTO(p.PaymentMethod.Id),
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

            var paymentMethodEntity = await _context.PaymentMethods
                .Include(pm => pm.User)
                .FirstOrDefaultAsync(pm => pm.Id == purchase.PaymentMethod.Id);

            if (paymentMethodEntity == null
                || paymentMethodEntity.User == null
                || string.IsNullOrEmpty(paymentMethodEntity.User.Name))
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
                if (item.AmountBought <= 0)
                {
                    _logger.LogError($"Invalid amount bought ({item.AmountBought}) for item {item.ItemId} in purchase {id}.");
                    return BadRequest("Invalid purchase: item quantity must be greater than zero.");
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
            _logger.LogInformation("Received request to create purchase. TotalPrice={TotalPrice}, ItemsCount={ItemsCount}", purchaseForCreate.TotalPrice, purchaseForCreate.PurchaseItems?.Count ?? 0);

            //any validation defined in PurchaseForCreateDTO is checked before running the method so they don't have to be checked again
            if (purchaseForCreate.PurchaseItems == null || purchaseForCreate.PurchaseItems.Count == 0)
            {
                ModelState.AddModelError("PurchaseItems", "Error! You must include at least one item to purchase");
            }

            if (string.IsNullOrEmpty(purchaseForCreate.City) || string.IsNullOrEmpty(purchaseForCreate.Country) || string.IsNullOrEmpty(purchaseForCreate.Street))
                ModelState.AddModelError("Address", "Error! City, Country and Street are mandatory");

            // Look up the PaymentMethod entity from the database
            PaymentMethod? paymentMethodEntity = null;
            if (purchaseForCreate.PaymentMethod != null)
            {
                paymentMethodEntity = await _context.PaymentMethods
                    .Include(pm => pm.User)
                    .FirstOrDefaultAsync(pm => pm.Id == purchaseForCreate.PaymentMethod.Id);

                if (paymentMethodEntity == null)
                {
                    ModelState.AddModelError("PaymentMethod", "Error! Payment method with the provided ID does not exist");
                }
            }
            else
            {
                ModelState.AddModelError("PaymentMethod", "Error! You must select a payment method");
            }

            //we must check that all the items to be purchased exist in the database
            var itemIds = purchaseForCreate.PurchaseItems?.Select(pi => pi.ItemId).ToList() ?? new List<int>();

            var items = await _context.Items
                .Where(i => itemIds.Contains(i.Id))
                .ToListAsync();

            // Validate each item exists and has enough quantity
            if (purchaseForCreate.PurchaseItems != null)
            {
                foreach (var pi in purchaseForCreate.PurchaseItems)
                {
                    var item = items.FirstOrDefault(i => i.Id == pi.ItemId);

                    if (item == null)
                    {
                        ModelState.AddModelError("PurchaseItems", $"Error! Item with id '{pi.ItemId}' does not exist");
                    }
                    else if (pi.AmountBought > item.QuantityAvailableForPurchase)
                    {
                        ModelState.AddModelError("PurchaseItems", $"Error! Requested quantity for item '{item.Name}' exceeds available stock");
                    }
                    else if (pi.AmountBought <= 0)
                    {
                        ModelState.AddModelError("PurchaseItems", $"Error! Invalid amount bought for item '{item.Name}'. Quantity must be greater than zero.");
                    }
                    else
                    {
                        // Update the price from the database
                        pi.Price = item.PurchasePrice;
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Calculate total price from items
            decimal totalPrice = purchaseForCreate.PurchaseItems?.Sum(pi => pi.Price * pi.AmountBought) ?? 0;

            // Create Purchase using object initializer syntax
            var purchase = new Purchase
            {
                City = purchaseForCreate.City,
                Country = purchaseForCreate.Country,
                Street = purchaseForCreate.Street,
                Description = purchaseForCreate.Description,
                Date = DateTime.Today,
                TotalPrice = totalPrice,
                PaymentMethod = paymentMethodEntity!,
                PurchaseItems = purchaseForCreate.PurchaseItems!.Select(pi =>
                {
                    var item = items.First(i => i.Id == pi.ItemId);
                    return new PurchaseItem
                    {
                        ItemId = item.Id,
                        Item = item,
                        AmountBought = pi.AmountBought,
                        Price = pi.Price
                    };
                }).ToList()
            };

            _context.Purchases.Add(purchase);

            try
            {
                //we store in the database both purchase and its purchaseitems
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(DateTime.Today + ":" + ex.Message);
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
                new PaymentMethodDTO(purchase.PaymentMethod.Id),
                purchaseForCreate.PurchaseItems
            );

            return CreatedAtAction("GetPurchase", new { id = purchase.Id }, purchaseDetail);
        }
    }
}
