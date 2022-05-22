using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            string alph = "абвгдежзийклмнопрстуфхцчшщьыэюя";
            int n = alph.Length;
            int m = (int)Math.Pow(n, 2);
            List<int> cyph = Translate(alph);
            List<int> X = new List<int>() { 545, 417, 572, 403, 168 };
            List<int> Y = new List<int>() { 279, 899, 737, 511, 903 };
            List<int> key = new List<int>();
            List<int> text = new List<int>();
            for (int i = 0; i < X.Count - 1; i++)
            {
                for (int j = i + 1; j < Y.Count; j++)
                {
                    key = Key(X[i], Y[i], X[j], Y[j], m);
                    if (key.Count != 0)
                    {
                        for (int q = 0; q < key.Count; q++)
                        {
                            text = Decode(cyph, key[q] / m, key[q] % m, m, n);
                            if (Check(text) == true)
                            {
                                Print(text, alph, key[q], m);
                                break;
                            }
                        }
                    }
                }
            }
            Console.ReadKey();
        }

        static bool Check(List<int> text)
        {
            double freq = 0, sum = 0;
            for (int i = 0; i < text.Count; i++)
            {
                if (text[i] == 14 || text[i] == 0 || text[i] == 5)
                    freq++;
                sum++;
            }
            freq /= sum;
            if (freq < 0.22)
                return false;
            return true;
        }

        static void Print(List<int> text, string alph, int key, int m)
        {
            Console.WriteLine("a = {0}, b = {1}", key / m, key % m);
            for (int i = 0; i < text.Count; i++)
                Console.Write(alph[text[i]]);
            Console.WriteLine();
        }

        static List<int> Decode(List<int> text, int a, int b, int m, int n)
        {
            int l = 0;
            List<int> ans = new List<int>();
            a = Inverse(m, a);
            for (int i = 0; i < text.Count; i++)
            {
                l = (a * (text[i] - b + m)) % m;
                ans.Add(l / n);
                ans.Add(l % n);
            }
            return ans;
        }

        static List<int> Key(int x0, int y0, int x1, int y1, int m)
        {
            List<int> ans = new List<int>();
            ans = Comparison((x0 - x1 + m) % m, (y0 - y1 + m) % m, m);
            for (int i = 0; i < ans.Count; i++)
            {
                if (GCD(ans[i], m) == 1)
                    ans[i] = ans[i] * m + ((((y0 - ans[i] * x0) % m) + m) % m);
                else
                    ans.RemoveAt(i);
            }
            return ans;
        }

        static List<int> Comparison(int a, int b, int m)
        {
            List<int> ans = new List<int>();
            int d = GCD(m, a);
            if (d != 1)
            {
                if (b % d == 0)
                {
                    m /= d;
                    ans = Comparison(a / d, b / d, m);
                    for (int i = 1; i < d; i++)
                        ans.Add(ans[0] + i * m); 
                }
            }
            else
                ans.Add((b * Inverse(m, a)) % m);
            return ans;
        }

        static int Inverse(int n, int a)
        {
            int q, u, u0 = 0, u1 = 1, m = n;
            while (a != 1) 
            {
                q = n / a;
                u = u0 - u1 * q;
                q = n % a;
                n = a;
                a = q;
                u0 = u1;
                u1 = u % m;
            }
            return (u1 + m) % m;
        }

        static int GCD(int a, int b)
        {
            if (a == 0)
                return 1;
            else
                return b == 0 ? Math.Abs(a) : GCD(b, a % b);
        }

        static List<int> Translate(string alph)
        {
            var sr = File.OpenText("var2.txt");
            List<int> ind = new List<int>();
            char a, b;
            int i, j;
            int n = alph.Length;
            while (!sr.EndOfStream)
            {
                a = (char)sr.Read();
                b = (char)sr.Read();
                while (a < 1072 || b < 1072)
                {
                    if (a < 1072)
                        a = b;
                    b = (char)sr.Read();
                }
                for (i = 0; i < alph.Length; i++)
                    if (a == alph[i])
                        break;
                for (j = 0; j < alph.Length; j++)
                    if (b == alph[j])
                        break;
                ind.Add(i * n + j);
            }
            return ind;
        }
    }
}
