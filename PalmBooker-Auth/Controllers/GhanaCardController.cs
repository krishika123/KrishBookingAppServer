using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EBookkeepingAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GhanaCardController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        public GhanaCardController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet("GhanaCard/{cardNumber}")]
        public async Task<IActionResult> SearchAllAgents(string cardNumber)
        { 
            var request = new HttpRequestMessage(HttpMethod.Get, "http://psl-app-vm3/EBookkeepingApi/api/Guin/GetNiaIdDetails/" + cardNumber);
            request.Headers.Add("Accept", "application/json");
            var client = _clientFactory.CreateClient();
            var results ="";
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsStringAsync();
                results = res;
            }
          
            return new JsonResult(results);
        }

        [HttpGet("ValidateTin/{tinNumber}")]
        public async Task<IActionResult> ValidateTin(string tinNumber)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://psl-app-vm3/EBookkeepingApi/api/Guin/GetTinDetails/" + tinNumber);
            request.Headers.Add("Accept", "application/json");
            var client = _clientFactory.CreateClient();
            var results = "";
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsStringAsync();
                results = res;
            }

            return new JsonResult(results);
        }

       
    }
}
