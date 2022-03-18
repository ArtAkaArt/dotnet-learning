using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HelloApp2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            using (var client = new HttpClient())
            {

            
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://test/api");
                var response = await client.SendAsync(request);
                ViewData["Message"] = await response.Content.ReadAsStringAsync();
            }
        }
    }
}