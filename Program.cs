using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
namespace ConsoleApp1
{

    public class Hmac
    {
        private const int KeySize = 32;
        public static byte[] GenerateRandomKey()
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[KeySize];
                randomNumberGenerator.GetBytes((randomNumber));

                return randomNumber;
            }
        }
        public static byte[] ComputeHmacsha256(byte[] toBeHashed, byte[] key)
        {
            using (var hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(toBeHashed);
            }
        }
    }


class Program
    {
        public static byte[] GenerateHMAC(String text, byte[] keyBytes)
        {

            var encoding = new ASCIIEncoding();

            var textBytes = encoding.GetBytes(text);

            Byte[] hashBytes;

            using (var hash = new HMACSHA256(keyBytes))

                hashBytes = hash.ComputeHash(textBytes);

            return hashBytes;

        }
        static void Main(string[] args)
            {


            if (args.Length==0)
            {
                Console.WriteLine("The number of arguments shouldn't be equal to zero");
                return;
            }
            if (args.Length < 3)
            {
                Console.WriteLine("The number of arguments should be >=3");
                return;
            }
            if (args.Length%2==0)
            {
                Console.WriteLine("The number of arguments must be odd");
                return;
            }

            var a = Hmac.GenerateRandomKey();
           // computerMove = choice
           var generator = new RandomGenerator();
            var computerMove = generator.RandomNumber(0, args.Length);
            var valueHmac = GenerateHMAC(args[computerMove], a);
            Console.WriteLine("HMAC:  " + Convert.ToBase64String(valueHmac));
            int i = 0;
            for(i=0; i<args.Length; i++)
            {
                Console.WriteLine(i + " - " + args[i]);
            }
            Console.WriteLine(i + " - " + "Exit");
            Console.Write("Enter you'r move: ");
            int userMove = Convert.ToInt32(Console.ReadLine());
            while (userMove > i || userMove < 0)
            {
                Console.Write("You'r move is incorrect. Try again: ");
                userMove = Convert.ToInt32(Console.ReadLine());
            }
            Console.WriteLine("You'r move: " + args[userMove]);
            Console.WriteLine("Computer's move: " + args[computerMove]);
            int winNumbsLength = (args.Length / 2);
            if (userMove == i) return;
            int[] winNumbs = new int[winNumbsLength];
            int u = userMove, g = 0;
            for (int j =0 ; j < winNumbs.Length; j++)
            {
                u++;
                if (u == args.Length) u = 0;
                winNumbs[g] = u;
                g++;
            }
            if (userMove == computerMove)
            {
                Console.WriteLine("It's draw");
                Console.WriteLine("HMAC key:  " + Convert.ToBase64String(a));
                return;
            }
            for (int j = 0; j < winNumbs.Length; j++)
            {
                if(winNumbs[j]==computerMove)
                {
                    Console.WriteLine("You win");
                    Console.WriteLine("HMAC key:  " + Convert.ToBase64String(a));
                    return;
                }

            }
            Console.WriteLine("You lose");
            Console.WriteLine("HMAC key:  " + Convert.ToBase64String(a));
            return;
        }
    }
    public class RandomGenerator
    {
        // Instantiate random number generator.  
        // It is better to keep a single Random instance 
        // and keep using Next on the same instance.  
        private readonly Random _random = new Random();

        // Generates a random number within a range.      
        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}

