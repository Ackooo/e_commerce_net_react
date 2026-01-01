using System;
using System.Collections.Generic;
using System.Linq;

namespace dj1;

public class Box
{
    public int Id { get; set; }
    public int Size { get; set; }
    public int Value { get; set; } // Optional: priority/value of the box

    public Box(int id, int size, int value = 1)
    {
        Id = id;
        Size = size;
        Value = value;
    }
}

public class Node
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Box> Boxes { get; set; } = new List<Box>();

    public Node(int id, string name)
    {
        Id = id;
        Name = name;
    }
}

public class Edge
{
    public Node From { get; set; }
    public Node To { get; set; }
    public double Distance { get; set; }

    public Edge(Node from, Node to, double distance)
    {
        From = from;
        To = to;
        Distance = distance;
    }
}

public class PathState
{
    public Node CurrentNode { get; set; }
    public double Distance { get; set; }
    public int CartLoad { get; set; }
    public List<Node> Path { get; set; }
    public List<Box> CollectedBoxes { get; set; }

    public PathState(Node node, double distance, int cartLoad, List<Node> path, List<Box> boxes)
    {
        CurrentNode = node;
        Distance = distance;
        CartLoad = cartLoad;
        Path = new List<Node>(path);
        CollectedBoxes = new List<Box>(boxes);
    }
}

public class WarehouseOptimizer
{
    private Dictionary<Node, List<Edge>> graph = new Dictionary<Node, List<Edge>>();
    private int cartCapacity;
    private double targetFillPercentage;

    public WarehouseOptimizer(int cartCapacity, double targetFillPercentage = 0.9)
    {
        this.cartCapacity = cartCapacity;
        this.targetFillPercentage = targetFillPercentage;
    }

    public void AddEdge(Node from, Node to, double distance)
    {
        if(!graph.ContainsKey(from))
            graph[from] = new List<Edge>();
        if(!graph.ContainsKey(to))
            graph[to] = new List<Edge>();

        graph[from].Add(new Edge(from, to, distance));
        graph[to].Add(new Edge(to, from, distance)); // Bidirectional
    }

    public PathState FindOptimalPath(Node start, Node end)
    {
        var targetLoad = (int)(cartCapacity * targetFillPercentage);
        var priorityQueue = new SortedSet<(double priority, int stateId)>();
        var states = new Dictionary<int, PathState>();
        var visited = new HashSet<string>();
        int stateIdCounter = 0;

        // Initial state
        var initialState = new PathState(start, 0, 0, new List<Node> { start }, new List<Box>());
        states[stateIdCounter] = initialState;
        priorityQueue.Add((0, stateIdCounter));
        stateIdCounter++;

        PathState bestSolution = null;

        while(priorityQueue.Count > 0)
        {
            var (_, currentStateId) = priorityQueue.Min;
            priorityQueue.Remove(priorityQueue.Min);

            var currentState = states[currentStateId];

            // Create state key for visited check
            string stateKey = $"{currentState.CurrentNode.Id}_{currentState.CartLoad}";
            if(visited.Contains(stateKey))
                continue;
            visited.Add(stateKey);

            // Check if we reached destination with sufficient load
            if(currentState.CurrentNode.Id == end.Id && currentState.CartLoad >= targetLoad)
            {
                if(bestSolution == null || currentState.Distance < bestSolution.Distance)
                {
                    bestSolution = currentState;
                }
                continue;
            }

            // Don't explore further if cart is already full
            if(currentState.CartLoad >= cartCapacity)
                continue;

            // Explore neighbors
            if(graph.ContainsKey(currentState.CurrentNode))
            {
                foreach(var edge in graph[currentState.CurrentNode])
                {
                    var nextNode = edge.To;
                    var newDistance = currentState.Distance + edge.Distance;
                    var newPath = new List<Node>(currentState.Path) { nextNode };
                    var newBoxes = new List<Box>(currentState.CollectedBoxes);
                    int newLoad = currentState.CartLoad;

                    // Try to collect boxes at this node
                    foreach(var box in nextNode.Boxes)
                    {
                        if(newLoad + box.Size <= cartCapacity)
                        {
                            newLoad += box.Size;
                            newBoxes.Add(box);
                        }
                    }

                    // Calculate priority: favor shorter paths and fuller carts
                    double loadDeficit = Math.Max(0, targetLoad - newLoad);
                    double priority = newDistance + (loadDeficit * 0.1); // Weighted priority

                    var newState = new PathState(nextNode, newDistance, newLoad, newPath, newBoxes);
                    states[stateIdCounter] = newState;
                    priorityQueue.Add((priority, stateIdCounter));
                    stateIdCounter++;
                }
            }
        }

        return bestSolution;
    }

    public void PrintSolution(PathState solution)
    {
        if(solution == null)
        {
            Console.WriteLine("No solution found that meets the requirements.");
            return;
        }

        Console.WriteLine($"Optimal Path Found:");
        Console.WriteLine($"Total Distance: {solution.Distance:F2}");
        Console.WriteLine($"Cart Load: {solution.CartLoad}/{cartCapacity} ({(solution.CartLoad * 100.0 / cartCapacity):F1}%)");
        Console.WriteLine($"\nPath: {string.Join(" -> ", solution.Path.Select(n => n.Name))}");
        Console.WriteLine($"\nCollected Boxes ({solution.CollectedBoxes.Count}):");

        foreach(var box in solution.CollectedBoxes)
        {
            Console.WriteLine($"  Box {box.Id}: Size {box.Size}");
        }
    }
}

// Example usage
class Program
{
    static void Main()
    {
        // Create warehouse nodes
        var entrance = new Node(0, "Entrance");
        var aisle1 = new Node(1, "Aisle 1");
        var aisle2 = new Node(2, "Aisle 2");
        var aisle3 = new Node(3, "Aisle 3");
        var checkout = new Node(4, "Checkout");

        // Add boxes to nodes
        aisle1.Boxes.Add(new Box(1, 30, 5));
        aisle1.Boxes.Add(new Box(2, 25, 3));
        aisle2.Boxes.Add(new Box(3, 50, 8));
        aisle2.Boxes.Add(new Box(4, 40, 6));
        aisle3.Boxes.Add(new Box(5, 35, 4));
        aisle3.Boxes.Add(new Box(6, 45, 7));
        aisle3.Boxes.Add(new Box(7, 20, 2));

        // Create optimizer
        var optimizer = new WarehouseOptimizer(cartCapacity: 200, targetFillPercentage: 0.9);

        // Build graph
        optimizer.AddEdge(entrance, aisle1, 10);
        optimizer.AddEdge(entrance, aisle2, 15);
        optimizer.AddEdge(aisle1, aisle2, 8);
        optimizer.AddEdge(aisle1, aisle3, 12);
        optimizer.AddEdge(aisle2, aisle3, 7);
        optimizer.AddEdge(aisle2, checkout, 20);
        optimizer.AddEdge(aisle3, checkout, 18);

        // Find optimal path
        var solution = optimizer.FindOptimalPath(entrance, checkout);
        optimizer.PrintSolution(solution);
    }
}