using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebGame.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string UserAnswer{ get; set; }
        public string SystemAnswer { get; set; }
    }
}
