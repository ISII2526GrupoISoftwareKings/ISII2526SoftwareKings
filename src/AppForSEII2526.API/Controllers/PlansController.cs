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
                    .Select(pi => new PlanItemDTO(pi.PlanId, pi.Class.Name, pi.Class.Price, pi.Class.Capacity, pi.Class.Date, pi.Goal)).ToList())).FirstOrDefaultAsync();


            if (plan == null)
            {
                _logger.LogError($"Plan with id {id} not found.", id);
                return NotFound();
            }

            return Ok(plan);
        }

    }
}
