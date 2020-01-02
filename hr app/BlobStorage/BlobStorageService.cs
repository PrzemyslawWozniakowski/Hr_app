using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hr_app.EntityFramework;
using hr_app.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
 
namespace hr_app.BlobStorage
{
     public  class BlobStorageService  
    {

        private readonly DataContext _context;
        private IConfiguration _configuration;

        public BlobStorageService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public void AddToStorage(JobApplication application, IFormFile CV)
        {
            string fileName = "cv" + application.FirstName.ToString() + application.LastName.ToString() + application.JobOfferId.ToString();
            string connectionString = _configuration["AzureBlob"];
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            var section = _configuration.GetSection("Azure");
            string containerName = section.GetValue<string>("ContainerName");
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            blobClient.Upload(CV.OpenReadStream());

            application.CvUrl= blobClient.Uri.ToString();
        }

        public void deleteFromStorage(JobApplication application)
        {
            string fileName = "cv" + application.FirstName.ToString() + application.LastName.ToString() + application.JobOfferId.ToString();
            string connectionString = _configuration["AzureBlob"];
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            var section = _configuration.GetSection("Azure");
            string containerName = section.GetValue<string>("ContainerName");
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            containerClient.DeleteBlobIfExists(fileName);
        }
    }  
}  

