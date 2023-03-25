/// <summary>
/// A BrainFuck interpreter writen out of boredom and stupidity.
/// </summary>
namespace BrainFuck
{
    struct StackValuePointer
    {
        public byte Value;
        public int Index;
    }

    class Program
    {
        public static int StackPointer = 0;
        public static int BracketCount = 0;

        public static List<StackValuePointer> stack = new List<StackValuePointer>();

        static void Main(string[] args)
        {
            // Get file path
            string? file = null;
            string code;

            if (args.Length == 0)
            {
                //No file path was specified, so we'll ask for one
                while (file == null || file == "")
                {
                    Console.Write("Enter file path (or \"hw\" for example HelloWorld program): ");

                    file = Console.ReadLine();
                }
            }
            else
            {
                //File path was specified, so we'll use that
                file = args[0];
            }

            if (file != "hw")
            {
                //Try to read the file, and if it fails, print an error message and exit
                try
                {
                    code = System.IO.File.ReadAllText(file);
                }
                catch (Exception e)
                {
                    //Red for dramatic effect
                    ConsoleColor oldColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ReadKey();

                    //Reset color because we're nice people :)
                    //Right? Right!? BE CONSIDERATE GODDAMNIT!
                    Console.ForegroundColor = oldColor;
                    return;
                }
            }
            //If the user entered "hw", we'll use a HelloWorld program as the code
            else
            {
                code =
                    "++++++++[>++++[>++>+++>+++>+<<<<-]>+>+>->>+[<]<-]>>.>---.+++++++..+++.>>.<-.<.+++.------.--------.>>+.>++.";
            }

            // Run code
            for (int i = 0; i < code.Length; i++)
            {
                // Syntax check
                switch (code[i])
                {
                    case '>':
                        // Increment stack pointer
                        StackPointer++;
                        break;
                    case '<':
                        // Decrement stack pointer
                        StackPointer--;
                        break;
                    case '+':
                        // Increment value at current stack position
                        IncrementDecrementStackValue(StackPointer, true);
                        break;
                    case '-':
                        // Decrement value at current stack position
                        IncrementDecrementStackValue(StackPointer, false);
                        break;
                    case '.':
                        // Print value at current stack position
                        Console.Write(Convert.ToChar(FindStackValuePointer(StackPointer).Value));
                        break;
                    case ',':
                        // Set value at current stack position to CLI input
                        ModifyStackValue(StackPointer, Convert.ToByte(Console.ReadKey().KeyChar));
                        break;
                    case '[':
                        // If the value at the current pointer is 0, skip to the next ]
                        if (FindStackValuePointer(StackPointer).Value == 0)
                        {
                            BracketCount++;
                            while (BracketCount > 0)
                            {
                                i++;
                                if (code[i] == '[')
                                    BracketCount++;
                                else if (code[i] == ']')
                                    BracketCount--;
                            }
                        }
                        break;
                    case ']':
                        // If the value at the current pointer is not 0, skip back to the previous [
                        if (FindStackValuePointer(StackPointer).Value != 0)
                        {
                            BracketCount++;
                            while (BracketCount > 0)
                            {
                                i--;
                                if (code[i] == ']')
                                    BracketCount++;
                                else if (code[i] == '[')
                                    BracketCount--;
                            }
                        }
                        break;
                }
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Finds the stack value pointer at the specified index, or creates a new one if it doesn't exist.
        /// </summary>
        public static StackValuePointer FindStackValuePointer(int index)
        {
            // If the stack value pointer doesn't exist, create a new one
            if (stack.FindIndex(x => x.Index == index) == -1)
            {
                StackValuePointer newPointer = new StackValuePointer { Value = 0, Index = index };
                stack.Add(newPointer);

                return newPointer;
            }

            // Otherwise, return the existing one
            return stack[stack.FindIndex(x => x.Index == index)];
        }

        /// <summary>
        /// Modifies the value at the specified index.
        /// </summary>
        public static byte ModifyStackValue(int index, byte value)
        {
            //We have to do this because we can't directly modify the value of a struct inside of a list
            StackValuePointer newPointer = FindStackValuePointer(index);
            newPointer.Value = value;

            stack[stack.FindIndex(x => x.Index == index)] = newPointer;

            return value;
        }

        /// <summary>
        /// Increments or decrements the value at the specified index.
        /// </summary>
        public static void IncrementDecrementStackValue(int index, bool increment)
        {
            //We have to do this because we can't directly modify the value of a struct inside of a list
            //I chose not to use ModifyStackValue to avoid an extra call to FindStackValuePointer
            StackValuePointer newPointer = FindStackValuePointer(index);
            newPointer.Value += (byte)(increment ? 1 : -1);

            stack[stack.FindIndex(x => x.Index == index)] = newPointer;
        }
    }
}
