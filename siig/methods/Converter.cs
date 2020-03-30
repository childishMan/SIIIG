using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace siig.methods
{
    class MyConverter
    {
        public static List<double> DictionaryToList(Dictionary<int, double> dict)
        {
            var list = new List<double>();

            for (int i = dict.Keys.First(); i <= dict.Keys.Last(); i++)
            {
                if (dict.Keys.Contains(i))
                {
                    list.Add(dict[i]);
                }
                else
                {
                    list.Add(0);
                }
            }

            return list;
        }

        public static Dictionary<int,double> ListToDictionary(List<double> list)
        {
            var dict = new  Dictionary<int,double>();
            int cnt = 0;
            foreach (var item in list)
            {
                dict.Add(cnt++,item);
            }

            return dict;
        }
    }
}
