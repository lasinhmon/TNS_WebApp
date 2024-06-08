using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TNS_KPI.Areas.KPI_Accounting.Pages
{
    [IgnoreAntiforgeryToken]
    public class DefaultModel : PageModel
    {
        private readonly ILogger<DefaultModel> _logger;

        public DefaultModel(ILogger<DefaultModel> logger)
        {
            _logger = logger;
        }

        public int RandomNumber { get; set; }

        public string RandomWinner { get; set; }

        public void OnGet()
        {
            Random random = new Random();
            RandomNumber = random.Next(1000);
            RandomWinner = $"Player {random.Next(1, 4)}";
        }

        public IActionResult OnPostRandomNumber([FromBody] RandomNumberDTO randomNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model");
            }
            Random random = new Random();
            int number = random.Next(randomNumber.Min, randomNumber.Max);
            return new JsonResult(number);
        }
    }
    public class RandomNumberDTO
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
