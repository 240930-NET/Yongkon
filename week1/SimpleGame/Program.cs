using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace SimpleGame;

class Program
{
    static void Main(string[] args)
    {
        int inputOption = 0;
        int comPlayer = 0;
        string[] choices = {"Rock", "Paper", "Scissors"};
        string input = "";
        string pattern = "^[1239]$";

        Console.Clear();

        while(inputOption !=9) {
            // 1: Rock
            // 2: Paper
            // 3: Scissors
            // 9: Exit
            Console.WriteLine("Please Choose (1) Rock (2) Paper (3) Scissors ");
            Console.WriteLine("If you want to exit, select (9) ");
            input = Console.ReadLine();

            // Check Validation
            Regex regex = new Regex(pattern);

            if (!regex.IsMatch(input)) {
                Console.WriteLine("Please enter valid input!!\n");
                continue;
            }

            // Convert string to int
            inputOption = int.Parse(input);            

            //Computer Choices (using random)
            Random randomChoice = new Random();
            comPlayer = randomChoice.Next(1, choices.Length + 1);

            if (inputOption == 9) {
                Console.WriteLine("See you later!");
                break;
            }

            Console.WriteLine($"Your choice is ... {choices[inputOption - 1]}");
            Console.WriteLine($"Computer's choice is ... {choices[comPlayer - 1]}");
            // 1: Rock     2: Paper     3: Scissors
            // 1 < 2, 1 > 3, 2 < 3 ..... win

            switch(inputOption, comPlayer) {
                case(2,1):
                case(1,3):
                case(3,2):
                    Console.WriteLine("You win!!");
                    break;
                case(1,1):
                case(2,2):
                case(3,3):
                    Console.WriteLine("It's a tie!!");
                    break;
                default:
                    Console.WriteLine("You lose.");
                    break;                
            }
            Console.WriteLine();

        }
   
    }

}