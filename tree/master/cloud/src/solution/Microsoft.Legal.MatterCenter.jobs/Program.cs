﻿using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Microsoft.Legal.MatterCenter.Jobs
{
    public class Program
    {     
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json");
            var configuration = builder.Build();
            KeyVaultHelper kv = new KeyVaultHelper(configuration);
            kv.GetKeyVaultSecrets();
            var azureStorageConnectionString = configuration["Data:DefaultConnection:AzureStorageConnectionString"];
            JobHostConfiguration config = new JobHostConfiguration(azureStorageConnectionString);
            config.UseTimers();
            var host = new JobHost(config);            
            host.RunAndBlock();
        }
    }
}
