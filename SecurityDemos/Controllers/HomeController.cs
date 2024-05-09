using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SecurityDemos.Models;
using Azure.Storage.Blobs;
using Azure.Identity;

namespace SecurityDemos.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration; 

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult GetAnonImage()
    {
        return View();
    }

    public IActionResult GetSecureImage()
    {
        return View();
    }

    public IActionResult GetBlobImage()
    {
        try{
       //donwload an image from blob storage and display it as an image on a page
        BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration.GetConnectionString("AnonymousStorageAccount"));
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("anonymousimages");
        BlobClient blobClient = containerClient.GetBlobClient("MSLogo.png");
        var blobDownloadInfo = blobClient.Download();
        var imageStream = blobDownloadInfo.Value.Content;
        return File(imageStream, "image/png");
        }
        catch (Exception ex)
        {
            return Content(ex.Message);
        }
    }

    public IActionResult GetSecureBlobImage()
    {
        try{
       //donwload an image from blob storage and display it as an image on a page
        BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri(_configuration.GetConnectionString("SecureStorageAccount")), new DefaultAzureCredential(), null);

        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("secureimages");
        BlobClient blobClient = containerClient.GetBlobClient("MSLogo.png");
        var blobDownloadInfo = blobClient.Download();
        var imageStream = blobDownloadInfo.Value.Content;
        return File(imageStream, "image/png");
        }
        catch (Exception ex)
        {
            return Content(ex.Message);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
