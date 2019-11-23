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
            double tCoefficient = 0;
            double minTemperature = 0;
            double maxTemperature = 0;
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
                Console.WriteLine("3. Podaj parametry do symulowanego wyżarzania");
                Console.WriteLine("4. Rozwiąż problem komiwojażera za pomocą metody symulowanego wyżarzania");
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
                            Console.Write("Podaj współczynnik wyżarzania z zakresu [0; 1): ");
                            tCoefficient = double.Parse(Console.ReadLine());
                            Console.Write("Wprowadź początkową temperaturę wyżarzania (większą od 0): ");
                            maxTemperature = double.Parse(Console.ReadLine());
                            Console.Write("Wprowadź minimalną temperaturę wyżarzania (większą od 0): ");
                            minTemperature = double.Parse(Console.ReadLine());
                            Console.Write("\nAby kontynuować kliknij [ENTER]");
                            Console.ReadKey();
                            break;
                        }
                    case 4:
                        {
                            Console.Clear();
                            SimulatedAnnealing sa = new SimulatedAnnealing(g.Filename, choice);
                            sa.StartSA(minTemperature, maxTemperature, tCoefficient);
                            Console.WriteLine("Średni czas w ms: " + sa.Time);
                            //Console.WriteLine("Średnia waga cyklu: " + sa.TotalCost);
                            Console.WriteLine("Najlepszy, oszacowany cykl ma wagę: " + sa.BestCycleCost);
                            Console.WriteLine("\nOszacowana ścieżka:");
                            sa.Route.Display();
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
