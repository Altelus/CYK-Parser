using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CYK
{
    class Program
    {
        static void Main(string[] args)
        {
            // cnf reading
            Dictionary<string, List<string>> cnfDictionary = new Dictionary<string, List<string>>();

            //string[] lines = System.IO.File.ReadAllLines(@"D:\Users\Altelus\Desktop\cnf1.txt");
            string[] lines = System.IO.File.ReadAllLines(@"D:\Users\Altelus\Desktop\cnf2.txt");
            //string[] lines = System.IO.File.ReadAllLines(@"D:\Users\Altelus\Desktop\cnf3.txt");
            //string[] lines = System.IO.File.ReadAllLines(@"D:\Users\Altelus\Desktop\cnf4.txt");

            foreach (string line in lines)
            {
                string[] words = Regex.Split(line, @" -> ");
                //string[] words = Regex.Split(line, @"[\s\n]");

                // S -> AB BC...
                // 0 1  2  3

                List<string> values;
                // Add an entry
                if (cnfDictionary.ContainsKey(words[1]))
                {
                    values = cnfDictionary[words[1]];

                    // don't add if already exists
                    if (!values.Contains(words[0]))
                    {
                        values.Add(words[0]);
                        cnfDictionary[words[1]] = values;
                    }
                }
                else // new entry
                {
                    values = new List<string>();
                    values.Add(words[0]);

                    cnfDictionary.Add(words[1], values);
                }
               

                //// S -> AB BC...
                //// 0 1  2  3
                //for (int i = 2; i < words.Length; i++)
                //{
                //    List<string> values;

                //    // Add an entry
                //    if (cnfDictionary.ContainsKey(words[i]))
                //    {
                //        values = cnfDictionary[words[i]];

                //        // don't add if already exists
                //        if (!values.Contains(words[0]))
                //        {
                //            values.Add(words[0]);
                //            cnfDictionary[words[i]] = values;
                //        }
                //    }
                //    else // new entry
                //    {
                //        values = new List<string>();
                //        values.Add(words[0]);

                //        cnfDictionary.Add(words[i], values);
                //    }
                //}
            }

            // CYK

            //cnf1
            //string s = "b a a b a";

            //cnf2
            string s = "I saw the man with the telescope";

            //cnf3
            //string s = "jeff trains geometry students";

            
            //cnf3
            //string s = "time flies like an arrow";
           
            string[] t = Regex.Split(s, @"[\s\n]");

            List<string> [][] triTable = new List<string>[t.Length][];

            for (int i = 0; i < triTable.Length; i++)
            {
                triTable[i] = new List<string>[t.Length];
                for (int j = 0; j < triTable[i].Length; j++)
                {
                    triTable[i][j] = new List<string>(10);
                }
            }

            // construct first row
            for (int j = 0; j < triTable[0].Length; j++)
            {
                if (cnfDictionary.ContainsKey(t[j]))
                {
                    triTable[0][j] = cnfDictionary[t[j]];
                }
                else
                {
                    triTable[0][j] = new List<string>();
                    triTable[0][j].Add("-");
                }
            }

            // rows
            for (int row = 1; row < t.Length; row++)
            {
                //cols
                for (int col = 0; col < t.Length - row; col++)
                {
                    // {Xi1,Xj1} U {Xi2,Xj2} == Xi1Xi2 Xi1Xj2 Xj1Xi2 Xj1Xj2
                    int i1 = row - 1;
                    int j1 = col;

                    int i2 = 0;
                    int j2 = row + col;

                    for (int comparisons = 0; comparisons < row; comparisons++)
                    {
                        List<string> set1 = triTable[i1][j1];
                        List<string> set2 = triTable[i2][j2];

                        //sets EG. NP -> DET N
                        if (set1.Count > 0 & set2.Count > 0)
                        {
                            foreach (string set1_rule in set1)
                            {
                                foreach (string set2_rule in set2)
                                {
                                    // {B} {A,C} = {BA, BC}
                                    if (cnfDictionary.ContainsKey(set1_rule + " " + set2_rule))
                                    {
                                        foreach (string value in cnfDictionary[set1_rule + " " + set2_rule])
                                        {
                                            if (!triTable[row][col].Contains(value))
                                                triTable[row][col].Add(value);
                                        }
                                    }
                                }
                            }
                        }
                        //singles EG. NP -> N
                        if (set1.Count > 0)
                        {
                            foreach (string set_rule in set1)
                            {
                                if (cnfDictionary.ContainsKey(set_rule))
                                {
                                    foreach (string value in cnfDictionary[set_rule])
                                    {
                                        if (!triTable[row][col].Contains(value))
                                            triTable[row][col].Add(value);
                                    }
                                }
                            }
                        }
                        //singles EG. NP -> N
                        if (set2.Count > 0)
                        {
                            foreach (string set_rule in set2)
                            {
                                if (cnfDictionary.ContainsKey(set_rule))
                                {
                                    foreach (string value in cnfDictionary[set_rule])
                                    {
                                        if (!triTable[row][col].Contains(value))
                                            triTable[row][col].Add(value);
                                    }
                                }
                            }
                        }

                        //for (int setA = 0; setA < triTable[i][i].Count; setA++)
                        //{
                        //    string setA_rule = triTable[i][i][setA];

                        //    for (int setB = 0; setB < triTable[j][i + 1].Count; setB++)
                        //    {
                        //        string setB_rule = triTable[j][i + 1][setB];

                        //        // {b} {A,C} = {BA, BC}
                        //        if (cnfDictionary.ContainsKey(setA_rule + setB_rule))
                        //        {
                        //            foreach (string value in cnfDictionary[setA_rule + setB_rule])
                        //            {
                        //                if (!triTable[row][col].Contains(value))
                        //                    triTable[row][col].Add(value);
                        //            }
                        //        }

                        //    }
                        //}

                        i1--;
                        i2++;
                        j2--;
                    }
                        //Console.WriteLine("BLEIGH!");
                }
            }

            if  (triTable[t.Length-1][0].Contains("S"))
                Console.WriteLine("VALID CFG");

        }
    }
}
