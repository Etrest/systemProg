using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;





static void BellmanFord(Edge[] edges, int source, int verticesCount)
{
    int[] distance = new int[verticesCount];
    int[] predecessor = new int[verticesCount];

    // Step 1: Initialize distance and predecessor arrays
    for (int i = 0; i < verticesCount; ++i)
    {
        distance[i] = int.MaxValue;
        predecessor[i] = -1;
    }

    distance[source] = 0;

    // Step 2: Relax edges repeatedly
    for (int i = 1; i < verticesCount; ++i)
    {
        for (int j = 0; j < edges.Length; ++j)
        {
            int u = edges[j].Source;
            int v = edges[j].Destination;
            int weight = edges[j].Weight;

            if (distance[u] != int.MaxValue && distance[u] + weight < distance[v])
            {
                distance[v] = distance[u] + weight;
                predecessor[v] = u;
            }
        }
    }

    // Step 3: Check for negative-weight cycles
    for (int i = 0; i < edges.Length; ++i)
    {
        int u = edges[i].Source;
        int v = edges[i].Destination;
        int weight = edges[i].Weight;

        if (distance[u] != int.MaxValue && distance[u] + weight < distance[v])
        {
            throw new ArgumentException("The graph contains a negative-weight cycle");
        }
    }

    // Step 4: Print the shortest path distances and predecessors
    Console.WriteLine("Vertex\tDistance\tPredecessor");

    for (int i = 0; i < verticesCount; ++i)
    {
        Console.WriteLine("{0}\t{1}\t\t{2}", i, distance[i], predecessor[i]);
    }
}
public class Edge
{
    public int Source;
    public int Destination;
    public int Weight;
}