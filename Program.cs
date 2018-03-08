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
        const string operators = "^/%*+-";

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
                for (int i = 0; i < amount; i++)
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
                //input = input.ReplaceFirst("(" + currentThing + ")", result);
                input = input.ReplaceFirst(currentThing, result);
                //we have to check if the brackets are prefixed with a command
                string cmd = null;
                Func<double, double> funcToUse = null;
                int anotherIndex = input.IndexOf("(" + currentThing + ")");
                for (int i = anotherIndex; i >= 0; i--)
                {
                    if (operators.Contains(input[i]) || input[i] == '(' || i == 0)
                    {
                        int h = anotherIndex - i - 1;
                        if (h > 0)
                            cmd = input.Substring(i + 1, h);
                        funcToUse = getCommand(cmd);
                        if (funcToUse != null)
                            break;
                    }
                }
                if (funcToUse != null)
                {
                    double newResult = double.Parse(result);
                    newResult = funcToUse(double.Parse(result));
                    input = input.ReplaceFirst(cmd + "(" + result + ")", newResult.ToString("G99"));
                }
                else
                    input = input.ReplaceFirst("(" + currentThing + ")", result);
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
                    for (int i = 0; i < arr.Count - 1; i++)
                    {
                        if (arr[i].text == operators[j].ToString())
                        {
                            arr[i].text = operations(arr[i].text, arr[i - 1].text, arr[i + 1].text).ToString("G99");
                            arr.Remove(arr[i - 1]);
                            i--;
                            arr.Remove(arr[i + 1]);
                        }
                    }
            return arr[0].text;
        }

        double operations(string operation, string beforevalue, string aftervalue)
        {
            if (operation == "^") return (Math.Pow(double.Parse(beforevalue, CultureInfo.InvariantCulture), double.Parse(aftervalue, CultureInfo.InvariantCulture)));
            if (operation == "*") return (double.Parse(beforevalue, CultureInfo.InvariantCulture) * double.Parse(aftervalue, CultureInfo.InvariantCulture));
            if (operation == "%") return (double.Parse(beforevalue, CultureInfo.InvariantCulture) % double.Parse(aftervalue, CultureInfo.InvariantCulture));
            if (operation == "+") return (double.Parse(beforevalue, CultureInfo.InvariantCulture) + double.Parse(aftervalue, CultureInfo.InvariantCulture));
            if (operation == "/") return (double.Parse(beforevalue, CultureInfo.InvariantCulture) / double.Parse(aftervalue, CultureInfo.InvariantCulture));
            if (operation == "-") return (double.Parse(beforevalue, CultureInfo.InvariantCulture) - double.Parse(aftervalue, CultureInfo.InvariantCulture));
            return 0;
        }

        Func<double, double> getCommand(string name)
        {
            switch (name)
            {
                case "sqrt":
                    return (d) =>
                    {
                        return Math.Sqrt(d);
                    };
                case "sigmoid":
                    return (d) =>
                    {
                        return 1 / (1 + Math.Exp(-d));
                    };
                case "sin":
                    return (d) =>
                    {
                        return Math.Sin(d);
                    };
                case "cos":
                    return (d) =>
                    {
                        return Math.Cos(d);
                    };
                case "tan":
                    return (d) =>
                    {
                        return Math.Tan(d);
                    };
                case "arctan":
                    return (d) =>
                    {
                        return Math.Atan(d);
                    };
                case "asin":
                    return (d) =>
                    {
                        return Math.Asin(d);
                    };
                case "acos":
                    return (d) =>
                    {
                        return Math.Acos(d);
                    };
                case "ln":
                    return (d) =>
                    {
                        return Math.Log(d);
                    };
                case "log":
                    return (d) =>
                    {
                        return Math.Log10(d);
                    };
                case "floor":
                    return (d) =>
                    {
                        return Math.Floor(d);
                    };
                case "round":
                    return (d) =>
                    {
                        return Math.Round(d);
                    };
                case "abs":
                    return (d) =>
                    {
                        return Math.Abs(d);
                    };
                default:
                    return null;
            }
        }
    }
}
