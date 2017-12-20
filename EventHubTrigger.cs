using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Net.Http;
using System.Text;

namespace AkruFunction
{
    public static class EventHubTrigger
    {
        [FunctionName("EventHubTrigger")]
        public static void Run([EventHubTrigger("EventHubConnectionString", Connection = "Endpoint=sb://datarouter.servicebus.windows.net/;SharedAccessKeyName=ephsend;SharedAccessKey=9vpZ5lf6P0hQ5lEBD8Fazz1cJOzoFbM5vjaeIIWvLfw=;EntityPath=ms-datarouter")]EventData myEventHubMessage, TraceWriter log)
        {
            log.Info($"C# Event Hub trigger function processed a message: {myEventHubMessage}");
            //var routingKey = myEventHubMessage.Properties["routingKey"];
            //log.Info($"C# Event Hub trigger function processed event routingKey: {routingKey}");

            string authToken = myEventHubMessage.Properties["Authorization"].ToString();

            string data = Encoding.UTF8.GetString(myEventHubMessage.GetBytes());
            log.Info($"C# Event Hub trigger function processed event data: {data}");

            string URL = "http://datarouterappinsights.asepoc.mindspheredemo.online/api/timeseries/";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            client.DefaultRequestHeaders.Add("Authorization", authToken);

            //ServicePointManager.ServerCertificateValidationCallback += 
            //(sender, cert, chain, sslPolicyErrors) => true;

            HttpResponseMessage response = client.PostAsync("", new StringContent(data)).Result;
            log.Info(response.ToString());

            log.Info(response.ToString());
            log.Info(response.Content.ToString());

            string responseBodyAsText = response.Content.ReadAsStringAsync().Result;
            log.Info(responseBodyAsText);

        }
    }
}
