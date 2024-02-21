using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace mc_dotnet_webapp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public string GUID { get; private set; }
        public string ResponseData { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync(string guid)
        {
            GUID = guid;
            
            if(!String.IsNullOrWhiteSpace(GUID)) {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"https://archpoccacheapi.azurewebsites.net/patientAPI/?guid={GUID}");

                if (response.IsSuccessStatusCode)
                {
                    ResponseData = await response.Content.ReadAsStringAsync();
                } else
                {
                    ResponseData = "Error retrieving data.";
                }
            }
        }
        
        /*
        public void OnGet(string guid)
        {
            GUID = guid;
        }
        */

    }
}
