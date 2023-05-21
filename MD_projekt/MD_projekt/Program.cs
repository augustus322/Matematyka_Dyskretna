using System.Collections;

int verticesNumber;
double probability;
bool isGraphConnected = false;

Random random = new Random();

#region Input vertices number

while (true)
{
    Console.Write("Podaj liczbę wierzchołków: ");

    if (int.TryParse(Console.ReadLine(), out verticesNumber))
    {
        if (verticesNumber > 0)
        {
            break;
        }

        Console.WriteLine("Podaj liczbę większą od zera");

        continue;
    }

    Console.WriteLine("Podaj liczbę całkowitą");
}

#endregion
#region Input connection probability

while (true)
{
    Console.Write("Podaj prawdopodbieństwo istnienia krawędzi: ");

    if (double.TryParse(Console.ReadLine(), out probability))
    {
        if (probability < 0 || probability > 1)
        {
            Console.WriteLine("Podaj liczbę z przedziału [0,1]");

            continue;
        }

        break;
    }

    Console.WriteLine("Podaj liczbę rzeczywistą");
}

#endregion

Console.WriteLine();

#region Functions

int[,] CreateMatrix(int verticesNumber)
{
    int[,] matrix = new int[verticesNumber, verticesNumber];

    for (int i = 0; i < verticesNumber; i++)
    {
        for (int j = i; j < verticesNumber; j++)
        {
            if (i == j)
            {
                matrix[i, j] = 0;
            }
            else
            {
                if (IsEdgeExist(probability))
                {
                    matrix[i, j] = 1;
                    matrix[j, i] = 1;
                }
                else
                {
                    matrix[i, j] = 0;
                    matrix[j, i] = 0;
                }
            }
        }
    }
    return matrix;
}
bool IsEdgeExist(double probability)
{
    double a = random.NextDouble();

    if (a < probability)
    {
        return true;
    }
    return false;
}
void DisplayMatrix(int verticesNumber, int[,] matrix)
{
    for (int i = 0; i < verticesNumber; i++)
    {
        for (int j = 0; j < verticesNumber; j++)
        {
            Console.Write($"{matrix[i, j]} ");
        }
        Console.WriteLine();
    }
}
void DisplayLayers(int verticesNumber, int[,] matrix)
{
    bool la = true;
    for (int i = 0; i < verticesNumber; i++)
    {

        for (int j = 0; j < verticesNumber; j++)
        {
            if (matrix[i, j] != -1)
            {
                if (la)
                {
                    Console.Write($"Warstwa: {i + 1}\n");
                    la = false;
                }
                Console.Write($"{matrix[i, j]} ");
            }
        }
        if (la == false)
        {
            Console.WriteLine();
        }
        la = true;
    }
}
int[] GetVertexDegrees(int verticesNumber, int[,] matrix)
{
    int[] vertexDegrees = new int[verticesNumber];

    for (int i = 0; i < verticesNumber; i++)
    {
        vertexDegrees[i] = 0;

        for (int j = 0; j < verticesNumber; j++)
        {
            if (matrix[i, j] == 1)
            {
                vertexDegrees[i]++;
            }
        }
    }

    return vertexDegrees;
}
int[,] CreateGraphTree(int verticesNumber, int[,] matrix, int startingVertex, out bool isGraphConnected)
{
    int[,] treeMatrix = new int[verticesNumber, verticesNumber];

    for (int i = 0; i < verticesNumber; i++)
    {
        for (int j = 0; j < verticesNumber; j++)
        {
            treeMatrix[i, j] = 0;
        }
    }

    Queue queue = new Queue();

    int[] visitedVertices = new int[verticesNumber];
    for (int i = 0; i < verticesNumber; i++)
    {
        visitedVertices[i] = verticesNumber + 1;
    }

    queue.Enqueue(startingVertex);

    while (queue.Count != 0)
    {
        int temp = (int)queue.Dequeue();
        visitedVertices[temp] = temp;

        for (int j = 0; j < verticesNumber; j++)
        {
            if (matrix[temp, j] == 1 && !(Array.Exists(visitedVertices, x => x == j)) && !queue.Contains(j))
            {
                // Array.Exists(visitedVertices, x => x == j)
                queue.Enqueue(j);
                //availableConnections[j] = 1;
                treeMatrix[temp, j] = 1;
            }
        }
    }

    isGraphConnected = IsGraphConnected(visitedVertices);

    return treeMatrix;
}
int[,] GraphLayers(int verticesNumber, int[,] matrix, int startingVertex, out bool isGraphConnected)
{
    int[,] layersMatrix = new int[verticesNumber, verticesNumber];

    for (int i = 0; i < verticesNumber; i++)
    {
        for (int j = 0; j < verticesNumber; j++)
        {
            layersMatrix[i, j] = -1;
        }
    }

    Queue queue = new Queue();

    int[] visitedVertices = new int[verticesNumber];
    for (int i = 0; i < verticesNumber; i++)
    {
        visitedVertices[i] = verticesNumber + 1;
    }

    layersMatrix[0, 0] = startingVertex;
    int layerNumber = 1;
    queue.Enqueue(startingVertex);

    bool nextLayer = false;

    while (queue.Count != 0)
    {
        int temp = (int)queue.Dequeue();
        visitedVertices[temp] = temp;

        for (int j = 0; j < verticesNumber; j++)
        {
            if (matrix[temp, j] == 1 && !(Array.Exists(visitedVertices, x => x == j)) && !queue.Contains(j))
            {
                if (nextLayer)
                {
                    layerNumber++;
                    nextLayer = false;
                }
                queue.Enqueue(j);
                layersMatrix[layerNumber, j] = j;
                //layersMatrix[temp, j] = 1;
                //layersMat[layerNumber, j] = j;
            }
        }
        if (nextLayer == false)
        {
            nextLayer = true;
        }
    }

    isGraphConnected = IsGraphConnected(visitedVertices);

    return layersMatrix;
}
bool IsGraphConnected(int[] visitedVertices)
{
    int verticesNumber = visitedVertices.Length;

    foreach (var item in visitedVertices)
    {
        if (item == verticesNumber + 1)
        {
            return false;
        }
    }
    return true;
}
static void DisplayVertexDegrees(int[] vertexDegrees)
{
    Console.Write("Stopnie wierzchołków: [");

    foreach (var item in vertexDegrees.OrderBy(x => x).Reverse())
    {
        Console.Write($"{item}, ");
    }

    Console.WriteLine("]");
}
double GraphDensity(int verticesNumber, int[] vertexDegrees)
{
    int sum = 0;
    double density = 0;

    foreach (var item in vertexDegrees)
    {
        sum += item;
    }

    density = (sum / 2) / (0.5 * verticesNumber * (verticesNumber - 1));
    return density;
}
#endregion

