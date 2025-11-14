using AppForSEII2526.API.DTOs.RestockDTOs;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestocksController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<RestocksController> _logger;

        public RestocksController(ApplicationDbContext context, ILogger<RestocksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(RestockDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateRestock(RestockForCreateDTO restockForCreate)
        {
            if (restockForCreate == null)
                return BadRequest("Error! Invalid payload");

            if (restockForCreate.RestockItems == null || restockForCreate.RestockItems.Count == 0)
                ModelState.AddModelError("RestockItems", "Error! You must include at least one item to restock");

            if (string.IsNullOrWhiteSpace(restockForCreate.Title))
                ModelState.AddModelError("Title", "Error! Title is mandatory");

            if (string.IsNullOrWhiteSpace(restockForCreate.DeliveryAddress))
                ModelState.AddModelError("DeliveryAddress", "Error! Delivery address is mandatory");

            if (string.IsNullOrWhiteSpace(restockForCreate.ApplicationUserName))
                ModelState.AddModelError("ApplicationUserName", "Error! UserName is mandatory");

            if (restockForCreate.ExpectedDate < DateTime.Today)
                ModelState.AddModelError("ExpectedDate", "Error! Expected date cannot be in the past");

            if (restockForCreate.RestockItems != null)
            {
                for (int i = 0; i < restockForCreate.RestockItems.Count; i++)
                {
                    var it = restockForCreate.RestockItems[i];
                    if (it.QuantityForRestock <= 0)
                        ModelState.AddModelError($"RestockItems[{i}].QuantityForRestock", "Error! Quantity to buy must be greater than zero");

                    if (it.ItemId <= 0)
                        ModelState.AddModelError($"RestockItems[{i}].ItemId", "Error! Invalid item identifier");
                }
            }

            ApplicationUser? user = null;
            if (ModelState.ErrorCount == 0)
            {
                user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == restockForCreate.ApplicationUserName);
                if (user == null)
                    ModelState.AddModelError("ApplicationUserName", "Error! UserName is not registered");
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var itemIds = restockForCreate.RestockItems.Select(ri => ri.ItemId).Distinct().ToList();

            var itemsFromDb = _context.Items
                .Include(i => i.RestockItems)
                    .ThenInclude(ri => ri.Restock)
                .Where(i => itemIds.Contains(i.Id))
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    i.Brand,
                    i.RestockPrice
                })
                .ToList();

            var restock = new Restock
            {
                Title = restockForCreate.Title,
                DeliveryAddress = restockForCreate.DeliveryAddress,
                Description = restockForCreate.Description,
                ExpectedDate = restockForCreate.ExpectedDate,
                RestockDate = DateTime.Now,
                RestockItems = new List<RestockItem>(),
                ApplicationUser = user!
            };

            foreach (var dtoItem in restockForCreate.RestockItems)
            {
                var dbItem = itemsFromDb.FirstOrDefault(x => x.Id == dtoItem.ItemId);
                if (dbItem == null)
                {
                    var label = string.IsNullOrWhiteSpace(dtoItem.Name) ? $"ItemId={dtoItem.ItemId}" : $"{dtoItem.Name} ({dtoItem.Brand})";
                    ModelState.AddModelError("RestockItems", $"Error! Item '{label}' is not valid for restocking");
                    continue;
                }

                restock.RestockItems.Add(new RestockItem(
                    itemId: dbItem.Id,
                    restock: restock,
                    quantity: dtoItem.QuantityForRestock,
                    restockPrice: dbItem.RestockPrice
                ));

                dtoItem.PriceOfRestock = dbItem.RestockPrice;
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            restock.TotalPrice = restock.RestockItems.Sum(ri => ri.RestockPrice * ri.Quantity);

            _context.Add(restock);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving the restock");
                return Conflict("Error " + ex.Message);
            }


            var detail = new RestockDetailDTO(
                restock.Id,
                restock.RestockDate,
                restock.DeliveryAddress,
                restock.Title,
                restock.Description,
                restock.ExpectedDate,
                restockForCreate.RestockItems,
                restock.TotalPrice
            );

            detail.ApplicationUserName = user!.UserName;

            return Ok(detail);
            //return CreatedAtAction("GetRestock", new { id = restock.Id }, detail);
        }


    }
}
