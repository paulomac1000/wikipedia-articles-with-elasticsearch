using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WikipediaSearchEngine.Models;

namespace WikipediaSearchEngine.Controllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly IElasticClient _elasticClient;
        private readonly IConfiguration _configuration;

        private static bool _isElasticDatabaseLocked;

        public ApiController(IElasticClient elasticClient, IConfiguration configuration)
        {
            _elasticClient = elasticClient;
            _configuration = configuration;

            _isElasticDatabaseLocked = false;
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

        [HttpPost("recreate-index")]
        public IActionResult RecreateIndex()
        {
            if (_isElasticDatabaseLocked)
                return Json(new { success = false, error = "Index already recreating" });

            var error = RunRecreateIndexScript();
            return Json(new { success = string.IsNullOrEmpty(error), error = error });
        }

        [HttpPost("lock-elastic-database-status")]
        public IActionResult LockElasticDatabaseStatus()
        {
            return Json(new { status = _isElasticDatabaseLocked, statusName = _isElasticDatabaseLocked ? "locked" : "unlocked" });
        }

        private string RunRecreateIndexScript()
        {
            _isElasticDatabaseLocked = true;

            var pythonPath = _configuration["PythonPath"];
            if (string.IsNullOrEmpty(pythonPath))
                return "PythonPath setting not given, check appsettings.json.";

            var start = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = "recreateIndex.py",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = Process.Start(start);
            if (process == null)
                return "Unable to execute script, check if script exist.";

            Task.Run(() =>
            {
                process.WaitForExit();

                _isElasticDatabaseLocked = false;
                process.Dispose();
            });

            return null;
        }
    }
}