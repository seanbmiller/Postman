using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Extensions;

namespace grab
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            CommandLine.Parser.Default.ParseArguments(args, options);

            if (options.collection != null)
                getCollection(options);

            if (options.envName != null)
                getEnvironmentTemplate(options);

        }

        private static void getCollection(Options options)
        {
            //get list from collections
            var grabCollection = options.collection;
            var fileName = options.collection + ".postman_collection.json";
            if (!options.outputPath.EndsWith(@"\"))
            {
                options.outputPath = options.outputPath + @"\";
            }
            var outPath = options.outputPath + fileName;

            var url = "https://api.getpostman.com/collections";
            var apiKey = options.apiKey;
            var client = new RestClient(url);

            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("x-api-key", apiKey);

            var response = client.Execute(request);
            var collections = JsonConvert.DeserializeObject<PostmanCollection>(response.Content);

            //download collection
            Collection myCollection = collections.Collections.FirstOrDefault(c => c.name == grabCollection);
            url = "https://api.getpostman.com/collections/" + myCollection.uid;
            client = new RestClient(url);
            request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("x-api-key", apiKey);
            response = client.Execute(request);

            //save to file
            dynamic data = JsonConvert.DeserializeObject(response.Content);
            var requests = data.collection;
            File.WriteAllText(outPath, requests.ToString());
        }

        private static void getEnvironmentTemplate(Options options)
        {
            var grabEnvTemplate = options.envName;
            var fileName = options.envName + ".postman_environment.json";
            if (!options.outputPath.EndsWith(@"\"))
            {
                options.outputPath = options.outputPath + @"\";
            }
            var outPath = options.outputPath + fileName;
            var url = "https://api.getpostman.com/environments";
            var apiKey = options.apiKey;
            var client = new RestClient(url);

            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("x-api-key", apiKey);

            var response = client.Execute(request);
            var collections = JsonConvert.DeserializeObject<PostmanEnvironments>(response.Content);
            Collection myEnv = collections.Environments.FirstOrDefault(c => c.name == grabEnvTemplate);
            url = "https://api.getpostman.com/environments/" + myEnv.uid;
            client = new RestClient(url);
            request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("x-api-key", apiKey);
            response = client.Execute(request);

            dynamic data = JsonConvert.DeserializeObject(response.Content);
            var requests = data.environment;
            File.WriteAllText(outPath, requests.ToString());
        }
    }

    public class PostmanEnvironments
    {
        public List<Environment> Environments { get; set; }
    }

    public class PostmanCollection
    {
        public List<Collection> Collections { get; set; }
    }

    public class Collection
    {
        public string id { get; set; }
        public string name { get; set; }
        public string owner { get; set; }
        public string uid { get; set; }
    }

    public class Environment : Collection
    {
    }
}
