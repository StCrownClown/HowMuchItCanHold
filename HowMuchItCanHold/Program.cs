using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace HowMuchItCanHold
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] rainBlock = new int[] { };
            string input = "";
            bool isValid = false;
            bool again = true;

            while (again)
            {
                ValidateInput(ref input, ref rainBlock, ref isValid);

                CheckHoldRain(ref rainBlock);

                PlayAgain(ref again, ref isValid);
            }
        }

        delegate int[] runNumber(int from, int to);

        static readonly Func<string, int[]> _split = i => i.Split(',').Select(a => Convert.ToInt32(a)).ToArray();

        static readonly Func<string, bool> _trysplit = i => i.Split(',').Any(s => int.TryParse(s, out int number));

        public static void PlayAgain(ref bool again, ref bool isValid)
        {
            Console.WriteLine("Do you want to play again ? (Y/N)");

            string input = Console.ReadLine();

            if (input.ToUpper() == "Y")
            {
                again = true;
                isValid = false;
            }
            else
            {
                again = false;
            }
        }

        public static void CheckHoldRain(ref int[] rainBlock)
        {
            int answer = 0;
            int round = rainBlock.Max();

            while (round > 0)
            {
                RemoveTower(ref rainBlock);

                for (int i = 0; i < rainBlock.Length; i++)
                {
                    if (i != 0 && i < rainBlock.Length - 1)
                    {
                        if (rainBlock[i] == 0 && rainBlock[i] < rainBlock[i - 1] && rainBlock[i] < rainBlock[i + 1])
                        {
                            answer += 1;
                            rainBlock[i] += 1;
                        }
                        else if (rainBlock[i] == 0 && rainBlock[i] < rainBlock[i - 1] && rainBlock[i + 1] >= 0)
                        {
                            int tmpAdd = 0;
                            for (int y = i; y < rainBlock.Length; y++)
                            {
                                if (rainBlock[y] == 0)
                                {
                                    tmpAdd += 1;
                                    rainBlock[y] += 1;
                                }
                                else if (rainBlock[y] > 0)
                                {
                                    answer += tmpAdd;
                                }
                                else if (rainBlock[y] < 0)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                round--;
            }

            Console.WriteLine("Output: " + answer);
        }

        public static void RemoveTower(ref int[] rainBlock)
        {
            for (int x = 0; x < rainBlock.Length; x++)
            {
                rainBlock[x] -= 1;
            }
        }

        public static void ValidateInput(ref string input, ref int[] rainBlock, ref bool isValid)
        {
            while (!isValid)
            {
                Console.WriteLine("########## Validate ##########");
                Console.WriteLine("Count of numbers should more than 3 numbers");
                Console.WriteLine("Value of Numbers must non-negative integers");
                Console.WriteLine("Not alphabets or special character except brackets");
                Console.WriteLine("Split each Number by commas (,)");
                Console.WriteLine("Example input: 1,2,3,4,5 or [1, 2, 3, 4, 5]");
                Console.WriteLine("Please enter rain-block input numbers");

                input = Console.ReadLine();
                input = input.Replace("]", "").Replace("[", "").Trim();

                try
                {
                    ValidateRainInput validate = new ValidateRainInput(input, ref isValid);

                    if (!isValid)
                    {
                        Console.WriteLine("Invalid input");

                        isValid = false;
                        continue;
                    }

                    rainBlock = _split(input);
                }
                catch
                {
                    Console.WriteLine("Invalid input");

                    isValid = false;
                    continue;
                }

                isValid = true;
            }
        }

        public class ValidateRainInput
        {
            public ValidateRainInput(string input, ref bool isValid)
            {
                isValid = Validation(input, isValid);
            }

            public bool Validation(string input, bool isValid)
            {
                int[] checkNumber = _split(input);

                if (Regex.IsMatch(input, @"^[0-9\,\ ]+$"))
                {
                    isValid = true;
                }

                if (_trysplit(input))
                {
                    isValid = true;
                }

                return isValid;
            }
        }
    }
}
