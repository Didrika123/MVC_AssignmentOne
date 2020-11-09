using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentOne.Controllers
{
    public class OddController : Controller
    {

        [Route("/FeverCheck")]
        public IActionResult FeverCheck()
        {
            return View();
        }

        [HttpPost]
        [Route("/FeverCheck")]
        public IActionResult FeverCheck(float temperature, string degreetype)
        {
            if(degreetype == "fahrenheit")
                temperature = (temperature - 32 ) * 5 / 9; //wHy iS thIs sO WiErD

            ViewBag.Temperature = temperature;
            
            return View();
        }
    }
}
