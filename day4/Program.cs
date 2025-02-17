// for it to be the start of a sequence, its letter must be x
// for it to continue the sequence, its previous must have been x,m,a and its next in same direction must be m,a,s
// for it to be the end of a sequence, it must be s and the previous letter must have been a

static bool IsValidSequence(char[,] letterMap, int y, int x, int dy, int dx)
{
    if (y < 0
        || x < 0
        || y >= letterMap.GetLength(0) 
        || x >= letterMap.GetLength(1))
    {
        Console.WriteLine($"Bad current letter pos {y}, {x}");
        return false;
    }

    char currentLetter = letterMap[y, x];

    // We are only in a valid position of some sequence if the previous value and the next value are also in sequence
    bool validatePreviousLetter = currentLetter == 'M' || currentLetter == 'A' || currentLetter == 'S';
    bool validateNextLetter = currentLetter == 'X' || currentLetter == 'M' || currentLetter == 'A';

    if (validatePreviousLetter)
    {
        if (y - dy < 0
            || x - dx < 0
            || y - dy >= letterMap.GetLength(0)
            || x - dx >= letterMap.GetLength(1))
        {
            Console.WriteLine($"Bad prev letter pos {y}, {x}");
            return false;
        }

        char prevLetter = letterMap[y - dy, x - dx];
        if (!((prevLetter == 'X' && currentLetter == 'M')
            || (prevLetter == 'M' && currentLetter == 'A')
            || (prevLetter == 'A' && currentLetter == 'S')))
        {
            Console.WriteLine($"Bad previous letter in sequence {prevLetter} -> {currentLetter}");
            return false;
        }
    }

    if (validateNextLetter)
    {
        if (y + dy < 0
            || x + dx < 0
            || y + dy >= letterMap.GetLength(0)
            || x + dx >= letterMap.GetLength(1))
        {
            Console.WriteLine($"Bad next letter pos {y}, {x}");
            return false;
        }

        char nextLetter = letterMap[y + dy, x + dx];
        if (!((currentLetter == 'X' && nextLetter == 'M')
            || (currentLetter == 'M' && nextLetter == 'A')
            || (currentLetter == 'A' && nextLetter == 'S')))
        {
            Console.WriteLine($"Bad next letter in sequence {currentLetter} -> {nextLetter}");
            return false;
        }
    }

    // If we have gotten here we have validated that we are in a position of some sequence that is valid
    // If we have not hit the base case (S) recursively call on next position in sequence
    if (currentLetter == 'S')
    {
        Console.WriteLine("End of sequence");
        return true;
    }
    else
    {
        return IsValidSequence(letterMap, y + dy, x + dx, dy, dx);
    }
}

static void Part1()
{
    int count = 0;

    var lines = File.ReadAllLines("input.txt");

    // assuming that all lines are the same length and there is 1 line
    char[,] wordMap = new char[lines.Length, lines[0].Length];

    // populate word map
    for (int y = 0; y < lines.Length; y++)
    {
        for (int x = 0; x < lines[y].Length; x++)
        {
            wordMap[y, x] = lines[y][x];
        }
    }

    // count all valid sequences and validate whether there are any possible sequences
    for (int y = 0; y < wordMap.GetLength(0); y++)
    {
        for (int x = 0; x < wordMap.GetLength(1); x++)
        {
            if (wordMap[y, x] == 'X')
            {
                for (int dy = -1; dy < 2; dy++)
                {
                    for (int dx = -1; dx < 2; dx++)
                    {
                        if (dy == 0 && dx == 0) continue;
                        count += IsValidSequence(wordMap, y, x, dy, dx) ? 1 : 0;
                    }
                }
            }
        }
    }

    Console.WriteLine($"Part 1: {count}");
}

static void Part2()
{
    // sequence check starts with the A now
    // sequence must have both directions match (-1, -1 or 1, 1) and (-1, 1 or 1, -1)
    int count = 0;

    var lines = File.ReadAllLines("input.txt");

    // assuming that all lines are the same length and there is 1 line
    char[,] wordMap = new char[lines.Length, lines[0].Length];

    // populate word map
    for (int y = 0; y < lines.Length; y++)
    {
        for (int x = 0; x < lines[y].Length; x++)
        {
            wordMap[y, x] = lines[y][x];
        }
    }

    // count all valid sequences and validate whether there are any possible sequences
    for (int y = 0; y < wordMap.GetLength(0); y++)
    {
        for (int x = 0; x < wordMap.GetLength(1); x++)
        {
            if (wordMap[y, x] == 'A')
            {
                if ((IsValidSequence(wordMap, y, x, -1, -1) || IsValidSequence(wordMap, y, x, 1, 1))
                    && (IsValidSequence(wordMap, y, x, 1, -1) || IsValidSequence(wordMap, y, x, -1, 1)))
                {
                    count++;
                    Console.WriteLine($"Found X-MAS at start {y},{x}");
                    Console.WriteLine($"{wordMap[y-1,x-1]}.{wordMap[y-1,x+1]}");
                    Console.WriteLine($".{wordMap[y,x]}.");
                    Console.WriteLine($"{wordMap[y+1,x-1]}.{wordMap[y+1,x+1]}");
                }
            }
        }
    }

    Console.WriteLine($"Part 2: {count}");
}

Part1();
Part2();