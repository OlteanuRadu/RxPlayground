using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace RxDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            var one = Observable.Interval(TimeSpan.FromSeconds(2)).Take(1000);
            var two = Observable.Interval(TimeSpan.FromMilliseconds(250)).Take(100);
            var three = Observable.Interval(TimeSpan.FromMilliseconds(150)).Take(100);


            //Observable.When(
            //            one.And(two).And(three).Then((first, second, third) =>
            //                    new { One = first, Two = second, Three = third })
            //            ).Subscribe(_ =>
            //            {
            //                Console.ForegroundColor = ConsoleColor.Cyan;
            //                Console.WriteLine($" Thread Id : {Thread.CurrentContext.ContextID} {_}");
            //                Console.ResetColor();
            //            }, () => Console.WriteLine("Done ... "));

            var x = GetNumbers(2000, 100, 1);
            var y = GetNumbers(10000, 100, 2);
            var z = GetNumbers(20000, 100, 3);
            var t = GetNumbers(30000, 100, 4);

            Observable.When(
                 x.And(y).And(z).And(t).Then(
                 (first, second, third, forth) => new { First = first, Second = second, Thirt = third, Forth = forth }))
                 .Subscribe(_ =>
                 {
                     Console.ForegroundColor = ConsoleColor.Cyan;
                     Console.WriteLine($"Thread Id : {Thread.CurrentThread.ManagedThreadId}  is proccesing the value {_}");
                     Console.ResetColor();
                 },
                 () => Console.WriteLine("The collection was succesfully processed"));

            Observable.Interval(TimeSpan.FromMilliseconds(1000)).Do(_ =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Adding {_} to the stream ... from Thread Id {Thread.CurrentThread.ManagedThreadId}");

            }).Select(_ => char.ConvertFromUtf32((int)_ + 65))
                .Subscribe(_ =>
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Thread Id : {Thread.CurrentThread.ManagedThreadId}  is proccesing the value {_}");
                    Console.ResetColor();
                });

            Console.ReadLine();
        }
        static IObservable<long> GetNumbers(int dataFrequency, int length, int streamId)
        {
            var res = Observable.Interval(TimeSpan.FromMilliseconds(dataFrequency)).Do(_ =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Adding {_} to the stream {streamId}... from Thread Id {Thread.CurrentThread.ManagedThreadId}");
                Console.ResetColor();
            }).Take(length);

            return Observable.Synchronize(res);
        }
    }
}
