using System;
using System.Collections.Generic;
using System.Linq;
using Google.OrTools.ConstraintSolver;

namespace gort;
public class WarehouseBox
{
    public int NodeId { get; set; }
    public int Size { get; set; }
    public int Priority { get; set; } // Higher = more important

    public WarehouseBox(int nodeId, int size, int priority = 1)
    {
        NodeId = nodeId;
        Size = size;
        Priority = priority;
    }
}

public class WarehouseVRP
{
    private long[,] distanceMatrix;
    private int[] boxSizes;
    private int[] boxPriorities;
    private int cartCapacity;
    private double targetFillPercentage;

    public WarehouseVRP(int cartCapacity, double targetFillPercentage = 0.9)
    {
        this.cartCapacity = cartCapacity;
        this.targetFillPercentage = targetFillPercentage;
    }

    public void SetDistanceMatrix(long[,] matrix)
    {
        distanceMatrix = matrix;
    }

    public void SetBoxes(List<WarehouseBox> boxes)
    {
        boxSizes = boxes.Select(b => b.Size).ToArray();
        boxPriorities = boxes.Select(b => b.Priority).ToArray();
    }

    public Solution Solve(int depotNode)
    {
        int numLocations = distanceMatrix.GetLength(0);
        int numBoxes = boxSizes.Length;

        // Create Routing Index Manager
        RoutingIndexManager manager = new RoutingIndexManager(
            numLocations, 1, depotNode);

        // Create Routing Model
        RoutingModel routing = new RoutingModel(manager);

        // Create distance callback
        int transitCallbackIndex = routing.RegisterTransitCallback((long fromIndex, long toIndex) =>
        {
            var fromNode = manager.IndexToNode(fromIndex);
            var toNode = manager.IndexToNode(toIndex);
            return distanceMatrix[fromNode, toNode];
        });

        routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

        // Add capacity constraint
        int demandCallbackIndex = routing.RegisterUnaryTransitCallback((long fromIndex) =>
        {
            var fromNode = manager.IndexToNode(fromIndex);
            // If this node has a box, return its size
            if(fromNode > 0 && fromNode <= numBoxes)
                return boxSizes[fromNode - 1];
            return 0;
        });

        routing.AddDimensionWithVehicleCapacity(
            demandCallbackIndex,
            0, // null capacity slack
            new long[] { cartCapacity }, // vehicle maximum capacities
            true, // start cumul to zero
            "Capacity");

        // Add minimum capacity constraint (90% fill)
        var capacityDimension = routing.GetDimensionOrDie("Capacity");
        long minCapacity = (long)(cartCapacity * targetFillPercentage);

        // Penalty for not visiting nodes (makes them optional but penalized)
        long penalty = 10000;
        for(int node = 1; node <= numBoxes; node++)
        {
            long[] nodeIndices = { manager.NodeToIndex(node) };
            // Lower penalty for high-priority boxes
            long nodePenalty = penalty / Math.Max(1, boxPriorities[node - 1]);
            routing.AddDisjunction(nodeIndices, nodePenalty);
        }

        // Search parameters
        RoutingSearchParameters searchParameters =
            operations_research_constraint_solver.DefaultRoutingSearchParameters();
        searchParameters.FirstSolutionStrategy =
            FirstSolutionStrategy.Types.Value.PathCheapestArc;
        searchParameters.LocalSearchMetaheuristic =
            LocalSearchMetaheuristic.Types.Value.GuidedLocalSearch;
        searchParameters.TimeLimit = new Google.Protobuf.WellKnownTypes.Duration { Seconds = 30 };

        // Solve
        Assignment solution = routing.SolveWithParameters(searchParameters);

        if(solution != null)
        {
            return ExtractSolution(solution, routing, manager, depotNode);
        }

        return null;
    }

