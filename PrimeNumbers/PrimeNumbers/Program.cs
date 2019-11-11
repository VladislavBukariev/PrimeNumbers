using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PrimeNumbers
{
	class Program
	{
		ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
		static HashSet<int> res = new HashSet<int>();
		static int threadCount;
		static int range;
		static void Main(string[] args)
		{
			Program obj = new Program();
			DateTime start = DateTime.Now;

			switch (args.Select(int.Parse).ToArray().Length)
			{
				case 1:
					range = ((args.Select(int.Parse).ToArray())[0] > 0 && (args.Select(int.Parse).ToArray())[0] <= 1000000000) ? (args.Select(int.Parse).ToArray())[0] : 100;
					break;
				case 2:
					range = ((args.Select(int.Parse).ToArray())[0] > 0 && (args.Select(int.Parse).ToArray())[0] <= 1000000000) ? (args.Select(int.Parse).ToArray())[0] : 100;
					threadCount = ((args.Select(int.Parse).ToArray())[1] > 0 && (args.Select(int.Parse).ToArray())[1] <= 100) ? (args.Select(int.Parse).ToArray())[1] : 1;
					break;
				default:
					range = 1000;
					threadCount = 1;
					break;
			}

			Console.WriteLine($"Prime numbers from {range} with {threadCount} threads");
			obj.primeNumbers();
			Console.WriteLine($"Count of prime numbers from {range} is {res.Count}");
			Console.WriteLine($"Ececution: {(DateTime.Now.Subtract(start))}");
			Console.WriteLine();

			//obj.printResult();				
		}

		private void printResult()
		{
			SortedSet<int> sorted = new SortedSet<int>(res);
			foreach (var r in sorted)
			{
				Console.Write($" {r}");
			}
			Console.WriteLine();
		}

		private void primeNumbers()
		{
			HashSet<int> source = new HashSet<int>();
			res.Add(2);

			var list_threads = new List<Thread>(threadCount);

			for (int th = 0; th < threadCount; th++)
			{
				var thr = new Thread(threadData);
				list_threads.Add(thr);
				thr.Start(th);
			}

			foreach (var item in list_threads)
			{
				item.Join();
			}			
		}
		private void threadData(object logicThread)
		{
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();

			for (int i = 3 + (((int)logicThread) * 2); i < range; i += (2 * threadCount))
			{
				if (!isComplex(i))
				{
					WriteHashSet(res, i);
				}
			}
			stopWatch.Stop();
			Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} executed in {stopWatch.ElapsedMilliseconds} ms");
		}
		private void WriteHashSet(HashSet<int> destintionSet, int item)
		{
			rwLock.EnterWriteLock();
			destintionSet.Add(item);
			rwLock.ExitWriteLock();
		}
		private bool isComplex(int num)
		{
			bool result = false;
					
			for (int i = 2; i <= Math.Sqrt(num); i++)
			{
				if (num % i == 0)
				{
					result = true;
					break;
				}
			}
			
			return result;
		}
	}
}