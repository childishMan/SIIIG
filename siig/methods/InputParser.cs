using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace visual
{
    static class inputParser
    {
        /// <summary>
        /// parsing string into Dictionary
        /// </summary>
        static public Dictionary<int, double> Parse(string input)
        {
            if (isCorrect(input))
            {

                var signal = new Dictionary<int, double>();

                var pair = input.Split(' ');
                int key;
                double value;
                foreach (var item in pair)
                {
                    var data = item.Split(';');
                    var keyParse = int.TryParse(data.First(), out key);
                    var valueParse = double.TryParse(data.Last(), out value);
                    if (valueParse && keyParse)
                    {
                        if (!signal.ContainsKey(key))
                            signal.Add(key, value);
                    }
                }
                if (signal.Count == 0)
                {
                    return null;
                }
                return signal;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// regex string check to be in correct format and cleaning some not valid data from it
        /// </summary>
        static public bool isCorrect(string input)
        {
            Regex CorrectInput = new Regex("^((-)?[0-9]*;[0-9]*(.[0-9]*)?)");

            var pairs = input.Split(' ');

            foreach (var pair in pairs)
            {
                if (!CorrectInput.IsMatch(pair.Trim(' ')) && pair != "")
                    return false;
            }

            return true;
        }


    }

    static class DictionaryWorker
    {
        public static string ToString(Dictionary<int, double> dict)
        {
            string res = "";
            foreach (var item in dict)
            {
                res += $"{item.Key};{item.Value} ";
            }

            return res;
        }

        public static Dictionary<int, double> Sort(Dictionary<int, double> dict)
        {
            var l = dict.OrderBy(key => key.Key);
            return l.ToDictionary(key => key.Key, val => val.Value);
        }
    }

    [TestFixture]
    class ParserTesting
    {
        [Test]
        public void TestRegex()
        {
            string f = "1;2 2";
            var dict = new Dictionary<int, double>()
            {
                {1,2 },
                {5,8 }
            };

            Assert.False(inputParser.isCorrect(f));
            Assert.True(inputParser.isCorrect("1;2  5;8"));
            Assert.False(inputParser.isCorrect("f;5 -1;7"));
            Assert.True(inputParser.isCorrect("-1;8 7;2 0;2.5"));
        }
    }
}