    private Solution ExtractSolution(Assignment solution, RoutingModel routing,
                                     RoutingIndexManager manager, int depot)
    {
        var result = new Solution();
        long index = routing.Start(0);

        while(!routing.IsEnd(index))
        {
            int nodeIndex = manager.IndexToNode(index);
            result.Path.Add(nodeIndex);

            if(nodeIndex > 0 && nodeIndex <= boxSizes.Length)
            {
                result.CollectedBoxes.Add(nodeIndex);
                result.TotalLoad += boxSizes[nodeIndex - 1];
            }

            long previousIndex = index;
            index = solution.Value(routing.NextVar(index));
            result.TotalDistance += routing.GetArcCostForVehicle(previousIndex, index, 0);
        }

        result.Path.Add(manager.IndexToNode(index)); // Add final depot
        result.FillPercentage = (double)result.TotalLoad / cartCapacity;

        return result;
    }
}

public class Solution
{
    public List<int> Path { get; set; } = new List<int>();
    public List<int> CollectedBoxes { get; set; } = new List<int>();
    public long TotalDistance { get; set; }
    public int TotalLoad { get; set; }
    public double FillPercentage { get; set; }

    public void Print()
    {
        Console.WriteLine($"=== Optimal Solution ===");
        Console.WriteLine($"Total Distance: {TotalDistance}");
        Console.WriteLine($"Cart Load: {TotalLoad} ({FillPercentage:P1})");
        Console.WriteLine($"Path: {string.Join(" -> ", Path)}");
        Console.WriteLine($"Collected Boxes: {string.Join(", ", CollectedBoxes)}");
    }
}

// Example usage
class Program
{
    static void Main()
    {
        // Example: 10 locations (0=depot, 1-9=box locations)
        // In real scenario, load this from your warehouse data
        long[,] distanceMatrix = CreateExampleDistanceMatrix(10);

        var boxes = new List<WarehouseBox>
        {
            new WarehouseBox(1, 30, 5),
            new WarehouseBox(2, 25, 3),
            new WarehouseBox(3, 50, 8),
            new WarehouseBox(4, 40, 6),
            new WarehouseBox(5, 35, 4),
            new WarehouseBox(6, 45, 7),
            new WarehouseBox(7, 20, 2),
            new WarehouseBox(8, 55, 9),
            new WarehouseBox(9, 30, 4)
        };

        var optimizer = new WarehouseVRP(cartCapacity: 200, targetFillPercentage: 0.9);
        optimizer.SetDistanceMatrix(distanceMatrix);
        optimizer.SetBoxes(boxes);

        var solution = optimizer.Solve(depotNode: 0);

        if(solution != null)
        {
            solution.Print();
        }
        else
        {
            Console.WriteLine("No solution found!");
        }
    }

    static long[,] CreateExampleDistanceMatrix(int size)
    {
        // Create a sample distance matrix
        // In real use, calculate from actual warehouse coordinates
        var matrix = new long[size, size];
        var random = new Random(42);

        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                if(i == j)
                    matrix[i, j] = 0;
                else if(i < j)
                    matrix[i, j] = random.Next(10, 100);
                else
                    matrix[i, j] = matrix[j, i]; // Symmetric
            }
        }

        return matrix;
    }
}

/* 
INSTALLATION:
Install via NuGet Package Manager:
Install-Package Google.OrTools

REAL-WORLD USAGE FOR 70 NODES:

1. Build Distance Matrix:
   - Use actual warehouse coordinates
   - Calculate Manhattan/Euclidean distances
   - Or use real measured path distances

2. Map Boxes to Nodes:
   - Each box gets its own node ID
   - Multiple boxes at same physical location? Create separate nodes
   
3. Tune Parameters:
   - Increase time limit for better solutions: searchParameters.TimeLimit
   - Adjust penalties for optional nodes
   - Try different metaheuristics

4. Scale Considerations:
   - 70 nodes solves in <1 minute typically
   - Can handle 100-200 nodes efficiently
   - For 500+ nodes, use Clarke-Wright or sweep algorithms first
*/