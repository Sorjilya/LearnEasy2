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

        private static bool IsRandomArrayValid(int[] ar)
		{
            if (ar == null)
                return false;
            if (ar.Length < 2)
                return true;
            for(int i = 0; i < ar.Length-1; i++)
			{
				if (ar[i] != ar[i+1] + 1)
				{
                    return true;
				}
			}
            return false;
		}
        public static int[] GenerateValidRandomIntVector(int length, int minValue, int maxValue)
        {
            int[] res = GenerateRandomIntVector(length, minValue, maxValue);
			if (IsRandomArrayValid(res))
			{
                return res;
			}
			else
			{
                return GenerateValidRandomIntVector(length, minValue, maxValue);
            }
        }
        public static bool AreIntArraysCyclicalEqual(int[] ar1, int[] ar2)
		{
            if (ar1.Length != ar2.Length)
			{
                return false;
			}
            for(int i = 0; i < ar1.Length; i++)
			{
                bool tempAns = true;
                for(int j = 0; j < ar2.Length; j++)
				{
                    if (ar1[(i+j)% ar1.Length] != ar2[j])
                    {
                        tempAns = false;
                    }
                }
				if (tempAns)
				{
                    return true;
				}
			}
            return false;
		}
        public static int[] GenerateValidRandomDifferentIntVector(int length, int minValue, int maxValue, List<int[]> prevRand, string word)
        {
            int[] res = GenerateValidRandomIntVector(length, minValue, maxValue);
            bool isDif = true;
            for(int i = 0; i < prevRand.Count; i++)
			{
				if (AreIntArraysCyclicalEqual(prevRand[i], res))
				{
                    isDif = false;
				}
			}
            /*for (int i = 0; i < res.Length - 1; i++)
            {
                if (word[i] == word[i + 1])
                {
                    isDif = false;
                }
            }*/
            if (isDif)
            {
                return res;
            }
			else
			{
                return GenerateValidRandomDifferentIntVector(length, minValue, maxValue, prevRand, word);
            }
        }
        public static int Factorial(int f)
        {
            if (f == 0)
                return 1;
            else
                return f * Factorial(f - 1);
        }
        public static string[] GenerateWordVariations(string word, int count)
		{
            int maxCount = Factorial(word.Length) / Factorial(word.Length - word.Length/2)/(word.Length / 2);
            if (count > maxCount)
			{
                count = maxCount;
            }
            string[] words = new string[count];
            List<int[]> prevRand = new List<int[]>();
            for (int i = 0; i < count; i++)
            {
                int[] randNum = GenerateValidRandomDifferentIntVector(word.Length / 2, 0, word.Length, prevRand, word);
                prevRand.Add(randNum);
                char[] tmpWordChars = word.ToLower().ToCharArray();
                for (int j = 0; j < randNum.Length-1; j++)
                {
                    char temp = tmpWordChars[randNum[j]];
                    tmpWordChars[randNum[j]] = tmpWordChars[randNum[j + 1]];
                    tmpWordChars[randNum[j + 1]] = temp;
                }
                tmpWordChars[0] = Char.ToUpper(tmpWordChars[0]);
                words[i] = new string(tmpWordChars);
            }
            return words;
            // TODO: исправить повторяющиеся буквы(+макс кол-во)
		}
    }
}
