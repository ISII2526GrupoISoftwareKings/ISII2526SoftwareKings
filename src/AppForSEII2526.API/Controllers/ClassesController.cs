using AppForSEII2526.API.DTOs.ClassDTOs;
using AppForSEII2526.API.DTOs.TypeItemDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<ClassesController> _logger;

        public ClassesController(ApplicationDbContext context, ILogger<ClassesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //[HttpGet]
        //[Route("[action]")]
        //[ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        //public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)
        //{
        //    if (op2 == 0)
        //    {
        //        string error = "Op2 cannot be 0 to compute a division";
        //        _logger.LogError(error);
        //        return BadRequest(error);
        //    }
        //    decimal result = op1 / op2;
        //    return Ok(result);
        //}


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ClassForPlanDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelError), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetClassesForPlan(string? className, DateTime? date)
        {
            DateTime finalDate;
            DateTime startDate = new DateTime(2025, 10, 10);

            if (date != null && date < startDate)
            {
                ModelState.AddModelError("Date&finalDate", "Date must be later than 10/10/2025");
                _logger.LogError($"{DateTime.Now} Error: Date must be later than 10/10/2025");
                return BadRequest(new ValidationProblemDetails(ModelState));
            } else if (date != null){
                finalDate = date.Value.AddDays(7);
            }
            else //if not renting dates are provided a value by default is assigned
            {
                date = new DateTime(2025,10,10);
                finalDate = date.Value.AddDays(7);
            }



            IList<ClassForPlanDTO> classesDTO = await _context.Classes 
                .Include(c => c.TypeItems)
                .Where(c => (c.Name.Contains(className) || (className == null)) && c.Date >= date && c.Date <= finalDate)
                .OrderBy(c => c.Id)
                .Select(c => new ClassForPlanDTO(c.Id, c.Name, c.TypeItems.Select(ti => new TypeItemForClassDTO(ti.Id, ti.Name)).ToList(), c.Date, c.Price))
                .ToListAsync();

            if (!classesDTO.Any())
            {
                _logger.LogError($"{DateTime.Now} Error: No classes available between {date:d} and {finalDate:d}");
                return BadRequest(new { Message = "No classes available for the selected date and filters." });
            }
            return Ok(classesDTO);
        }
    }
}
