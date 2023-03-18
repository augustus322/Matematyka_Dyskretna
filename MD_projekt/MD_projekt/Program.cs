using System.Collections;

int verticesNumber;
double probability;
bool connectedGraph = false;

Random random = new Random();

// input vertices number
while (true)
{
    Console.Write("Podaj liczbę wierzchołków: ");

    if (int.TryParse(Console.ReadLine(), out verticesNumber))
    {
        break;
    }

    Console.WriteLine("Podaj liczbę całkowitą");
}

// input connection probability
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

Console.WriteLine();

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

int[,] testMatrix = {   {0,0,1,1,1},
                        {0,0,1,0,1},
                        {1,1,0,1,1},
                        {1,0,1,0,1},
                        {1,1,1,1,0} };

//int[,] testMatrix = {   {0,1,0,0,0},
//                        {1,0,0,0,0},
//                        {0,0,0,1,1},
//                        {0,0,1,0,0},
//                        {0,0,1,0,0} };

int[,] CreateGraphTree(int verticesNumber, int[,] matrix, out bool isGraphConnected)
{
    int startingVertex;

    while (true)
    {
        Console.Write($"Podaj startowy wierzchołek do przeszukiwania liczac od 0 do {verticesNumber-1}: ");

        if (int.TryParse(Console.ReadLine(), out startingVertex))
        {
            if (startingVertex < verticesNumber && startingVertex >= 0)
            {
                break;
            }
        }

        Console.WriteLine("Podaj istniejący wierzchołek");
    }

    int[,] treeMatrix = new int[verticesNumber, verticesNumber];

    for (int i = 0; i < verticesNumber; i++)
    {
        for (int j = 0; j < verticesNumber; j++)
        {
            treeMatrix[i, j] = 0;
        }
    }

    Queue queue = new Queue();
    //int[] availableConnections = new int[verticesNumber];
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

//int[,] matrix = CreateMatrix(verticesNumber);
//int[] vertexDegrees = GetVertexDegrees(verticesNumber, matrix);

//while(connectedGraph == false)
//{
//    matrix = CreateMatrix(verticesNumber);
//    vertexDegrees = GetVertexDegrees(verticesNumber, matrix);

//    DisplayMatrix(verticesNumber, matrix);

//    if(probability == 0)
//    {
//        Console.WriteLine("\nGraf jest niespójny");
//        break;
//    }

//    Console.WriteLine();
//    // write vertices connections
//    Console.Write("Stopnie wierzchołków: [");

//    foreach (var item in vertexDegrees.OrderBy(x => x).Reverse())
//    {
//        Console.Write($"{item}, ");
//    }

//    Console.Write("]");

//    // check graph connectivity

//    foreach (var item in vertexDegrees)
//    {
//        if (item == 0)
//        {
//            connectedGraph = false;
//            Console.WriteLine("\nGraf jest niespójny");
//            break;
//        }
//        else
//        {
//            connectedGraph = true;
//        }
//    }
//}

int[,] graphTree = CreateGraphTree(verticesNumber, testMatrix, out bool isGraphConnected);

for (int i = 0; i < verticesNumber; i++)
{
    for (int j = 0; j < verticesNumber; j++)
    {
        Console.Write($"{graphTree[i, j]} ");
    }
    Console.WriteLine();
}

Console.WriteLine($"Graf jest {(isGraphConnected ? "spójny" : "niespójny")} ");