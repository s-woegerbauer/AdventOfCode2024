namespace AdventOfCode2024;

public static class Day14
{
    public static void Solve()
    {
        var testInput = InputOutputHelper.GetInput(true, "14");
        var input = InputOutputHelper.GetInput(false, "14");
        
        PartOne(true, testInput);
        PartOne(false, input);

        PartTwo(true, testInput);
        PartTwo(false, input);
    }

    private static void PartOne(bool isTest, string[] input)
    {
        var height = 103;
        var width = 101;
        List<Robot> robots = input.Select(Robot.Parse).ToList();

        if (isTest)
        {
            width = 11;
            height = 7;
        }

        for (var i = 0; i < 100; i++)
        {
            foreach (var robot in robots)
            {
                robot.Move(height, width);
            }
        }

        var safetyFactor = CountRobotsInQuadrants(robots, width, height).Aggregate(1, (acc, count) => acc * count);
        InputOutputHelper.WriteOutput(isTest, safetyFactor);
    }
    
    private static int[] CountRobotsInQuadrants(List<Robot> robots, int width, int height)
    {
        var quadrantCounts = new int[4];

        foreach (var robot in robots)
        {
            if (robot.X == width / 2 || robot.Y == height / 2)
            {
                continue;
            }

            if (robot.X < width / 2 && robot.Y < height / 2)
            {
                quadrantCounts[0]++;
            }
            else if (robot.X >= width / 2 && robot.Y < height / 2)
            {
                quadrantCounts[1]++;
            }
            else if (robot.X < width / 2 && robot.Y >= height / 2)
            {
                quadrantCounts[2]++;
            }
            else if (robot.X >= width / 2 && robot.Y >= height / 2)
            {
                quadrantCounts[3]++;
            }
        }

        return quadrantCounts;
    }
    
    private static void PartTwo(bool isTest, string[] input)
    {
        var height = 103;
        var width = 101;
        var result = 0;
        List<Robot> robots = input.Select(Robot.Parse).ToList();

        if (isTest)
        {
            width = 11;
            height = 7;
        }

        for (var i = 0; i < 100000; i++)
        {
            foreach (var robot in robots)
            {
                robot.Move(height, width);
            }

            if (AreAllPositionsUnique(robots))
            {
                Console.Clear();
                result = i;
                PrintGrid(robots, width, height);
                break;
            }
        }

        InputOutputHelper.WriteOutput(isTest, result);
    }

    private static bool AreAllPositionsUnique(List<Robot> robots)
    {
        var positions = new HashSet<(int, int)>();
        foreach (var robot in robots)
        {
            if (!positions.Add((robot.X, robot.Y)))
            {
                return false;
            }
        }
        return true;
    }
    
    private static void PrintGrid(List<Robot> robots, int width, int height)
    {
        var grid = new char[height, width];
        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                grid[i, j] = '.';
            }
        }

        foreach (var robot in robots)
        {
            grid[robot.Y, robot.X] = '#';
        }

        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                Console.Write(grid[i, j]);
            }
            Console.WriteLine();
        }
    }
    
    public class Robot
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Dx { get; set; }
        public int Dy { get; set; }
        
        public Robot(int x, int y, int dx, int dy)
        {
            X = x;
            Y = y;
            Dx = dx;
            Dy = dy;
        }
        
        public static Robot Parse(string input)
        {
            var parts = input.Split(" ");
            var positions = parts[0].Skip(2);
            var velocities = parts[1].Skip(2);
            var positionString = string.Join("", positions);
            var velocityString = string.Join("", velocities);
            var positionParts = positionString.Split(",");
            var velocityParts = velocityString.Split(",");
            
            var x = int.Parse(positionParts[0]);
            var y = int.Parse(positionParts[1]);
            var dx = int.Parse(velocityParts[0]);
            var dy = int.Parse(velocityParts[1]);

            return new Robot(x, y, dx, dy);
        }
        
        public void Move(int height, int width)
        {
            X = (X + Dx) % width;
            Y = (Y + Dy) % height;

            if (X < 0)
            {
                X += width;
            }

            if (Y < 0)
            {
                Y += height;
            }
        }
    }
}