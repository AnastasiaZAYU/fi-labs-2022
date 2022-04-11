using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            string alph = "абвгдежзийклмнопрстуфхцчшщъыьэюя";
            int n = alph.Length;
            string keys = "вильямшекспиргамлетъ";
            List<int> key = new List<int>();
            for (int i = 0; i < keys.Length; i++)
                for (int j = 0; j < alph.Length; j++)
                    if (keys[i] == alph[j])
                        key.Add(j);
            double ind;
            List<double> index = new List<double>();
            List<int> cyph = new List<int>();
            var sr = File.OpenText("text.txt");
            FilterText(sr);
            sr = File.OpenText("TEXT");
            List<int> text = Translate(sr, alph);
            int k = text.Count;
            Console.WriteLine("I0 = {0:N4}", I(Count(text, n), k));
            for (int i = 2; i < 6; i++)
            {
                cyph = Encode(text, n, key.GetRange(0, i));
                ind = I(Count(cyph, n), k);
                index.Add(ind);
                Console.WriteLine("I{0} = {1:N4}", i, ind);
            }
            for (int i = 10; i < 21; i++)
            {
                cyph = Encode(text, n, key.GetRange(0, i));
                ind = I(Count(cyph, n), k);
                index.Add(ind);
                Console.WriteLine("I{0} = {1:N4}", i, ind);
            }
            sr = File.OpenText("var2.txt");
            text = Translate(sr, alph);
            int r = Period(text, text.Count);
            Console.WriteLine("r = {0}", r);
            KeyCaesar(text, r, n, alph);
            key = KeyMg(text, r, n, alph);
            Print(text, key, r, alph, n);

            Console.ReadKey();
        }

        static List<int> KeyCaesar(List<int> cyph, int r, int n, string alph)
        {
            List<int> keys = new List<int>();
            string key = "";
            int y, index;
            int[] count = new int[n];
            int[] x = new int[r];
            for (int i = 0; i < r; i++)
            {
                if (i == 0 || i == 3 || i == 5 || i == 6 || i == 8)
                    x[i] = 5;
                else x[i] = 14;
            }
            for (int j = 0; j < r; j++)
            {
                Array.Clear(count, 0, n);
                for (int i = j; i < cyph.Count; i += r)
                    count[cyph[i]]++;
                y = Array.IndexOf(count, count.Max());
                index = (n + y - x[j]) % n;
                keys.Add(index);
                key += alph[index];
            }
            Console.WriteLine("Key #1 : {0}", key);
            return keys;
        }

        static int Period(List<int> cyph, int n)
        {
            int d, p = 0, max = 0;
            for (int r = 10; r < 31; r++)
            {
                d = 0;
                for (int i = 1; i < n - r; i++)
                    if (cyph[i] == cyph[i + r])
                        d++;
                if (d > max)
                {
                    max = d;
                    p = r;
                }
                Console.WriteLine("D{0} = {1}", r, d);
            }
            return p;
        }

        static List<int> KeyMg(List<int> cyph, int r, int n, string alph)
        {
            double[] p = { 0.0801, 0.0159, 0.0454, 0.0170, 0.0298, 0.0849, 0.0094, 0.0165, 0.0735, 0.0121, 0.0349, 0.0440, 0.0321, 0.0670, 0.1097, 0.0281, 0.0473, 0.0547, 0.0626, 0.0262, 0.0026, 0.0097, 0.0048, 0.0144, 0.0073, 0.0036, 0.0004, 0.0190, 0.0174, 0.0032, 0.0064, 0.0201 };
            List<int> keys = new List<int>();
            string key = "";
            int index;
            int[] count = new int[n];
            double[] mi = new double[n];
            for (int i = 0; i < r; i++)            
            {
                Array.Clear(count, 0, n);
                Array.Clear(mi, 0, n);
                for (int j = i; j < cyph.Count; j += r)
                    count[cyph[j]]++;
                for (int g = 0; g < n; g++)
                    for (int t = 0; t < n; t++)
                        mi[g] += p[t] * count[(t + g) % n];
                index = Array.IndexOf(mi, mi.Max());
                key += alph[index];
                keys.Add(index);
            }
            Console.WriteLine("Key #2 : {0}", key);
            return keys;
        }

        static List<int> Decode(List<int> sr, int n, List<int> key)
        {
            int count = key.Count;
            List<int> ans = new List<int>();
            for (int i = 0; i < sr.Count; i++)
                ans.Add((n + sr[i] - key[i % count]) % n);
            return ans;
        }

        static List<int> Encode(List<int> sr, int n, List<int> key)
        {
            int count = key.Count;
            List<int> ans = new List<int>();
            for (int i = 0; i < sr.Count; i++)
            {
                ans.Add((sr[i] + key[i % count]) % n);
            }            
            return ans;
        }

        static double I(List<int> count, int n)
        {
            double ans = 0;
            for (int i = 0; i < count.Count; i++)
                ans += count[i] * (count[i] - 1);
            ans /= n * (n - 1);
            return ans;
        }

        static List<int> Count(List<int> sr, int n)
        {
            List<int> count = new List<int>();
            for (int i = 0; i < n; i++)
                count.Add(0);
            for (int j = 0; j < sr.Count; j++)
                count[sr[j]]++;
            return count;
        }

        static void Print(List<int> cyph, List<int> key, int r, string alph, int n)
        {
            for (int i = 0; i < cyph.Count; i++)
                Console.Write(alph[(n + cyph[i] - key[i % r]) % n]);
            Console.WriteLine();
        }

        static List<int> Translate(StreamReader sr, string alph)
        {
            List<int> ind = new List<int>();
            char letter;
            while (!sr.EndOfStream)
            {
                letter = (char)sr.Read();
                for (int i = 0; i < alph.Length; i++)
                    if (letter == alph[i])
                        ind.Add(i);
            }
            return ind;
        }

        static void FilterText(StreamReader sr)
        {
            var sw = File.CreateText("TEXT");
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
                }
                text = text.Replace(" ", string.Empty);
                sw.Write(text);
            }
            sw.Close();
        }
    }
}
