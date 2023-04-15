using System;
using System.Collections.Generic;
using System.IO;

namespace Ants
{
    class Program
    {

        static int nr_orase;
        static int[,] distanta_orase;

        public static double Distanta(Oras oras1, Oras oras2)
        {
            int distantaX = Math.Abs(oras1.x - oras2.x);
            int distantaY = Math.Abs(oras1.y - oras2.y);

            return Math.Sqrt(distantaX * distantaX + distantaY * distantaY);
        }
        static void Main(string[] args)
        {
            double[,] temperatura;
            List<Oras>[] temperatura_curenta;
            List<Oras> temperatura_curenta_minima = new List<Oras>();
            ulong[] Lenght_path;
            ulong Lenght_path_min = ulong.MaxValue;
            ulong Lenght_path_Current;
            List<Oras> allCities;
            double defaultT = 10e-6;
            double Q = 100;//cantitate feromoni lasata de fiecare furnica
            int tMax = 50;
            List<double> p = new List<double>();
            int alfa = 1;//importanta feromonilor
            int beta = 10;//prioritatea distantei
            double ro = 0.5;//evaporare

            Oras[] k;
            Random rnd = new Random();

            nr_orase = 20;
            temperatura = new double[nr_orase, nr_orase];
            distanta_orase = new int[nr_orase, nr_orase];
            k = new Oras[nr_orase];
            temperatura_curenta = new List<Oras>[nr_orase];
            Lenght_path = new ulong[nr_orase];

            for (int i = 0; i < nr_orase; i++)
            {
                temperatura_curenta[i] = new List<Oras>();
            }
            allCities = new List<Oras>();

            using (StreamReader sr = new StreamReader("C:\\Users\\user\\Desktop\\laborator_2_marti_ora_8\\laborator4\\Ants\\Ants\\Orase.txt"))
            {
                int i = 0;
                while (sr.Peek() >= 0)
                {
                    string str;
                    string[] strArray;
                    str = sr.ReadLine();

                    strArray = str.Split(' ');
                    Oras oras = new Oras(Int32.Parse(strArray[0]), Int32.Parse(strArray[1]));

                    allCities.Add(oras);
                    k[i] = oras;

                    i++;

                }
            }

            for (int i = 0; i < nr_orase; i++)
            {
                for (int j = 0; j < nr_orase; j++)
                {
                    temperatura[i, j] = defaultT;
                    distanta_orase[i, j] = (int)Distanta(allCities[i], allCities[j]);
                }
            }


            for (int i = 0; i < nr_orase; i++)
            {
                int ind = rnd.Next(0, nr_orase - 1);
                Oras start = allCities[ind];

                for (int j = 0; j < nr_orase; j++)
                {
                    int r = rnd.Next(i, nr_orase - 1);
                    Oras tmp = k[r];
                    k[r] = k[i];
                    k[i] = tmp;
                }

                int index = 0;

                for (int l = 0; l < nr_orase; l++)
                {
                    if (k[l] == start)
                    {
                        index = l;
                    }
                }

                Oras temp = k[0];
                k[0] = k[index];
                k[index] = temp;
            }

            for (int i = 0; i < nr_orase; i++)
            {
                temperatura_curenta[i].Add(k[i]);
            }

            for (int tt = 0; tt < tMax; tt++)
            {
                for (int kk = 0; kk < nr_orase; kk++)
                {
                    for (int s = 0; s < nr_orase - 1; s++)
                    {
                        Oras i = k[kk];
                        p.Clear();

                        List<Oras> unvisitedCities = new List<Oras>();
                        foreach (var e in allCities)
                        {
                            if (!temperatura_curenta[kk].Contains(e))
                            {
                                unvisitedCities.Add(e);
                            }
                        }

                        double sum = 0;

                        foreach (Oras l in unvisitedCities)
                        {
                            sum += Math.Pow(temperatura[allCities.IndexOf(i), allCities.IndexOf(l)], alfa) * Math.Pow(1.0 / distanta_orase[allCities.IndexOf(i), allCities.IndexOf(l)], beta);
                        }

                        foreach (Oras j in unvisitedCities)
                        {
                            p.Add((Math.Pow(temperatura[allCities.IndexOf(i), allCities.IndexOf(j)], alfa) * Math.Pow(1.0 / distanta_orase[allCities.IndexOf(i), allCities.IndexOf(j)], beta)) / sum);
                        }
                        //generam o probab random pe care o comparam cu p calculat mai sus, in functie de asta hotaram daca adaugam orasul din unvisited in tur
                        double rndProb = rnd.NextDouble();
                        double probSum = 0;
                        for (int pCt = 0; pCt < p.Count; pCt++)
                        {
                            probSum += p[pCt];
                            if (rndProb <= probSum)
                            {
                                temperatura_curenta[kk].Add(unvisitedCities[pCt]);
                                k[kk] = unvisitedCities[pCt];
                                break;
                            }
                        }
                    }
                }

                Lenght_path_Current = ulong.MaxValue;
                for (int i = 0; i < nr_orase; i++)
                {
                    Lenght_path[i] = 0;
                    for (int ii = 0; ii < temperatura_curenta[i].Count - 1; ii++)
                    {
                        Lenght_path[i] += (ulong)distanta_orase[allCities.IndexOf(temperatura_curenta[i][ii]), allCities.IndexOf(temperatura_curenta[i][ii + 1])];

                    }

                    Console.WriteLine(Lenght_path[i]);
                    if (Lenght_path[i] < Lenght_path_min)
                    {
                        Lenght_path_min = Lenght_path[i];
                        temperatura_curenta_minima.Clear();
                        foreach (var item in temperatura_curenta[i])
                        {
                            temperatura_curenta_minima.Add(item);
                        }
                    }
                    if (Lenght_path[i] < Lenght_path_Current)
                    {
                        Lenght_path_Current = Lenght_path[i];
                    }

                }
                for (int i = 0; i < nr_orase; i++)
                {
                    for (int j = 0; j < nr_orase; j++)
                    {
                        double sum = 0;
                        for (int kk = 0; kk < nr_orase; kk++)
                        {
                            if (temperatura_curenta[kk].IndexOf(allCities[i]) == (temperatura_curenta[kk].IndexOf(allCities[j]) - 1))
                            {
                                sum += Q / Lenght_path[kk];
                            }
                        }
                        temperatura[i, j] = (1 - ro) * temperatura[i, j] + sum + (temperatura_curenta_minima.IndexOf(allCities[i]) == (temperatura_curenta_minima.IndexOf(allCities[j]) - 1) ? Q / Lenght_path_min : 0);
                    }
                }
                for (int i = 0; i < nr_orase; i++)
                {
                    temperatura_curenta[i].Clear();
                    temperatura_curenta[i].Add(k[i]);
                }
            }

            Console.WriteLine("L = " + Lenght_path_min);//Lk
            foreach (Oras c in temperatura_curenta_minima)
            {
                Console.WriteLine(c.x.ToString() + " " + c.y.ToString());
            }

            Console.ReadLine();

        }
    }
}
