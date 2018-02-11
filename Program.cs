using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Begin(input);
            }
        }

        private string Begin(string input)
        {
            input = "(" + input + ")";
            while (input.Contains("("))
            {
                string currentThing = "";
                int last = input.IndexOf(')');
                for (int i = last; i >= 0; i--)
                {
                    if (input[i] == '(')
                    {
                        currentThing = input.Substring(i + 1, last - i - 1);
                        Console.WriteLine("Next step");
                        Console.WriteLine(currentThing);
                        break;
                    }
                }
                string result = calc(currentThing);
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
                    for (int i = 0; i < arr.Count-1; i++)
                    {
                        if (arr[i].text == operators[j].ToString())
                        {
                            arr[i].text = operations(arr[i].text, arr[i - 1].text, arr[i + 1].text).ToString();
                            arr.Remove(arr[i - 1]);
                            i--;
                            arr.Remove(arr[i+1]);
                        }
                    }
            return arr[0].text;
        }

        float operations(string operation, string beforevalue, string aftervalue)
        {
            if (operation == "/")
                return (float.Parse(beforevalue) / float.Parse(aftervalue));
            if (operation == "^")
                return (float)(Math.Pow(double.Parse(beforevalue), double.Parse(aftervalue)));
            if (operation == "*")
                return (float.Parse(beforevalue) * float.Parse(aftervalue));
            if (operation == "+")
                return (float.Parse(beforevalue) + float.Parse(aftervalue));
            if (operation == "-")
                return (float.Parse(beforevalue) - float.Parse(aftervalue));
            return 0;
        }
    }
}
