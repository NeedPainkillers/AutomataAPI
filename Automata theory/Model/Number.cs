using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Automata_theory.Model
{
    public class Number
    {
        public int num { get; set; } = 0;
        public string romanian { get; set; } = string.Empty;
        public bool[] ranks { get; set; } = new bool[4] { false, false, false, false };
        public int ErrorCode { get; set; } = 0;
        public string ErrorWord { get; set; } = "";
        public string FullSentence { get; set; } = "";

        /// <summary>
        /// Error codes explained:
        /// 11 - number is to big
        /// 12x - multiple same rank numerals given, where x is word position in line (121 means second word is incorrect)
        /// 21 - wrong amount of words given
        /// 22 - wrong numeral word given (word not found)
        /// 23 - expected hundred as second word 
        /// 22x - unknown word on pos x
        /// 3x - error codes connected with specific positions of numerals in line (30 means first word is incorrect)
        /// </summary>
        /// <returns></returns>

        public string GetString()
        {
            return num.ToString();
        }

        public string Add(Numeral numeral, int position)
        {
            if (numeral.Rank == 4)
            {
                if (ranks[0] || ranks[1])
                {
                    ErrorCode = 1200 + position * 10 + numeral.Rank;
                    ErrorWord = numeral.Word;
                    return "Error: 12";
                }
                ranks[0] = true;
                ranks[1] = true;
                ranks[3] = true;
            }
            else
            {
                if (ranks[numeral.Rank - 1])
                {
                    ErrorCode = 1200 + position*10 + numeral.Rank;
                    ErrorWord = numeral.Word;
                    return "Error: 12";
                }

                ranks[numeral.Rank - 1] = true;
                if (ranks[0])
                {
                    ranks[1] = true;
                }
            }
            if (numeral.Numerical.Equals(100))
            {
                num *= 100;
                if (num > 999)
                {
                    ErrorCode = 11;
                    ErrorWord = numeral.Word;
                    return "Error: 11";
                }
                if (num == 0)
                {
                    num = 100;
                }
                else
                {
                    ranks[0] = false;
                }
                ranks[1] = false;
                return "Success";
            }
            num += numeral.Numerical;
            if (num > 999)
            {
                ErrorCode = 11;
                ErrorWord = numeral.Word;
                return "Error: 11";
            }
            return "Success";
        }
    }
}

