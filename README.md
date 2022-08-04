# Longest Password

## Notes

I was recently given this as an example question for a interview coding test. I found it fun, but wanted to revist it in Visual Studio intead of via a poor online editor / compiler / intellisense.

Since there were a couple of ways to do this, I felt like I wanted to add some benchmarking it as well using [BenchmarkDotnet's](https://github.com/dotnet/BenchmarkDotNet) excellent tools.'

## Possible Implementations

Checking whether we have a number of digits or a number of letters is relatively simple, suing `char.IsDigit(char)` and `char.IsLetter(char)`. You then have to just count them.

If the count remainder of the count divided by 2 is 0 then we have an even number, and vice versa.

For the validation of the character set it's a bit more tricky. The obvious answer, and in my opinion, the easiest to read is to use Regular Expressions. I've always been a big fan of `RegEx`, but it's slow. I also think `RegEx` is easily readable, but if speed is important, you need an alternative.

These would be the options I can think of. Can you think of any others?

1. Use `RegEx.IsMatch(@"[a-zA-Z0-9]+")` - You can test this [online](https://regex101.com/).
1. Use `Enumerable.All(char.IsLetterOrDigit)` with `char.IsLetterOrDigit()` but strictly this is Unicode, not the strict character set described in the task.
1. Use `Enumerable.All(c => (c >= 48 && c <= 57 || c >= 65 && c <= 90 || c >= 97 && c <= 122))` which is valid, and stricky checks the ASCII code ranges.

We also have another few variants relating to calculating the longest word:

1. `words.Max(x => x.Length)` which is an aggregation operation
1. `words.Select(x => x.Length).OrderByDescending(x => x).First()` to select the lengths into a new collection and then order them.

## Benchmarks

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i7-10750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.302
  [Host]     : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT
  DefaultJob : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT


|                         Method |      Mean |     Error |    StdDev |    Median | Rank |  Gen 0 |  Gen 1 | Allocated |
|------------------------------- |----------:|----------:|----------:|----------:|-----:|-------:|-------:|----------:|
| SolutionRegexWithOrderedSelect | 14.139 us | 0.2826 us | 0.5837 us | 13.978 us |    3 | 3.5248 | 0.0305 |  22,112 B |
|           SolutionRegExWithMax | 14.116 us | 0.2799 us | 0.6317 us | 13.968 us |    3 | 3.5095 | 0.0305 |  22,112 B |
|        SolutionIsLetterOrDigit |  2.115 us | 0.0409 us | 0.0383 us |  2.110 us |    2 | 0.2403 |      - |   1,520 B |
|  SolutionStrictIsLetterOrDigit |  1.394 us | 0.0170 us | 0.0133 us |  1.396 us |    1 | 0.1488 |      - |     944 B |
|                       Solution |  2.091 us | 0.0414 us | 0.1140 us |  2.053 us |    2 | 0.1678 |      - |   1,072 B |

// * Hints *
Outliers
  Evaluation.SolutionRegexWithOrderedSelect: Default -> 1 outlier  was  removed (22.30 us)
  Evaluation.SolutionIsLetterOrDigit: Default        -> 1 outlier  was  removed (2.28 us)
  Evaluation.SolutionStrictIsLetterOrDigit: Default  -> 3 outliers were removed (1.45 us..1.54 us)
  Evaluation.Solution: Default                       -> 2 outliers were removed (2.45 us, 2.48 us)

// * Legends *
  Mean      : Arithmetic mean of all measurements
  Error     : Half of 99.9% confidence interval
  StdDev    : Standard deviation of all measurements
  Median    : Value separating the higher half of all measurements (50th percentile)
  Rank      : Relative position of current benchmark mean among all benchmarks (Arabic style)
  Gen 0     : GC Generation 0 collects per 1000 operations
  Gen 1     : GC Generation 1 collects per 1000 operations
  Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  1 us      : 1 Microsecond (0.000001 sec)

// * Diagnostic Output - MemoryDiagnoser *


// ***** BenchmarkRunner: End *****
// ** Remained 0 benchmark(s) to run **
Run time: 00:03:09 (189.14 sec), executed benchmarks: 5

Ding ding ding. We have a winner. N.B. The first three all use the regular expression and it is notable how much slower it is. Done.

## Question

You would like to set a password for a bank account. However, there are three restrictions on the format of the password:

* it has to contain only alphanumerical characters (a−z, A−Z, 0−9);
* there should be an even number of letters;
* there should be an odd number of digits.

You are given a string S consisting of N characters. String S can be divided into words by splitting it at, and removing, the spaces. The goal is to choose the longest word that is a valid password. You can assume that if there are K spaces in string S then there are exactly K + 1 words.

For example, given "test 5 a0A pass007 ?xy1", there are five words and three of them are valid passwords: "5", "a0A" and "pass007". Thus the longest password is "pass007" and its length is 7. Note that neither "test" nor "?xy1" is a valid password, because "?" is not an alphanumerical character and "test" contains an even number of digits (zero).

**Write a function:**

`int solution(string &S);`

that, given a non-empty string S consisting of N characters, returns the length of the longest word from the string that is a valid password. If there is no such word, your function should return −1.

For example, given S = "test 5 a0A pass007 ?xy1", your function should return 7, as explained above.

**Assume that:**

* N is an integer within the range [1..200];
* string S consists only of printable ASCII characters and spaces.

In your solution, focus on correctness. The performance of your solution will not be the focus of the assessment.

## Fastest Solution

* C#
```
public int Solution(string s)
{
    // reject any empty or null string
    if (string.IsNullOrWhiteSpace(s))
    {
        return -1;
    }

    // split the string into valid words using the space character as the delimiter
    var words = s.Split(' ').Where(x => x.IsStrictValidLetterOrDigit()).ToList();

    // not clear from the spec, but if N is range [1..200], should we error if it is too large?

    // aggregate if possible
    return (words.Any()) ? words.Max(x => x.Length) : -1;
}

public static bool IsStrictValidLetterOrDigit(this string s)
{
    return
        // this is strict and matches the spec
        s.All(c => (c >= 48 && c <= 57 || c >= 65 && c <= 90 || c >= 97 && c <= 122)) &&
        // make sure we have an odd number of digits (note IsDigit is radix-10 digit) IsNumber is any unicode number
        s.Count(c => char.IsDigit(c)) % 2 != 0 &&
        // make sure we have an even number of letters 
        s.Count(c => char.IsLetter(c)) % 2 == 0;
}
```

Count the number of letters and digits. Check if it's even number of letters and odd number of digits.

**Complexity:**

* **worst-case time complexity:** `O(n)`, where `n` is the length of `S`.
* **worst-case space complexity:** `O(n)`, where `n` is the length of `S`.
