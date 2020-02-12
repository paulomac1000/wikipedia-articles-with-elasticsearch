using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Threading.Tasks;
using WikipediaSearchEngine.Models;

namespace WikipediaSearchEngine.Controllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly IElasticClient _elasticClient;

        public ApiController(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        [HttpGet("simple-query-string")]
        public async Task<IActionResult> SimpleQueryString(string term)
        {
            var res = await _elasticClient.SearchAsync<Document>(x => x
                .Query(q => q.SimpleQueryString(qs => qs.Query(term))));

            var res2 = await _elasticClient.SearchAsync<Document>(x => x
                .Query(q => q.CommonTerms(qs => qs.Query(term))));

            if (!res.IsValid)
                throw new InvalidOperationException(res.DebugInformation);

            return Json(res.Documents);
        }
    }
}