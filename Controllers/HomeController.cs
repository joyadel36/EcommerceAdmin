using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Xml.Linq;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
 
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment)
        {
          
            _hostingEnvironment = hostingEnvironment;
          
            _logger = logger;
        }

        public IActionResult Index()
        {
            var enResourcePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Views", "Shared", "_Layout.en-us.resx");
            var arResourcePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Views", "Shared", "_Layout.ar.resx");



            ViewBag.en = GetValue(enResourcePath,"hello");
           ViewBag.ar = GetValue(arResourcePath, "hello"); 



            return View();
        }

        private string GetValue(string filePath, string key)
        {
            var doc = XDocument.Load(filePath);
            var dataElement = doc.Root.Elements("data").FirstOrDefault(d => d.Attribute("name")?.Value == key);

            if (dataElement != null)
            {
               return dataElement.Element("value")!.Value.ToString();
            }
            return "Not fount";
        }
        private void SetLocalizedValue(string filePath, string key, string value)
        {
            var doc = XDocument.Load(filePath);
            var dataElement = doc.Root.Elements("data").FirstOrDefault(d => d.Attribute("name")?.Value == key);

            if (dataElement != null)
            {
                dataElement.Element("value").Value = value;
            }
            else
            {
                var newDataElement = new XElement("data",
                    new XAttribute("name", key),
                    new XAttribute(XNamespace.Xml + "space", "preserve"),
                    new XElement("value", value));

                doc.Root.Add(newDataElement);
            }

            doc.Save(filePath);
           
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
