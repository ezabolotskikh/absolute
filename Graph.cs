using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApplication1
{
    class Graph
    {
        private int[] IJ, H, L, numComp;
        private int m, vmax;
        public Graph(int[] I, int[] J)
        {
            int vmax = 0;
            this.m = I.Length;

            for (int i = 0; i < m; i++)
                if (I[i] > vmax) vmax = I[i];

            this.vmax = vmax;

            this.IJ = new int[m*2];
            this.H = new int[vmax + 1];
            this.L = new int[m];
            this.numComp = new int[vmax + 1];

            for (int k = 0; k < vmax + 1; k++)
            {
                H[k] = -1;
            }

            for (int k = 0; k < m; k++)
            {
                AddArc(I[k], J[k], k);
            }
        }
        public void AddArc(int i, int j, int l)
        {
            this.IJ[l] = i;
            this.IJ[2*m - l - 1] = j;
            this.L[l] = H[i];
            this.H[i] = l;
        }
        public void Print()
        {
            FileStream graph = new FileStream("d:\\graph.txt", FileMode.Create); 
            StreamWriter writer = new StreamWriter(graph); 
            writer.Write("digraph {"); 
            writer.WriteLine();
            for (int i = 0; i < this.m; i++)
                writer.Write(this.IJ[i] + "->" + this.IJ[2*m - i - 1] + ";");
            writer.Write("}");
            writer.Close();

            for (int v = 0; v < H.Length; v++)
            {
                int l = H[v];
                while (l != -1)
                {
                    Console.WriteLine(IJ[l] + "->" + IJ[2*m - l - 1]);
                    l = L[l];
                }
            }
        }
        public void Delete(int h, int arc)
        {
            if (H[h] == arc) //Если дуга в H
            {
                int temp = H[h];
                H[h] = L[H[h]];
                L[temp] = -1;
            }
            else
            {
                for (int k = H[h]; k != -1; k = L[k])
                {
                    if (L[k] == arc)
                    {
                        int temp = L[k];
                        L[k] = L[L[k]];
                        L[temp] = -1;
                    }
                }
            }
        }
        private void DFS(int vertex, int currComp)
        {
            numComp[vertex] = currComp;
            for (int i = H[vertex]; i != -1; i = L[i])
            {
                if (numComp[IJ[2*m - i - 1]] == -1)
                {
                    DFS(IJ[2 * m - i - 1], currComp);
                }
            }
        }
        public void ConnectedComponent()
        {
            int currComp = 0;
            for (int i = 0; i < vmax; i++)
            {
                if (numComp[i] != -1)
                {
                    DFS(i, currComp++);
                }
            }

            for (int i = 0; i < vmax; i++)
            {
                Console.Write(i + 1);
                Console.WriteLine(" " + numComp[i]);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int[] I = new int[] {1, 2, 2, 3, 1, 5};
            int[] J = new int[] {4, 1, 3, 5, 3, 4};
            Graph g = new Graph(I, J);
            //g.Delete(2, 2);
            g.Print();
            Console.WriteLine();
            g.ConnectedComponent();// Не работает, доделать.
        }
    }
}
