using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            string alph = "абвгдежзийклмнопрстуфхцчшщыьэюя ";
            int n = alph.Length;
            FilterText();
            var sr = new StreamReader("TEXT", Encoding.GetEncoding(866));
            double[] s = Symbol(sr, alph);
            for (int i = 0; i < n; i++)
                Console.WriteLine("{0}  -  {1:N5}", alph[i], s[i]);
            sr = new StreamReader("TEXT", Encoding.GetEncoding(866));
            double[,] b = Biggram(sr, alph);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (b[i,j] != 0)
                        Console.WriteLine("{0}{1}  -  {2:N5}", alph[i], alph[j], b[i, j]);
                }
            }
            sr = new StreamReader("TEXT", Encoding.GetEncoding(866));
            double[,] b2 = BiggramIS(sr, alph);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (b2[i, j] != 0)
                        Console.WriteLine("{0}{1}  -  {2:N5}", alph[i], alph[j], b2[i, j]);
                }
            }
            Console.WriteLine("H1 = {0:N5}", Entropy(s, n));
            Console.WriteLine("H2 = {0:N5}", Entropy(b, n));
            Console.WriteLine("H3 = {0:N5}", Entropy(b2, n));
            Console.WriteLine("H1\\\" \" = {0:N5}", Entropy(s, n-1));
            Console.WriteLine("H2\\\" \" = {0:N5}", Entropy(b, n - 1));
            Console.WriteLine("H3\\\" \" = {0:N5}", Entropy(b2, n - 1));
            Console.ReadKey();
            sr.Close();
        }

        static double[,] Biggram(StreamReader sr, string alph)
        {
            int flag, i = 0, j = 0, sum = 0;
            char l1, l2;
            int n = alph.Length;
            double[,] count = new double[n,n];
            Array.Clear(count, 0, n);
            while (!sr.EndOfStream)
            {
                l1 = (char)sr.Read();
                l2 = (char)sr.Read();
                flag = 0;
                for (int k = 0; k < n; k++)
                {
                    if (alph[k] == l1)
                    {
                        i = k;
                        if (flag == 1)
                            break;
                        else flag++;
                    }
                    if (alph[k] == l2)
                    {
                        j = k;
                        if (flag == 1)
                            break;
                        else flag++;
                    }
                }
                count[i, j]++;
                sum++;
            }
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                    count[i, j] = count[i, j] / sum;
            }
            return count;
        }

        static double[,] BiggramIS(StreamReader sr, string alph)
        {
            int i = 0, j = 0, sum = 0;
            char l1, l2;
            int n = alph.Length;
            double[,] count = new double[n, n];
            Array.Clear(count, 0, n);
            l2 = (char)sr.Read();
            for (j = 0; j < n; j++)
            {
                if (l2 == alph[j])
                    break;
            }
            while (!sr.EndOfStream)
            {
                l1 = l2;
                i = j;
                l2 = (char)sr.Read();
                for (j = 0; j < n; j++)
                {
                    if (l2 == alph[j])
                        break;
                }
                count[i, j]++;
                sum++;
            }
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                    count[i, j] = count[i, j] / sum;
            }
            return count;
        }

        static double[] Symbol(StreamReader sr, string alph)
        {
            int k, sum = 0;
            char letter;
            int n = alph.Length;
            double[] count = new double[n];
            Array.Clear(count, 0, n);
            while (!sr.EndOfStream)
            {
                k = sr.Read();
                letter = (char)k;
                for (int i = 0; i < n; i++)
                {
                    if (letter == alph[i])
                    {
                        count[i]++;
                        sum++;
                        break;
                    }
                }
            }
            for (int i = 0; i < n; i++)
                count[i] = count[i] / sum;
            return count;
        }

        static double Entropy(double[,] count, int n)
        {
            double h = 0;
            for (int i = n - 1; i >= 0; i--) 
            {
                for (int j = n - 1; j >= 0; j--) 
                {
                    if (count[i, j] != 0)
                        h += (count[i, j] * Math.Log(count[i, j], 2));
                }
            }
            return -h;
        }

        static double Entropy(double[] count, int n)
        {
            double h = 0;
            for (int i = n - 1; i >= 0; i--) 
                h += (count[i] * Math.Log(count[i], 2));
            return -h;
        }

        static void FilterText()
        {
            var text = File.ReadAllText("TEXT", Encoding.GetEncoding(866));
            int index;
            text = text.ToLower();
            for (int i = 0; i < text.Length; i++)
            {
                index = (int)text[i];
                if ((index < 1072 || index > 1103) && index != 32)
                    text = text.Remove(i, 1).Insert(i, " ");
                if (index == 1098)
                    text = text.Remove(i, 1).Insert(i, "ь");
            }
            text = Regex.Replace(text, @"\s+", " ");
            File.WriteAllText("TEXT", text, Encoding.GetEncoding(866));
        }
    }
}
