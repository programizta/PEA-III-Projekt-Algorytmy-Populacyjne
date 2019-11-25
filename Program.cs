using System;
using System.Diagnostics;

namespace III_Projekt
{
    class Program
    {
        static string filename;
        static int choice;

        static void Main(string[] args)
        {
            Graph g = new Graph();
            string filename;
            int numOfGenerations = 0;
            int sizeOfPopulation = 0;
            double interruptionTime = 0;
            int numOfChoice;

            while (true)
            {
                Console.Clear();
                Console.Write("Program do wyznaczania szacowanego, optymalnego cyklu Hamiltona dla asymetrycznego problemu komiwojażera (ATSP)");

                if (g.GetNumberOfCities() != 0)
                {
                    Console.WriteLine("\n\nLiczba wierzchołków aktualnie wczytanego grafu: " + g.GetNumberOfCities());
                    Console.Write(Environment.NewLine);
                }
                else
                {
                    Console.WriteLine("\n\nAktualnie nie wczytano żadnego grafu");
                    Console.Write(Environment.NewLine);
                }

                Console.WriteLine("Możliwości do wyboru: ");
                Console.WriteLine("1. Wczytaj macierz grafu");
                Console.WriteLine("2. Wyświetl macierz kosztów");
                Console.WriteLine("3. Podaj parametry dla algorytmu genetycznego");
                Console.WriteLine("4. Rozwiąż problem komiwojażera algorytmem genetycznym");
                Console.WriteLine("5. Zakończ działanie programu\n");
                Console.Write("Którą opcję chcesz wybrać? Podaj numer: ");
                numOfChoice = int.Parse(Console.ReadLine());

                switch (numOfChoice)
                {
                    case 1:
                        {
                            choice = 0;
                            Console.Clear();
                            Console.Write("Podaj nazwę pliku z grafem: ");
                            filename = Console.ReadLine();
                            if (filename.Contains(".t")) choice = 0;
                            else choice = 1;
                            g = new Graph(filename, choice);
                            Console.Write("Wczytano graf z " + g.GetNumberOfCities() + " wierzchołkami\nAby kontynuować kliknij [ENTER]");
                            Program.filename = filename;
                            Console.ReadKey();
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            if (g.GetNumberOfCities() != 0) g.DisplayCostMatrix();
                            else Console.WriteLine("Nie wczytano żadnego grafu do programu!");
                            Console.Write("\nAby kontynuować kliknij [ENTER]");
                            Console.ReadKey();
                            break;
                        }
                    case 3:
                        {
                            Console.Clear();
                            Console.Write("Podaj czas działania algorytmu: ");
                            interruptionTime = double.Parse(Console.ReadLine());
                            Console.Write("Wprowadź początkową liczebność populacji: ");
                            sizeOfPopulation = int.Parse(Console.ReadLine());
                            Console.Write("Podaj liczbę pokoleń, która ma się urodzić: ");
                            numOfGenerations = int.Parse(Console.ReadLine());
                            Console.Write("\nAby kontynuować kliknij [ENTER]");
                            Console.ReadKey();
                            break;
                        }
                    case 4:
                        {
                            Console.Clear();
                            Genetic ga = new Genetic(interruptionTime, sizeOfPopulation, g.Filename, choice);
                            ga.StartGeneticAlgorithm(numOfGenerations);
                            Console.WriteLine("Najlepszy, oszacowany cykl ma wagę: " + ga.BestCycleCost);
                            Console.WriteLine("\nOszacowana ścieżka:");
                            ga.FinalRoute.Display();
                            Console.WriteLine("\nKoniec. Aby wrócić do głównego menu, kliknij dowolny klawisz...");
                            Console.ReadKey();
                            break;
                        }
                    case 5:
                        {
                            Console.Write("\nZakończono działanie programu\nAby kontynuować kliknij [ENTER]");
                            Console.ReadKey();
                            return;
                        }
                }
            }
        }
    }
}
