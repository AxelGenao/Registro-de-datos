using System;

namespace Recursividad
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Recursividad Factorial");
            Console.WriteLine(fact(3));
            Console.WriteLine(fact(4));
            Console.WriteLine(fact(8));

            Console.WriteLine("Tail-Recursion Factorial");
            Console.WriteLine(tailFact(3));
            Console.WriteLine(tailFact(4));
            Console.WriteLine(tailFact(8));

            Console.WriteLine("Factorial Iterativo");
            Console.WriteLine(iterFact(3));
            Console.WriteLine(iterFact(4));
            Console.WriteLine(iterFact(8));

            Console.WriteLine("Recursividad Sumatoria");
            Console.WriteLine(Sum(5));
            Console.WriteLine(Sum(10));
            Console.WriteLine(Sum(100));

            Console.WriteLine("Tail-Recursion Sumatoria");
            Console.WriteLine(tailSum(5));
            Console.WriteLine(tailSum(10));
            Console.WriteLine(tailSum(100));

            Console.WriteLine("Sumatoria Iterativo");
            Console.WriteLine(iterSum(5));
            Console.WriteLine(iterSum(10));
            Console.WriteLine(iterSum(100));

            Console.WriteLine("Recursividad Algoritmo de Euclides");
            Console.WriteLine(RecurMCD(9, 3));
            Console.WriteLine(RecurMCD(60, 48));
            Console.WriteLine(RecurMCD(48, 60));

            Console.WriteLine("Tail-Recursion Algoritmo de Euclides");
            Console.WriteLine(tailMCD(9, 3));
            Console.WriteLine(tailMCD(60, 48));
            Console.WriteLine(tailMCD(48, 60));

            Console.WriteLine("Algoritmo de Euclides Iterativo");
            Console.WriteLine(iterMCD(9, 3));
            Console.WriteLine(iterMCD(60, 48));
            Console.WriteLine(iterMCD(48, 60));
        }

        #region Ejercicio 3
        static int fact(int n)
        {
            if (n == 0) return 1;
            else return n * fact(n - 1);
        }

        static int tailFact(int n, int sum = 1)
        {
            if (n == 1) return sum;
            else return tailFact(n - 1, sum * n);
        }

        static int otherFact(int n)
        {
            return (n == 0) ? 1 : n * otherFact(n - 1);
        }

        static int Sum(int n)
        {
            if (n == 1) return n;
            else return n + Sum(n - 1);
        }

        static int tailSum(int n, int sumado = 0)
        {
            if (n == 0) return sumado;
            else return tailSum(n - 1, sumado + n);
        }

        static int otherSum(int n)
        {
            return (n == 1) ? n : n + Sum(n - 1);
        }

        static int RecurMCD(int m, int n)
        {
            if ((m % n) == 0) return n;
            else if (m > n) return RecurMCD(n, m % n) % m;
            else return RecurMCD(m, n % m) % n;
        }

        static int tailMCD(int m, int n)
        {
            if (m % n == 0) return n;
            else return tailMCD(n, m % n);
        }

        static int otherMCD(int m, int n)
        {
            return (m % n == 0) ? n : otherMCD(n, m % n);
        }
        #endregion

        #region Ejercicio 4
        static int iterFact(int n)
        {
            int res = 1;
            for (int i = 1; i <= n; i++)
                res = res * i;
            return res;
        }

        static int iterSum(int n)
        {
            int res = 0;
            for (int i = 1; i <= n; i++)
                res += i;
            return res;
        }

        static int iterMCD(int m, int n)
        {
            int a = m, b = n;
            while (a != 0 && b != 0)
            {
                if (a % b == 0) return b;

                if (a > b)
                {
                    int z = b;
                    b = a % b;
                    a = z;
                }
                else
                {
                    int z = a;
                    a = b;
                    b = z;
                }
            }

            return b;
        }
        #endregion
    }
}
