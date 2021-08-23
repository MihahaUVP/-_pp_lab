using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
namespace pp_lab1
{
    class Program
    {
        const int n = 600;
        static void Main(string[] args)
        {
            //программа перемножает две квадратные матрицы одинакового размера 
            int[,] Matrix = new int[n, n];
            int[,] Matrix2 = new int[n, n];
            Random random = new Random();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Matrix[i, j] = random.Next(1, 9);
                    Matrix2[i, j] = random.Next(1, 9);
                }
            }
            //printMatrix(Matrix, n); не используется, так как вывод матрицы большого размера в консоль мешает демонстрировать сравнение скорости параллельных и однопоточных выислений
            Console.WriteLine();
            //printMatrix(Matrix2, n);
            Console.WriteLine("Результат умножения матриц");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            multMatrix(Matrix, Matrix2, n);
            stopwatch.Stop();
            TimeSpan elapsed = stopwatch.Elapsed;

            Console.WriteLine("Теперь параллельно");
            stopwatch.Reset();
            stopwatch.Start();
            PMultMatrix(Matrix, Matrix2, n);
            stopwatch.Stop();
            Console.WriteLine("Время исполнения однопоточного вычисления" +  elapsed);
            Console.WriteLine("Время выполнения параллельных вычислений " + stopwatch.Elapsed);
        }
        static void PMultMatrix(int[,] M1, int[,] M2, int m)//параллельное умножение матриц
        {
            int numberOfThreads = Environment.ProcessorCount;
            int numberOfRowsInOneThread = m / numberOfThreads;
            int[,] newMatrix = new int[m, m];
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < m; i+=numberOfRowsInOneThread)
            {
                int[] row = new int[m];//строка
                int firstRow = i;
                int lastRow = i + numberOfRowsInOneThread;
                if (lastRow >= m)
                {
                    lastRow = m - 1;
                }
                Thread t = new Thread(state => MultRow(m, M1, M2, newMatrix,firstRow,lastRow));
                threads.Add(t);
                threads[threads.Count - 1].Start();
            }
            foreach (var th in threads)
            {
                th.Join();
            }
            //printMatrix(newMatrix, m);
        }
        static void MultRow(int m, int[,] M1, int[,] M2,  int[,] newMatrix,int firstRow,int lastRow)
        {
            //int[] newRow = new int[m];
            for (int i = firstRow; i <= lastRow; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    int c = 0;
                    for (int k = 0; k < m; k++)
                    {
                        c += M1[i, k] * M2[k, j];
                    }
                    newMatrix[i, j] = c;
                }
            }
            //return newRow;
        }
        static void multMatrix(int[,] M1, int[,] M2, int m)
        {
            int[,] newMatrix = new int[m, m];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    int c = 0;
                    for (int k = 0; k < m; k++)
                    {
                        c += M1[i, k] * M2[k, j];
                    }
                    newMatrix[i, j] = c;
                }
            }
            //printMatrix(newMatrix, m);
        }
        static void printMatrix(int[,] M, int m)
        {
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Console.Write(M[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
