using AppForSEII2526.API.DTOs.PlanDTOs;
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

                .Select(p => new PlanForDetailDTO(p.Id, p.TotalPrice, p.CreatedDate, p.Name, p.PaymentMethod.User.Name, p.PaymentMethod.User.Surname, p.Description, p.Weeks, p.CreatedDate, p.HealthIssues, p.PaymentMethod, p.PlanItems
                    .Select(pi => new PlanItemDTO(pi.PlanId, pi.Class.Id, pi.Class.Price, pi.Class.Capacity, pi.Class.Date, pi.Goal)).ToList())).FirstOrDefaultAsync();


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


            if (dto == null)
            {
                _logger.LogError("Plan creation failed: DTO is null.");
                return BadRequest("Invalid plan data.");
            }

            var availableClasses = await _context.Classes
                .Where(c => c.Date >= DateTime.Today)
                .ToListAsync();

            if (!availableClasses.Any())
            {
                return BadRequest("No available classes for enrollment.");
            }

            if (dto.PlanItems == null || !dto.PlanItems.Any())
            {
                return BadRequest("You must select at least one class to create a plan.");
            }

            var invalidDates = await _context.Classes
                .Where(c => dto.PlanItems.Select(pi => pi.ClassId).Contains(c.Id) && c.Date < DateTime.Today)
                .ToListAsync();

            if (invalidDates.Any())
            {
                return BadRequest("One or more selected classes have invalid dates (before today).");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Plan name is mandatory.");

            if (dto.Weeks <= 0)
                return BadRequest("Number of weeks must be greater than 0.");

            if (dto.PaymentMethod == null || dto.PaymentMethod.Id <= 0)
                return BadRequest("A valid payment method must be provided.");


            var selectedClassIds = dto.PlanItems.Select(pi => pi.ClassId).ToList();
            var selectedClasses = await _context.Classes
                .Where(c => selectedClassIds.Contains(c.Id))
                .ToListAsync();

            foreach (var c in selectedClasses)
            {
                if (c.Capacity <= 0)
                {
                    return Conflict($"Class '{c.Name}' does not have enough capacity.");
                }
            }

            var user = await _context.Users.FirstOrDefaultAsync(au => au.UserName == dto.NameUser);
            if (user == null)
                ModelState.AddModelError("RentalApplicationUser", "Error! UserName is not registered");

            try
            {
                decimal totalPrice = dto.PlanItems.Sum(i => i.Price);

                var plan = new Plan
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Weeks = dto.Weeks,
                    CreatedDate = dto.CreatedDate,
                    HealthIssues = dto.HealthIssues,
                    PaymentMethod = dto.PaymentMethod,
                    TotalPrice = totalPrice,
                    PlanItems = dto.PlanItems.Select(pi => new PlanItem
                    {
                        ClassId = pi.ClassId,
                        Goal = pi.Goal
                    }).ToList()
                };

                _context.Plans.Add(plan);
                await _context.SaveChangesAsync();

                var result = new PlanForDetailDTO(
                    plan.Id,
                    plan.TotalPrice,
                    plan.CreatedDate,
                    plan.Name,
                    dto.NameUser,
                    dto.SurnameUser,
                    plan.Description,
                    plan.Weeks,
                    plan.CreatedDate,
                    plan.HealthIssues,
                    plan.PaymentMethod,
                    dto.PlanItems
                );

                return CreatedAtAction("GetPlan", new { id = plan.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating plan: {ex.Message}");
                return BadRequest("Error while creating plan.");
            }
        }




    }
}
