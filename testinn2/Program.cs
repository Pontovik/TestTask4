using System;
using System.Collections.Generic;
using System.Linq;

namespace Tinkoff
{
    public class Programm
    {
        public static Dictionary<string, int> values = new Dictionary<string, int>
        {
            ["zero"] = 0
        };

        public static int num = 0;
        public static void Count(string[] lines)
        {
            Dictionary<string, int>[] tempValues = new Dictionary<string, int>[10];
            if (lines == null)
            {
                throw new ArgumentNullException("file is empty");
            }

            if (lines.Length > Math.Pow(10, 5))
            {
                throw new ArgumentOutOfRangeException("too many lines");
            }

            if (lines.Any(n => n.Contains(' ')))
            {
                throw new ArgumentException("there are spaces in file");
            }

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i][0] == '{')
                {
                    tempValues[num] = new Dictionary<string, int>();
                    if (num == 0)
                    {
                        tempValues[num] = values.ToDictionary(x => x.Key,
                            x => x.Value);
                    }
                    else
                    {
                        tempValues[num] = tempValues[num - 1].ToDictionary(x => x.Key,
                            x => x.Value);
                    }
                    num++;
                }
                else if (lines[i][0] == '}')
                {
                    tempValues[num - 1].Clear();
                    num--;
                }
                else if (num == 0)
                {
                    GetNewVal(lines[i]);
                }
                else
                {
                    GetNewVal(lines[i], tempValues[num - 1]);
                }
            }
        }

        public static void GetNewVal(string line, Dictionary<string, int> tempValues)
        {
            string result = string.Empty;
            int i = 0;
            while (line[i] != '=')
            {
                result += line[i];
                i++;
            }
            if (result.Length > 10)
            {
                throw new ArgumentOutOfRangeException("var is too big");
            }
            int temp = 0;
            int.TryParse(line[(i + 1)..], out temp);
            if (temp != 0)
            {
                FindValue(line[..i], temp, tempValues);
            }
            else
            {
                FindValue(line[..i], tempValues[line[(i + 1)..]], tempValues);
                Console.WriteLine(tempValues[line[(i + 1)..]]);
            }
        }

        public static void FindValue(string firstHalf, int toAdd, Dictionary<string, int> tempValues)
        {
            var keys = tempValues.Keys;
            var leftSide = tempValues.Where(n => n.Key == firstHalf).FirstOrDefault().Key;
            if (leftSide != null)
            {
                tempValues[leftSide] = toAdd;
            }
            else
            {
                tempValues.Add(firstHalf, toAdd);
            }
        }

        public static void GetNewVal(string line)
        {
            string result = string.Empty;
            int i = 0;
            while (line[i] != '=')
            {
                result += line[i];
                i++;
            }
            if (result.Length > 10)
            {
                throw new ArgumentOutOfRangeException("var is too big");
            }
            int temp = 0;
            int.TryParse(line[(i + 1)..], out temp);
            if (temp != 0)
            {
                FindValue(line[..i], temp);
            }
            else
            {
                FindValue(line[..i], values[line[(i + 1)..]]);
                Console.WriteLine(values[line[(i + 1)..]]);
            }
        }

        public static void FindValue(string firstHalf, int toAdd)
        {

            var keys = values.Keys;
            var leftSide = values.Where(n => n.Key == firstHalf).FirstOrDefault().Key;
            if (leftSide != null)
            {
                values[leftSide] = toAdd;
            }
            else
            {
                values.Add(firstHalf, toAdd);
            }
        }
        public static void Main()
        {
            var check = new string[]
            {
                "thats=zero",
                "a=10",
                "ten=a",
                "aboba=ten",
                "ten=-10",
                "{",
                "b=a",
                "a=1337",
                "c=a",
                "{",
                "d=a",
                "e=aboba",
                "}",
                "}",
                "lol=a"
            };
            Count(check);
        }
    }
}
