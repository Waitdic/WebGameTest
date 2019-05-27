using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebGame.Models
{
    public class AnswerInitializer
    {
        public static void Initialize(AnswerContext context)
        {
            if (!context.Answers.Any())
            {
                context.Answers.AddRange(
                    new Answer
                    {
                        SystemAnswer = "Здравствуйте. Загаданы фрукты и музыкальные инструменты. "
                    });
                context.Counters.AddRange(
                   new Counter
                   {
                       Number = 0,
                       TrueAnswerNumber = 0,
                       PathTrueWord = ""
                   }
                   );
                context.SaveChanges();
            }
            else
            {
                context.Database.EnsureDeleted();
                context.SaveChanges();
                context.Database.EnsureCreated();
                context.Answers.AddRange(
                    new Answer
                    {
                        SystemAnswer = "Здравствуйте.Загаданы фрукты и музыкальные инструменты. "
                    });
                    context.Counters.AddRange(
                   new Counter
                   {
                       Number = 0,
                       TrueAnswerNumber = 0,
                       PathTrueWord = ""
}
                    );
                context.SaveChanges();
            }
        }
    }
}
