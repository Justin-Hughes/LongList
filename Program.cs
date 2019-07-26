using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;


namespace Tracker
{

    class Program
    {
        static void Main (string [] args)
        {
            string filePath = $"{Directory.GetCurrentDirectory ()}\\LongListSave.txt";

            bool fileCheck = File.Exists ("LongListSave.txt") ? true : false;

            List<string> Page = new List<string> ();


            if (fileCheck == false)
            {
                File.Create ("LongListSave.txt");
            }


            LoadList ();


            bool stillWorking = true;
            bool pageCheck;
            bool escape;

            char verify = 'n';

            int displayLength;
            int pageCount = Page.Count / 20;
            int pageStart;
            int pageIndex = 0;
            int currentSelection = 0;
            if (Page.Count % 20 != 0)
            {
                ++pageCount;
            }

            if (pageCount > 1)
            {
                Console.WriteLine ("There are currently {0} pages.", pageCount); 
                Console.WriteLine ("What page would you like to start on?");
                do
                {
                    pageStart = Convert.ToInt32 (Console.ReadLine ());
                    pageCheck = true;
                    if (pageStart > pageCount)
                    {
                        Console.WriteLine ("There are only {0} pages.", pageCount);
                        pageCheck = false;
                    }
                    else if (pageStart <= 0)
                    {
                        Console.WriteLine ("Please select a page between 1 and {0}", pageCount);
                        pageCheck = false;
                    }
                } while (pageCheck == false);
                if (pageStart != 1)
                {
                    pageIndex = ((pageStart - 1) * 20);
                    if ((pageIndex + 20) >= Page.Count)
                    {
                        pageIndex = Page.Count - (Page.Count - pageIndex);
                    }
                }
                else
                {
                    pageIndex = 0;
                }
            }


            while (stillWorking)
            {
                int currentListIndex = pageIndex + currentSelection;// assigns the current Page[] index value to currentListIndex

                PrintPage (Page); //Having this here auto refreshes the page

                Console.SetCursorPosition (Console.CursorLeft, (Console.CursorTop + currentSelection));// sets cursor to current selection

                ConsoleKey input = Console.ReadKey ().Key; // receives the button input

                if (input == ConsoleKey.F1) // add new items to list
                {
                    Console.SetCursorPosition (Console.CursorLeft, 26);
                    Console.WriteLine ("What would you like to add?");
                    string inputPage;
                    bool inputCheck;
                    do
                    {
                        inputPage = Console.ReadLine ();
                        inputCheck = inputPage.StartsWith ("* /");
                        if (inputCheck == true)
                        {
                            Console.WriteLine ("Your item cannot begin with '* /' ");
                        }
                    } while (inputCheck == true);

                    Page.Add (inputPage);
                    Console.CursorTop = (6 + currentSelection);

                }


                else if (input == ConsoleKey.F2) // Completes the current item
                {

                    Console.SetCursorPosition (Console.CursorLeft, 26);
                    do
                    {
                        try
                        {
                            Console.WriteLine ("Complete the current item? (y/n)");
                            Console.WriteLine ();
                            verify = Convert.ToChar (Console.ReadLine ());
                            escape = true;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine ("Please enter a y or an n.");
                            escape = false;
                        }

                    } while (escape != true);


                    if (verify == 'y')
                    {
                        string listString = Page [currentListIndex];
                        Page [currentListIndex] = $"* /{listString}/ *";
                        currentSelection++;
                    }
                }

                else if (input == ConsoleKey.F3) // Re-enters the current item
                {
                    Console.SetCursorPosition (Console.CursorLeft, 26);
                    do
                    {
                        try
                        {
                            Console.WriteLine ("Re-Enter the current item? (y/n)");
                            Console.WriteLine ();
                            verify = Convert.ToChar (Console.ReadLine ());
                            escape = true;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine ("Please enter a y or an n.");
                            escape = false;
                        }

                    } while (escape != true);

                    if (verify == 'y')
                    {
                        string listString = Page [currentListIndex];
                        Page [currentListIndex] = $"* /{listString}/ *";
                        currentSelection++;
                        Page.Add (listString);
                    }
                }




                else if (input == ConsoleKey.F4) // Previous Page 
                {
                    if (pageIndex == 0)
                    {
                        Console.SetCursorPosition (Console.CursorLeft, 26);
                        Console.WriteLine ("No Previous Page.");
                        Console.WriteLine ("Press Any Key To Acknowledge.");
                        Console.ReadKey ();

                    }
                    else
                    {
                        currentSelection = 0;
                        if ((pageIndex - 20) < 0) // prevents oob exception
                        { pageIndex = 0; }
                        else
                        {
                            pageIndex -= 20;
                        }
                    }
                }

                else if (input == ConsoleKey.F5) // Next Page
                {

                    if (pageIndex + 20 >= Page.Count)
                    {
                        Console.SetCursorPosition (Console.CursorLeft, 26);
                        Console.WriteLine ("You Have Reached The Last Page.");
                        Console.WriteLine ("Press Any Key To Acknowledge.");
                        Console.ReadKey ();
                    }
                    else
                    {
                        currentSelection = 0;
                        if ((pageIndex + 20) > (Page.Count))
                        { pageIndex += ((pageIndex + 20) % (Page.Count - 1)); }
                        else
                        {
                            pageIndex += 20;
                        }
                        PrintPage (Page);
                    }
                }

                else if (input == ConsoleKey.F6) // Refreshes the page and resets selection to top
                {
                    currentSelection = 0;
                }

                else if (input == ConsoleKey.F7) // Save file
                {
                    SaveList ();
                }

                else if (input == ConsoleKey.F12) // Exits
                {
                    Console.SetCursorPosition (Console.CursorLeft, 26);
                    do
                    {
                        try
                        {
                            Console.WriteLine ();
                            Console.WriteLine ("Would you like to exit? (y/n)");
                            verify = Convert.ToChar (Console.ReadLine ());
                            escape = true;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine ("Please enter a y or an n.");
                            escape = false;
                        }

                    } while (escape != true);

                    if (verify == 'y')
                    {
                        SaveList ();
                        stillWorking = false;
                    }
                }

                else if (input == ConsoleKey.DownArrow)
                {

                    if (currentSelection > 19)
                    {
                        currentSelection = 0;
                    }
                    else if (currentSelection + 1 >= Page.Count)
                    { }
                    else
                    {
                        ++currentSelection;
                        while (Page [currentSelection].StartsWith ("* /"))
                        {
                            ++currentSelection;
                        }
                    }
                }
            }


            void PrintPage (List<string> List)
            {
                UserMenu ();
                bool removed = false;
                while (Page [0].StartsWith ("* /"))
                {
                    Page.Remove (Page [0]);
                    removed = true;
                }
                if (removed)
                {
                    currentSelection--;
                }
                if (List.Count >= 20)
                {
                    displayLength = 20;
                }
                else
                {
                    displayLength = List.Count;
                }
                if (displayLength + pageIndex >= Page.Count)
                {
                    displayLength = Page.Count - pageIndex;
                }
                List<string> display = List.GetRange (pageIndex, displayLength);
                for (int i = 0; i < displayLength; i++)
                {
                    if (i == currentSelection)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    else if (Page [i + pageIndex].StartsWith ("* /"))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }

                    string elem = display [i];
                    Console.WriteLine (elem.ToString ());

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.SetCursorPosition (Console.CursorLeft, 6);
            }


            void UserMenu ()
            {
                string menu = ("|F1 - ADD|F2 - COMPLETE|F3 - RE-ENTER|F4 - PREVIOUS PAGE|F5 - NEXT PAGE|F6 - REFRESH|F7 - SAVE|F12 - EXIT|");
                string menu2 = "|CYCLE THROUGH YOUR LIST WITH THE DOWN ARROW.|";
                Console.Clear ();
                Console.SetCursorPosition (((Console.WindowWidth - menu.Length) / 2), Console.CursorTop);
                Console.WriteLine(menu);
                Console.CursorLeft = (Console.WindowWidth - menu2.Length) / 2;
                Console.WriteLine (menu2);
                Console.SetCursorPosition (Console.CursorLeft, 6);
            }
            void SaveList ()
            {
                using (StreamWriter save = new StreamWriter (filePath))
                {
                    for (int i = 0; i < Page.Count; i++)
                        save.WriteLine (Page [i]);
                }
            }

            void LoadList ()
            {
                using (StreamReader load = new StreamReader (filePath))
                {
                    string line;
                    while ((line = load.ReadLine ()) != null)
                    {
                        Page.Add (line); 
                    }
                }
            }
        }
    }
}
