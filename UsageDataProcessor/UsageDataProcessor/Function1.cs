
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace UsageDataProcessor
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            var myObj = new { spentYesterday = 1.234, avgLastMonth = 3.456 };
            var jsonToReturn = JsonConvert.SerializeObject(myObj);

            return (ActionResult)new JsonResult(myObj);
        }
    }
}
