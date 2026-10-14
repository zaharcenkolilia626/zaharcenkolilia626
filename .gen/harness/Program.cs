using System;
using System.Collections.Generic;

// Fuzzes the REAL EmberLevelGenerator.cs and ChamberConfig.cs - not a port of them, so
// System.Random behaves exactly as it will on the device. Prints distributions, because a
// green pass counter hides a degenerate generator while a histogram does not.
internal static class Program
{
    private static readonly int[][] Table = new int[][]
    {
        //       kL kR  d  Tlo Thi wLo wHi  places
        // The two sides deliberately hold DIFFERENT numbers of stones: equal counts make
        // every high target a mirror split, which the variety guard then rejects, and the
        // target band silently collapses (chamber I could never reach 17 or 18).
        new int[] { 2, 3, 1, 12, 18, 3, 9, 11 },
        new int[] { 3, 2, 3, 14, 20, 3, 10, 11 },
        new int[] { 3, 4, 1, 21, 30, 4, 12, 13 },
        new int[] { 4, 3, 1, 24, 33, 5, 13, 13 },
        new int[] { 4, 5, 1, 32, 44, 6, 16, 15 },
    };

    private static ChamberConfig Config(int row)
    {
        int[] r = Table[row];
        ChamberConfig c = new ChamberConfig();
        c.LeftCount = r[0];
        c.RightCount = r[1];
        c.DecoyCount = r[2];
        c.TargetLow = r[3];
        c.TargetHigh = r[4];
        c.WeightLow = r[5];
        c.WeightHigh = r[6];
        c.Placements = r[7];
        return c;
    }

    private static int Main()
    {
        const int Seeds = 2000;
        int failures = 0;

        for (int chamber = 0; chamber < Table.Length; chamber++)
        {
            ChamberConfig config = Config(chamber);
            if (!config.IsFeasible())
            {
                Console.WriteLine("chamber " + chamber + ": TABLE IS NOT FEASIBLE");
                failures++;
                continue;
            }

            EmberLevelGenerator generator = new EmberLevelGenerator();
            Dictionary<int, int> targetHist = new Dictionary<int, int>();
            HashSet<string> traySignatures = new HashSet<string>();
            int minWeight = int.MaxValue;
            int maxWeight = int.MinValue;
            long weightTotal = 0;
            long weightCount = 0;
            int allEqual = 0;
            int mirrored = 0;
            int consecutiveRepeat = 0;
            int outOfBand = 0;
            int badSum = 0;
            int wrongCount = 0;
            string previous = null;

            for (int attempt = 0; attempt < Seeds; attempt++)
            {
                EmberLevelGenerator.ChamberPlan plan = generator.Build(chamber, attempt, config);

                if (plan.Tray.Length != config.StoneCount) wrongCount++;
                if (plan.Target < config.TargetLow || plan.Target > config.TargetHigh) outOfBand++;

                int left = 0, right = 0, leftCount = 0, rightCount = 0;
                List<int> leftParts = new List<int>();
                List<int> rightParts = new List<int>();
                for (int i = 0; i < plan.Tray.Length; i++)
                {
                    int w = plan.Tray[i];
                    if (w < config.WeightLow || w > config.WeightHigh) outOfBand++;
                    if (w < minWeight) minWeight = w;
                    if (w > maxWeight) maxWeight = w;
                    weightTotal += w;
                    weightCount++;
                    if (plan.SolutionSide[i] < 0) { left += w; leftCount++; leftParts.Add(w); }
                    else if (plan.SolutionSide[i] > 0) { right += w; rightCount++; rightParts.Add(w); }
                }

                // The stored split must actually solve the chamber: that is the whole claim.
                if (left != plan.Target || right != plan.Target) badSum++;
                if (leftCount != config.LeftCount || rightCount != config.RightCount) wrongCount++;

                if (config.LeftCount >= 3 && Distinct(leftParts) < 2) allEqual++;
                if (config.RightCount >= 3 && Distinct(rightParts) < 2) allEqual++;
                leftParts.Sort();
                rightParts.Sort();
                if (Join(leftParts) == Join(rightParts)) mirrored++;

                int count;
                targetHist.TryGetValue(plan.Target, out count);
                targetHist[plan.Target] = count + 1;

                string signature = Signature(plan.Tray);
                traySignatures.Add(signature);
                if (previous != null && signature == previous) consecutiveRepeat++;
                previous = signature;
            }

            Console.WriteLine("chamber " + (chamber + 1) +
                "  stones=" + config.StoneCount +
                "  targets=" + Histogram(targetHist, config.TargetLow, config.TargetHigh) +
                "  distinctTrays=" + traySignatures.Count + "/" + Seeds +
                "  weight min/avg/max=" + minWeight + "/" + (weightTotal / (double)weightCount).ToString("0.00") + "/" + maxWeight +
                "  allEqual=" + allEqual +
                "  mirrored=" + mirrored +
                "  repeatOfPrev=" + consecutiveRepeat +
                "  outOfBand=" + outOfBand +
                "  unsolvable=" + badSum +
                "  wrongCount=" + wrongCount);

            if (badSum != 0 || outOfBand != 0 || wrongCount != 0 || allEqual != 0 || consecutiveRepeat != 0)
            {
                failures++;
            }
        }

        Console.WriteLine(failures == 0 ? "GENERATOR OK" : "GENERATOR FAILURES: " + failures);
        return failures == 0 ? 0 : 1;
    }

    private static int Distinct(List<int> values)
    {
        HashSet<int> seen = new HashSet<int>(values);
        return seen.Count;
    }

    private static string Join(List<int> values)
    {
        return string.Join("-", values);
    }

    private static string Signature(int[] tray)
    {
        int[] copy = (int[])tray.Clone();
        Array.Sort(copy);
        return string.Join("-", copy);
    }

    private static string Histogram(Dictionary<int, int> hist, int low, int high)
    {
        List<string> parts = new List<string>();
        for (int t = low; t <= high; t++)
        {
            int n;
            hist.TryGetValue(t, out n);
            parts.Add(t + ":" + n);
        }

        return "[" + string.Join(" ", parts) + "]";
    }
}
