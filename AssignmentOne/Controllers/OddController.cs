using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssignmentOne.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentOne.Controllers
{
    public class OddController : Controller
    {
        
        [HttpGet]
        [Route("/FeverCheck")]
        public IActionResult FeverCheck()
        {
            return View();
        }

        [HttpPost]
        [Route("/FeverCheck")]
        public IActionResult FeverCheck(float temperature, string degreetype)
        {
            FeverCheckService.DegreeType type = degreetype == "fahrenheit" ? FeverCheckService.DegreeType.Fahrenheit : FeverCheckService.DegreeType.Celcius;
            ViewBag.FeverResult = FeverCheckService.CheckFever(temperature, type);
            return View();
        }



        [HttpGet]
        [Route("/GuessingGame")]
        public IActionResult GuessingGame()
        {
            //Load data from Cookies
            int? secretNumber = HttpContext.Session.GetInt32("GGSecretNumber");
            int? numGuesses = HttpContext.Session.GetInt32("GGNumGuesses");
            int? guess = HttpContext.Session.GetInt32("GGGuess");
            Request.Cookies.TryGetValue("GGHighscore", out string highscoreStr);

            //Build a Guess entry from Cookie data
            GuessEntry guessEntry = GuessGameService.BuildGuessEntry(guess, secretNumber, numGuesses, highscoreStr);

            //Update Cookies  (Why? If Cookies dont exist or Won Game -> the guessEntry builder will alter the values)
            HttpContext.Session.SetInt32("GGSecretNumber", guessEntry.SecretNumber);
            HttpContext.Session.SetInt32("GGNumGuesses", guessEntry.NumGuesses);
            HttpContext.Session.Remove("GGGuess"); //Consume the guess in this run so we can know when a new guess appears so we only do error check on new guesses.
            if(highscoreStr != guessEntry.Highscore.ToString()) //Dont update clients highscore cookie if unchanged to save some time
            {
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddMonths(1);
                Response.Cookies.Append("GGHighscore", guessEntry.Highscore.ToString(), options);
            }

            return View(guessEntry);
        }


        [HttpPost]
        [ValidateAntiForgeryToken] //Use asp-for tags in your form, and u will get autocreated token
        [Route("/GuessingGame")]
        public IActionResult GuessingGame(GuessEntry guessEntry)
        {
            int? numGuesses = HttpContext.Session.GetInt32("GGNumGuesses");

            //Save the guess in Cookie
            HttpContext.Session.SetInt32("GGGuess", guessEntry.Guess);

            //Tally numGuesses
            if (GuessGameService.IsValidGuess(guessEntry.Guess))
                HttpContext.Session.SetInt32("GGNumGuesses", (++numGuesses).Value);

            //Load standard GuessGame View
            return RedirectToAction(nameof(GuessingGame));
        }
    }
}
