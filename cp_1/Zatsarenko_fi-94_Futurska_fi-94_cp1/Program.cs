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
            string alph = "абвгдежзийклмнопрстуфхцчшщыьэюя";
            FilterText();
            var sr1 = File.OpenText("TEXT1");
            double[] s1 = Symbol(sr1, alph + " ");
            Print(s1, alph + " ");
            var sr2 = File.OpenText("TEXT2");
            double[] s2 = Symbol(sr2, alph);
            Print(s2, alph);
            sr1 = File.OpenText("TEXT1");
            double[,] b1 = Biggram(sr1, alph + " ");
            Print(b1, alph + " ");
            sr2 = File.OpenText("TEXT2");
            double[,] b2 = Biggram(sr2, alph);
            Print(b2, alph);
            sr1 = File.OpenText("TEXT1");
            double[,] c1 = BiggramIS(sr1, alph + " ");
            Print(c1, alph + " ");
            sr2 = File.OpenText("TEXT2");
            double[,] c2 = BiggramIS(sr2, alph);
            Print(c2, alph);
            Console.WriteLine("H1 = {0:N5}", Entropy(s1));
            Console.WriteLine("H2 = {0:N5}", Entropy(b1));
            Console.WriteLine("H2IS = {0:N5}", Entropy(c1));
            Console.WriteLine("H1\\\" \" = {0:N5}", Entropy(s2));
            Console.WriteLine("H2\\\" \" = {0:N5}", Entropy(b2));
            Console.WriteLine("H2IS\\\" \" = {0:N5}", Entropy(c2));

            Console.ReadKey();
            sr1.Close();
            sr2.Close();
        }

        static void Print(double[] array, string alph)
        {
            int n = alph.Length;
            for (int i = 0; i < n; i++)
                Console.WriteLine("{0}  -  {1:N5}", alph[i], array[i]);
            Console.WriteLine();
        }

        static void Print(double[,] array, string alph)
        {
            int n = alph.Length;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)                
                        if (array[i, j] != 0)                        
                            Console.WriteLine("{0}{1}  -  {2:N5}", alph[i], alph[j], array[i, j]);            
            Console.WriteLine();
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
                for (j = 0; j < n; j++)
                    count[i, j] = count[i, j] / sum;
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
                if (l2 == alph[j])
                    break;
            while (!sr.EndOfStream)
            {
                l1 = l2;
                i = j;
                l2 = (char)sr.Read();
                for (j = 0; j < n; j++)
                    if (l2 == alph[j])
                        break;
                count[i, j]++;
                sum++;
            }
            for (i = 0; i < n; i++)
                for (j = 0; j < n; j++)
                    count[i, j] = count[i, j] / sum;
            return count;
        }

        static double[] Symbol(StreamReader sr, string alph)
        {
            int sum = 0;
            char letter;
            int n = alph.Length;
            double[] count = new double[n];
            Array.Clear(count, 0, n);
            while (!sr.EndOfStream)
            {
                letter = (char)sr.Read();
                for (int i = 0; i < n; i++)
                    if (letter == alph[i])
                    {
                        count[i]++;
                        sum++;
                        break;
                    }
            }
            for (int i = 0; i < n; i++)
                count[i] = count[i] / sum;
            return count;
        }

        static double Entropy(double[,] count)
        {
            int n = count.GetLength(0);
            double h = 0;
            for (int i = 0; i < n; i++) 
                for (int j = 0; j < n; j++) 
                    if (count[i, j] != 0)
                        h += (count[i, j] * Math.Log(count[i, j], 2));
            return -h/2;
        }

        static double Entropy(double[] count)
        {
            double h = 0;
            for (int i = 0; i < count.Length; i++)  
                h += (count[i] * Math.Log(count[i], 2));
            return -h;
        }

        static void FilterText()
        {
            var sr = File.OpenText("text.txt");
            var sw1 = File.CreateText("TEXT1");
            var sw2 = File.CreateText("TEXT2");
            int index;
            string text;
            while (!sr.EndOfStream)
            {
                text = sr.ReadLine();
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
                text = text.TrimStart();
                sw1.Write(text);
                text = text.Replace(" ", string.Empty);
                sw2.Write(text);
            }
            sw1.Close();
            sw2.Close();
            sr.Close();
        }
    }
}
