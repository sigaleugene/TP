using Dadata;
using Dadata.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using T_project.Models;

namespace T_project.Controllers
{
    public static class ResultMessage
    {
        public static string Sucсess {  get { return "Поздравляем! Ваш город -"; }  }
        public static string Failure { get { return "Упс!!! Похоже адреса из разных городов"; } }
        public static string Default { get { return "Введите адреса"; } }
    }
    public class HomeController : Controller
    {
      
        public IActionResult Index(string result = null)
        {
            ViewBag.result = result?? ResultMessage.Default;
            return View();
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

        public async Task<ActionResult> CheckAdress(string address, string address1)
        {

            Address a1 = await Chesk(address);
            Address a2 = await Chesk(address1);
            string result;
            if (a1.region_type !="г")
                 result =a1.city==a2.city ? $"{ResultMessage.Sucсess}{a1.city}" : $"{ResultMessage.Failure}";
            else
                 result = a1.region == a2.region ? $"{ResultMessage.Sucсess}{a1.region}" : $"{ResultMessage.Failure}"; ;
            return Redirect(Url.Action( "index", new { result }));
        }

        static async Task<Address> Chesk(string adress)
        {
            var token = "9a578c71241d51abdd2a7ee579d5c039e6db7a07";
            var secret = "26a4af2a58cafee4a142420e2cbcbdd22c40b802";
            var api = new CleanClientAsync(token, secret);
            Address result = await api.Clean<Address>(adress);
            return result;
        }
    }
}
