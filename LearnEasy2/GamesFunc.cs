using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEasy2
{
	class GamesFunc
	{
        public static int[] GenerateRandomIntVector(int length, int minValue, int maxValue)
        {
            if (length > (maxValue - minValue))
                throw new ArgumentException("Not enough unique numbers in range.");

            HashSet<int> usedNumbers = new HashSet<int>();
            int[] result = new int[length];
            byte[] randomBytes = new byte[4];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                for (int i = 0; i < length; i++)
                {
                    int randomNumber;
                    do
                    {
                        rng.GetBytes(randomBytes);
                        uint randomUInt32 = BitConverter.ToUInt32(randomBytes, 0);
                        randomNumber = minValue + (int)(randomUInt32 % (uint)(maxValue - minValue));
                    } while (usedNumbers.Contains(randomNumber));

                    usedNumbers.Add(randomNumber);
                    result[i] = randomNumber;
                }
            }

            return result;
        }
    }
}
