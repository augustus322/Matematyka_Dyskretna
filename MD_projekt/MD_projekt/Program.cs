using System.Xml.Schema;

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

while(connectedGraph == false)
{
    int[,] matrix = CreateMatrix(verticesNumber);
    DisplayMatrix(verticesNumber, matrix);

    if(probability == 0)
    {
        Console.WriteLine("\nGraf jest niespójny");
        break;
    }

    int[] vertexDegrees = GetVertexDegrees(verticesNumber, matrix);
    Console.WriteLine();
    // write vertices connections
    Console.Write("Stopnie wierzchołków: [");

    foreach (var item in vertexDegrees.OrderBy(x => x).Reverse())
    {
        Console.Write($"{item}, ");
    }

    Console.Write("]");

    // check graph connectivity

    foreach (var item in vertexDegrees)
    {
        if (item == 0)
        {
            connectedGraph = false;
            Console.WriteLine("\nGraf jest niespójny");
            break;
        }
        else
        {
            connectedGraph = true;
        }
    }
}

int[,] CreateGraphTree(int verticesNumber)
{
    int[,] treeMatrix = new int[verticesNumber, verticesNumber];
    int[] verticesPool = new int[verticesNumber];

    for (int i = 0; i < verticesNumber; i++)
    {
        for (int j = 0; j < verticesNumber; j++)
        {

        }
    }

    return treeMatrix;
}