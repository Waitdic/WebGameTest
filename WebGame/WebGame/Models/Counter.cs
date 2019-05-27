using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebGame.Models
{
    public class Counter
    {
        public int CounterId { get; set; }
        public int Number { get; set; }
        public int TrueAnswerNumber { get; set; } 
        public string PathTrueWord { get; set; }

    }
}
