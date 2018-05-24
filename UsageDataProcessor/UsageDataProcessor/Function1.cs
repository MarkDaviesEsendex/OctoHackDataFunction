using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace UsageDataProcessor
{
    public static class UsageFunction
    {
        private static DocumentClient _client;
        private const string EndpointUrl = "https://octohackdata.documents.azure.com:443/";
        private const string PrimaryKey = "K1U7AQe66iTN5v2FUDkt6gh5NcBvQR2KO0CwmrQkIIjuABJuwWXEqX5aoxJGVIkO8IuF0oW4LIu3DghOBTwzmA==";

        [FunctionName("GetUsageData")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            _client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

            var meterReference = GetRequestValue(req, "meter_identifier");
            var intervalStart = DateTime.Parse(GetRequestValue(req, "interval_start"));
            var intervalEnd = DateTime.Parse(GetRequestValue(req, "interval_end"));

            var queryOptions = new FeedOptions { EnableCrossPartitionQuery = true };
            var eneryData = _client.CreateDocumentQuery<EnergyData>(UriFactory.CreateDocumentCollectionUri("octo", "userconsumption"), queryOptions)
                .Where(data => data.MeterIdentifier == meterReference &&
                               data.IntervalStart >= intervalStart &&
                               data.IntervalEnd <= intervalEnd).ToList();
            return req.CreateResponse(HttpStatusCode.OK, eneryData);
        }

        private static string GetRequestValue(HttpRequestMessage req, string keyName) => req.GetQueryNameValuePairs()
            .FirstOrDefault(q => string.Compare(q.Key, keyName, StringComparison.OrdinalIgnoreCase) == 0)
            .Value;
    }

    public class EnergyData
    {
        [JsonProperty("meter_identifier")]
        public string MeterIdentifier { get; set; }

        [JsonProperty("interval_start")]
        public DateTime IntervalStart { get; set; }

        [JsonProperty("interval_end")]
        public DateTime IntervalEnd { get; set; }

        [JsonProperty("kWh_per_half_hour")]
        public string KwhPerHalfHour { get; set; }
    }
}
