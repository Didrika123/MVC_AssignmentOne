using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentOne.Models
{
    public class GuessGameService
    {
        private static int _minGuess = 1;
        private static int _maxGuess = 100;
        private static int _defaultHighscore = 25;
        public static GuessEntry BuildGuessEntry(int? guess, int? secretNumber, int? numGuesses, string highscoreStr)
        {
            bool validHighscore = int.TryParse(highscoreStr, out int highscore) && highscore > 0;

            GuessEntry guessEntry = new GuessEntry()
            {
                Guess = guess.HasValue? guess.Value : _minGuess - 1,
                Highscore = validHighscore? highscore : _defaultHighscore,
                NumGuesses = numGuesses.HasValue? numGuesses.Value : 0,
                SecretNumber = IsValidGuess(secretNumber)? secretNumber.Value : new Random().Next(_minGuess, _maxGuess + 1),
            };
            if(guess.HasValue) //If user loads page without a guess then skip the error check
                CheckError(guessEntry);
            CheckStatus(guessEntry);

            return guessEntry;
        }

        public static bool IsValidGuess(int? guess)
        {
            return guess.HasValue && (guess >= _minGuess && guess <= _maxGuess);
        }
        private static void CheckError(GuessEntry guessEntry)
        {
            guessEntry.Error = "";
            if (!IsValidGuess(guessEntry.Guess))
                guessEntry.Error = $"Guess must be in the range {_minGuess}-{_maxGuess}";
        }
        private static void CheckStatus(GuessEntry guessEntry)
        {
            guessEntry.Status = "";
            if(guessEntry.NumGuesses > 0 && IsValidGuess(guessEntry.Guess)) 
            {
                if (guessEntry.Guess > guessEntry.SecretNumber)
                    guessEntry.Status = $"To High! ({guessEntry.NumGuesses} guesses)";
                else if (guessEntry.Guess < guessEntry.SecretNumber)
                    guessEntry.Status = $"Too Low! ({guessEntry.NumGuesses} guesses)";
                else
                {
                    bool newHighscore = false;
                    if(guessEntry.NumGuesses < guessEntry.Highscore)
                    {
                        guessEntry.Highscore = guessEntry.NumGuesses;
                        newHighscore = true;
                    }
                    guessEntry.Status = $"You Win! ({guessEntry.NumGuesses} guesses) " + (newHighscore? "New Highscore!" : "");
                    guessEntry.Guess = _minGuess - 1;
                    guessEntry.NumGuesses = 0;
                    guessEntry.SecretNumber = _minGuess - 1;
                }
            }
        }
    }
}
