using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net.Http.Headers;

namespace mc_dotnet_webapp.Pages
{


    [Authorize]
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
            //GUID = User.FindFirst(c => c.Type == "https://dev-stu62oqd.us.com/GUID")?.Value;
            GUID = guid;

            if (!String.IsNullOrWhiteSpace(GUID)) {
                var client = _httpClientFactory.CreateClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await HttpContext.GetTokenAsync("access_token"));

                //var response = await client.GetAsync($"https://archpoccacheapi.azurewebsites.net/patientAPI/?guid={GUID}");
                var response = await client.GetAsync($"https://localhost:7051/patientAPI/?guid={GUID}");

                if (response.IsSuccessStatusCode)
                {
                    ResponseData = await response.Content.ReadAsStringAsync();
                } else
                {
                    ResponseData = "Error retrieving data.";
                }
            }
        }

        public async Task OnPostAsync(string guid)
        {
            GUID = guid;
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var idtoken = await HttpContext.GetTokenAsync("id_token");
            Console.WriteLine(idtoken);


            Console.WriteLine("test123");

            Console.WriteLine(idtoken);

            if (!String.IsNullOrWhiteSpace(GUID))
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"https://archpoccacheapi.azurewebsites.net/patientAPI/?guid={GUID}");

                if (response.IsSuccessStatusCode)
                {
                    ResponseData = await response.Content.ReadAsStringAsync();
                }
                else
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
