using System;
using System.Collections.Generic;
using System.Linq;

namespace visual
{   

    static public class Operations
    {
        /// <summary>
        /// scaling your Operations by factor
        /// </summary>
        static public Dictionary<int, double> Scale(Dictionary<int, double> input, int factor)
        {
            var result = new Dictionary<int, double>();
            foreach (var item in input)
            {
                result.Add(item.Key, item.Value * factor);
            }

            return result;
        }
        /// <summary>
        /// reversing Operations
        /// </summary>
        static public Dictionary<int, double> Reverse(Dictionary<int, double> input)
        {
            var result = new Dictionary<int, double>();

            foreach (var item in input)
            {
                result.Add(item.Key * -1, item.Value);
            }

            return result;
        }
        /// <summary>
        /// shifting Operations by time
        /// </summary>
        static public Dictionary<int, double> ShiftByTime(Dictionary<int, double> input, int shift)
        {
            var result = new Dictionary<int, double>();

            foreach (var item in input)
            {
                result.Add(item.Key + shift, item.Value);
            }

            return result;
        }
        /// <summary>
        /// expanding in tyme by expandfactor. to make expand - pass true, to squeeze - false
        /// </summary>
        static public Dictionary<int, double> ExpandInTime(Dictionary<int, double> input, double expandFactor, bool exp)
        {
            var result = new Dictionary<int, double>();
            int iterExpandFactor = Convert.ToInt32(expandFactor);
            int cnt = 1;
            int i = input.Keys.Min() - 1;
            if (iterExpandFactor == 1)
            {
                return input;
            }
            if (!exp)
            {
                if (iterExpandFactor > input.Count)
                {
                    throw new IndexOutOfRangeException();
                }
                else
                {
                    for (i = input.Keys.First((key) => key > i); i < input.Keys.Last(); i = input.Keys.First((key) => key > i))
                    {
                        if (cnt == 1 || cnt % iterExpandFactor == 0)
                            result.Add(i, input[i]);
                        cnt++;
                    }
                }
            }
            else
            {
                double avg = 0;
                int nextKey = 0, newKey = 0;
                int pos = 0;
                int expFactor = 1;
                for(i = input.Keys.First((key)=> key>i); i < input.Keys.Last(); i = input.Keys.First((key) => key > i))
                {
                    newKey = i * expFactor;
                    cnt = 1;
                    nextKey = input.Keys.First((key) => key > i);
                    result.Add(newKey, input[i]);

                    avg = (input[i] > input[nextKey]) ? (input[i] - input[nextKey]) / expandFactor : (input[nextKey] - input[i]) / expandFactor;

                    for (int j = iterExpandFactor - 1; j > 0; j--)
                    {
                        pos = Convert.ToInt32(newKey + ((nextKey  - i)) * cnt);
                        if (input[i] > input[nextKey])
                            result.Add(pos, input[i] - avg * (iterExpandFactor - j));
                     else
                            result.Add(pos, input[nextKey] - avg * j);
                        cnt++;
                    }
                    expFactor = iterExpandFactor;
                }
                result.Add(input.Keys.Last() * iterExpandFactor, input.Values.Last());
            }

            return result;
        }
        /// <summary>
        /// adding two signals
        /// </summary>
        static public Dictionary<int, double> AddTwoSignals(Dictionary<int, double> firstSignal, Dictionary<int, double> secondSignal)
        {
            var result = new Dictionary<int, double>();
            foreach (var item in firstSignal)
            {
                int key = item.Key;
                if (secondSignal.ContainsKey(key))
                {
                    result.Add(item.Key, (firstSignal[key] + secondSignal[key])); 
                }
                else
                {
                    result.Add(item.Key, item.Value);
                }
            }
            foreach(var item in secondSignal)
            {
                if (!result.ContainsKey(item.Key))
                {
                    result.Add(item.Key, item.Value);
                }
            }


            return result;
        }
        /// <summary>
        /// multiplying two signals
        /// </summary>
        static public Dictionary<int, double> Multiply(Dictionary<int, double> firstSignal, Dictionary<int, double> secondSignal)
        {
            var result = new Dictionary<int, double>();

            foreach(var item in firstSignal)
            {
                int key = item.Key;
                if (secondSignal.ContainsKey(key))
                {
                    result.Add(key, firstSignal[key] * secondSignal[key]);
                }
                else
                {
                    result.Add(key, 0);
                }
            }
            foreach(var item in secondSignal)
            {
                if (!result.ContainsKey(item.Key))
                {
                    result.Add(item.Key, 0);
                }
            }

            return result;
        }
    }

}
