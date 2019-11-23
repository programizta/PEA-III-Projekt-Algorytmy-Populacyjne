using System;
using System.IO;

namespace III_Projekt
{
    class Graph
    {
        protected int numOfCities;
        protected int[,] costMatrix;
        public int BestCycleCost { get; protected set; }
        string[] allNumbers;
        public string _absoluteDirectory { get; protected set; }
        int xPosition;
        int yPosition;
        bool endParsingFromFile;
        bool startedParsingNumbers;
        public string Filename { get; protected set; }

        public Graph() { }

        public Graph(string filename, int choice)
        {
            BestCycleCost = int.MaxValue;
            Filename = filename;
            numOfCities = 0;
            xPosition = 0;
            yPosition = 0;
            endParsingFromFile = false;
            _absoluteDirectory = Environment.CurrentDirectory + "\\..\\..\\Macierze PEA\\Grafy\\";
            if (choice == 0) ParseSmallGraph(_absoluteDirectory, filename);
            else ParseLargeGraph(_absoluteDirectory, filename);
        }

        /// <summary>
        /// parser grafów o małej liczbie wierzchołków
        /// (inna struktura pliku tekstowego)
        /// </summary>
        /// <param name="absoluteDirectory"></param>
        /// <param name="filename"></param>
        protected void ParseSmallGraph(string absoluteDirectory, string filename)
        {
            absoluteDirectory += filename;
            char[] separators = { ' ', '\r' };

            try
            {
                string allText = File.ReadAllText(absoluteDirectory);
                allNumbers = allText.Split(separators);
                if (allNumbers is null || allNumbers.Equals("")) throw new Exception("Taki plik nie istnieje!");

                int.TryParse(allNumbers[0], out numOfCities);
                costMatrix = new int[numOfCities, numOfCities];
                CreateCostMatrix();
                SetInfinityOnInaccesiblePlaces();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected void SetInfinityOnInaccesiblePlaces()
        {
            for (int i = 0; i < numOfCities; i++)
            {
                costMatrix[i, i] = int.MaxValue;
            }
        }

        /// <summary>
        /// parser grafów o dużej liczbie wierzchołków
        /// (inna struktura pliku tekstowego)
        /// </summary>
        /// <param name="absoluteDirectory"></param>
        /// <param name="filename"></param>
        protected void ParseLargeGraph(string absoluteDirectory, string filename)
        {
            absoluteDirectory += filename;
            bool parsedNumOfCities = false;

            try
            {
                using (StreamReader reader = new StreamReader(absoluteDirectory))
                {
                    do
                    {
                        string[] lineTextArray = reader.ReadLine().ToString().Split(' ');

                        if (!parsedNumOfCities)
                        {
                            bool tryToParse = TryToParseNumberOfCities(lineTextArray);
                            if (tryToParse)
                            {
                                costMatrix = new int[numOfCities, numOfCities];
                                parsedNumOfCities = true;
                            }
                        }

                        if (parsedNumOfCities)
                        {
                            ParseNumbersToMatrix(lineTextArray);
                        }
                    } while (!endParsingFromFile);

                    SetInfinityOnInaccesiblePlaces();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected bool TryToParseNumberOfCities(string[] lineTextArray)
        {
            bool success = false;
            foreach (string word in lineTextArray)
            {
                if (word.Contains("DIMENSION:")) success = true;
                if (success)
                {
                    if (int.TryParse(word, out numOfCities)) return true;
                }
            }
            return false;
        }

        protected void ParseNumbersToMatrix(string[] lineTextArray)
        {
            foreach (string word in lineTextArray)
            {
                if (word.Contains("EDGE_WEIGHT_SECTION")) startedParsingNumbers = true;
                if (startedParsingNumbers)
                {
                    if (int.TryParse(word, out costMatrix[yPosition, xPosition]))
                    {
                        if (xPosition < numOfCities - 1) xPosition++;
                        else
                        {
                            xPosition = 0;
                            yPosition++;
                        }
                    }
                }

                if (xPosition == numOfCities - 1 && yPosition == numOfCities - 1)
                {
                    endParsingFromFile = true;
                }
            }
        }

        protected void CreateCostMatrix()
        {
            bool success = false;
            int aux = 1;
            for (int i = 0; i < numOfCities;)
            {
                for (int j = 0; j < numOfCities;)
                {
                    success = int.TryParse(allNumbers[aux], out costMatrix[i, j]);
                    aux++;

                    if (success) j++;
                }
                if (success) i++;
            }
        }

        public int GetNumberOfCities()
        {
            return numOfCities;
        }

        public void DisplayCostMatrix()
        {
            for (int i = 0; i < numOfCities; i++)
            {
                for (int j = 0; j < numOfCities; j++)
                {
                    if (costMatrix[i, j] == int.MaxValue) Console.Write(0 + " ");
                    else Console.Write(costMatrix[i, j] + " ");
                }
                Console.Write(Environment.NewLine);
            }
        }
    }
}
