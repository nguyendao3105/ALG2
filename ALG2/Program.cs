using System;
using System.Collections.Generic;
namespace ALG2
{
    //MatchString abstract Strategy
    public abstract class MatchStringAlgorithms
    {
        public abstract void MatchString(string Pattern, string Text);
    }
    public class NaiveMatchString : MatchStringAlgorithms //Naive matchstring
    {
        public override void MatchString(string Pattern, string Text)
        {
            int n = Text.Length;
            int m = Pattern.Length;
            if(n < m)
            {
                Console.WriteLine("Cannot find " + Pattern + " in " + Text);
            }
            for(int i = 0; i < n - m; i++)
            {
                if(Text[i] == Pattern[0])
                {
                    bool match = true;
                    for(int j = i + 1; j < i+m; j++)
                    {
                        if(Text[j] != Pattern[j - i])
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match == true)
                    {
                        Console.WriteLine(Pattern + " matches " + Text + " at position " + (i + 1).ToString());
                        return;
                    }
                }
            }
            Console.WriteLine("Cannot find " + Pattern + " in " + Text);
        }
    }
    public class RabinKarp : MatchStringAlgorithms //RabinKarp MatchString
    {

        public override void MatchString(string P, string T)
        {
            int basee;
            int q;
            string userInput;
            Console.Write("Input Base: ");
            userInput = Console.ReadLine();
            basee = Convert.ToInt32(userInput);
            Console.Write("Input q: ");
            userInput = Console.ReadLine();
            q = Convert.ToInt32(userInput);
            RabinKarpMS(P, T, basee, q);
        }
        long Remainder(long left, long right)
        {
            long result = left - (left / right) * right;
            return (result > 0) ? result : right + result;
        }
        long Pow(long basee, long exp)
        {
            long result = 1;
            for (int i = 0; i < exp; i++)
            {
                result = result * basee;
            }
            return result;
        }
        private void RabinKarpMS(string Pattern, string Text, int basee, int q)
        {
            int n = Text.Length;
            int m = Pattern.Length;

            long h = Remainder(Pow(basee, m - 1) ,q); //padding for rolling hash
            long hashValue = 0; //hash value for t
            long p = 0; //hash value for P

            for(int i = 0; i < m; i++)
            {
                p =  Remainder(basee * p + Convert.ToInt64(Pattern[i]), q);
               hashValue = Remainder(basee * hashValue + Convert.ToInt64(Text[i]), q);
            }

            for(int shift = 0; shift < n-m; shift++)
            {
                if(p == hashValue) //There may be a hit
                {
                    bool match = true;
                    for (int j = shift + 1; j < shift + m; j++)
                    {
                        if (Text[j] != Pattern[j - shift])
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match == true)
                    {
                        Console.WriteLine(Pattern + " matches " + Text + " at position " + (shift + 1).ToString());
                    }
                }

                if(shift < n - m)
                {
                    hashValue = Remainder(basee * (hashValue - (Convert.ToInt64(Text[shift]))* h) + Convert.ToInt64(Text[shift + m]), q);
                }
            }
        }
    }
    //Add two number as string strategy
    public abstract class AddTwoNumberAsString
    {
        public abstract void Add(string number1, string number2);
    }
    public class Approach1 : AddTwoNumberAsString
    {
        public override void Add(string number1, string number2)
        {
            int n = number1.Length;
            int m = number2.Length;
            int delta = n - m;

            Stack<string> sum = new Stack<string>();

            int excess = 0;//so nho
            int cursor = n - 1;//cursor to 2 string

            while (cursor >= 0)
            {
                if (number1[cursor] != ',')
                {
                    int nbr1 = Convert.ToInt32(number1[cursor]) - 48;
                    int nbr2 = (cursor - delta >= 0) ? Convert.ToInt32(number2[cursor - delta]) - 48 : 0;//if length of nbr1 > nbr 2
                    int s = nbr1 + nbr2 + excess;
                    excess = (nbr1 + nbr2) / 10;
                    sum.Push((s % 10).ToString());
                }
                cursor--;
            }

            if (m > n)//length of nbr2 > nbr1
            {
                int idxLast = m - n - 1;
                idxLast = (number2[idxLast] != ',') ? idxLast - 1 : idxLast;
                //process excess amount
                int s = Convert.ToInt32(number2[cursor - n]) - 48 + excess;
                sum.Push(s.ToString());
                idxLast--;

                while (idxLast >= 0)
                {
                    if (number2[idxLast] != ',')
                    {
                        sum.Push((number2[idxLast]).ToString());
                    }
                    idxLast--;
                }
            }
            else
            {
                if (excess > 0) sum.Push(excess.ToString());
            }

            //Print Result
            Console.WriteLine(" " + number1);
            Console.WriteLine("+");
            Console.WriteLine(" " + number2);
            for (int i = 0; i < ((n > m) ? n : m) + 1; i++)
            {
                Console.Write("_");
            }
            Console.WriteLine();
            Console.Write(" ");
            while (sum.Count > 0)
            {
                if (sum.Count % 3 == 0) Console.Write(',');
                Console.Write(sum.Pop());
            }
        }
    }
    //Context
    public class TwoStringProcessor
    {
        MatchStringAlgorithms _matchStringAlgorithms;
        AddTwoNumberAsString _addStringAlgorithms;

        String _p;
        string _t;
        public TwoStringProcessor(MatchStringAlgorithms matchStringAlgorithms)
        {
            _matchStringAlgorithms = matchStringAlgorithms;
        }

        public TwoStringProcessor(AddTwoNumberAsString addStringAlgorithms)
        {
            _addStringAlgorithms = addStringAlgorithms;
        }

        public string P { get => _p; set => _p = value; }
        public string T { get => _t; set => _t = value; }

        //Strategy Interface

        public void MatchString()
        {
            try
            {
                _matchStringAlgorithms.MatchString(P, T);
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void AddString()
        {
            try
            {
                _addStringAlgorithms.Add(P, T);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            TwoStringProcessor alg2;

            //MatchString
            /*
            alg2 = new TwoStringProcessor(new NaiveMatchString());
            alg2.P = "abcaaa";
            alg2.T = "hcnslcmabccabcaaaddeff";
            alg2.MatchString();

            alg2 = new TwoStringProcessor(new RabinKarp());
            alg2.P = "abcaaa";
            alg2.T = "hcnslcmabccabcaaaddeff";
            alg2.MatchString();
            */

            //Add two number as string
            alg2 = new TwoStringProcessor(new Approach1());
            alg2.P = "12,334,213,415,453,245,654,635,626,543,251,341,234";
            alg2.T = "32,434,743,211,234,678,000,223,213,763,318,369,232";
            alg2.AddString();
        }
    }
}
