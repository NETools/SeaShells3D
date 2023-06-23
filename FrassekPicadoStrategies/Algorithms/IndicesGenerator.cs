using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrassekPicadoStrategies.Algorithms
{
    internal class SurfaceGenerator
    {
        public static List<int> GenerateShellSurfaceTopology(int maximumEllipseSamples, int numVertices)
        {
            if (numVertices == 0) return new List<int>();

            List<int> _indices = new List<int>();

            for (int i = 0; i < numVertices - maximumEllipseSamples; i++)
            {
                int cornerA = GaloisIndex(i, 1, 0, maximumEllipseSamples); // 1
                int cornerB = GaloisIndex(i, 0, 0, maximumEllipseSamples); // 0
                int cornerC = GaloisIndex(i, 0, 1, maximumEllipseSamples); // 8
                int cornerD = GaloisIndex(cornerA, 0, 1, maximumEllipseSamples); // 9

                _indices.Add(cornerA); // 1
                _indices.Add(cornerB); // 0
                _indices.Add(cornerC); // 8

                _indices.Add(cornerD); // 9
                _indices.Add(cornerC); // 8
                _indices.Add(cornerA); // 1
            }

            if (_indices.Count == 0) return new List<int>();

            return _indices;
        }

        private static int GetHeight(int index, int n)
        {
            return (int)(index / n);

            /*
             * i = 0
             * (int)(0 / 8) = 0
             * 
             * i = 1
             * 
             * (int)(1 / 8) = 0
             * 
             * i = 2
             * 
             * (int)(2 / 8) = 0
             * 
             * 
             * 
             * (int)(7 / 8) = 0
             * 
             * 
             * 
             * i = 9
             * 
             * (int)(9 / 8) = (int)(1) = 1
             * 
             */


        }
        private static int GaloisIndex(int currentIndex, int increaseLeftRight, int increaseUpDown, int n)
        {
            int currentHeight = GetHeight(currentIndex, n);
            int baseIndex = currentHeight * n; // n = samples (also 8) ==> 1 * 8 = 8

            int relativeIndex = currentIndex - baseIndex; // 9 - 8 = 1
            int adjustedIndexLeftRight = Mod(relativeIndex + increaseLeftRight, n);

            // (1 + 1 == 2) * 8   == 16                                       + 1 = 17
            int trueIndex = (currentHeight + increaseUpDown) * n + adjustedIndexLeftRight;

            return trueIndex;
        }

        private static int Mod(int a, int b)
        {
            if (a >= 0)
                return a % b;
            else return b - Mod(-a, b);
        }

    }
}
