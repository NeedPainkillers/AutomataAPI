using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Automata_theory.Model;
using Microsoft.Extensions.Options;

namespace Automata_theory.Lib
{
    public interface IHandler
    {
        List<Numeral> GetNumerals(string filePath);
        Number GetNumberDecimal(string line);
        //Number GetNumberRomanian(string line);
        //Number GetNumberRomanian(int number);
        Number GetNumberRomanian(Number number);
    }

    public class Handler : IHandler
    {
        public static List<Numeral> Numerals { get; set; } = new List<Numeral>();
        public Handler(IOptions<Settings> settings)
        {
            GetNumerals(Environment.CurrentDirectory + settings.Value.numeralsPath);
        }

        public List<Numeral> GetNumerals(string filePath)
        {
            List<string> file = File.ReadAllLines(filePath).ToList();
            Numerals = (from line in file
                        let split = line.Split(" ")
                        where int.TryParse(split[0], out _) && int.TryParse(split[2], out _) && split.Count() > 2
                        select new Numeral() { Numerical = int.Parse(split[0]), Word = split[1], Rank = short.Parse(split[2]) }).ToList();
            return Numerals;
        }

        public Number GetNumberDecimal(string line)
        {
            Number number = new Number();
            string lineTrimmed = Regex.Replace(line, @"\s", "").Trim();
            List<string> split = lineTrimmed.Split(" ").ToList();
            if (split.Count > 4 && number.ErrorCode == 0)
            {
                number.ErrorCode = 21; //don't mind that, this one is task requirement
            }
            for (int i = 0; i < split.Count; i++)
            {
                Numeral numeral = Numerals.Find(x => x.Word.Equals(split[i]));
                if (numeral == null)
                {
                    number.ErrorCode = 22;
                    return number;
                }
                if (split.Count > 2 && !split[1].Equals(Numerals.Find(x => x.Numerical.Equals(100)).Word))
                {
                    number.ErrorCode = 23;
                    return number;
                }
                if((numeral.Rank != 1 && i == 0) || (numeral.Rank != 2 && split.Count == 2 && i ==0) || (numeral.Rank == 4 && (i != 2 || i != 0)))
                {
                    number.ErrorCode = 30 + i;
                    return number;
                }
                number.Add(numeral, i);
                if (number.ErrorCode != 0)
                {
                    return number;
                }
            }
            return number;
        }

        public Number GetNumberRomanian(Number number)
        {
            //
            var romanNumerals = new string[][]
            {
                new string[]{"", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"}, // ones
                new string[]{"", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC"}, // tens
                new string[]{"", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM"}, // hundreds
                new string[]{"", "M", "MM", "MMM"} // thousands
            };

            // split integer string into array and reverse array
            var intArr = number.num.ToString().Reverse().ToArray();
            var len = intArr.Length;
            var romanNumeral = "";
            var i = len;

            // starting with the highest place (for 3046, it would be the thousands
            // place, or 3), get the roman numeral representation for that place
            // and add it to the final roman numeral string
            while (i-- > 0)
            {
                romanNumeral += romanNumerals[i][Int32.Parse(intArr[i].ToString())];
            }
            //Not mine. Source https://pastebin.com/w0hm9n5W

            number.romanian = romanNumeral;

            return number;
        }

    }
}
