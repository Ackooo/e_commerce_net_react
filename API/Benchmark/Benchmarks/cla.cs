using System;
using System.Collections.Generic;
using System.Linq;

namespace cla;

public class WarehouseNode
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public List<Box> Boxes { get; set; } = new List<Box>();

    public WarehouseNode(int id, string name, double x, double y)
    {
        Id = id;
        Name = name;
        X = x;
        Y = y;
    }
}

public class Box
{
    public int Id { get; set; }
    public int NodeId { get; set; }
    public int Size { get; set; }
    public int Value { get; set; }

    public Box(int id, int nodeId, int size, int value = 1)
    {
        Id = id;
        NodeId = nodeId;
        Size = size;
        Value = value;
    }
}

public class WarehouseSolution
{
    public List<int> Path { get; set; } = new List<int>();
    public List<Box> CollectedBoxes { get; set; } = new List<Box>();
    public double TotalDistance { get; set; }
    public int TotalLoad { get; set; }
    public double FillPercentage { get; set; }
    public string AlgorithmUsed { get; set; }

    public void Print()
    {
        Console.WriteLine($"\n=== {AlgorithmUsed} Solution ===");
        Console.WriteLine($"Total Distance: {TotalDistance:F2}");
        Console.WriteLine($"Cart Load: {TotalLoad} ({FillPercentage:P1})");
        Console.WriteLine($"Path: {string.Join(" -> ", Path)}");
        Console.WriteLine($"Boxes Collected: {CollectedBoxes.Count} (IDs: {string.Join(", ", CollectedBoxes.Select(b => b.Id))})");
    }
}

// ==================== APPROACH 1: TWO-PHASE (KNAPSACK + TSP) ====================
public class TwoPhaseOptimizer
{
    private List<WarehouseNode> nodes;
    private int startNodeId;
    private int cartCapacity;
    private double targetFill;
    private double[,] distanceMatrix;

    public TwoPhaseOptimizer(List<WarehouseNode> nodes, int startNodeId, int cartCapacity, double targetFill = 0.9)
    {
        this.nodes = nodes;
        this.startNodeId = startNodeId;
        this.cartCapacity = cartCapacity;
        this.targetFill = targetFill;
        BuildDistanceMatrix();
    }

    private void BuildDistanceMatrix()
    {
        int n = nodes.Count;
        distanceMatrix = new double[n, n];

        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < n; j++)
            {
                if(i == j)
                    distanceMatrix[i, j] = 0;
                else
                {
                    double dx = nodes[i].X - nodes[j].X;
                    double dy = nodes[i].Y - nodes[j].Y;
                    distanceMatrix[i, j] = Math.Sqrt(dx * dx + dy * dy);
                }
            }
        }
    }

    public WarehouseSolution Solve()
    {
        // Phase 1: Knapsack - Select boxes to collect
        var allBoxes = nodes.SelectMany(n => n.Boxes).ToList();
        var selectedBoxes = KnapsackDP(allBoxes, (int)(cartCapacity * targetFill));

        // Get unique nodes to visit
        var nodesToVisit = selectedBoxes.Select(b => b.NodeId).Distinct().ToList();
        nodesToVisit.Insert(0, startNodeId); // Add start

        // Phase 2: TSP - Find shortest path through selected nodes
        var path = NearestNeighborTSP(nodesToVisit);

        // Build solution
        var solution = new WarehouseSolution
        {
            AlgorithmUsed = "Two-Phase (Knapsack + TSP)",
            Path = path,
            CollectedBoxes = selectedBoxes,
            TotalLoad = selectedBoxes.Sum(b => b.Size)
        };

        // Calculate total distance
        for(int i = 0; i < path.Count - 1; i++)
        {
            solution.TotalDistance += distanceMatrix[path[i], path[i + 1]];
        }

        solution.FillPercentage = (double)solution.TotalLoad / cartCapacity;
        return solution;
    }

    private List<Box> KnapsackDP(List<Box> boxes, int capacity)
    {
        int n = boxes.Count;
        var dp = new int[n + 1, capacity + 1];

        // Build DP table
        for(int i = 1; i <= n; i++)
        {
            for(int w = 0; w <= capacity; w++)
            {
                if(boxes[i - 1].Size <= w)
                    dp[i, w] = Math.Max(dp[i - 1, w], dp[i - 1, w - boxes[i - 1].Size] + boxes[i - 1].Value);
                else
                    dp[i, w] = dp[i - 1, w];
            }
        }

        // Backtrack to find selected boxes
        var selected = new List<Box>();
        int remainingCapacity = capacity;
        for(int i = n; i > 0 && remainingCapacity > 0; i--)
        {
            if(dp[i, remainingCapacity] != dp[i - 1, remainingCapacity])
            {
                selected.Add(boxes[i - 1]);
                remainingCapacity -= boxes[i - 1].Size;
            }
        }

        return selected;
    }

    private List<int> NearestNeighborTSP(List<int> nodeIds)
    {
        var path = new List<int>();
        var unvisited = new HashSet<int>(nodeIds);

        int current = nodeIds[0];
        path.Add(current);
        unvisited.Remove(current);

        while(unvisited.Count > 0)
        {
            int nearest = unvisited.OrderBy(n => distanceMatrix[current, n]).First();
            path.Add(nearest);
            unvisited.Remove(nearest);
            current = nearest;
        }

        return path;
    }
}

