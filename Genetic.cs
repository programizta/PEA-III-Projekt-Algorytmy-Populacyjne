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
        int[,] allIndividuals; // macierz, która reprezentuje każdego osobnika (cykl Hamiltona) w populacji
        int[] populationCosts; // tablica przechowująca wartości cykli Hamiltona dla każdego osobnika w populacji

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

            allIndividuals = new int[sizeOfPopulation, numOfCities];
            populationCosts = new int[sizeOfPopulation];
            CreateAndShuffle(allIndividuals);

            for (int i = 0; i < sizeOfPopulation; i++)
            {
                populationCosts[i] = GetPathLength(allIndividuals.GetRow(i).ToArray());
            }
        }

        /// <summary>
        /// Jest to metoda generująca permutacje bez powtórzeń losowanych miast
        /// stosując algorytm przemieszania Fishera-Yatesa
        /// </summary>
        /// <param name="populationPaths"></param>
        private void CreateAndShuffle(int[,] populationPaths)
        {
            int numOfIndexes;
            int generatedIndex;

            for (int i = 0; i < sizeOfPopulation; i++)
            {
                numOfIndexes = numOfCities;

                while (numOfIndexes > 1)
                {
                    numOfIndexes--;
                    generatedIndex = randomGenerator.Next(numOfIndexes + 1);
                    int number = populationPaths[i, generatedIndex];
                    populationPaths[i, generatedIndex] = populationPaths[i, numOfIndexes];
                    populationPaths[i, numOfIndexes] = number;
                }
            }
        }

        private void Mutate(int[] individual, int numOfIndividualInPopulation)
        {
            int firstIndex = randomGenerator.Next(0, numOfCities - 1);
            int secondIndex;
            int auxNumber;

            do
            {
                secondIndex = randomGenerator.Next(0, numOfCities - 1);
            } while (firstIndex == secondIndex);

            auxNumber = individual[firstIndex];
            individual[firstIndex] = individual[secondIndex];
            individual[secondIndex] = auxNumber;

            // oblicz koszt po permutacji wierzchołków!
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
    }
}
