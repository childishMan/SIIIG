﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace siig.models
{
    static class Corelation
    {
        public static List<double> DictionaryToList(Dictionary<int, double> dict)
        {
            var List = new List<double>();

            for (int i = dict.Keys.First(); i <= dict.Keys.Last(); i++)
            {
                if (dict.Keys.Contains(i))
                {
                    List.Add(dict[i]);
                }
                else
                {
                    List.Add(0);
                }
            }

            return List;
        }

        public static List<double> MutualCorealtion(Dictionary<int, double> FirstSignal, Dictionary<int, double> SecondSignal, bool normalize = false)
        {
            var x1 = DictionaryToList(FirstSignal);
            var x2 = DictionaryToList(SecondSignal);
            
            int size = (x1.Count >= x2.Count) ? x1.Count : x2.Count;

            List<double> corelated = new List<double>();
            for (int i = 0; i < size * 2 - 1; i++)
            {
                corelated.Add(0);
            }

            var sum1 = 0.0;
            var sum2 = 0.0;

            for (int i = 0; i < size; ++i)
            {

                sum1 = sum2 = 0.0;
                for (int k = 0; k < size - i; ++k)
                {
                    sum1 += x1[k] * x2[k + i];
                    sum2 += x2[k] * x1[k + i];
                }

                corelated[size + i - 1] = sum1 / size;
                corelated[size - i - 1] = sum2 / size;
            }

            if (normalize)
            {
                sum1 = x1.Sum((x) => x * x);
                sum2 = x2.Sum((x) => x * x);

                var sf = Math.Sqrt(sum1 * sum2) / size;

                for (int i = 0; i < corelated.Count; i++)
                {
                    corelated[i] = corelated[i] / sf;
                }
            }
            return corelated;
        }

        public static List<double> autoCorelation(Dictionary<int, double> Signal , bool normalize = false)
        {
            var x = DictionaryToList(Signal);

            List<double> corelated = new List<double>();

            var size = x.Count;
            for (int i = 0; i < size; ++i)
            {
                var sum = 0.0;
                for (int k = 0; k < size - i; ++k)
                {
                    sum += x[k] * x[k + i];
                }
                corelated.Add(sum / size);
            }

            if (normalize)
            {
                var sf = corelated.First();
                for (int i = 0; i < corelated.Count; i++)
                {
                    corelated[i] = corelated[i] / sf;
                }
            }

            return corelated;
        }

    }

}