// ==================== APPROACH 2: GENETIC ALGORITHM ====================
public class GeneticOptimizer
{
    private List<WarehouseNode> nodes;
    private int startNodeId;
    private int cartCapacity;
    private double targetFill;
    private double[,] distanceMatrix;
    private List<Box> allBoxes;
    private Random random = new Random();

    private int populationSize = 100;
    private int generations = 500;
    private double mutationRate = 0.15;
    private double crossoverRate = 0.7;

    public GeneticOptimizer(List<WarehouseNode> nodes, int startNodeId, int cartCapacity, double targetFill = 0.9)
    {
        this.nodes = nodes;
        this.startNodeId = startNodeId;
        this.cartCapacity = cartCapacity;
        this.targetFill = targetFill;
        this.allBoxes = nodes.SelectMany(n => n.Boxes).ToList();
        BuildDistanceMatrix();
    }

    private void BuildDistanceMatrix()
    {
        int n = nodes.Count;
        distanceMatrix = new double[n, n];

        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < n; j++)
            {
                if(i == j)
                    distanceMatrix[i, j] = 0;
                else
                {
                    double dx = nodes[i].X - nodes[j].X;
                    double dy = nodes[i].Y - nodes[j].Y;
                    distanceMatrix[i, j] = Math.Sqrt(dx * dx + dy * dy);
                }
            }
        }
    }

    private class Chromosome
    {
        public bool[] BoxGenes { get; set; } // Which boxes to collect
        public List<int> PathGenes { get; set; } // Order to visit nodes
        public double Fitness { get; set; }

        public Chromosome(int boxCount)
        {
            BoxGenes = new bool[boxCount];
            PathGenes = new List<int>();
        }
    }

    public WarehouseSolution Solve()
    {
        var population = InitializePopulation();

        for(int gen = 0; gen < generations; gen++)
        {
            // Evaluate fitness
            foreach(var chromosome in population)
                chromosome.Fitness = EvaluateFitness(chromosome);

            // Sort by fitness (lower is better)
            population = population.OrderBy(c => c.Fitness).ToList();

            // Create new generation
            var newPopulation = new List<Chromosome>();

            // Elitism - keep top 10%
            int eliteCount = populationSize / 10;
            newPopulation.AddRange(population.Take(eliteCount));

            // Crossover and mutation
            while(newPopulation.Count < populationSize)
            {
                var parent1 = TournamentSelection(population);
                var parent2 = TournamentSelection(population);

                var child = random.NextDouble() < crossoverRate
                    ? Crossover(parent1, parent2)
                    : new Chromosome(allBoxes.Count) { BoxGenes = (bool[])parent1.BoxGenes.Clone() };

                if(random.NextDouble() < mutationRate)
                    Mutate(child);

                newPopulation.Add(child);
            }

            population = newPopulation;
        }

        // Get best solution
        var best = population.OrderBy(c => EvaluateFitness(c)).First();
        return ChromosomeToSolution(best);
    }

    private List<Chromosome> InitializePopulation()
    {
        var population = new List<Chromosome>();
        int targetLoad = (int)(cartCapacity * targetFill);

        for(int i = 0; i < populationSize; i++)
        {
            var chromosome = new Chromosome(allBoxes.Count);

            // Randomly select boxes until near target
            int currentLoad = 0;
            var shuffled = allBoxes.OrderBy(x => random.Next()).ToList();

            for(int j = 0; j < allBoxes.Count; j++)
            {
                if(currentLoad + shuffled[j].Size <= cartCapacity &&
                    (currentLoad < targetLoad || random.NextDouble() < 0.3))
                {
                    int originalIndex = allBoxes.IndexOf(shuffled[j]);
                    chromosome.BoxGenes[originalIndex] = true;
                    currentLoad += shuffled[j].Size;
                }
            }

            population.Add(chromosome);
        }

        return population;
    }

    private double EvaluateFitness(Chromosome chromosome)
    {
        var selectedBoxes = new List<Box>();
        int totalLoad = 0;

        for(int i = 0; i < chromosome.BoxGenes.Length; i++)
        {
            if(chromosome.BoxGenes[i])
            {
                selectedBoxes.Add(allBoxes[i]);
                totalLoad += allBoxes[i].Size;
            }
        }

        if(totalLoad > cartCapacity)
            return double.MaxValue; // Invalid

        // Get nodes to visit
        var nodesToVisit = selectedBoxes.Select(b => b.NodeId).Distinct().ToList();
        if(nodesToVisit.Count == 0)
            return double.MaxValue;

        nodesToVisit.Insert(0, startNodeId);

        // Calculate path distance using nearest neighbor
        double distance = 0;
        var unvisited = new HashSet<int>(nodesToVisit);
        int current = startNodeId;
        unvisited.Remove(current);

        while(unvisited.Count > 0)
        {
            int nearest = unvisited.OrderBy(n => distanceMatrix[current, n]).First();
            distance += distanceMatrix[current, nearest];
            unvisited.Remove(nearest);
            current = nearest;
        }

        // Fitness: distance + penalty for not reaching target fill
        int targetLoad = (int)(cartCapacity * targetFill);
        double fillPenalty = Math.Max(0, targetLoad - totalLoad) * 0.5;

        return distance + fillPenalty;
    }

    private Chromosome TournamentSelection(List<Chromosome> population)
    {
        int tournamentSize = 5;
        var tournament = Enumerable.Range(0, tournamentSize)
            .Select(_ => population[random.Next(population.Count)])
            .ToList();

        return tournament.OrderBy(c => c.Fitness).First();
    }

    private Chromosome Crossover(Chromosome parent1, Chromosome parent2)
    {
        var child = new Chromosome(allBoxes.Count);

        // Single-point crossover for box selection
        int crossoverPoint = random.Next(allBoxes.Count);
        for(int i = 0; i < allBoxes.Count; i++)
        {
            child.BoxGenes[i] = i < crossoverPoint ? parent1.BoxGenes[i] : parent2.BoxGenes[i];
        }

        return child;
    }

    private void Mutate(Chromosome chromosome)
    {
        // Flip random box selection
        int mutateIndex = random.Next(chromosome.BoxGenes.Length);
        chromosome.BoxGenes[mutateIndex] = !chromosome.BoxGenes[mutateIndex];
    }

    private WarehouseSolution ChromosomeToSolution(Chromosome chromosome)
    {
        var selectedBoxes = new List<Box>();
        for(int i = 0; i < chromosome.BoxGenes.Length; i++)
        {
            if(chromosome.BoxGenes[i])
                selectedBoxes.Add(allBoxes[i]);
        }

        var nodesToVisit = selectedBoxes.Select(b => b.NodeId).Distinct().ToList();
        nodesToVisit.Insert(0, startNodeId);

        // Build path using nearest neighbor
        var path = new List<int>();
        var unvisited = new HashSet<int>(nodesToVisit);
        int current = startNodeId;
        path.Add(current);
        unvisited.Remove(current);

        double distance = 0;
        while(unvisited.Count > 0)
        {
            int nearest = unvisited.OrderBy(n => distanceMatrix[current, n]).First();
            distance += distanceMatrix[current, nearest];
            path.Add(nearest);
            unvisited.Remove(nearest);
            current = nearest;
        }

        return new WarehouseSolution
        {
            AlgorithmUsed = "Genetic Algorithm",
            Path = path,
            CollectedBoxes = selectedBoxes,
            TotalLoad = selectedBoxes.Sum(b => b.Size),
            TotalDistance = distance,
            FillPercentage = (double)selectedBoxes.Sum(b => b.Size) / cartCapacity
        };
    }
}

