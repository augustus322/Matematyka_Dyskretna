int verticesNumber;
double probability;

Random random = new Random();

while (true)
{
    Console.Write("Podaj liczbę wierzchołków: ");

    if (int.TryParse(Console.ReadLine(), out verticesNumber))
    {
        break;
    }

    Console.WriteLine("Podaj liczbę całkowitą");
}

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

int[,] matrix = new int[verticesNumber, verticesNumber];
int[] vertexDegrees = new int[verticesNumber];

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

bool IsEdgeExist(double probability)
{
    double a = random.NextDouble();

    if (a < probability)
    {
        return true;
    }

    return false;
}

for (int i = 0; i < verticesNumber; i++)
{
    for (int j = 0; j < verticesNumber; j++)
    {
        Console.Write($"{matrix[i, j]} ");
    }

    Console.WriteLine();
}

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

Console.WriteLine();

Console.Write("Stopnie wierzchołków: [");

foreach (var item in vertexDegrees.OrderBy(x => x).Reverse())
{
    Console.Write($"{item}, ");
}

Console.Write("]");