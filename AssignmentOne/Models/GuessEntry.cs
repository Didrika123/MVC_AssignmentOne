using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentOne.Models
{
    public class GuessEntry
    {
        public int Guess { get; set; }
        public int SecretNumber { get; set; }
        public int NumGuesses { get; set; }
        public int Highscore { get; set; }
        public string Status { get; set; }
        public string Error { get; set; }

    }
}