// ==================== APPROACH 3: SIMULATED ANNEALING ====================
public class SimulatedAnnealingOptimizer
{
    private List<WarehouseNode> nodes;
    private int startNodeId;
    private int cartCapacity;
    private double targetFill;
    private double[,] distanceMatrix;
    private List<Box> allBoxes;
    private Random random = new Random();

    private double initialTemp = 10000;
    private double coolingRate = 0.995;
    private int iterationsPerTemp = 100;

    public SimulatedAnnealingOptimizer(List<WarehouseNode> nodes, int startNodeId, int cartCapacity, double targetFill = 0.9)
    {
        this.nodes = nodes;
        this.startNodeId = startNodeId;
        this.cartCapacity = cartCapacity;
        this.targetFill = targetFill;
        this.allBoxes = nodes.SelectMany(n => n.Boxes).ToList();
        BuildDistanceMatrix();
    }

    private void BuildDistanceMatrix()
    {
        int n = nodes.Count;
        distanceMatrix = new double[n, n];

        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < n; j++)
            {
                if(i == j)
                    distanceMatrix[i, j] = 0;
                else
                {
                    double dx = nodes[i].X - nodes[j].X;
                    double dy = nodes[i].Y - nodes[j].Y;
                    distanceMatrix[i, j] = Math.Sqrt(dx * dx + dy * dy);
                }
            }
        }
    }

    private class State
    {
        public HashSet<int> SelectedBoxIndices { get; set; }
        public List<int> Path { get; set; }
        public double Energy { get; set; }

        public State()
        {
            SelectedBoxIndices = new HashSet<int>();
            Path = new List<int>();
        }

        public State Clone()
        {
            return new State
            {
                SelectedBoxIndices = new HashSet<int>(SelectedBoxIndices),
                Path = new List<int>(Path)
            };
        }
    }

    public WarehouseSolution Solve()
    {
        var currentState = GenerateInitialState();
        currentState.Energy = CalculateEnergy(currentState);

        var bestState = currentState.Clone();
        bestState.Energy = currentState.Energy;

        double temperature = initialTemp;

        while(temperature > 1)
        {
            for(int i = 0; i < iterationsPerTemp; i++)
            {
                var newState = GenerateNeighbor(currentState);
                newState.Energy = CalculateEnergy(newState);

                double delta = newState.Energy - currentState.Energy;

                // Accept if better, or with probability based on temperature
                if(delta < 0 || random.NextDouble() < Math.Exp(-delta / temperature))
                {
                    currentState = newState;

                    if(currentState.Energy < bestState.Energy)
                    {
                        bestState = currentState.Clone();
                    }
                }
            }

            temperature *= coolingRate;
        }

        return StateToSolution(bestState);
    }

    private State GenerateInitialState()
    {
        var state = new State();
        int targetLoad = (int)(cartCapacity * targetFill);
        int currentLoad = 0;

        // Greedily select high-value boxes
        var sortedBoxes = allBoxes
            .Select((box, index) => new { Box = box, Index = index })
            .OrderByDescending(x => (double)x.Box.Value / x.Box.Size)
            .ToList();

        foreach(var item in sortedBoxes)
        {
            if(currentLoad + item.Box.Size <= cartCapacity && currentLoad < targetLoad)
            {
                state.SelectedBoxIndices.Add(item.Index);
                currentLoad += item.Box.Size;
            }
        }

        return state;
    }

    private State GenerateNeighbor(State state)
    {
        var neighbor = state.Clone();

        // Random operation: add, remove, or swap box
        int operation = random.Next(3);

        if(operation == 0 && neighbor.SelectedBoxIndices.Count < allBoxes.Count)
        {
            // Add a random unselected box
            var unselected = Enumerable.Range(0, allBoxes.Count)
                .Where(i => !neighbor.SelectedBoxIndices.Contains(i))
                .ToList();

            if(unselected.Count > 0)
            {
                int toAdd = unselected[random.Next(unselected.Count)];
                neighbor.SelectedBoxIndices.Add(toAdd);
            }
        }
        else if(operation == 1 && neighbor.SelectedBoxIndices.Count > 0)
        {
            // Remove a random box
            var toRemove = neighbor.SelectedBoxIndices.ElementAt(random.Next(neighbor.SelectedBoxIndices.Count));
            neighbor.SelectedBoxIndices.Remove(toRemove);
        }
        else if(neighbor.SelectedBoxIndices.Count > 0)
        {
            // Swap: remove one, add another
            var toRemove = neighbor.SelectedBoxIndices.ElementAt(random.Next(neighbor.SelectedBoxIndices.Count));
            neighbor.SelectedBoxIndices.Remove(toRemove);

            var unselected = Enumerable.Range(0, allBoxes.Count)
                .Where(i => !neighbor.SelectedBoxIndices.Contains(i))
                .ToList();

            if(unselected.Count > 0)
            {
                int toAdd = unselected[random.Next(unselected.Count)];
                neighbor.SelectedBoxIndices.Add(toAdd);
            }
        }

        return neighbor;
    }

    private double CalculateEnergy(State state)
    {
        var selectedBoxes = state.SelectedBoxIndices.Select(i => allBoxes[i]).ToList();
        int totalLoad = selectedBoxes.Sum(b => b.Size);

        if(totalLoad > cartCapacity)
            return double.MaxValue;

        var nodesToVisit = selectedBoxes.Select(b => b.NodeId).Distinct().ToList();
        if(nodesToVisit.Count == 0)
            return double.MaxValue;

        nodesToVisit.Insert(0, startNodeId);

        // Calculate shortest path distance
        double distance = 0;
        var unvisited = new HashSet<int>(nodesToVisit);
        int current = startNodeId;
        unvisited.Remove(current);

        while(unvisited.Count > 0)
        {
            int nearest = unvisited.OrderBy(n => distanceMatrix[current, n]).First();
            distance += distanceMatrix[current, nearest];
            unvisited.Remove(nearest);
            current = nearest;
        }

        // Energy: distance + penalty for not meeting target
        int targetLoad = (int)(cartCapacity * targetFill);
        double fillPenalty = Math.Max(0, targetLoad - totalLoad) * 0.5;

        return distance + fillPenalty;
    }

    private WarehouseSolution StateToSolution(State state)
    {
        var selectedBoxes = state.SelectedBoxIndices.Select(i => allBoxes[i]).ToList();
        var nodesToVisit = selectedBoxes.Select(b => b.NodeId).Distinct().ToList();
        nodesToVisit.Insert(0, startNodeId);

        var path = new List<int>();
        var unvisited = new HashSet<int>(nodesToVisit);
        int current = startNodeId;
        path.Add(current);
        unvisited.Remove(current);

        double distance = 0;
        while(unvisited.Count > 0)
        {
            int nearest = unvisited.OrderBy(n => distanceMatrix[current, n]).First();
            distance += distanceMatrix[current, nearest];
            path.Add(nearest);
            unvisited.Remove(nearest);
            current = nearest;
        }

        return new WarehouseSolution
        {
            AlgorithmUsed = "Simulated Annealing",
            Path = path,
            CollectedBoxes = selectedBoxes,
            TotalLoad = selectedBoxes.Sum(b => b.Size),
            TotalDistance = distance,
            FillPercentage = (double)selectedBoxes.Sum(b => b.Size) / cartCapacity
        };
    }
}

