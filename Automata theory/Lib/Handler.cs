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
        ChessLine GetShuffledLine(ChessLine chessLine);
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
            if (line.Equals(string.Empty))
                return number;
            line = line.ToLower();
            string lineTrimmed = Regex.Replace(line, @"\s\s+", " ").Trim();
            List<string> split = lineTrimmed.Split(" ").ToList();
            number.FullSentence = lineTrimmed;

            for (int i = 0; i < split.Count; i++)
            {
                //if(i > 3)
                //{
                //    number.ErrorCode = 21; //don't mind that, this one is task requirement
                //    number.ErrorWord = split[4];
                //    return number;

                //}
                Numeral numeral = Numerals.Find(x => x.Word.Equals(split[i]));
                if (numeral == null)
                {
                    number.ErrorCode = 220+i;
                    number.ErrorWord = split[i];
                    return number;
                }
                if(numeral.Word.Equals("hundred") && i == 0 )
                {
                    number.ErrorCode = 40;
                    number.ErrorWord = split[i];
                    return number;
                }
                number.Add(numeral, i);
                if (number.ErrorCode != 0)
                {
                    if(number.ErrorCode > 1200)
                    {
                        int rank = number.ErrorCode % 10;
                        switch (rank)
                        {
                            case 1:
                                {
                                    number.FullSentence = (from item in split
                                                           let n = Numerals.Find(x => x.Word.Equals(item))
                                                           where n.Rank == 1 || n.Rank == 4
                                                           select item).Take(1).First();
                                    break;
                                }
                            case 2:
                                {
                                    number.FullSentence = (from item in split
                                                           let n = Numerals.Find(x => x.Word.Equals(item))
                                                           where n.Rank == 1 || n.Rank == 2 || n.Rank == 4
                                                           select item).Take(1).First();
                                    break;
                                }
                            case 3:
                                {
                                    number.FullSentence = (from item in split
                                                           let n = Numerals.Find(x => x.Word.Equals(item))
                                                           where n.Rank == 3
                                                           select item).Take(1).First();

                                    break;
                                }
                            case 4:
                                {
                                    if (number.ranks[3])
                                    {
                                        number.FullSentence = (from item in split
                                                               let n = Numerals.Find(x => x.Word.Equals(item))
                                                               where n.Rank == 4
                                                               select item).Take(1).First();
                                        break;
                                    }
                                    if(number.ranks[0] && number.ranks[1])
                                    {
                                        if ((number.ErrorCode % 1200 / 10) < 2)
                                        {
                                            number.FullSentence = (from item in split
                                                                   let n = Numerals.Find(x => x.Word.Equals(item))
                                                                   where n.Rank == 1
                                                                   select item).Take(1).First();
                                            break;
                                        }
                                        number.FullSentence = string.Concat((from item in split
                                                               let n = Numerals.Find(x => x.Word.Equals(item))
                                                               where n.Rank == 1 || n.Rank == 2
                                                               select item + " ").Take(2)).Trim();
                                        break;
                                    }
                                    number.FullSentence = (from item in split
                                                           let n = Numerals.Find(x => x.Word.Equals(item))
                                                           where n.Rank == 1 || n.Rank == 2
                                                           select item).Take(1).First();
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                    return number;
                }
            }
            if (split.Count > 2 && !split[1].Equals(Numerals.Find(x => x.Numerical.Equals(100)).Word))
            {
                number.ErrorCode = 23;
                number.ErrorWord = split[1];
                return number;
            }
            //if (split.Count > 4 && number.ErrorCode == 0)
            //{
            //    number.ErrorCode = 21; //don't mind that, this one is task requirement
            //}

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

        public ChessLine GetShuffledLine(ChessLine chessLine)
        {
            chessLine.line1 = Regex.Replace(chessLine.line1, @"\s\s+", " ").Trim();
            chessLine.line2 = Regex.Replace(chessLine.line2, @"\s\s+", " ").Trim();
            List<string> split1 = chessLine.line1.Split(" ").ToList();
            List<string> split2 = chessLine.line2.Split(" ").ToList();
            List<string> split = new List<string>();
            if (split1.Count == 0)
            {
                split.AddRange(split2);
            }
            else
            {
                for (int i = 0, j =0 ; i < split1.Count || j < split2.Count ; i++, j++)
                {
                    if (i < split1.Count)
                        split.Add(split1[i]);
                    if (j < split2.Count)
                        split.Add(split2[j]);
                }
            }
            for (int i = 0; i < split.Count - 1 ; i++)
            {
                split[i] += " ";
            }
            chessLine.Result = string.Concat(split).TrimEnd();
            return chessLine;
        }


    }
}
