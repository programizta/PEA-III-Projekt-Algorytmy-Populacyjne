using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace III_Projekt
{
    class Individual
    {
        public int[] Path { get; set; }
        public int PathCost { get; set; }

        public Individual(int[] path, int pathCost)
        {
            PathCost = pathCost;
            CopyFromTo(path, Path);
        }

        public Individual() { }

        private void CopyFromTo(int[] from, int[] to)
        {
            for (int i = 0; i < from.Length; i++)
            {
                to[i] = from[i];
            }
        }
    }
}