// ==================== EXAMPLE USAGE ====================
class Program
{
    static void Main()
    {
        // Create sample warehouse (70 nodes for realistic test)
        var nodes = CreateWarehouse(70);
        int startNode = 0;
        int cartCapacity = 200;

        Console.WriteLine("Testing 3 Optimization Approaches on 70-Node Warehouse\n");
        Console.WriteLine("=".PadRight(60, '='));

        // Test all three approaches
        var watch = System.Diagnostics.Stopwatch.StartNew();

        // 1. Two-Phase
        var twoPhase = new TwoPhaseOptimizer(nodes, startNode, cartCapacity);
        var solution1 = twoPhase.Solve();
        var time1 = watch.ElapsedMilliseconds;
        solution1.Print();
        Console.WriteLine($"Time: {time1}ms");

        // 2. Genetic Algorithm
        watch.Restart();
        var genetic = new GeneticOptimizer(nodes, startNode, cartCapacity);
        var solution2 = genetic.Solve();
        var time2 = watch.ElapsedMilliseconds;
        solution2.Print();
        Console.WriteLine($"Time: {time2}ms");

        // 3. Simulated Annealing
        watch.Restart();
        var annealing = new SimulatedAnnealingOptimizer(nodes, startNode, cartCapacity);
        var solution3 = annealing.Solve();
        var time3 = watch.ElapsedMilliseconds;
        solution3.Print();
        Console.WriteLine($"Time: {time3}ms");

        // Compare results
        Console.WriteLine("\n" + "=".PadRight(60, '='));
        Console.WriteLine("COMPARISON:");
        Console.WriteLine($"Two-Phase:           Distance={solution1.TotalDistance:F2}, Fill={solution1.FillPercentage:P1}, Time={time1}ms");
        Console.WriteLine($"Genetic Algorithm:   Distance={solution2.TotalDistance:F2}, Fill={solution2.FillPercentage:P1}, Time={time2}ms");
        Console.WriteLine($"Simulated Annealing: Distance={solution3.TotalDistance:F2}, Fill={solution3.FillPercentage:P1}, Time={time3}ms");
    }

