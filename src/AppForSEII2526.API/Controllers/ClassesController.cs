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
            // Practica SSDD: uso del logger para registrar la inicialización del servicio:
            _logger.LogInformation("TodoService initialized");
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
        public async Task<ActionResult> GetClassesForPlan(string? className, DateTime? date, decimal maximumprice)
        {
            DateTime finalDate;
            DateTime startDate = DateTime.Today;

            _logger.LogInformation("Received request to get classes. className={ClassName}, date={Date}", className, date);


            if (date != null && date < startDate)
            {
                //return BadRequest( Problem("fromDate must be earlier than toDate", 
                //    $"fromDate ({fromDate}) toDate({toDate})", 400,"Bad Request", 
                //    "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"));
                ModelState.AddModelError("Date&finalDate", "Date must be later than Today");
                _logger.LogError($"{DateTime.Now} Error: Date must be later than Today");
                return BadRequest(new ValidationProblemDetails(ModelState));
            } else if (date != null){
                finalDate = date.Value.AddDays(7);
            }
            else //if not renting dates are provided a value by default is assigned
            {
                date = startDate;
                finalDate = startDate.AddDays(7);
                _logger.LogDebug("No date provided. Defaulting to {StartDate} - {FinalDate}", startDate, finalDate);
            }



            IList<ClassForPlanDTO> classesDTO = await _context.Classes 
                .Include(c => c.TypeItems)
                .Where(c => (c.Name.Contains(className) || (className == null)) && c.Date >= date && c.Date <= finalDate && c.Price <= maximumprice)
                .OrderBy(c => c.Id)
                .Select(c => new ClassForPlanDTO(c.Id, c.Name, c.TypeItems.Select(ti => new TypeItemForClassDTO(ti.Id, ti.Name)).ToList(), c.Date, c.Price, c.Capacity))
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
