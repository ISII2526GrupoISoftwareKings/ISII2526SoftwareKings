using AppForSEII2526.API.DTOs.ItemDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<ItemsController> _logger;

        public ItemsController(ApplicationDbContext context, ILogger<ItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /*
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)
        {
            if (op2 == 0)
            {
                string error = "Op2 cannot be 0 to compute the division";
                _logger.LogError(error);
                return BadRequest(error);
            }
            decimal result = op1 / op2;
            return Ok(result);
        }
        */
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ItemForPurchasingDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelError), (int)HttpStatusCode.BadRequest)]

        public async Task<ActionResult> GetItemsForPurchasing(string? itemName, string? brandName)
        {
            IList<ItemForPurchasingDTO> itemsDTOS = await _context.Items
                .Include(item => item.Brand)
                .Where(item =>
                    ((itemName == null || item.Name.Contains(itemName)) &&
                     (brandName == null || item.Brand.Name.Contains(brandName))) &&
                     item.QuantityAvailableForPurchase > 0
                )

                .OrderBy(item => item.Name)
                .Select(item => new ItemForPurchasingDTO(item.Id, item.Name, item.Brand.Name, item.Description, item.PurchasePrice, item.QuantityAvailableForPurchase))
                .ToListAsync();

            if (itemsDTOS == null || !itemsDTOS.Any())
            {
                _logger.LogWarning($"{DateTime.Now} Warning: No items available for purchasing.");
                return BadRequest("There are no items available for purchase.");
            }


            return Ok(itemsDTOS);
        }
    }
}
