// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace sameHashCodeStrings
{

    public static class Program
    {
         static void Main(string[] args)
        {
            string a = "AB";
            Console.WriteLine("{0} {1} {2}", a.GetHashCode(),"B".GetHashCode(),"C".GetHashCode());
            Console.ReadLine();

            /* String Hashcode Formula in C# :
             * 
             * fixed (char* ptr = this)
            {
                int num = 352654597;
                int num2 = num;
                int* ptr2 = (int*)ptr;
                int num3;
                for (num3 = Length; num3 > 2; num3 -= 4)
                {
                    num = ((num << 5) + num + (num >> 27)) ^ *ptr2;
                    num2 = ((num2 << 5) + num2 + (num2 >> 27)) ^ ptr2[1];
                    ptr2 += 2;
                }

                if (num3 > 0)
                {
                    num = ((num << 5) + num + (num >> 27)) ^ *ptr2;
                }

                return num + num2 * 1566083941;
            }
            */


        }

        async static Task<bool> TestKata(string string1, string string2, string string3)
        {
            bool hashCodeEqual = string1.GetHashCode() == string2.GetHashCode() && string2.GetHashCode() == string3.GetHashCode();
            bool stringNotEqual = !String.Equals(string1, string2) && !String.Equals(string1, string3) && !String.Equals(string2, string3);
            return hashCodeEqual && stringNotEqual;
        }

        //public async static Task<string> GetRandomString(int length)
        //{
        //    throw new NotImplementedException();
        //}
        //public async static Task<int> GetSameHashDifferentString(string s)
        //{
        //    throw new NotImplementedException();
        //}
    }

    public class RandomStringGenerator 
    {
        internal static readonly char[] _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        ////public static async Task<string> GetRandomString(int length = 2) 
        ////{
        ////    char[] result = new char[length];
        ////    var partitioner = Partitioner.Create(0,length);
        ////    Parallel.ForEach(partitioner, (range, loopstate) =>
        ////    {
        ////        for (int i = range.Item1; i < range.Item2; i++)
        ////        {
        ////            result[i] = _chars[RandomNumberGenerator.GetInt32(_chars.Length - 1)];
        ////        }
        ////    });
        ////    return new String(result);
           
        ////}

     
    }

    public class SameHashStringGenerator
    {
    }

}

