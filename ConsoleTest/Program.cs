using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVLTreeLib;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = new AVLTree<int, int>();
            var dict = new SortedDictionary<int, int>();
            

            var rnd = new Random();
            var listInput = new List<int>(10000);
            var listOutput = new List<int>(10000);
            for (int i = 0; i < listInput.Capacity; i++) listInput.Add(i+1);
            while (listInput.Count > 0)
            {
                var t = listInput[rnd.Next(0, listInput.Count)];
                listOutput.Add(t);
                listInput.Remove(t);
            }

            var watch = new Stopwatch();

            Console.WriteLine("Testing AVL Tree:");
            watch.Start();
            for (int i = 0; i < listOutput.Capacity; i++) tree.Add(listOutput[i], 0);
            watch.Stop();
            Console.WriteLine($"Time elapsed to add items: {watch.ElapsedMilliseconds} ms");
            
            watch.Restart();
            for (int i = (int)(listOutput.Count * 0.5); i <= (int)(listOutput.Count * 0.7); i++) tree.RemoveKey(i);
            watch.Stop();
            Console.WriteLine($"Time elapsed to remove items: {watch.ElapsedMilliseconds} ms");

            watch.Restart();
            for (int i = 0; i < 10000; i++) tree.ContainsKey(i);
            watch.Stop();
            Console.WriteLine($"Time elapsed to find items: {watch.ElapsedMilliseconds} ms");

            Console.WriteLine("\nTesting Sorted Dictionary:");
            watch.Start();
            for (int i = 0; i < listOutput.Capacity; i++) dict.Add(listOutput[i], 0);
            watch.Stop();
            Console.WriteLine($"Time elapsed to add items: {watch.ElapsedMilliseconds} ms");

            watch.Restart();
            for (int i = 5000; i <= 7000; i++) dict.Remove(i);
            watch.Stop();
            Console.WriteLine($"Time elapsed to remove items: {watch.ElapsedMilliseconds} ms");

            watch.Restart();
            for (int i = 0; i < 10000; i++) dict.ContainsKey(i);
            watch.Stop();
            Console.WriteLine($"Time elapsed to find items: {watch.ElapsedMilliseconds} ms");
            Console.ReadLine();
        }
    }
}
