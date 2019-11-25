using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace III_Projekt
{
    /// <summary>
    /// Klasa, która umożliwia w prosty sposób porównywanie dwóch obiektów klasy Individual
    /// </summary>
    class ListComparer : IComparer<Individual>
    {
        public int Compare(Individual x, Individual y)
        {
            if (x.PathCost == 0 || y.PathCost == 0)
            {
                return 0;
            }

            return x.PathCost.CompareTo(y.PathCost);
        }
    }

    class Genetic : Graph
    {
        readonly ListComparer gg;
        public Stack FinalRoute { get; set; }
        Random randomGenerator;
        readonly int sizeOfPopulation;
        float mutationProbability;
        float crossProbability;
        int numberOfGenerations;

        /// <summary>
        /// lista reprezentująca populację (ścieżkę oraz koszt)
        /// </summary>
        public List<Individual> Population { get; set; }

        public Genetic(int sizeOfPopulation, string filename, int choice) : base(filename, choice)
        {
            gg = new ListComparer();
            FinalRoute = new Stack();
            randomGenerator = new Random();
            this.sizeOfPopulation = sizeOfPopulation;
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

            // wygenerowanie pierwszego cyklu
            for (int i = 0; i < numOfCities; i++)
            {
                auxIndividual[i] = i;
            }

            for (int i = 0; i < sizeOfPopulation; i++)
            {
                numOfIndexes = numOfCities;

                // przemieszanie Fishera-Yatesa
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
                individual.SetAsParent();
                Population.Add(individual);
            }

            // wstępna selekcja populacji
            PopulationSelection(Population);
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

        /// <summary>
        /// Metoda wyznaczająca długość cyklu Hamiltona
        /// </summary>
        /// <param name="arrayOfIndexes"></param>
        /// <returns></returns>
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
        /// Metoda, która oznacza wszystkie miasta jako nieodwiedzone w tablicy odwiedzin
        /// </summary>
        /// <param name="visitTable"></param>
        private void MarkAllCitiesAsUnvisited(bool[] visitTable)
        {
            for (int i = 0; i < visitTable.Length; i++)
            {
                visitTable[i] = false;
            }
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

            MarkAllCitiesAsUnvisited(visitedCities);

            int parentChoice = randomGenerator.Next(0, 2);

            int firstIndexOfCutPoint;
            int secondIndexOfCutPoint;
            int costOfChildsPath;

            do
            {
                firstIndexOfCutPoint = randomGenerator.Next(0, numOfCities - 1);
            } while (numOfCities - firstIndexOfCutPoint <= 2);

            secondIndexOfCutPoint = randomGenerator.Next(firstIndexOfCutPoint, numOfCities - 1);
            int firstIterator = firstIndexOfCutPoint;
            int secondIterator = secondIndexOfCutPoint - firstIndexOfCutPoint;

            // pierwszy fragment ścieżki potomka jest uzyskiwany od pierwszego rodzica
            if (parentChoice == 1)
            {
                for (int i = 0; i < secondIndexOfCutPoint - firstIndexOfCutPoint; i++)
                {
                    visitedCities[firstParent.Path[firstIterator]] = true;
                    pathForChildren[i] = firstParent.Path[firstIterator];
                    firstIterator++;
                }

                for (int i = 0; i < numOfCities; i++)
                {
                    if (!visitedCities[secondParent.Path[i]])
                    {
                        visitedCities[secondParent.Path[i]] = true;
                        pathForChildren[secondIterator] = secondParent.Path[i];
                        secondIterator++;
                    }
                }

                costOfChildsPath = GetPathLength(pathForChildren);
                return new Individual(pathForChildren, costOfChildsPath);
            }

            // w przeciwnym wypadku pierwszy fragment potomka jest uzyskiwany
            // od drugiego rodzica
            for (int i = 0; i < secondIndexOfCutPoint - firstIndexOfCutPoint; i++)
            {
                visitedCities[secondParent.Path[firstIterator]] = true;
                pathForChildren[i] = secondParent.Path[firstIterator];
                firstIterator++;
            }

            for (int i = 0; i < numOfCities; i++)
            {
                if (!visitedCities[firstParent.Path[i]])
                {
                    visitedCities[firstParent.Path[i]] = true;
                    pathForChildren[secondIterator] = firstParent.Path[i];
                    secondIterator++;
                }
            }

            costOfChildsPath = GetPathLength(pathForChildren);
            return new Individual(pathForChildren, costOfChildsPath);
        }

        /// <summary>
        /// Metoda odpowiedzialna za selekcję "najlepszych osobników" do późniejszej reprodukcji
        /// </summary>
        /// <param name="Population"></param>
        private void PopulationSelection(List<Individual> Population)
        {
            Population.Sort(gg);

            while (Population.Count() > (int)(sizeOfPopulation * 0.95))
            {
                Population.RemoveAt(Population.Count() - 1);
            }
        }

        /// <summary>
        /// Metoda, która oznacza wszystkich osobników w populacji jako potencjalnych rodzicieli
        /// </summary>
        /// <param name="population"></param>
        private void SetAllAsParents(List<Individual> population)
        {
            foreach (var individual in population)
            {
                individual.SetAsParent();
            }
        }

        /// <summary>
        /// Metoda rozwiązująca problem komiwojażera algorytmem genetycznym
        /// </summary>
        /// <param name="numOfGenerations"></param>
        public void StartGeneticAlgorithm(int numOfGenerations)
        {
            CreatePopulation();
            numberOfGenerations = 0;

            while (numberOfGenerations <= numOfGenerations)
            {
                int populationCount = Population.Count();
                int numOfChildrenToBorn = 50;
                Individual firstParent;
                Individual secondParent;

                for (int i = 0; i < numOfChildrenToBorn; i++)
                {
                    do
                    {
                        firstParent = new Individual(Population.ElementAt(randomGenerator.Next(0, populationCount - 1)));
                        secondParent = new Individual(Population.ElementAt(randomGenerator.Next(0, populationCount - 1)));
                    } while (firstParent.Equals(secondParent) && firstParent.IsItParent() && secondParent.IsItParent());
                    // sprawdzamy czy dwa wylosowane osobniki nie są takie same
                    // oraz sprawdzamy czy są rodzicami
                    crossProbability = (float)randomGenerator.NextDouble();

                    if (crossProbability >= 0.6)
                    {
                        Individual child = new Individual(PMXChildCreation(firstParent, secondParent));
                        Population.Add(child);
                    }
                }

                SetAllAsParents(Population);
                mutationProbability = (float)randomGenerator.NextDouble();

                if (mutationProbability > 0.01 && mutationProbability < 0.1)
                {
                    var individualToMutate = Population.ElementAt(randomGenerator.Next(0, Population.Count));
                    Mutate(individualToMutate);
                }

                PopulationSelection(Population);
                numberOfGenerations++;
            }

            var bestIndividual = Population.First();

            if (bestIndividual.PathCost < BestCycleCost)
            {
                for (int i = 0; i < numOfCities; i++)
                {
                    FinalRoute.Push(bestIndividual.Path[i]);
                }
                BestCycleCost = bestIndividual.PathCost;
            }
        }
    }
}
