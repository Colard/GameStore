using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace GameStore.Controllers.Functions.Data
{
    public class JSImageByteConverter
    {
        //prepare img to html view
        public static string FromBase64ToImageString(byte[] input)
        {
            return "data:image/jpeg;base64," + Convert.ToBase64String(input, 0, input.Length);
        }

        //prepare img array to html view
        public static IEnumerable<string> ArrOfBase64ToImageString(IEnumerable<byte[]> input)
        {
            List<string> output = new List<string>();
            foreach (byte[] byteArr in input)
            {
                output.Add(FromBase64ToImageString(byteArr));
            }

            return output;
        }

        //check img format
        public static bool CheckImgStringToBase64(string input, out byte[] output)
        {
            Regex exp = new Regex(@"^data:image/jpeg;base64,|^data:image/png;base64,");
            if (checkFileFormat(input, exp))
            {
                string base64String = exp.Replace(input, String.Empty);
                return TryGetFromBase64String(base64String, out output);
            }

            output = null;
            return false;
        }

        //check img code format
        public static bool TryGetFromBase64String(string input, out byte[] output)
        {
            output = null;
            try
            {
                output = Convert.FromBase64String(input);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static bool checkFileFormat(string file, Regex exp)
        {
            return exp.IsMatch(file);
        }
    }
}