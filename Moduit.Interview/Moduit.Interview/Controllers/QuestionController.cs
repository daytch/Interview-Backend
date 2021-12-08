using Microsoft.AspNetCore.Mvc;
using Moduit.Interview.Models;
using System;
using System.Collections.Generic;
using Flurl;
using Flurl.Http;
using System.Threading.Tasks;
using System.Linq;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Moduit.Interview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        const string endpoint = "https://screening.moduit.id/backend/question/";

        [HttpGet]
        [Route("One")]
        public async Task<ResponseOne> GetNumberOne()
        {
            var response = await endpoint.AppendPathSegment("one").GetJsonAsync<ResponseOne>();
            return response;
        }

        [HttpGet]
        [Route("Two")]
        public async Task<List<ResponseOne>> GetNumberTwo()
        {
            var response = await endpoint.AppendPathSegment("two").GetJsonAsync<List<ResponseOne>>();
            response = response.Where(x => x.title.Contains("Ergonomic") || x.description.Contains("Ergonomic") && (x.tags != null && x.tags.Contains("Sport")))
                .OrderByDescending(x => x.id).TakeLast(3).ToList();
            return response;
        }


        [HttpGet]
        [Route("Three")]
        public async Task<List<ResponseExpected>> GetNumberThree()
        {
            var response = await endpoint.AppendPathSegment("three").GetJsonAsync<List<ResponseThree>>();
            List<ResponseExpected> resp = new List<ResponseExpected>();
            ResponseThree resp3 = new ResponseThree();
            foreach (ResponseThree item in response)
            {
                resp3.id = item.id;
                resp3.category = item.category;
                resp3.createdAt = item.createdAt;
                if (item.items != null)
                {
                    foreach (Item i in item.items)
                    {
                        ResponseExpected re = new ResponseExpected()
                        {
                            id = resp3.id,
                            category = resp3.category,
                            description = i.description,
                            createdAt = resp3.createdAt,
                            footer = i.footer,
                            title = i.title
                        };
                        resp.Add(re);
                    }
                }
            }
            return resp;
        }

    }
}
