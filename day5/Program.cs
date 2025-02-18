static bool CycleDfs(Dictionary<int, HashSet<int>> constraintGraph, Dictionary<int, VisitState> pageNumVisitationState, int pageNum)
{
    switch (pageNumVisitationState[pageNum])
    {
        case VisitState.Seen:
            // If we have gotten to this, we are looking at a node already seen.
            // As we should have only "before -> after" edges, this means that an "after -> before" occured
            return true;
        case VisitState.Finished:
            return false;
        case VisitState.Unseen:
            break;
    }

    // We have now seen this page
    pageNumVisitationState[pageNum] = VisitState.Seen;

    // Check all possible "after" constraints of this page to ensure none are broken
    foreach (int possibleAfterPage in constraintGraph[pageNum])
    {
        if (CycleDfs(constraintGraph, pageNumVisitationState, possibleAfterPage))
        {
            // If any subsequent Dfs check on a subsequent page has cycle, break
            return true;
        }
    }

    // If we have made it through all children and no cycle was found, we can consider this page fully "visited" and no cycles are found
    pageNumVisitationState[pageNum] = VisitState.Finished;

    return false;
}

static void Part1()
{
    Dictionary<int, HashSet<int>> constraintGraph = new Dictionary<int, HashSet<int>>();
    List<List<int>> updates = new List<List<int>>();
    int validUpdateMedianSum = 0;

    // Parse to build constraint graph and updates
    var lines = File.ReadAllLines("input.txt");
    foreach(string line in lines)
    {
        if (line.Contains('|'))
        {
            var split = line.Split("|");
            int beforePage = Convert.ToInt32(split[0]);
            int afterPage = Convert.ToInt32(split[1]);

            if (!constraintGraph.ContainsKey(beforePage))
            {
                constraintGraph[beforePage] = new HashSet<int>();
            }

            constraintGraph[beforePage].Add(afterPage);
        }
        else if (line.Contains(','))
        {
            var split = line.Split(",");
            List<int> pageNumbers = new List<int>();
            foreach (string pageNum in split)
            {
                pageNumbers.Add(Convert.ToInt32(pageNum));
            }

            updates.Add(pageNumbers);
        }
    }

    // Iterate through all updates to validate whether their order is valid
    foreach (List<int> update in updates)
    {
        // Constraint graph now has all pages which need to occur "before" another page
        // Conceptually an update is just adding an additional constraint for each page
        // Copy the source constraint graph and add this additional constraint and validate that the resulting set still isn't cyclical
        Dictionary<int, HashSet<int>> updateGraph = new Dictionary<int, HashSet<int>>();
        Dictionary<int, VisitState> pageVisitations = new Dictionary<int, VisitState>();
        bool invalidGraph = false;

        for (int i = 0; i < update.Count; i++)
        {
            HashSet<int> newPageConstraint = new HashSet<int>((constraintGraph.TryGetValue(update[i], out HashSet<int> pageConstraints) ? pageConstraints : new HashSet<int>()).Intersect(update));
            updateGraph[update[i]] = newPageConstraint;

            // Add next page in sequence as hard constraint
            if (i < update.Count - 1)
            {
                newPageConstraint.Add(update[i + 1]);
            }

            pageVisitations[update[i]] = VisitState.Unseen;
        }

        // Now we have a set of pages each with the global and local constraints for ordering
        // Do a DFS to find any cycles for all pages listed in the update
        foreach (int pageToVisit in pageVisitations.Keys)
        {
            if (pageVisitations[pageToVisit] == VisitState.Unseen
                && CycleDfs(updateGraph, pageVisitations, pageToVisit))
            {
                // We had a cycle, meaning that some before constrained page appeared after some page
                invalidGraph = true;
                break;
            }
        }

        if (!invalidGraph)
        {
            validUpdateMedianSum += update[update.Count / 2];
        }
    }

    Console.WriteLine($"Median sum: {validUpdateMedianSum}");
}

