using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebGame.Controllers;
using Microsoft.EntityFrameworkCore;


namespace WebGame.Models
{
    public class MakeSystemAnswer 
    {
        AnswerContext db;
        Counter counter;
        string userAnswer;
        string systemAnswer;
        int count;
        int trueAnswerCounter;
        string[] trueAnswer = { "груша", "барабан", "яблоко", "гитара" };
        string[] firstLevel = {"Нет", "Это не правильно", "К сожалению это не так", "Ты не угадал", "Попробуй еще раз",
                                "Ну что ты так долго думаешь","Давай напрягай извилины", "Как с тобой не просто"};

        string[] secondLevel = { "Нет, слово длиннее", "Слово короче", "С длиной ты угадал" };     
        string wordHelp;
        

        public MakeSystemAnswer(ref AnswerContext context)
        {
            db = context;
        }

        public string MakingSystemAnswer(Answer answer)
        {
            if (String.IsNullOrEmpty(answer.UserAnswer))
                return systemAnswer = "Здесь ничего нет";
    
            userAnswer = answer.UserAnswer.ToLower();         
            userAnswer = userAnswer.Trim(new char[] { ' ', '_', '-', '.', '/' });          

            counter = db.Counters.FirstOrDefault();          
            counter.Number++;
            db.Counters.Update(counter);
            db.SaveChanges();

            CheckWordHelp();
            count = counter.Number;
            trueAnswerCounter = counter.TrueAnswerNumber;
            
            Random rnd = new Random();

            //Make system answer
            //The counter is needed for the variability of answers
            int levDistance = Levenshtein(trueAnswer[trueAnswerCounter], userAnswer);
            if (levDistance < 3 && levDistance > 0)
                return systemAnswer = "Подправь маленько, почти написал";

            if (userAnswer != trueAnswer[trueAnswerCounter])
            {
                if (count < 4)
                    return systemAnswer = FirstLevelAnswer();
                else if (count < 6)
                {
                    return systemAnswer = SecondLevelAnswer();
                }
                else 
                {
                    if (wordHelp == ".")
                    {
                        switch (rnd.Next(0, 2))
                        {
                            case 0:
                                return systemAnswer = SecondLevelAnswer();
                            case 1:
                                return systemAnswer = FirstLevelAnswer();
                        }
                    }
                    else
                    {
                        switch(rnd.Next(0,3))
                        {
                            case 0:
                                return systemAnswer = WordHelp();
                            case 1:
                                return systemAnswer = SecondLevelAnswer();
                            case 2:
                                return systemAnswer = FirstLevelAnswer();
                        }
                    }
                }
            }
            else
            {
                counter.TrueAnswerNumber++;
                if (counter.TrueAnswerNumber > 3)
                    counter.TrueAnswerNumber = 0;
                counter.Number = 0;
                wordHelp = trueAnswer[counter.TrueAnswerNumber];
                counter.PathTrueWord = wordHelp;
                db.Counters.Update(counter);
                db.SaveChanges();
                return systemAnswer = " ПРАВИЛЬНО!!! Давай еще. ";
            }          
            return null;
        }            

        private string SecondLevelAnswer()
        {
            if (userAnswer.Length < trueAnswer[trueAnswerCounter].Length)
                return secondLevel[0];
            if (userAnswer.Length > trueAnswer[trueAnswerCounter].Length)
                return secondLevel[1];
            else return secondLevel[2];
        }

        private string FirstLevelAnswer()
        {
            Random rnd = new Random();
            return systemAnswer = firstLevel[rnd.Next(0, 8)];
        }

        private string WordHelp()
        {
            //wordHelp - the word contains all the available characters for the tooltip. 
            //If the character has already been output, it will be replaced by " ".
            Random rnd = new Random();
            bool check = false;
            bool wordHelpNull = true;
            int indexOfChar = 0;
            char helpSymbol = '_';

            while (check == false)
            {
                indexOfChar = rnd.Next(0, wordHelp.Length);
                if (!Char.IsWhiteSpace(wordHelp[indexOfChar]))
                {
                    helpSymbol = wordHelp[indexOfChar];
                    wordHelp = wordHelp.Replace(wordHelp[indexOfChar], ' ');

                    for (int i = 0; i < wordHelp.Length; i++)
                    {
                        if (!Char.IsWhiteSpace(wordHelp[i]))
                        {
                            wordHelpNull = false;
                            break;
                        }
                    }
                    if (wordHelpNull == true)
                        wordHelp = ".";

                    counter.PathTrueWord = wordHelp;
                    db.Counters.Update(counter);
                    db.SaveChanges();
                    check = true;                
                }               
            }               
            return ($"В слове на {indexOfChar + 1} месте стоит буква << {helpSymbol} >> ");
        }

        private void CheckWordHelp()
        {
            if (String.IsNullOrEmpty(counter.PathTrueWord))
                wordHelp = trueAnswer[counter.TrueAnswerNumber];
            else wordHelp = counter.PathTrueWord;
        }

        private int Levenshtein(string first, string second)
        {
            int diff;
            int[,] m = new int[first.Length + 1, second.Length + 1];

            for (int i = 0; i <= first.Length; i++) { m[i, 0] = i; }
            for (int j = 0; j <= second.Length; j++) { m[0, j] = j; }

            for (int i = 1; i <= first.Length; i++)
            {
                for (int j = 1; j <= second.Length; j++)
                {
                    diff = (first[i - 1] == second[j - 1]) ? 0 : 1;

                    m[i, j] = Math.Min(Math.Min(m[i - 1, j] + 1,
                                             m[i, j - 1] + 1),
                                             m[i - 1, j - 1] + diff);
                }
            }
            return m[first.Length, second.Length];
        }

    }
}
