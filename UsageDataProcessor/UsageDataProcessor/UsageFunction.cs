using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;

namespace UsageDataProcessor
{
    public static class UsageFunction
    {
        private static DocumentClient _client;
        private const string EndpointUrl = "https://octohackdata.documents.azure.com:443/";
        private const string PrimaryKey = "K1U7AQe66iTN5v2FUDkt6gh5NcBvQR2KO0CwmrQkIIjuABJuwWXEqX5aoxJGVIkO8IuF0oW4LIu3DghOBTwzmA==";

        [FunctionName("GetUsageData")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            _client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

                        var myObj = new { spentYesterday = 1.234, avgLastMonth = 3.456 };
            var jsonToReturn = JsonConvert.SerializeObject(myObj);

            return (ActionResult)new JsonResult(myObj);
        }
    }
}
