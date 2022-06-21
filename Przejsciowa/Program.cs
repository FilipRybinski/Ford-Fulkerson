using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace Ford_Fulkerson
{

    public class Program
    {
        public static string OutPath = "Out0402.txt";
        public static string InPath = "In0402.txt";  
        public class Graph
        {
            private int vertices;
            private int[,] Array;
            public Graph(int v)
            {
                vertices = v;
                Array=new int[v+1,v+1];
            }
            public void AddEdge(int a,int b,int c)
            {
                Array[a, b] = c;
            }
            public int[,] GetArray()
            {
                return Array;
            }
        }

        public static bool DFS(int Source, int Outlet, int v, int[,] Array, int[] Parent,StreamWriter sw)
        {
            bool[] visited = new bool[v];
            Stack<int> stack = new Stack<int>();
            foreach (bool a in visited) { a.Equals(false); }
            foreach (int b in Parent) { b.Equals(0); }
            visited[Source] = true;
            Parent[Source] = -1;
            stack.Push(Source);
            Console.Write("STOS" + " " + stack.Peek() + " ");
                sw.Write("STOS" + " " + stack.Peek() + " ");
            while (stack.Count != 0)
            {

                int u = stack.Peek();
                stack.Pop();
                for (int i = 1; i < v; i++)
                {
                    if (!visited[i] && Array[u, i] > 0)
                    {
                        if (i == Outlet)
                        {
                            Parent[i] = u;
                            for (int z = Outlet; z != Source; z = Parent[z])
                            {
                                Console.Write(z + " ");
                                sw.Write(z + " ");
                            }
                            Console.WriteLine(Source);
                                sw.WriteLine(Source);
                            return true;
                        }
                        stack.Push(i);
                        Console.Write(i + " ");
                            sw.Write(i + " ");
                        Parent[i] = u;
                        visited[i] = true;
                    }
                }
            }
                sw.WriteLine();
            Console.WriteLine();
        
            return false;
        }
        public static int Max(int[,] Array,int Source,int Outlet, int v,StreamWriter sw)
        {
                int[,] Array2 = new int[v, v];
                Array2 = Fill(Array, Array2, v);
                int u = default;
                int max_flow = 0;
                int[] parent = new int[v];
                
                while (DFS(Source, Outlet, v, Array2, parent,sw))
                {
                    int flow = int.MaxValue;
                    for (int i = Outlet; i != Source; i = parent[i])
                    {
                        u = parent[i];
                        flow = Math.Min(flow, Array2[u, i]);
                    }
                    for (int z = Outlet; z != Source; z = parent[z])
                    {
                        u = parent[z];
                        Array2[u, z] -= flow;
                        Array2[z, u] += flow;
                    }
                    max_flow += flow;
                }
                for (int i = 1; i < v; i++)
                {
                    Console.Write("[" + i + "] ");
                    sw.Write("[" + i + "] ");
                    for (int j = 1; j < v; j++)
                    {
                        if (Array[i, j] > 0)
                        {
                            Console.Write(j + " " + Array2[j, i] + ", ");
                            sw.Write(j + " " + Array2[j, i] + ", ");
                        }
                    }
                    Console.WriteLine();
                    sw.WriteLine();
                }
            sw.WriteLine("Max="+max_flow);
            return max_flow;
        }
        public static int[,] Fill(int[,] Array, int[,] Array2,int v)
        {
            for (int i = 1; i < v - 1; i++)
            {
                for (int j = 1; j < v; j++)
                {
                    Array2[i, j] = Array[i, j];
                }
            }
            return Array2;
        }
        public static void Main(string[] args)
        {
            int[,] Array;
            int j = 0;
            int V;
            int Source;
            int Outlet;
            try
            {
                using (StreamReader sr = new StreamReader(InPath))
                {
                    string line;
                    string[] spliteline;
                    line = sr.ReadLine();
                    spliteline = line.Split(' ');
                    var result = spliteline.Select(e => int.Parse(e)).ToList();
                    V=result[0];
                    Source = result[1];
                    Outlet = result[2];
                    Graph Ggraph = new Graph(V);
                    while ((line = sr.ReadLine()) != null)
                    {
                        spliteline = line.Split(' ');
                        result = spliteline.Select(e => int.Parse(e)).ToList();
                        for (int i = 0; i < result.Count; i++)
                        {
                            Ggraph.AddEdge(j+1, result[i],result[i+1]);
                            i++;
                        }
                        j++;
                        
                    }
                    Array = Ggraph.GetArray();
                    using (StreamWriter sw = new StreamWriter(OutPath))
                    {
                        Console.WriteLine("Max=" + Max(Array, Source, Outlet, V + 1, sw));
                    }
                }
            }catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
    }
}


