using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net.Http.Headers;
using System.Text.Json;

namespace mc_dotnet_webapp.Pages
{


    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public string GUID { get; private set; }
        public string ResponseData { get; private set; }
        public string MorePatientDetails { get; private set; }

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

                var response = await client.GetAsync($"https://archpoccacheapi.azurewebsites.net/patientAPI/?guid={GUID}");
                //var response = await client.GetAsync($"https://localhost:7051/patientAPI/?guid={GUID}");

                if (response.IsSuccessStatusCode)
                {
                    ResponseData = await response.Content.ReadAsStringAsync();
                    var patientData = JsonSerializer.Deserialize<PatientData>(ResponseData);
                    var patientId = patientData?.Id;

                    MorePatientDetails = await FetchFromFhirAsync(patientId);
                } else
                {
                    ResponseData = "Error retrieving data.";
                }
            }
        }


        public async Task<string> FetchFromFhirAsync(string patientId)
        {
            if (!String.IsNullOrWhiteSpace(patientId))
            {
                var client = _httpClientFactory.CreateClient();
                var fhirResponse = await client.GetAsync($"https://hapi.fhir.org/baseR4/Patient/{patientId}");
                if (fhirResponse.IsSuccessStatusCode)
                {
                    return await fhirResponse.Content.ReadAsStringAsync();
                }
                else
                {
                    _logger.LogError($"Failed to retrieve more patient details from FHIR for Patient ID: {patientId}");
                    return "Error retrieving more patient details from FHIR.";
                }
            }
            return "No Patient ID provided.";
        }

        private class PatientData
        {
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Address { get; set; }
            public string HealthCardNumber { get; set; }
        }

    }
}
