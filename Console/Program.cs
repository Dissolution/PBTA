using System.Text;

using Core.Extensions;

int[] modifiers = {
    5,
    4,
    3,
    2,
    1,
    0,
    -1,
    -2,
    -3,
    -4,
    -5,
    -6,
};

bool?[] vantages = {
    true,
    null,
    false,
};

Result[] results = {
    Result.Success,
    Result.Partial,
    Result.Failure,
};



var rollResults = new Dictionary<Result, int>();

int count = 0;

var text = new StringBuilder();

var csv = new StringBuilder()
    .AppendLine("Modifier,Advantage,--,Disadvantage");

// Do all our rolls
foreach (var vantage in vantages)
{
    foreach (var modifier in modifiers)    
    {
        // Reset results
        count = 0;
        foreach (var result in results)
        {
            rollResults[result] = 0;
        }
        // Start our section
        text.Append('-', 64).AppendLine()
            .Append($"Roll 2d6 {modifier:+0;-0}");
        if (vantage is not null)
        {
            text.Append(" w/").Append(vantage == true ? "Advantage" : "Disadvantage");
        }
        text.AppendLine();
        // Do the rolls
        var rolls = Roll2d6(vantage).OrderByDescending(r => r);
        foreach (var roll in rolls)
        {
            var result = GetResult(roll + modifier);
            // if (IsCritical(roll))
            // {
            //     result |= Result.Critical;
            // }

            rollResults[result] += 1;
            count++;
        }
        // Show the aggregates
        int countWidth = Math.Max(count.ToString("N0").Length, "Count".Length);
        double dCount = count;

        text.Append(" Result ").Append('\t').Append("Count".PadLeft(countWidth)).Append('\t').Append("Percent").AppendLine()
            .Append("--------").Append('\t').Append('-', countWidth).Append('\t').Append("-------").AppendLine();
        foreach (var result in new[] { Result.Success, Result.Partial, Result.Failure })
        {
            text.Append(result).Append(":\t");
            var resultCount = rollResults[result];
            //var critCount = rollResults[Result.Critical | result];
            var total = resultCount;// + critCount;
            text.Append(total.ToString("N0").PadLeft(countWidth))
                .Append('\t')
                .Append($"{total / dCount,7:P2}");
            // if (critCount > 0 && resultCount > 0)
            // {
            //     text.Append('\t').Append($"({resultCount/dCount,6:P2} | {critCount/dCount,6:P2})");
            // }
            text.AppendLine();
        }
        // Onto next set
        text.AppendLine();
    }
}

// Print
Console.Write(text);

Console.WriteLine("Press Enter to Close");
Console.ReadLine();
return 0;


static IEnumerable<int> Roll2d6(bool? vantage = null)
{
    if (vantage is null)
    {
        for (var x = 1; x <= 6; x++)
        {
            for (var y = 1; y <= 6; y++)
            {
                yield return (x + y);
            }
        }
    }
    else
    {
        var rolls = new int[3];
        for (var x = 1; x <= 6; x++)
        {
            for (var y = 1; y <= 6; y++)
            {
                for (var z = 1; z <= 6; z++)
                {
                    rolls[0] = x;
                    rolls[1] = y;
                    rolls[2] = z;
                    
                    if (vantage == false)
                    {
                        Array.Sort<int>(rolls, (a, b) => a.CompareTo(b));
                    }
                    else
                    {
                        Array.Sort<int>(rolls, (a, b) => b.CompareTo(a));
                    }

                    yield return rolls[0] + rolls[1];
                }
            }
        }
    }
}

static bool IsCritical(int roll) => roll == 2 || roll == 12;

static Result GetResult(int roll)
{
    if (roll <= 6) return Result.Failure;
    if (roll <= 9) return Result.Partial;
    return Result.Success;
}

[Flags]
public enum Result
{
    Critical = 1 << 0,
    Failure = 1 << 1,
    Partial = 1 << 2,
    Success = 1 << 3,
}

public sealed record class Key(int Modifier, bool? Vantage);
