using AppForSEII2526.API.DTOs.PlanDTOs;
using AppForSEII2526.API.DTOs.TypeItemDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlansController> _logger;

        public PlansController(ApplicationDbContext context, ILogger<PlansController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PlanForDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPlan(int id)
        {
            PlanForDetailDTO? plan = await _context.Plans
                .Where(p => p.Id == id)
                .Include(p => p.PlanItems)
                    .ThenInclude(pi => pi.Class)
                        .ThenInclude(c => c.TypeItems)

                .Include(p => p.PaymentMethod)
                    .ThenInclude(pm => pm.User)

                .Select(p => new PlanForDetailDTO(p.Id, p.TotalPrice, p.CreatedDate, p.Name, p.PaymentMethod.User.Name, p.PaymentMethod.User.Surname, p.Description, p.Weeks, p.CreatedDate, p.HealthIssues, new PaymentmethodDTO(p.PaymentMethod.Id, p.PaymentMethod.User.Name), p.PlanItems
                    .Select(pi => new PlanItemDTO(pi.Class.Id, pi.Class.Name,pi.Class.TypeItems.Select(ti => new TypeItemForClassDTO(ti.Id, ti.Name)).ToList(), pi.Class.Price, pi.Class.Capacity, pi.Class.Date, pi.Goal)).ToList())).FirstOrDefaultAsync();


            if (plan == null)
            {
                _logger.LogError($"Plan with id {id} not found.", id);
                return NotFound();
            }

            return Ok(plan);
        }

        [HttpPost]
        [Route("[action]")]

        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(PlanForDetailDTO), (int)HttpStatusCode.Created)]
        
        public async Task<ActionResult> CreatePlan(PlanForCreateDTO dto)
        {

            if (dto.PlanItems == null || !dto.PlanItems.Any())
            {
                ModelState.AddModelError("PlanItems", "You must select at least one class to create a plan.");
            }

            var fechaLimite = new DateTime(2025, 10, 10);

            var invalidDates = await _context.Classes
                .Where(c => dto.PlanItems.Select(pi => pi.ClassId).Contains(c.Id) && c.Date < fechaLimite)
                .ToListAsync();

            if (invalidDates.Any())
            {
                ModelState.AddModelError("PlanItems", "One or more selected classes have invalid dates (before today).");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
                ModelState.AddModelError("Name", "Plan name is mandatory.");

            if (dto.Weeks <= 0)
                ModelState.AddModelError("Weeks", "Number of weeks must be greater than 0.");

            var paymentMethod = await _context.PaymentMethods
                .Include(pm => pm.User)
                .FirstOrDefaultAsync(pm => pm.Id == dto.PaymentMethod.Id);

            if (paymentMethod == null)
            {
                ModelState.AddModelError("PaymentMethod", "A valid payment method must be provided.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }


            var selectedClassIds = dto.PlanItems.Select(pi => pi.ClassId).ToList();
            var selectedClasses = await _context.Classes
                .Where(c => selectedClassIds.Contains(c.Id))
                .ToListAsync();

            foreach (var c in selectedClasses)
            {
                if (c.Capacity <= 0)
                {
                    ModelState.AddModelError("PlanItems", $"Class '{c.Name}' does not have enough capacity.");
                }
            }

            var user = await _context.Users.FirstOrDefaultAsync(au => au.UserName == dto.NameUser);
            if (user == null)
                ModelState.AddModelError("ApplicationUser", "Error! UserName is not registered");

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

                decimal totalPrice = dto.PlanItems.Sum(i => i.Price);

                var plan = new Plan
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Weeks = dto.Weeks,
                    CreatedDate = DateTime.Now,
                    HealthIssues = dto.HealthIssues,
                    PaymentMethod = paymentMethod,
                    TotalPrice = totalPrice,
                    PlanItems = dto.PlanItems.Select(pi => new PlanItem
                    {
                        ClassId = pi.ClassId,
                        Goal = pi.Goal
                    }).ToList()
                };

                _context.Plans.Add(plan);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: {ex.Message}");
                return Conflict("Error while saving your plan.");
            }

            var result = new PlanForDetailDTO(
                    plan.Id,
                    plan.TotalPrice,
                    plan.CreatedDate,
                    plan.Name,
                    user.Name,
                    user.Surname,
                    plan.Description,
                    plan.Weeks,
                    plan.CreatedDate,
                    plan.HealthIssues,
                    new PaymentmethodDTO(plan.PaymentMethod.Id, plan.PaymentMethod.User.Name),
                    dto.PlanItems
            );

                return CreatedAtAction("GetPlan", new { id = plan.Id }, result);
        }
    }
}
