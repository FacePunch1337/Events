using System;

namespace Events
{
    delegate void EventListener(string str);

    class EditModule
    {

        public event EventListener NewChar;
        public void Type()
        {

            String str = String.Empty;
            ConsoleKeyInfo keyPressed;
            do
            {
                keyPressed = Console.ReadKey();

                //NewChar.Invoke(str);

                NewChar(str);
                if (keyPressed.Key == ConsoleKey.Enter)
                {
                    Program.stringCountModule.StrCount();
                    Console.Write((char)10);
                    str += '\n';
                }
                else if (keyPressed.Key == ConsoleKey.F2)
                {

                    Program.saveModule.Save();
                    continue;

                }
                else if (keyPressed.Key == ConsoleKey.Spacebar)
                {

                    Program.stringCountModule.WordCount();
                    continue;

                }
                else
                {

                    str += keyPressed.KeyChar;
                    NewChar?.Invoke(str);


                }

                //if(NewChar != null) NewChar
            } while (keyPressed.Key != ConsoleKey.Escape);
        }
    }
    class ChangeModule
    {
        public int detected;
        public void OnNewChar(string str)
        {

            PrintSymbol('*');
        }
        public EventListener OnSaved = str =>
        {
            if (str.Equals("OK")) PrintSymbol('v');
            else PrintSymbol('x');
        };

        private static void PrintSymbol(char symbol)
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.CursorLeft = 30;
            Console.CursorTop = 0;
            Console.Write($"[{symbol}]");
            Console.CursorLeft = left;
            Console.CursorTop = top;
        }



    }
    class LengthModule
    {
        public EventListener OnNewChar = str =>
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.CursorLeft = 40;
            Console.CursorTop = 0;
            Console.Write($"Length [{str.Length}]");
            Console.CursorLeft = left;
            Console.CursorTop = top;
        };
    }

    class SaveModule
    {
        private static int save;
        public event EventListener SaveText;

        public void Save()
        {

            save++;
            SaveText?.Invoke("OK");
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.CursorLeft = 80;
            Console.CursorTop = 0;
            Console.Write($"Save [{save}]");
            Console.CursorLeft = left;
            Console.CursorTop = top;
        }
    }

    class SavedTimeModule
    {
        public EventListener OnSaved = str =>
        {


            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.CursorLeft = 60;
            Console.CursorTop = 0;
            Console.Write($"Time [{DateTime.Now.ToShortTimeString()}]");
            Console.CursorLeft = left;
            Console.CursorTop = top;



        };
    }

    class CatchWordModule
    {
        private String[] words = new string[] { "nigger", "bomb", "hello" };
        private int counter;
        public void OnNewChar(String str)
        {
            str = str.ToLower();
            counter = 0;
            int index = -1;
            foreach (String word in words)
            {
                do
                {
                    index = str.IndexOf(word, index + 1);
                    if (index != -1)
                    {
                        counter++;
                    }
                } while (index >= 0);
            }
            if (counter > 0)
            {
                int left = Console.CursorLeft;
                int top = Console.CursorTop;
                Console.CursorLeft = 100;
                Console.CursorTop = 0;
                Console.Write($"Catch word: {counter}");
                Console.CursorLeft = left;
                Console.CursorTop = top;
            }
        }

    }

    class StringCountModule
    {

        private static int str_counter = 1;
        private static int word_counter = 1;
        
        public ConsoleKeyInfo keyInfo;

        public void StrCount()
        {
            str_counter++;
        }

        public void WordCount()
        {

             word_counter++;
                
        }
        public EventListener StringCount = str =>
        {
            
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.CursorLeft = 40;
            Console.CursorTop = 2;
            Console.Write($"String/Word[{str_counter} | {word_counter}]");
            Console.CursorLeft = left;
            Console.CursorTop = top;




        };


    }



    class Program
    {
        public static SaveModule saveModule;
        public static StringCountModule stringCountModule;
        static void Main(string[] args)
        {
            
            
            var editModule = new EditModule();
            var changeModule = new ChangeModule();
            var lengthModule = new LengthModule();
            var savedTimeModule = new SavedTimeModule();
            var catchWordModule = new CatchWordModule();
            saveModule = new SaveModule();
            stringCountModule = new StringCountModule();

            editModule.NewChar += changeModule.OnNewChar;
            editModule.NewChar += lengthModule.OnNewChar;
            editModule.NewChar += catchWordModule.OnNewChar;
            editModule.NewChar += stringCountModule.StringCount;
            
            
            saveModule.SaveText += changeModule.OnSaved;
            saveModule.SaveText += savedTimeModule.OnSaved;

           



            editModule.Type();
           
        }
    }
}
