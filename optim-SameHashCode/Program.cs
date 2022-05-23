// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace sameHashCodeStrings
{

    public static class Program
    {
        async static Task Main()
        {

            /* String Hashcode Formula : s[0]*31^(n-1) + s[1]*31^(n-2) + ... + s[n-1] // where s[n] == ASCII code 
            
            ReverseEngineering + maths to guess?
            
            //Get a first random string

            //Guess a second string that has same hash but !Equal

            //Guess a third string that has same hash but !Equal

            */


        }

        async static Task<bool> TestKata(string string1, string string2, string string3)
        {
            bool hashCodeEqual = string1.GetHashCode() == string2.GetHashCode() && string2.GetHashCode() == string3.GetHashCode();
            bool stringNotEqual = !String.Equals(string1, string2) && !String.Equals(string1, string3) && !String.Equals(string2, string3);
            return hashCodeEqual && stringNotEqual;
        }

        public async static Task<string> GetRandomString(int length)
        {
            throw new NotImplementedException();
        }
        public async static Task<int> GetSameHashDifferentString(string s)
        {
            throw new NotImplementedException();
        }
    }

    public class RandomStringGenerator 
    {
        internal static readonly char[] _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        public static async Task<string> GetRandomString(int length = 2) 
        {
            char[] result = new char[length];
            var partitioner = Partitioner.Create(0,length);
            Parallel.ForEach(partitioner, (range, loopstate) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    result[i] = _chars[RandomNumberGenerator.GetInt32(_chars.Length - 1)];
                }
            });
            return new String(result);
           
        }

     
    }

    public class SameHashStringGenerator
    {
    }

}