int[,] testMatrixOfConnectedGraph ={
    {0,0,1,1,1},
    {0,0,1,0,1},
    {1,1,0,1,1},
    {1,0,1,0,1},
    {1,1,1,1,0}
};

int[,] testMatrixOfUnconnectedGraph = {
    {0,1,0,0,0},
    {1,0,0,0,0},
    {0,0,0,1,1},
    {0,0,1,0,0},
    {0,0,1,0,0}
};

int[,] testMatrix ={
    {0,1,0,1,1,0,0,0},
    {1,0,0,0,0,0,0,1},
    {0,0,0,1,0,0,0,1},
    { 1,0,1,0,0,0,0,1 },
    { 1,0,0,0,0,1,1,1 },
    { 0,0,0,0,1,0,0,0 },
    { 0,0,0,0,1,0,0,0 },
    { 0,1,1,1,1,0,0,0 }
};

int[,] matrix = new int[verticesNumber, verticesNumber];
int[] vertexDegrees = new int[verticesNumber];

int[,] graphTree = new int[verticesNumber, verticesNumber];
int[,] graphLayers = new int[verticesNumber, verticesNumber];

int startingVertex = 0;
bool isStartingVertexSet = false;

do
{
    matrix = CreateMatrix(verticesNumber);
    //matrix = testMatrixOfConnectedGraph;
    vertexDegrees = GetVertexDegrees(verticesNumber, matrix);

    if (probability == 0)
    {
        Console.WriteLine("Graf jest niespójny");

        break;
    }

    while (!isStartingVertexSet)
    {
        Console.Write($"Podaj startowy wierzchołek do przeszukiwania liczac od 0 do {verticesNumber - 1}: ");

        if (int.TryParse(Console.ReadLine(), out startingVertex))
        {
            if (startingVertex < verticesNumber && startingVertex >= 0)
            {
                isStartingVertexSet = true;
                break;
            }
        }
        Console.WriteLine("Podaj istniejący wierzchołek");
    }

    graphTree = CreateGraphTree(verticesNumber, matrix, startingVertex, out isGraphConnected);
    graphLayers = GraphLayers(verticesNumber, matrix, startingVertex, out isGraphConnected);

} while (!isGraphConnected);

