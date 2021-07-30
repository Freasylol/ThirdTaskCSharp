using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

class Program
{
    static string GetHMACKey (int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        string str = "";
        RandomNumberGenerator provider = RandomNumberGenerator.Create();
        while (str.Length != length)
        {
            byte[] oneByte = new byte[1];
            provider.GetBytes(oneByte);
            char character = (char)oneByte[0];
            if (valid.Contains(character))
            {
                str += character;
            }
        }
        return str;
    }

    static bool IsArgsEmpty (string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Error! Args not were entered. Example: rock paper scissors");
            return true;
        }
        return false;
    }

    static bool IsArgsNotOdd (string[] args)
    {
        if (args.Length % 2 != 1)
        {
            Console.WriteLine("Error! You need to enter odd number of args. Example: rock paper scissors");
            return true;
        }
        return false;
    }

    static bool IsArgsRepeat (string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            for (int j = 0; j < args.Length; j++) { 
                if ((args[i] == args[j]) && (i != j))
                {
                    Console.WriteLine("Error! Args must not be repeated. Example: rock paper scissors");
                    return true;
                }
            }
        }
        return false;
    }

    static bool IsOneArg (string[] args)
    {
        if (args.Length == 1)
        {
            Console.WriteLine("Error! You entered only one arg. Example: rock paper scissors");
            return true;
        }
        return false;
    }

    static bool CheckArgs (string[] args)
    {
        bool isArgsRepeat = IsArgsRepeat(args);
        bool isEmpty = IsArgsEmpty(args);
        bool isNotOdd = IsArgsNotOdd(args);
        bool isOneArg = IsOneArg(args);

        return isEmpty || isNotOdd || isArgsRepeat || isOneArg ? true : false;      
    }

    static void HMACGeneration (int computerMove, string HMACKey)
    {
        Encoding ascii = Encoding.ASCII;

        HMACSHA256 hmac = new HMACSHA256(ascii.GetBytes(HMACKey));

        string HMACStr = Convert.ToBase64String(hmac.ComputeHash(ascii.GetBytes(computerMove.ToString())));

        Console.WriteLine("HMAC: " + HMACStr);
    }

    static void Main(string[] args)
    {
        bool isWrongArgs = CheckArgs(args);

        if (isWrongArgs)
        {
            return;
        }
        else
        {
            Random rand = new Random();

            int computerMove = rand.Next(1, args.Length); ;

            string HMACKey = GetHMACKey(32);

            HMACGeneration(computerMove, HMACKey);

            double superiority = Math.Truncate((double)args.Length / 2);

            Console.WriteLine("Available moves: ");

            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine((i + 1) + " - " + args[i]);
            }

            Console.WriteLine("0 - exit");

            Console.WriteLine("Enter your move: ");

            int userMove = Convert.ToInt32(Console.ReadLine()) - 1;

            if (userMove == 0)
            {
                return;
            }

            Console.WriteLine("Your move: " + args[userMove]);

            Console.WriteLine("Computer move: " + args[computerMove]);

            if (userMove == computerMove)
            {
                Console.WriteLine("Round draw");
            } 
            else if (userMove <= computerMove + superiority)
            {
                Console.WriteLine("You win!");
            }
            else if ((userMove + superiority >= args.Length) && (computerMove > superiority))
            {
                Console.WriteLine("You win");
            }
            else
            {
                Console.WriteLine("Computer wins");
            }

            Console.WriteLine("HMAC key: " + HMACKey);

            Console.ReadKey();
        }
    }
}
