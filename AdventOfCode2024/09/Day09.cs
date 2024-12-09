using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024
{
    public static class Day09
    {
        public static void Solve()
        {
            var testInput = InputOutputHelper.GetInput(true, "09");
            var input = InputOutputHelper.GetInput(false, "09");

            PartOne(true, testInput);
            PartOne(false, input);

            PartTwo(true, testInput);
            PartTwo(false, input);
        }

        private static void PartOne(bool isTest, string[] input)
        {
            var result = CalculateChecksum(input[0]);
            InputOutputHelper.WriteOutput(isTest, result);
        }

        private static void PartTwo(bool isTest, string[] input)
        {
            var disk = ParseDiskMap(input[0]);
            var files = ParseFiles(disk);
            CompactFiles(disk, files);
            var result = CalculateChecksum(disk);
            InputOutputHelper.WriteOutput(isTest, result);
        }

        private static List<(int start, int length)> ParseFiles(List<char> disk)
        {
            var files = new List<(int start, int length)>();

            for (int i = 0; i < disk.Count; i++)
            {
                if (disk[i] != '.')
                {
                    int fileId = disk[i] - '0';
                    if (fileId >= files.Count)
                    {
                        files.Add((i, 1));
                    }
                    else
                    {
                        files[fileId] = (files[fileId].start, files[fileId].length + 1);
                    }
                }
            }

            return files;
        }

        private static void CompactFiles(List<char> disk, List<(int start, int length)> files)
        {
            var filesToCompact = Enumerable.Range(0, files.Count).Reverse().ToList();
            foreach (var fileId in filesToCompact)
            {
                int insertPos = 0;
                while (insertPos < files[fileId].start)
                {
                    if (Enumerable.Range(insertPos, files[fileId].length).All(pos => disk[pos] == '.'))
                    {
                        for (int i = 0; i < files[fileId].length; i++)
                        {
                            disk[files[fileId].start + i] = '.';
                            disk[insertPos + i] = (char)('0' + fileId);
                        }
                        break;
                    }
                    else
                    {
                        insertPos++;
                    }
                }
            }
        }

        private static long CalculateChecksum(string diskMap)
        {
            var blocks = ParseDiskMap(diskMap);
            blocks = CompactBlocks(blocks);
            return CalculateChecksum(blocks);
        }

        private static List<char> ParseDiskMap(string diskMap)
        {
            var blocks = new List<char>();
            for (int i = 0; i < diskMap.Length; i += 2)
            {
                int fileLength = diskMap[i] - '0';
                int freeSpaceLength = (i + 1 < diskMap.Length) ? diskMap[i + 1] - '0' : 0;
                blocks.AddRange(Enumerable.Repeat((char)('0' + i / 2), fileLength));
                blocks.AddRange(Enumerable.Repeat('.', freeSpaceLength));
            }
            return blocks;
        }
        
        private static List<char> CompactBlocks(List<char> blocks)
        {
            for (int readIndex = blocks.Count - 1; readIndex >= 0; readIndex--)
            {
                if (blocks[readIndex] != '.')
                {
                    int writeIndex = 0;
                    while (writeIndex < readIndex && blocks[writeIndex] != '.')
                    {
                        writeIndex++;
                    }
                    if (writeIndex < readIndex)
                    {
                        blocks[writeIndex] = blocks[readIndex];
                        blocks[readIndex] = '.';
                    }
                }
            }

            return blocks;
        }

        private static long CalculateChecksum(List<char> blocks)
        {
            long checksum = 0;
            for (int i = 0; i < blocks.Count; i++)
            {
                if (blocks[i] != '.')
                {
                    checksum += i * (blocks[i] - '0');
                }
            }
            return checksum;
        }
        

    }
}