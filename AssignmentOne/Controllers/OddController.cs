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
        
        [Route("/FeverCheck")]
        public IActionResult FeverCheck()
        {
            return View();
        }

        [HttpPost]
        [Route("/FeverCheck")]
        public IActionResult FeverCheck(float temperature, string degreetype)
        {
            FeverChecker.DegreeType type = degreetype == "fahrenheit" ? FeverChecker.DegreeType.Fahrenheit : FeverChecker.DegreeType.Celcius;
            ViewBag.FeverResult = FeverChecker.CheckFever(temperature, type);
            
            return View();
        }



        [Route("/GuessingGame")]
        public IActionResult GuessingGame()
        {
            int? secretNumber = HttpContext.Session.GetInt32("SecretNumber");
            int? numGuesses = HttpContext.Session.GetInt32("NumGuesses");
            if (!secretNumber.HasValue || !numGuesses.HasValue)
            {
                ResetGuessGame();
            }
            bool validHighscore = Request.Cookies.TryGetValue("GGHS", out string highscoreStr);
            int highscore = 0;
            if (validHighscore)
                validHighscore = int.TryParse(highscoreStr, out highscore);

            ViewBag.GuessGameHighscore = validHighscore ? highscore : 0;

            return View();
        }


        [HttpPost]
        [Route("/GuessingGame")]
        public IActionResult GuessingGame(string guess)
        {
            int? secretNumber = HttpContext.Session.GetInt32("SecretNumber");
            int? numGuesses = HttpContext.Session.GetInt32("NumGuesses");
            bool validGuess = int.TryParse(guess, out int guessInt);
            if (!secretNumber.HasValue || !numGuesses.HasValue || !validGuess)
            {
                if (!validGuess)
                    ViewBag.GuessGameError = "BAD GUESS!!!";
                return GuessingGame();
            }


            HttpContext.Session.SetInt32("NumGuesses", (++numGuesses).Value);
            ViewBag.GuessGameNumGuesses = numGuesses;

            bool validHighscore = Request.Cookies.TryGetValue("GGHS", out string highscoreStr);
            int highscore = 0;
            if (validHighscore)
                validHighscore = int.TryParse(highscoreStr, out highscore);

            ViewBag.GuessGameHighscore = validHighscore ? highscore : 0;

            if (guessInt < secretNumber)
            {
                ViewBag.GuessGameResult = "WRONG, Too Low!";
            }
            else if (guessInt > secretNumber)
            {
                ViewBag.GuessGameResult = "WRONG, To High!";
            }
            else
            {
                ViewBag.GuessGameResult = "WIN";

                if(!validHighscore || numGuesses < highscore)
                {
                    CookieOptions options = new CookieOptions();
                    options.Expires = DateTime.Now.AddMonths(1);
                    Response.Cookies.Append("GGHS", numGuesses.ToString(), options); //Guessing Game High Score
                    ViewBag.GuessGameHighscore = numGuesses;
                }

                ResetGuessGame();
            }
            return View();
        }
        private void ResetGuessGame()
        {
            HttpContext.Session.SetInt32("SecretNumber", new Random().Next(1, 100));
            HttpContext.Session.SetInt32("NumGuesses", 0);

        }
    }
}