    static List<WarehouseNode> CreateWarehouse(int nodeCount)
    {
        var nodes = new List<WarehouseNode>();
        var random = new Random(42);

        for(int i = 0; i < nodeCount; i++)
        {
            var node = new WarehouseNode(i, $"Node{i}", random.Next(0, 100), random.Next(0, 100));

            // Add 1-3 boxes per node (except start node)
            if(i > 0)
            {
                int boxCount = random.Next(1, 4);
                for(int j = 0; j < boxCount; j++)
                {
                    int boxId = nodes.Sum(n => n.Boxes.Count) + j;
                    node.Boxes.Add(new Box(boxId, i, random.Next(10, 60), random.Next(1, 10)));
                }
            }

            nodes.Add(node);
        }

        return nodes;
    }
}

/*
RECOMMENDATIONS FOR 70 NODES:

1. TWO-PHASE (Knapsack + TSP):
   ✓ Fastest (typically <100ms)
   ✓ Most predictable
   ✓ Good for real-time operations
   ✗ May not find global optimum
   
2. GENETIC ALGORITHM:
   ✓ Best quality solutions
   ✓ Handles complex constraints well
   ✓ Tunable (adjust generations, population)
   ✗ Slower (500ms-2s)
   ✗ Non-deterministic
   
3. SIMULATED ANNEALING:
   ✓ Good balance of speed and quality
   ✓ Less memory than genetic
   ✓ Adjustable (temperature schedule)
   ✗ Requires parameter tuning

CHOOSE:
- Real-time operations → Two-Phase
- Best solution quality → Genetic Algorithm  
- Balance of both → Simulated Annealing
*/