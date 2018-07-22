using System;

namespace ConsoleColours
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set the console window to accept ANSI Escape strings

            ANSIConsole.ActivateConsole();

            Console.WriteLine("Hello there.  This is a demo of some enhancements to your console output.");
            Console.WriteLine("");
            Console.WriteLine("Sometimes you might need to " + ConsoleEffects.Underline + "underline" + ConsoleEffects.NoUnderline + " some words, or make them " + ConsoleEffects.Bold + "bolder" + ConsoleEffects.Default + " to make them stand out.");
            Console.WriteLine("");
            Console.WriteLine("Perhaps if you want to show an error message, " + ConsoleEffects.Foreground_Red + ConsoleEffects.Bold + "you might highlight the output in red," + ConsoleEffects.Default + " before resuming normal output.");

            Console.WriteLine("");
            Console.WriteLine("Or perhaps you want to highlight multiple words: " + ConsoleEffects.Background_Cyan + "Option 1" + ConsoleEffects.Default + " " + ConsoleEffects.Foreground_Yellow + "Option 2" + ConsoleEffects.Default + " " + ConsoleEffects.Background_White + ConsoleEffects.Foreground_Magenta + "Option 3" + ConsoleEffects.Default + " " + ConsoleEffects.Bright_Background_Yellow + ConsoleEffects.Bright_Foreground_Cyan + "Option 4" + ConsoleEffects.Default);


            // Example using the standard Console Class.

            Console.WriteLine("");
            Console.WriteLine("");
            Console.Write("It took 5 commands using the standard Console class just to insert a single ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Red");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" word in this line.");

            Console.ReadLine();
        }
    }
}
