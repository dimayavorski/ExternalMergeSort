using System.Diagnostics;

namespace MergeSort
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"..\..\..\..\FileGenerator\bin\Release\net6.0\input.txt";
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var sorter = new FileSorter();
            sorter.SortFile(filePath);
            stopWatch.Stop();
            Console.WriteLine(stopWatch.Elapsed.ToString());
        }
    }
}
