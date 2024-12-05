namespace AdventOfCode2024;

public static class Day05
{
    public static void Solve()
    {
        var testInput = InputOutputHelper.GetInput(true, "05");
        var input = InputOutputHelper.GetInput(false, "05");
        
        PartOne(true, testInput);
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    private static void PartOne(bool isTest, string[] input)
    {
        var (rules, updates) = ParseInput(input);
        var validUpdates = updates.Where(update => IsValidUpdate(update, rules)).ToList();
        var middlePagesSum = validUpdates.Sum(update => GetMiddlePage(update));
        InputOutputHelper.WriteOutput(isTest, middlePagesSum);
    }

    private static void PartTwo(bool isTest, string[] input)
    {
        var (rules, updates) = ParseInput(input);
        var invalidUpdates = updates.Where(update => !IsValidUpdate(update, rules)).ToList();
        var correctedUpdates = invalidUpdates.Select(update => CorrectOrder(update, rules)).ToList();
        var middlePagesSum = correctedUpdates.Sum(update => GetMiddlePage(update));
        InputOutputHelper.WriteOutput(isTest, middlePagesSum);
    }
    
    
    private static List<int> CorrectOrder(List<int> update, List<(int, int)> rules)
    {
        var pageDependencies = new Dictionary<int, List<int>>();
        var pagesBeforeCount = new Dictionary<int, int>();

        foreach (var page in update)
        {
            pageDependencies[page] = new List<int>();
            pagesBeforeCount[page] = 0;
        }

        foreach (var (beforePage, afterPage) in rules)
        {
            if (pageDependencies.ContainsKey(beforePage) && pageDependencies.ContainsKey(afterPage))
            {
                pageDependencies[beforePage].Add(afterPage);
                pagesBeforeCount[afterPage]++;
            }
        }

        var pagesToProcess = new Queue<int>(pagesBeforeCount.Where(page => page.Value == 0).Select(page => page.Key));
        var sortedPages = new List<int>();

        while (pagesToProcess.Count > 0)
        {
            var currentPage = pagesToProcess.Dequeue();
            sortedPages.Add(currentPage);

            foreach (var dependentPage in pageDependencies[currentPage])
            {
                pagesBeforeCount[dependentPage]--;
                if (pagesBeforeCount[dependentPage] == 0)
                {
                    pagesToProcess.Enqueue(dependentPage);
                }
            }
        }

        return sortedPages;
    }

    private static (List<(int, int)>, List<List<int>>) ParseInput(string[] input)
    {
        var rules = new List<(int, int)>();
        var updates = new List<List<int>>();
        var isUpdateSection = false;

        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                isUpdateSection = true;
                continue;
            }

            if (!isUpdateSection)
            {
                var parts = line.Split('|');
                rules.Add((int.Parse(parts[0]), int.Parse(parts[1])));
            }
            else
            {
                updates.Add(line.Split(',').Select(int.Parse).ToList());
            }
        }

        return (rules, updates);
    }

    private static bool IsValidUpdate(List<int> update, List<(int, int)> rules)
    {
        var indexMap = update.Select((page, index) => (page, index)).ToDictionary(x => x.page, x => x.index);

        foreach (var (x, y) in rules)
        {
            if (indexMap.ContainsKey(x) && indexMap.ContainsKey(y) && indexMap[x] > indexMap[y])
            {
                return false;
            }
        }

        return true;
    }

    private static int GetMiddlePage(List<int> update)
    {
        return update[update.Count / 2];
    }
}