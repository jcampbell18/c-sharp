/*
 * Author: Jason Campbell
 * Assignment 0: Armstrong Numbers
 * 
 * Find all the Armstrong numbers from 10 through an integer entered by the user
 * that is greater than or equal to 10, inclusive. Finally, prints each of the numbers
 * and the total number of Armstrong numbers found.
 * 
 * Complete documentation found below
*/

using System;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace campbelljcscd371hw0
{
    /// <summary>
    ///     Class <c>Armstrong</c> finds all the Armstrong numbers from 10 through
    ///     an integer entered by the user that is greater than or equal to 10, inclusive.
    ///     Finally prints each of the numbers and the total number of Armstrong numbers found.
    /// </summary>
    class Armstrong
    {
        static void Main(string[] args)
        {
            int counter = 0;
            int input = UserInput();

            Console.WriteLine("ARMSTRONG NUMBERS FOUND FROM 10 THROUGH " + input + "\n");

            for (int num = 10; num <= input; num++)
            {
                if (SumOfDigits(num) == num)
                {
                    counter++;
                    Console.WriteLine(num);
                }

            }

            Console.WriteLine("\nTOTAL NUMBER OF ARMSTRONG NUMBERS FOUND WAS " + counter);

        }

        // Prompts the user for a number
        /// <summary>
        ///     This method that prompts the user for a number
        ///     The user will keep getting prompted until a valid number is entered.
        /// </summary>
        /// <remarks>
        ///     Tryparse: converts the string representation of a number to its 32-bit signed integer equivalent
        ///     <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.int32.tryparse?view=netcore-3.1"/>
        /// </remarks>
        /// <returns>the entered integer</returns>
        private static int UserInput()
        {
            int input;

            do
            {

                Console.WriteLine("Please input an integer whose value is 10 or greater: ");
                Int32.TryParse(Console.ReadLine(), out input);

            } while (input < 10);

            return input;
        }

        // Finds the sum of each digit of the number to the power of the number of digits
        /// <summary>
        ///     This method that finds the number of digits in the passed
        ///     number, and finds the sum of each digit of the number to the
        ///     power of the number of digits
        /// <example>
        ///     For example: 
        /// <code>
        ///     int sum = sumOfDigits(1234);
        /// </code>
        ///     results in:
        ///         n=4 (number of digits)
        ///         go through each digit of the number from right to left
        ///             4^3 + 3^3 + 2^3 + 1^3 = 64 + 27 + 8 + 1 = 100
        ///     returning 100
        /// <code>
        ///     int sum = sumOfDigits(371);
        /// </code>
        ///     results in:
        ///         n=3 (number of digits)
        ///         go through each digit of the number from right to left
        ///             1^3 + 7^3 + 3^3 = 1 + 343 + 27 = 371
        ///         returning 371
        /// </example>
        /// </summary>
        /// <param name="num">the current number in the for loop</param>
        /// <returns>the total sum</returns>
        private static int SumOfDigits(int num)
        {
            int numCopy = num;
            int nDigits = (int)(Math.Log10(num) + 1);
            int sum = 0;

            while (numCopy > 0)
            {
                sum += (int)Math.Pow((int)numCopy % 10, nDigits);
                numCopy /= 10;
            }

            return sum;
        }
    }
}
