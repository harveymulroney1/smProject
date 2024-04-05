using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace SocialMediaMP.Data
{

    public class FilterWords
    {
        public static Dictionary<string, string> FilterDictionary = new Dictionary<string, string>();

        void BuildDictionary()
        {
            FilterDictionary.Add("***", "");
        }
    }
}
