using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ConsoleApp1
{
    class Ledd
    {
        int random = new Random().Next(15);
        public string text = "";

        public Ledd(string text)
        {
            this.text = text;
        }

        public static Ledd Parse(string inp)
        {
            return new Ledd(inp);
        }
    }

    class Program
    {
        string operators = "^/*+-";

        static void Main(string[] args)
        {
            new Program().Run();
        }

        void Run()
        {
            string input;
            
            while ((input = Console.ReadLine()) != "T")
            {
                input = input.Replace(" ", "");
                var amount = 1;
                System.Diagnostics.Stopwatch v = new System.Diagnostics.Stopwatch();
                v.Start();
                for(int i = 0; i < amount; i++)
                    Begin(input);
                v.Stop();
                Console.WriteLine("finished in: " + (v.ElapsedMilliseconds) + "ms, " + (v.Elapsed.TotalSeconds) + " seconds.");
            }
            
        }

        private string Begin(string input)
        {
            input = "(" + input + ")";
            while (input.Contains("("))
            {
                string currentThing = "";
                int last = input.IndexOf(')');
                int g = 0;
                for (int i = last; i >= 0; i--)
                {
                    if (input[i] == '(')
                    {
                        g = i;
                        currentThing = input.Substring(i + 1, last - i - 1);
                        Console.WriteLine("Next step");
                        Console.WriteLine(currentThing);
                        break;
                    }
                }
                string result = calc(currentThing);
                input = input.ReplaceFirst(currentThing, result);
                int not = 0;
                //TODO: This code is bad. I should remember the positions of the current operation I'm dealing with
                //      instead of checking it again. Saves on memory and definitely won't fail!
                int anotherIndex = input.IndexOf("(", 1);
                for (int i = anotherIndex; i >= g; i--)
                {
                    if (operators.Contains(input[i]))
                        not = i;
                }
                string cmd = null;
                if (anotherIndex > 0)
                    cmd = input.Substring(not+1, anotherIndex-not-1);
                if (cmd?.Length > 0)
                {
                    currentThing = cmd + "(" + result + ")";
                    Console.WriteLine("Command " + currentThing);
                    result = getCommand(cmd).Invoke(double.Parse(result)).ToString("G99");
                    input = input.ReplaceFirst($"{currentThing}", result);
                }
                input = input.ReplaceFirst($"({currentThing})", result);
                Console.WriteLine("Now we have:");
                Console.WriteLine(input);
            }
            Console.WriteLine(input);
            return input;
        }

        string calc(string input)
        {
            string newString = "";
            for (int i = 0; i < input.Length; i++)
            {
                bool a = false;
                if (operators.Contains(input[i]) && i > 0 && !operators.Contains(input[i - 1]))
                {
                    a = true;
                    newString += " ";
                }
                newString += input[i];
                newString += a ? " " : "";
            }
            var arr = new List<Ledd>();
            var arr2 = newString.Split(' ');
            for (int i = 0; i < arr2.Length; i++)
            {
                arr.Add(Ledd.Parse(arr2[i]));
            }
            while (arr.Count > 1)
                for (int j = 0; j < operators.Length; j++)
                    for (int i = 0; i < arr.Count-1; i++)
                    {
                        if (arr[i].text == operators[j].ToString())
                        {
                            arr[i].text = operations(arr[i].text, arr[i - 1].text, arr[i + 1].text).ToString("G99");
                            arr.Remove(arr[i - 1]);
                            i--;
                            arr.Remove(arr[i+1]);
                        }
                    }
            return arr[0].text;
        }

        double operations(string operation, string beforevalue, string aftervalue)
        {
            if (operation == "^") return ((double)Math.Pow(double.Parse(beforevalue, CultureInfo.InvariantCulture), double.Parse(aftervalue, CultureInfo.InvariantCulture)));
            if (operation == "*") return (double.Parse(beforevalue, CultureInfo.InvariantCulture) * double.Parse(aftervalue, CultureInfo.InvariantCulture));
            if (operation == "+") return (double.Parse(beforevalue, CultureInfo.InvariantCulture) + double.Parse(aftervalue, CultureInfo.InvariantCulture));
            if (operation == "/") return (double.Parse(beforevalue, CultureInfo.InvariantCulture) / double.Parse(aftervalue, CultureInfo.InvariantCulture));
            if (operation == "-") return (double.Parse(beforevalue, CultureInfo.InvariantCulture) - double.Parse(aftervalue, CultureInfo.InvariantCulture));
            return 0;
        }

        Func<double, double> getCommand(string name)
        {
            switch(name)
            {
                case "sqrt":
                    return (d) =>
                    {
                        return Math.Sqrt(d);
                    };
                default:
                    return null;
            }
        }
    }
}
