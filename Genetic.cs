using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace III_Projekt
{
    public static class ArrayExtensions
    {
        public static IEnumerable<T> GetRow<T>(this T[,] items, int row)
        {
            for (var i = 0; i < items.GetLength(1); i++)
            {
                yield return items[row, i];
            }
        }
    }

    class Genetic : Graph
    {
        Random randomGenerator;
        readonly int interruptionTime;
        int sizeOfPopulation;
        float mutationProbability;
        float crossProbability;

        /// <summary>
        /// lista reprezentująca populację (ścieżkę oraz koszt)
        /// </summary>
        public List<Individual> Population { get; set; }

        public Genetic(int interruptionTime,
            int sizeOfPopulation,
            float mutationProbability,
            float crossProbability)
        {
            randomGenerator = new Random();
            this.interruptionTime = interruptionTime;
            this.sizeOfPopulation = sizeOfPopulation;
            this.mutationProbability = mutationProbability;
            this.crossProbability = crossProbability;

            Population = new List<Individual>();
        }

        /// <summary>
        /// Jest to metoda generująca permutacje (populację) bez powtórzeń losowanych miast
        /// stosując algorytm przemieszania Fishera-Yatesa
        /// </summary>
        /// <param name="populationPaths"></param>
        private void CreatePopulation()
        {
            int numOfIndexes;
            int generatedIndex;
            int[] auxIndividual = new int[numOfCities];

            for (int i = 0; i < sizeOfPopulation; i++)
            {
                numOfIndexes = numOfCities;

                while (numOfIndexes > 1)
                {
                    numOfIndexes--;
                    generatedIndex = randomGenerator.Next(numOfIndexes + 1);
                    int number = auxIndividual[generatedIndex];
                    auxIndividual[generatedIndex] = auxIndividual[numOfIndexes];
                    auxIndividual[numOfIndexes] = number;
                }

                int auxCost = GetPathLength(auxIndividual);
                Individual individual = new Individual(auxIndividual, auxCost);
                Population.Add(individual);
            }
        }

        /// <summary>
        /// Metoda generująca mutację dla wybranego osobnika w populacji
        /// </summary>
        /// <param name="individual"></param>
        /// <param name="numOfIndividualInPopulation"></param>
        private void Mutate(Individual individual)
        {
            int firstIndex = randomGenerator.Next(0, numOfCities - 1);
            int secondIndex;
            int auxNumber;

            do
            {
                secondIndex = randomGenerator.Next(0, numOfCities - 1);
            } while (firstIndex == secondIndex);

            auxNumber = individual.Path[firstIndex];
            individual.Path[firstIndex] = individual.Path[secondIndex];
            individual.Path[secondIndex] = auxNumber;
            individual.PathCost = GetPathLength(individual.Path);
        }

        private int GetPathLength(int[] arrayOfIndexes)
        {
            int weightOfPath = 0;

            for (int i = 0; i < numOfCities - 1; i++)
            {
                weightOfPath += costMatrix[arrayOfIndexes[i], arrayOfIndexes[i + 1]];
            }
            weightOfPath += costMatrix[arrayOfIndexes[numOfCities - 1], arrayOfIndexes[0]];

            return weightOfPath;
        }

        /// <summary>
        /// Metoda odpowiedzialna za utworzenie potomka według algorytmu krzyżowania PMX
        /// </summary>
        /// <param name="firstParent"></param>
        /// <param name="secondParent"></param>
        private Individual PMXChildCreation(Individual firstParent, Individual secondParent)
        {
            bool[] visitedCities = new bool[numOfCities];
            int[] pathForChildren = new int[numOfCities];

            for (int i = 0; i < numOfCities; i++)
            {
                visitedCities[i] = false;
            }

            int parentChoice = randomGenerator.Next();

            int firstIndexOfCutPoint;
            int secondIndexOfCutPoint;
            int costOfChildsPath;

            do
            {
                firstIndexOfCutPoint = randomGenerator.Next(0, numOfCities - 1);
            } while (numOfCities - firstIndexOfCutPoint <= 2); // ?

            secondIndexOfCutPoint = randomGenerator.Next(firstIndexOfCutPoint, numOfCities - 1);

            if (parentChoice % 2 == 0)
            {
                for (int i = firstIndexOfCutPoint; i < secondIndexOfCutPoint; i++)
                {
                    visitedCities[i] = true;
                    pathForChildren[i] = firstParent.Path[i];
                }

                for (int i = 0; i < numOfCities; i++)
                {
                    if (!visitedCities[i])
                    {
                        visitedCities[i] = true;
                        pathForChildren[i] = secondParent.Path[i];
                    }
                }

                costOfChildsPath = GetPathLength(pathForChildren);
                return new Individual(pathForChildren, costOfChildsPath);
            }

            for (int i = firstIndexOfCutPoint; i < secondIndexOfCutPoint; i++)
            {
                visitedCities[i] = true;
                pathForChildren[i] = secondParent.Path[i];
            }

            for (int i = 0; i < numOfCities; i++)
            {
                if (!visitedCities[i])
                {
                    visitedCities[i] = true;
                    pathForChildren[i] = firstParent.Path[i];
                }
            }

            costOfChildsPath = GetPathLength(pathForChildren);
            return new Individual(pathForChildren, costOfChildsPath);
        }

        public void StartGeneticAlgorithm(/*populacja, czas stopu, liczba pokoleń*/)
        {
            //utwórz populację
            //wystartuj czasomierz
            /*while(czasomierz <= czas stopu lub pokolenie <= liczba pokoleń)
             {
                for (kilka losowych iteracji)
                {
                    krzyżowanie dwóch losowych osobników rodzicielskich (OX lub PMX)
                    utworzenie nowego osobnika po krzyżowaniu
                }
                mutacja losowego osobnika z odpowiednim prawdopodobieństwem
                sortowanie rosnące osobników względem kosztów cykli hamiltona
                usuwamy losową liczbę najgorszych osobników

                
                inkrementacja numeru pokolenia
             }

             */

        }
    }
}