if (!isGraphConnected)
{
    return;
}

#region Display

Console.WriteLine("\nMacierz grafu:");

DisplayMatrix(verticesNumber, matrix);

Console.WriteLine();

DisplayVertexDegrees(vertexDegrees);

Console.WriteLine();

Console.WriteLine($"Gęstość grafu: {GraphDensity(verticesNumber, vertexDegrees)}\n");

Console.WriteLine("Macierz przeglądu grafu:");

DisplayMatrix(verticesNumber, graphTree);

Console.WriteLine();

Console.WriteLine("Warstwy drzewa grafu:");

DisplayLayers(verticesNumber, graphLayers);

Console.WriteLine();
Console.ReadLine();

#endregion

#region Test
string WriteMatrix(int verticesNumber, int[,] matrix)
{
    string result = string.Empty;

    for (int i = 0; i < verticesNumber; i++)
    {
        for (int j = 0; j < verticesNumber; j++)
        {
            result += $"{matrix[i, j]} ";
        }
        result += "\n";
    }

    return result;
}

string WriteLayers(int verticesNumber, int[,] matrix)
{
    string result = string.Empty;

    bool la = true;
    for (int i = 0; i < verticesNumber; i++)
    {
        for (int j = 0; j < verticesNumber; j++)
        {
            if (matrix[i, j] != -1)
            {
                if (la)
                {
                    result += $"Warstwa: {i + 1}\n";
                    la = false;
                }
                result += $"{matrix[i, j]} ";
            }
        }
        if (la == false)
        {
            result += "\n";
        }
        la = true;
    }
    return result;
}

static string WriteVertexDegrees(int[] vertexDegrees)
{
    string result = string.Empty;

    result += "[";

    foreach (var item in vertexDegrees.OrderBy(x => x).Reverse())
    {
        result += $"{item}, ";
    }

    result += "]\n";

    return result;
}

#endregion

string file = "result.txt";

File.WriteAllText(file, "Macierz grafu:\n");
string text = WriteMatrix(verticesNumber, matrix);
File.AppendAllText(file, text);

File.AppendAllText(file, "\nStopnie wierzchołków: ");
text = WriteVertexDegrees(vertexDegrees);
File.AppendAllText(file, text);

File.AppendAllText(file, $"\nGęstość grafu: {GraphDensity(verticesNumber, vertexDegrees)}\n");

File.AppendAllText(file, "\nMacierz przeglądu grafu:\n");
text = WriteMatrix(verticesNumber, graphTree);
File.AppendAllText(file, text);

File.AppendAllText(file, "\nWarstwy drzewa grafu:\n");
text = WriteLayers(verticesNumber, graphLayers);
File.AppendAllText(file, text);