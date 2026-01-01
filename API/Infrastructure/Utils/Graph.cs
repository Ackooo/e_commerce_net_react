namespace Infrastructure.Utils;

using Domain.Entities.Wms;

//static? 
public class Graph
{
    public List<Area> Nodes { get; set; } = [];


    public void AddNode(Area node)
    {
        Nodes.Add(node);
    }

    public void AddEdge(Area source, Area target, int weight)
    {
        source.AddEdge(target, weight);
        target.AddEdge(source, weight); // For undirected graph
    }

    // Dijkstra's Algorithm to find the shortest path from source node to target node
    public List<Area> Dijkstra(Area start, Area target)
    {
        // Distance to each node from the start
        Dictionary<Area, int> distances = [];
        // Previous node in optimal path from start
        Dictionary<Area, Area> previousNodes = [];
        // Priority queue to select the node with the smallest tentative distance
        SortedSet<NodeDistance> priorityQueue = [];

        // Initialize all distances to infinity and previous node to null
        foreach(var node in Nodes)
        {
            distances[node] = int.MaxValue;
            previousNodes[node] = null;
            priorityQueue.Add(new NodeDistance(node, int.MaxValue));
        }

        distances[start] = 0;
        priorityQueue.Add(new NodeDistance(start, 0));

        while(priorityQueue.Count > 0)
        {
            // Get the node with the smallest tentative distance
            Area currentNode = priorityQueue.Min.Node;
            priorityQueue.Remove(priorityQueue.Min);

            // If the target is reached, stop
            if(currentNode == target)
            {
                break;
            }

            // Explore each neighbor of the current node
            foreach(var neighbor in currentNode.Neighbors)
            {
                Area neighborNode = neighbor.Key;
                int edgeWeight = neighbor.Value;

                int newDist = distances[currentNode] + edgeWeight;
                if(newDist < distances[neighborNode])
                {
                    distances[neighborNode] = newDist;
                    previousNodes[neighborNode] = currentNode;
                    priorityQueue.Add(new NodeDistance(neighborNode, newDist));
                }
            }
        }

        // Reconstruct the path from target to start by following the previous nodes
        List<Area> path = [];
        Area current = target;
        while(current != null)
        {
            path.Insert(0, current);
            current = previousNodes[current];
        }

        // Return the path from start to target
        return path;
    }

    // Dijkstra's Algorithm
    public Dictionary<Area, int> Dijkstra(Area start)
    {
        var distances = new Dictionary<Area, int>();
        var previousNodes = new Dictionary<Area, Area>();
        var priorityQueue = new SortedList<int, Area>(); // Min-heap based on distance
        var visited = new HashSet<Area>();

        foreach(var node in Nodes)
        {
            distances[node] = int.MaxValue;
            previousNodes[node] = null;
        }
        distances[start] = 0;

        priorityQueue.Add(0, start);

        while(priorityQueue.Count > 0)
        {
            var currentNode = priorityQueue.Values[0];
            priorityQueue.RemoveAt(0);

            visited.Add(currentNode);

            foreach(var edge in currentNode.Edges)
            {
                if(visited.Contains(edge.Target)) continue;

                var newDist = distances[currentNode] + edge.Weight;
                if(newDist < distances[edge.Target])
                {
                    distances[edge.Target] = newDist;
                    previousNodes[edge.Target] = currentNode;
                    priorityQueue.Add(newDist, edge.Target);
                }
            }
        }

        return distances;
    }

    // Reconstruct the shortest path from start to destination
    public List<Area> GetPath(Area start, Area end)
    {
        var distances = Dijkstra(start);
        var path = new List<Area>();
        var currentNode = end;

        while(currentNode != null)
        {
            path.Insert(0, currentNode);
            currentNode = currentNode.Edges
                .FirstOrDefault(e => distances.ContainsKey(e.Target) && distances[e.Target] == distances[currentNode] - e.Weight)?.Source;
        }

        return path;
    }

}

public class NodeDistance(Area node, int distance) : IComparable<NodeDistance>
{
    public Area Node { get; set; } = node;
    public int Distance { get; set; } = distance;

    public int CompareTo(NodeDistance other)
    {
        return Distance.CompareTo(other.Distance);
    }
}