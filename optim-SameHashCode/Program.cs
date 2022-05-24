// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace sameHashCodeStrings
{
    public delegate void _bruteForceCallBack(ref ConcurrentDictionary<string, Int32> _hashStore,int length);

    public static class Program
    {
        public static ConcurrentDictionary<string, Int32> _hashStore = new ConcurrentDictionary<string, Int32>();
        public static ThreadBruteForce _threadBruteforce;
        static Thread _thread;


         static async Task Main(string[] args)
        {
            //_hashStore = new ConcurrentDictionary<string, Int32>();
            _threadBruteforce = new ThreadBruteForce(new _bruteForceCallBack(BruteForceCallback),_hashStore, 1);
             _thread = new Thread(new ThreadStart(_threadBruteforce.doBruteForceAsync));
            _thread.Name = "Bruteforce";
            string a = await RandomStringGenerator.GetRandomString();
            Console.WriteLine("First string : Hash : {0}  String : {1}", a.GetHashCode(), a);
            Console.WriteLine("Current thread {0} , state : {1}", _thread.Name, _thread.ThreadState);
            _thread.Start();
            Console.WriteLine("Current thread {0} , state : {1}", _thread.Name, _thread.ThreadState);
            Console.WriteLine("Started bruteforcing string hashes...");


            
            


        }

        public static void BruteForceCallback(ref ConcurrentDictionary<string,Int32> _hashStore,int lenght)
        {
            Console.WriteLine("Current thread {0} , state : {1}", _thread.Name, _thread.ThreadState);
            Console.WriteLine("Wanna check if same has string is found? ");
            var s = Console.ReadLine();
            if (s == "y")
            {
                var lines = _hashStore.Select(e => e.Key + " : " + e.Value.ToString());
                Console.WriteLine(string.Join(Environment.NewLine, lines));
            }
            _threadBruteforce.length++;
            _thread = new Thread(new ThreadStart(_threadBruteforce.doBruteForceAsync));
            _thread.Start();
;
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
        internal static readonly char[] _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public static async Task<string> GetRandomString(int length = 2)
        {
            char[] result = new char[length];
            var partitioner = Partitioner.Create(0, length);
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

    public static class HashStringGenerator
    {
        #region HilariousJokes.Get()
        /*Tentative très naive que je laisse pour amuser le lecteur
        public static async Task<string> getSameHashString(string s)
        {
            
            var index = 0;
            var result = new String("");
            var hashCode = s.GetHashCode();
            bool stop = true;
            do
            {
                result = await RandomStringGenerator.GetRandomString(s.Length);
                stop = result.GetHashCode() == hashCode && !result.Equals(s);

            } while (!stop);

            return result;

        }
        */
        #endregion
        public static async Task BruteForceHash(_bruteForceCallBack _callback,ConcurrentDictionary<string,Int32> _hashStore, int length = 1)
        {
            if(length == 1)
            {
                foreach (var _char in RandomStringGenerator._chars.ToArray())
                {
                    _hashStore.TryAdd(_char.ToString(), _char.ToString().GetHashCode());
                }
            } else
            {
                
                    foreach (var entry in _hashStore.Keys.Where(s => s.Length == length -1))
                    {
                        for (int i = 0; i < RandomStringGenerator._chars.Length; i++)
                        {
                        var input = String.Concat(entry, RandomStringGenerator._chars[i]);
                        _hashStore.TryAdd(input, input.GetHashCode());
                         }
                    }
                
            }

            
        }

        
    }

    public class ThreadBruteForce
    {
        private _bruteForceCallBack _callBack;
        public int length { get; set; }
        private ConcurrentDictionary<string, Int32> _hashStore;

        public ThreadBruteForce(_bruteForceCallBack callBack, ConcurrentDictionary<string, Int32> _hashStore, int length = 1)
        {
            _callBack = callBack;
            this.length = length;
            this._hashStore = _hashStore;
        }

        public async void doBruteForceAsync()
        {
            await HashStringGenerator.BruteForceHash(_callBack, _hashStore, length);
            if(_callBack != null)
            {
                _callBack(ref _hashStore,length+1);
            }
        }

    }

}

