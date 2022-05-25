// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace sameHashCodeStrings
{
    public delegate void _bruteForceCallBack(ref ConcurrentDictionary<string, Int32> _hashStore,int length);
   

    public static class Program
    {
        public static AutoResetEvent _are = new AutoResetEvent(true);
        public static ConcurrentDictionary<string, Int32> _hashStore = new ConcurrentDictionary<string, Int32>(Environment.ProcessorCount *2,4000000);
        public static ThreadBruteForce _threadBruteforce;
        public static ThreadTryGetSameHashString _threadTryGetSameHashString;
        public static char response;
        public static string a;
        static Thread _thread,_threadCheck;


         static async Task Main(string[] args)
        {
            //_hashStore = new ConcurrentDictionary<string, Int32>();
            //BB : HASH : -331290870
             a = await RandomStringGenerator.GetRandomString(4);
            _threadBruteforce = new ThreadBruteForce(new _bruteForceCallBack(BruteForceCallback),ref _hashStore, 1);
            _threadTryGetSameHashString = new ThreadTryGetSameHashString(ref _hashStore,a);
            _thread = new Thread(new ThreadStart(_threadBruteforce.doBruteForceAsync));
            _thread.Name = "Bruteforce";
            _thread.Priority = ThreadPriority.Lowest;
            
            _threadCheck = new Thread(new ThreadStart(tryGetStrings));
            
            Console.WriteLine("First string : Hash : {0}  String : {1}", a.GetHashCode(), a);
            Console.WriteLine("Current thread {0} , state : {1}", _thread.Name, _thread.ThreadState);
            //_thread.Start();
            //Console.WriteLine("Current thread {0} , state : {1}", _thread.Name, _thread.ThreadState);
            _thread.Start();

            do
            {
               
                Console.WriteLine("Started bruteforcing string hashes...");
                Console.WriteLine("Current thread {0} , state : {1}", _thread.Name, _thread.ThreadState);
                Console.WriteLine("Wanna check if same has string is found? ");
                response = Console.ReadLine()[0];
                if (response == 'y')
                {
                    //_thread.Suspend();
                    _threadCheck = new Thread(new ThreadStart(tryGetStrings));
                    _are.Set();
                    _threadCheck.Start();
                   
                   

                        //var lines = _hashStore.Select(e => e.Key + " : " + e.Value.ToString());
                        //Console.WriteLine(string.Join(Environment.NewLine, lines));
                    }
            } while (response != 'q');


            
            


        }
        public static void tryGetStrings()
        {

            Console.WriteLine("Waiting...");
            _are.WaitOne();
            var hashCode = a.GetHashCode();


            var values = _hashStore.Where(e => e.Value == hashCode);
            if (!(values.Count() > 0)) Console.WriteLine("Still nothing found...");
            else
            {
                Console.WriteLine("For initial string : {0} , hash : {1}", a, a.GetHashCode());
                Console.WriteLine("Values found  : ");
                foreach (var item in values)
                {
                    Console.WriteLine("string : {0} , hash : {1}", item.Key, item.Value);
                }
            }

            _are.Reset();

        }

        public static void BruteForceCallback(ref ConcurrentDictionary<string,Int32> _hashStore,int lenght)
        {
            _are.Set();
            _threadBruteforce.length++;
            _thread = new Thread(new ThreadStart(_threadBruteforce.doBruteForceAsync));
            _thread.Name = "Bruteforce";
            _are.WaitOne();
            _thread.Start();
;
        }

        async static Task<bool> TestKata(string string1, string string2, string string3)
        {
            bool hashCodeEqual = string1.GetHashCode() == string2.GetHashCode() && string2.GetHashCode() == string3.GetHashCode();
            bool stringNotEqual = !String.Equals(string1, string2) && !String.Equals(string1, string3) && !String.Equals(string2, string3);
            return hashCodeEqual && stringNotEqual;
        }
    }

    public class RandomStringGenerator 
    {
        internal static readonly char[] _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

        public static async Task<string> GetRandomString(int length = 2)
        {
            char[] result = new char[length];
            var partitioner = Partitioner.Create(0, length);
            var random = new Random();
            Parallel.ForEach(partitioner, (range, loopstate) =>
            {
                
                var randInt = random.Next(0, _chars.Length - 1);   
                for (int i = range.Item1; i < range.Item2; i++)
                {
                   
                    result[i] = _chars[randInt];
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

        public ThreadBruteForce(_bruteForceCallBack callBack, ref ConcurrentDictionary<string, Int32> _hashStore, int length = 1)
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

    public class ThreadTryGetSameHashString
    {
        private readonly ConcurrentDictionary<string, Int32> _hashStore;
        private readonly string s;

        public ThreadTryGetSameHashString(ref ConcurrentDictionary<string,Int32> _hashStore,string s)
        {
            this._hashStore = _hashStore;
            this.s = s;
        }

        public void tryGetStrings()
        {
            var hashCode = s.GetHashCode();
           
            
                var values = this._hashStore.Where(e => e.Value == hashCode);
                if (!(values.Count() > 0)) Console.WriteLine("Still nothing found...");
                else
                {
                    Console.WriteLine("For initial string : {0} , hash : {1}", s, s.GetHashCode());
                    Console.WriteLine("Values found  : ");
                    foreach (var item in values)
                    {
                        Console.WriteLine("string : {0} , hash : {1}", item.Key, item.Value);
                    }
                }
            
           
            
        }
        
    }

}

