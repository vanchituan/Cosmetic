using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CosmeticMVC.Libraries.ExportExcel
{
    public class GeneralAlphabe_
    {
        private static string[] _alphabets = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        public static string GetColumn(int indexColumn)
        {
            return GetAlphabet(indexColumn);
        }

        private static string GetAlphabet(int indexColumn)
        {
            var a = indexColumn / 26;
            var m = indexColumn % 26;
            if (a != 0 && m != 0)
            {
                return GetAlphabet(a - 1) + _alphabets[m];
            }
            if (a != 0 && m == 0)
            {
                return GetAlphabet(a - 1) + _alphabets[m];
            }
            return _alphabets[m];
        }
    }
}