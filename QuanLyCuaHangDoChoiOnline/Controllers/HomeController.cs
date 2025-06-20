using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using QuanLyCuaHangDoChoiOnline.Models;
using QuanLyCuaHangDoChoiOnline.Repositories;

namespace QuanLyCuaHangDoChoiOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var lstToy = ToyRes.GetToyWithSelling();    
            var lstToyType = ToyTypeRes.GetAllType();
            List<Toy> toys = new List<Toy>();
            if (lstToy.Count < 6)
            {
                for (int i = 0; i < lstToy.Count; i++)
                {
                    toys.Add(lstToy[i]);
                }
            } else
            {
                for (int i = 0; i < 6; i++)
                {
                    toys.Add(lstToy[i]);
                }
            }
            dynamic dy = new ExpandoObject();
            dy.toy = toys;
            dy.toytypeNAV = lstToyType;
            return View(dy);
        }
        
        public IActionResult About()
        {
            dynamic dy = new ExpandoObject();
            dy.toytypeNAV = ToyTypeRes.GetAllType();
            return View(dy);
        }
        public IActionResult Contact()
        {
            dynamic dy = new ExpandoObject();
            dy.toytypeNAV = ToyTypeRes.GetAllType();
            return View(dy);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
