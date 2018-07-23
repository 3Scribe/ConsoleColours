# Enhancing Console Output with Underlining, Colours and more

**Eddie Gahan**
**July 2018**

**Intro**

With the rise of .Net Core, GUI-less Console applications have come back into popular usage which is perfectly fine.  It’s just that they’re so bland; black and white and basic text when sometimes you need a splash of colour or an enhancement to bring focus to something of importance.  Recently, when developing a console app, I thought it would be nice to underline a single letter in a command to show what button could be pressed to trigger the command; similar to what you see when you press the Alt button in a Windows application.  After a bit of research, I found that it’s perfectly easy to do this and more with your console output.  Here’s what I found:

Whilst changing the colour, setting the console windows title and a few other effects can be done thru the standard Console class; underlining and other effects are not.  Also, to set a new foreground colour using the Console class a separate command must be issued which makes things awkward when you only want to highlight a single word in the middle of a sentence.  Underlining, inline colour changes and more can be achieved by inserting ANSI escape code sequences into your text output.  ANSI codes are a throwback to the Mainframe days of computing but have been carried forward and supported within Linux systems.  Up until 2016 the Microsoft Windows Console did not support the escape codes at all but in a surprise move, Microsoft included it in a major update release.  It’s not on by default so to get the effects, you must tell the application to act as if it’s an old mainframe terminal.  

In the attached sample code, you’ll find a static library that encapsulates the following steps as well as some CONSTs to make using the escape codes easier.  I’m only going to concentrate on some basic operations in this article but there is a lot more you can do using different codes.  A good reference to the ANSI codes can be found here: https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences.

**Step 1 – Turing on the terminal window emulation**

Turing on the terminal emulation is a matter of getting a pointer to the console window, retrieving its current output mode value, adding the value for Terminal emulation to it using an OR Assignment Operator and then setting the new value.  

N.B. The following code has only been built and tested in a Windows Environment and makes calls to import the kernal32.dll library.  Although I have not tested it, I suspect that this following step can be bypassed on Linux, MAC .NET Core apps as those operating systems have always support ANSI escape codes natively.  Perhaps by using a custom compiler directive to indicate the type of system, build errors can be avoided.

You’re going to need to insert a reference for the following three functions from the kernal32.dll library

````csharp
[DllImport("kernel32.dll", SetLastError = true)]  
static extern IntPtr GetStdHandle(int nStdHandle);  
	  
[DllImport("kernel32.dll")]  
static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);  
	  
[DllImport("kernel32.dll")]  
static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);  
````

The DllImport attribute can be found in the System.Runtime.InteropServices namespace.  As you can see in the following four lines of code, the actual procedure is pretty straightforward.  

````csharp
IntPtr _ConsoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);  
````

The first line which calls GetStdHandle returns a pointer to the standard output device which initially gets set to the Console Windows’ buffer.  I need this pointer for the next two function calls, so it gets stored in the _ConsoleHandle variable.  

````csharp
GetConsoleMode(_ConsoleHandle, out uint _ConsoleMode);  
_ConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;  
````

Next, I retrieve the current processing mode for the console window with I store in the variable _ConsoleMode.  In theory I could perform some checking on _ConsoleMode to see if the Terminal Processing mode flag has been set but because I’m adding it using the |= OR Assignment Operator the variable will be unaffected if the flags bits are already present.  If you’re unfamiliar with this operator, |=, I would recommend reading up on them.  

````csharp
SetConsoleMode(_ConsoleHandle, _ConsoleMode);  
````

With the additional flag added, the final step to update the console windows mode is called and now we’re ready to add in the text effects.

**Step 2 – Adding effects to your text output**

As I mentioned, some of the effects can already be achieved via the Console class but it does require multiple commands making the code cumbersome.  Here’s an example of the steps needed if you want to highlight a simple word in a sentence in red:

````csharp
Console.Write("It took 5 commands using the standard Console class just to insert a single ");  
Console.ForegroundColor = ConsoleColor.Red;  
Console.Write("Red");  
Console.ForegroundColor = ConsoleColor.Gray;  
Console.WriteLine(" word in this line.");  
````

As you can see you need five commands just to output a single line.  Using ANSI escape sequences, you can reduce that to a single line.  You can, if you wish, insert the Escape code directly into you text.  The escape code takes the form:

**ESC[<n>m**

where the _<n>_ represents the code related to the effect being called e.g. for underlining, the code is 4 and for the colour red, the code is 31.  ESC needs to be passed as an escape sequence in the string so the actual string you would insert into your code to set the colour red, would be this:

**\x1B[31m**

A _<n>_ value of zero resets the outback back to the default settings so to mark a single word in a sentence as red you could do the following:

````csharp
Console.WriteLine("This is the only \x1B[31mred\x1B[0m word in this sentence.");  
````

To me that’s still a little cumbersome so I’ve created a class with public CONSTs to represent the escape sequences (just the colours and some of the text effects) just to add a little sugar to the command like this:

````csharp
Console.WriteLine("This is the only " + ConsoleEffects.Foreground_Red + "red" + ConsoleEffects.Default + " word in this sentence.");  
````

**Conclusion**

Getting the effects I was looking for was really quite simple once I dug around a little. I did have to use a few lower level function calls of the type that are basic knowledge for a Windows C++ developer and it does make me wonder what else is down on that level waiting to be ported to the C# level.  On the other hand, .Net Core does allow us to develop for multiple operating systems so maybe it’s a level we should avoid for compatibilities sake.  Either way, please feel free to use and change the same code here and to maybe give your console apps a little bit more flair.