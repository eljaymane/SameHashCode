// See https://aka.ms/new-console-template for more information

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
}

