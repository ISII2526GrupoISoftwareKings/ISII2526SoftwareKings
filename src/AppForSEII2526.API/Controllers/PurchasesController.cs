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
                if (item.AmountBought <= 0)
                {
                    _logger.LogError($"Invalid amount bought ({item.AmountBought}) for item {item.ItemId} in purchase {id}.");
                    return BadRequest("Invalid purchase: item quantity must be greater than zero.");
                }
            }

            return Ok(purchase);
        }
    }
}
