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
1. `words.MaxBy(x => x.Length).First()` which is available now in NET6 and an aggreagtion 0(N), but kind of pointless in this scenario

## Benchmarks

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i7-10750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.302
  [Host]     : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT
  DefaultJob : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT


|                   Method |      Mean |     Error |    StdDev | Rank |  Gen 0 |  Gen 1 | Allocated |
|------------------------- |----------:|----------:|----------:|-----:|-------:|-------:|----------:|
|                UseSelect | 13.736 us | 0.2718 us | 0.3337 us |    4 | 3.5248 | 0.0305 |  22,144 B |
|                   UseMax | 12.451 us | 0.2434 us | 0.2158 us |    3 | 3.5095 | 0.0305 |  22,048 B |
|                 UseMaxBy | 15.916 us | 0.7284 us | 2.1476 us |    5 | 3.5248 | 0.0305 |  22,112 B |
|       UseIsLetterOrDigit |  1.862 us | 0.0368 us | 0.0882 us |    2 | 0.2308 |      - |   1,456 B |
| UseStrictIsLetterOrDigit |  1.522 us | 0.0301 us | 0.0411 us |    1 | 0.1545 |      - |     976 B |

Ding ding ding. We have a winner.

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

## Solutions

### Solution 1

* C++
```
#include <algorithm>
#include <sstream>
#include <cstring>
#include <vector>
vector<string> getWords(string S)
{
    stringstream ss(S);
    vector<string> res;
    string token;
    while(getline(ss, token, ' '))
        res.push_back(token);
    return res;
}

int check(string word)
{
    int alp=0, digit=0;
    for(char w : word)
    {
        if(isalpha(w)) alp++;
        else if(isdigit(w)) digit++;
        else return 0;
    }
    return (alp%2==0 && digit%2!=0) ? alp+digit : 0;
}

int solution(string &S) {
    // write your code in C++14 (g++ 6.2.0)
    vector<string> words = getWords(S);
    int res = 0;
    for(string word : words)
        res = max(res, check(word));
    return res==0 ? -1 : res;
}
```

* C++
```
int check(string word)
{
    int alp=0, digit=0;
    for(char w : word)
    {
        if(isalpha(w)) alp++;
        else if(isdigit(w)) digit++;
        else return -1;
    }
    return (alp%2==0 && digit%2!=0) ? alp+digit : -1;
}

int solution(string &S) {
    // write your code in C++14 (g++ 6.2.0)
    vector<string> words = getWords(S);
    int res = -1;
    for(string word : words)
        res = max(res, check(word));
    return res;
}
```

Count the number of letters and digits. Check if it's even number of letters and odd number of digits.

**Complexity:**

* **worst-case time complexity:** `O(n)`, where `n` is the length of `S`.
* **worst-case space complexity:** `O(n)`, where `n` is the length of `S`.