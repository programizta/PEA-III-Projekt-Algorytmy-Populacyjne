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
        private bool isParent;

        public Individual(int[] path, int pathCost)
        {
            Path = new int[path.Length];
            isParent = false;
            PathCost = pathCost;
            CopyFromTo(path, Path);
        }

        public Individual(Individual individual)
        {
            Path = new int[individual.Path.Length];
            isParent = individual.isParent;
            PathCost = individual.PathCost;
            CopyFromTo(individual.Path, Path);
        }

        public void SetAsParent()
        {
            isParent = true;
        }

        public bool IsItParent()
        {
            return isParent;
        }

        private void CopyFromTo(int[] from, int[] to)
        {
            for (int i = 0; i < from.Length; i++)
            {
                to[i] = from[i];
            }
        }

        public override bool Equals(object obj)
        {
            var individual = obj as Individual;
            return individual != null &&
                   EqualityComparer<int[]>.Default.Equals(Path, individual.Path) &&
                   PathCost == individual.PathCost &&
                   isParent == individual.isParent;
        }

        public override int GetHashCode()
        {
            var hashCode = -706048126;
            hashCode = hashCode * -1521134295 + EqualityComparer<int[]>.Default.GetHashCode(Path);
            hashCode = hashCode * -1521134295 + PathCost.GetHashCode();
            hashCode = hashCode * -1521134295 + isParent.GetHashCode();
            return hashCode;
        }
    }
}