static List<int> TopoSort(Dictionary<int, HashSet<int>> constraintGraph)
{
    Dictionary<int, int> pageNumInDegree = new Dictionary<int, int>();
    Queue<int> pageQueue = new Queue<int>();
    List<int> pageOrder = new List<int>();
    
    // Start with each page number in our graph having no inbound edges
    foreach (int pageNum in constraintGraph.Keys)
    {
        pageNumInDegree[pageNum] = 0;
    }

    // Walk through all edges of the graph to count each page and their number of in-degree edges
    foreach (HashSet<int> pageEdges in constraintGraph.Values)
    {
        foreach (int pageNum in pageEdges)
        {
            pageNumInDegree[pageNum]++;
        }
    }

    // Add all that have no in-degree edges
    foreach (var pageDegree in pageNumInDegree)
    {
        if (pageDegree.Value == 0)
        {
            pageQueue.Enqueue(pageDegree.Key);
        }
    }

    // Conceptually we are walking through all 
    while (pageQueue.Count > 0)
    {
        int pageNum = pageQueue.Dequeue();
        pageOrder.Add(pageNum);

        foreach (int pageEdge in constraintGraph[pageNum])
        {
            if (--pageNumInDegree[pageEdge] == 0)
            {
                pageQueue.Enqueue(pageEdge);
            }
        }
    }

    return pageOrder;
}

static void Part2()
{
    Dictionary<int, HashSet<int>> constraintGraph = new Dictionary<int, HashSet<int>>();
    List<List<int>> updates = new List<List<int>>();
    int validUpdateMedianSum = 0;

    // Parse to build constraint graph and updates
    var lines = File.ReadAllLines("input.txt");
    foreach(string line in lines)
    {
        if (line.Contains('|'))
        {
            var split = line.Split("|");
            int beforePage = Convert.ToInt32(split[0]);
            int afterPage = Convert.ToInt32(split[1]);

            if (!constraintGraph.ContainsKey(beforePage))
            {
                constraintGraph[beforePage] = new HashSet<int>();
            }

            constraintGraph[beforePage].Add(afterPage);
        }
        else if (line.Contains(','))
        {
            var split = line.Split(",");
            List<int> pageNumbers = new List<int>();
            foreach (string pageNum in split)
            {
                pageNumbers.Add(Convert.ToInt32(pageNum));
            }

            updates.Add(pageNumbers);
        }
    }

    // Iterate through all updates to validate whether their order is valid
    foreach (List<int> update in updates)
    {
        // Constraint graph now has all pages which need to occur "before" another page
        // Conceptually an update is just adding an additional constraint for each page
        // Copy the source constraint graph and add this additional constraint and validate that the resulting set still isn't cyclical
        Dictionary<int, HashSet<int>> updateGraph = new Dictionary<int, HashSet<int>>();
        Dictionary<int, VisitState> pageVisitations = new Dictionary<int, VisitState>();
        bool invalidGraph = false;

        for (int i = 0; i < update.Count; i++)
        {
            HashSet<int> newPageConstraint = new HashSet<int>((constraintGraph.TryGetValue(update[i], out HashSet<int> pageConstraints) ? pageConstraints : new HashSet<int>()).Intersect(update));
            updateGraph[update[i]] = newPageConstraint;

            // Add next page in sequence as hard constraint
            if (i < update.Count - 1)
            {
                newPageConstraint.Add(update[i + 1]);
            }

            pageVisitations[update[i]] = VisitState.Unseen;
        }

        // Now we have a set of pages each with the global and local constraints for ordering
        // Do a DFS to find any cycles for all pages listed in the update
        foreach (int pageToVisit in pageVisitations.Keys)
        {
            if (pageVisitations[pageToVisit] == VisitState.Unseen
                && CycleDfs(updateGraph, pageVisitations, pageToVisit))
            {
                // We had a cycle, meaning that some before constrained page appeared after some page
                invalidGraph = true;
                break;
            }
        }

        if (!invalidGraph)
        {
            continue;
        }

        // In an invalid graph, we need to remove the ordering constraint of the update and perform selection
        // that has valid dependency graph
        for (int i = 0; i < update.Count; i++)
        {
            HashSet<int> newPageConstraint = new HashSet<int>((constraintGraph.TryGetValue(update[i], out HashSet<int> pageConstraints) ? pageConstraints : new HashSet<int>()).Intersect(update));
            updateGraph[update[i]] = newPageConstraint;
        }

        var validPageOrder = TopoSort(updateGraph);
        validUpdateMedianSum += validPageOrder[validPageOrder.Count / 2];
    }

    Console.WriteLine($"Part2 Median sum: {validUpdateMedianSum}");
}

Part1();
Part2();

enum VisitState
{
    Unseen = 0,
    Seen = 1,
    Finished = 2,
};