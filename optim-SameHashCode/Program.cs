// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace sameHashCodeStrings
{
    public delegate void _bruteForceCallBack();

    public delegate void _tryGetStringsCallBack();
    public static class Program
    {
        const int CHECK_EVERY_MILLISECONDS = 7000;
        const int INITIAL_STRING_LENGTH = 4;
        public static ConcurrentDictionary<string, Int32> _hashStore = new ConcurrentDictionary<string, Int32>(Environment.ProcessorCount *2,5000000);
        public static Dictionary<string,Int32> _found = new Dictionary<string, Int32>();
        public static ThreadBruteForce threadBruteForce;
        public static ThreadCheckStrings threadCheckStrings;
        public static string initialString;
        private static Thread _threadBruteforce, _threadCheck;
        public static ManualResetEvent _mre = new ManualResetEvent(false);


         static async Task Main(string[] args)
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
            initialString = await RandomStringGenerator.GetRandomString(INITIAL_STRING_LENGTH);
            threadBruteForce = new ThreadBruteForce(new _bruteForceCallBack(BruteForceCallback),_hashStore, 1);
            threadCheckStrings = new ThreadCheckStrings(new _tryGetStringsCallBack(TryGetStringsCallBack));
            _threadCheck = new Thread(new ThreadStart(threadCheckStrings.doCheckStrings));
            _threadBruteforce = new Thread(new ThreadStart(threadBruteForce.doBruteForce));
            _threadBruteforce.Name = "Bruteforce";
            _threadBruteforce.Priority = ThreadPriority.Lowest;
            Console.WriteLine("First string : Hash : {0}  String : {1}", initialString.GetHashCode(), initialString);
            _mre.Set();
            _threadBruteforce.Start();
            Console.WriteLine("Thread {0} is in state : {1}", _threadBruteforce.Name, _threadBruteforce.ThreadState);
            Console.WriteLine("Started bruteforcing string hashes...");
            _threadCheck.Start();
        }
        public static async Task tryGetStrings()
        {
            Thread.Sleep(CHECK_EVERY_MILLISECONDS);
            _mre.Reset();
            Console.WriteLine("Checking...");
            var hashCode = initialString.GetHashCode();
            var values = _hashStore.Where(e => e.Value == hashCode && e.Key != initialString).ToDictionary(e => e.Key, e=> e.Value);
            if (!(values.Count() > 0))
            {
                Console.WriteLine("Nothing to add");
            }
            else
            {
                foreach (var item in values)
                {
                    _found.Add(item.Key, item.Value);
                   _hashStore.TryRemove(item.Key, out int value);
                }
                if (values.Count > 1) Console.WriteLine("Added {0} values", values.Count);
                if (values.Count == 1) Console.WriteLine("Added {0} value", values.Count);
            }
            if (_found.Count >= 3)
            {
                _threadBruteforce.Abort();
                Console.WriteLine("Thread {0} is in state {1}",_threadBruteforce.Name,_threadBruteforce.ThreadState);
                Console.WriteLine("For initial string {0} with hash {1} , here's what i've found :", initialString, initialString.GetHashCode());
                foreach (var e in _found)
                {
                    Console.WriteLine(" String : {0} , Hash : {1}", e.Key, e.Value);
                }
                Console.WriteLine("Press any key to quit...");
                Console.ReadKey();
                _threadCheck.Abort();
            }
        }

        public static async void TryGetStringsCallBack()
        {
            await doCleanup();
            _threadCheck = new Thread(new ThreadStart(threadCheckStrings.doCheckStrings));
            _threadCheck.Start();
        }

        public static async Task doCleanup()
        {
            Console.WriteLine("Cleaning up...");
            Console.WriteLine("Total Memory : {0}", GC.GetTotalMemory(false));
            foreach (var e in _hashStore)
            {
                if (e.Value != initialString.GetHashCode()) _hashStore.TryRemove(e.Key, out int i);
            }
            GC.Collect();
            _mre.Set();
        }

        public static async Task BruteForceHash(Dictionary<string, Int32> _buffer,
            ConcurrentDictionary<string, int> _hashStore, int length = 1)
        {
            if (length == 1)
            {
                foreach (var _char in RandomStringGenerator._chars.ToArray())
                {
                    _mre.WaitOne();
                    _hashStore.TryAdd(_char.ToString(), _char.ToString().GetHashCode());
                }
            }
            else
            {
                foreach (var entry in _buffer.Keys.Where(s => s.Length == length -1))
                {
                    for (int i = 0; i < RandomStringGenerator._chars.Length; i++)
                    {
                        var input = String.Concat(entry, RandomStringGenerator._chars[i]);
                        _mre.WaitOne();
                        _hashStore.TryAdd(input, input.GetHashCode());
                    }
                }
            }
        }

        public static async void BruteForceCallback()
        {
            _mre.Reset();
            threadBruteForce._buffer = new Dictionary<string, Int32>(_hashStore
                .Where(e => e.Key.Length == threadBruteForce.length).ToDictionary(e => e.Key, e => e.Value));
            _mre.Set();
            threadBruteForce.length++;
            _threadBruteforce = new Thread(new ThreadStart(threadBruteForce.doBruteForce));
            _threadBruteforce.Name = "Bruteforce";
            _threadBruteforce.Start();
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
    public class ThreadBruteForce
    {
        private _bruteForceCallBack _callBack;
        public int length { get; set; }
        private ConcurrentDictionary<string, Int32> _hashStore;
        public Dictionary<string, Int32> _buffer { get; set; }

        public ThreadBruteForce(_bruteForceCallBack callBack,ConcurrentDictionary<string, Int32> _hashStore, int length = 1)
        {
            _callBack = callBack;
            this.length = length;
            this._hashStore = _hashStore;
            _buffer = new Dictionary<string, Int32>();
        }

        public async void doBruteForce()
        {
            await Program.BruteForceHash(_buffer,_hashStore, length);
            if (_callBack != null)
            {
                _callBack();
            }
        }

    }

    public class ThreadCheckStrings
    {
        private _tryGetStringsCallBack _callBack;

        public ThreadCheckStrings(_tryGetStringsCallBack callback)
        {
            _callBack = callback;
        }

        public async void doCheckStrings()
        {
            await Program.tryGetStrings();
            if (_callBack != null)
            {
                _callBack();
            }
        }
    }

}

