using System.Text.RegularExpressions;

namespace Application
{
    public class LongestPassword
    {
        private string _passwords;

        public int Solution(string s)
        {
            // reject any empty or null string
            if (string.IsNullOrWhiteSpace(s))
            {
                return -1;
            }

            // split the string into valid words using rhe space character as the delimiter
            var words = s.Split(' ').Where(x => x.IsValid()).ToList();

            // not clear from the spec, but if N is range [1..200], should we error if it is too large?

            // aggregate if possible
            return (words.Any()) ? words.Max(x => x.Length) : -1;
        }

        public int SolutionSelect(string s)
        {
            // reject any empty or null string
            if (string.IsNullOrWhiteSpace(s))
            {
                return -1;
            }

            // split the string into valid words using rhe space character as the delimiter
            var words = s.Split(' ').Where(x => x.IsValid());

            // if no words then return
            if (!words.Any())
            {
                return -1;
            }

            // not clear from the spec, but if N is range [1..200], should we error if it is too large?

            // use select instead
            return (words.Any()) ? words.Select(x => x.Length).OrderByDescending(x => x).First() : -1;
        }

        public int SolutionMaxBy(string s)
        {
            // reject any empty or null string
            if (string.IsNullOrWhiteSpace(s))
            {
                return -1;
            }

            // split the string into valid words using rhe space character as the delimiter
            var words = s.Split(' ').Where(x => x.IsValid()).ToList();

            // not clear from the spec, but if N is range [1..200], should we error if it is too large?

            // words.MaxBy(s => s.Length) in NET6 is O(N)
            return words.Any() ? words.MaxBy(x => x.Length).First() : -1;
        }

        /// <summary>
        /// Note. This isn't stricktly [a-ZA-Z0-9].
        /// Valid letters and decimal digits are members of the following categories in UnicodeCategory: 
        /// UppercaseLetter, LowercaseLetter, TitlecaseLetter, ModifierLetter, OtherLetter, or DecimalDigitNumber.
        /// </summary>
        /// <param name="s"></param>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.char.isletterordigit?view=net-6.0"/>
        /// <returns></returns>
        public int SolutionIsLetterOrDigit(string s)
        {
            // reject any empty or null string
            if (string.IsNullOrWhiteSpace(s))
            {
                return -1;
            }

            // split the string into valid words using rhe space character as the delimiter
            var words = s.Split(' ').Where(x => x.IsValidLetterOrDigit()).ToList();

            // not clear from the spec, but if N is range [1..200], should we error if it is too large?

            // words.MaxBy(s => s.Length) in NET6 is O(N)
            return words.Any() ? words.Max(x => x.Length) : -1;
        }

        /// <summary>
        /// Note. This is stricktly ASCII [a-ZA-Z0-9]
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int SolutionStrictIsLetterOrDigit(string s)
        {
            // reject any empty or null string
            if (string.IsNullOrWhiteSpace(s))
            {
                return -1;
            }

            // split the string into valid words using rhe space character as the delimiter
            var words = s.Split(' ').Where(x => x.IsStrictValidLetterOrDigit()).ToList();

            // not clear from the spec, but if N is range [1..200], should we error if it is too large?

            // words.MaxBy(s => s.Length) in NET6 is O(N)
            return words.Any() ? words.Max(x => x.Length) : -1;
        }
    }

    public static class StringExtensions
    {
        /// <summary>
        /// Validates the permise that a password contains a fixed character set and a odd number of digits and an even number of letters.
        /// </summary>
        /// <param name="s"></param>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.char.isdigit"/>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.char.isletter"/>
        /// <returns></returns>
        public static bool IsValid(this string s)
        {
            return
                // do we have a match of the regex limiting the character set (could we have used lookaheads here)?
                new Regex(@"^[a-zA-Z0-9]+$").IsMatch(s) &&
                // make sure we have an odd number of digits (note IsDigit is radix-10 digit) IsNumber is any unicode number
                s.Count(c => char.IsDigit(c)) % 2 != 0 &&
                // make sure we have an even number of letters 
                s.Count(c => char.IsLetter(c)) % 2 == 0;
        }

        /// <summary>
        /// Validates the permise that a password contains a fixed character set and a odd number of digits and an even number of letters.
        /// </summary>
        /// <param name="s"></param>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.char.isdigit"/>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.char.isletter"/>
        /// <returns></returns>
        public static bool IsValidLetterOrDigit(this string s)
        {
            return
                // do we have a match of the letter or digit. Warning this has unicode support and doesn't therefore match the spec.
                s.All(char.IsLetterOrDigit) &&
                // make sure we have an odd number of digits (note IsDigit is radix-10 digit) IsNumber is any unicode number
                s.Count(c => char.IsDigit(c)) % 2 != 0 &&
                // make sure we have an even number of letters 
                s.Count(c => char.IsLetter(c)) % 2 == 0;
        }

        /// <summary>
        /// Validates the permise that a password contains a fixed character set and a odd number of digits and an even number of letters.
        /// </summary>
        /// <param name="s"></param>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.char.isdigit"/>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.char.isletter"/>
        /// <returns></returns>
        public static bool IsStrictValidLetterOrDigit(this string s)
        {
            return
                // this is stricker and matches the spec
                s.All(c => (c >= 48 && c <= 57 || c >= 65 && c <= 90 || c >= 97 && c <= 122)) &&
                // make sure we have an odd number of digits (note IsDigit is radix-10 digit) IsNumber is any unicode number
                s.Count(c => char.IsDigit(c)) % 2 != 0 &&
                // make sure we have an even number of letters 
                s.Count(c => char.IsLetter(c)) % 2 == 0;
        }
    }
}