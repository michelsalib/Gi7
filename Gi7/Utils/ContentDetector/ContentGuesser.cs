using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Gi7.Utils.ContentDetector
{
    public class ContentGuesser
    {
        public HeaderSignature GuessType(String filename, byte[] data)
        {
            string extension = filename.Split('.').Last();
            foreach (HeaderSignature header in HeaderSignature.StockSignatures)
                if (MatchesFileExtension(header.FileExtensions, extension)) return header;

            HeaderSignature result = null;
            int bestScore = int.MaxValue;
            byte[] realData = data.Take(100).ToArray();
            foreach (HeaderSignature header in HeaderSignature.StockSignatures)
            {
                int score = MatchesSignature(header.Signature, realData);
                if (score >= 0 && score < bestScore)
                {
                    if (score == 0)
                        return header;
                    result = header;
                    bestScore = score;
                }
            }
            return result;
        }

        private bool MatchesFileExtension(string[] extensions, string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return false;
            return extensions.Any(fileExtension => string.Compare(extension, fileExtension.Trim('.'), StringComparison.CurrentCultureIgnoreCase) == 0);
        }

        private int MatchesSignature(String signature, byte[] buffer)
        {
            if (buffer == null ||
                buffer.Length < signature.Length)
                return -1;
            return IsPatternContainedInBuffer(buffer, GetBytes(signature));
        }

        private int IsPatternContainedInBuffer(byte[] buffer, byte[] pattern)
        {
            if (pattern == null || pattern.Length <= 0 ||
                buffer == null || buffer.Length <= 0 ||
                pattern.Length > buffer.Length)
                return -1;
            return IndexOfPattern(buffer, pattern);
        }

        private int IndexOfPattern<T>(IEnumerable<T> array, IEnumerable<T> pattern)
        {
            bool found;

            IEnumerator<T> i = array.GetEnumerator();
            IEnumerator<T> j = pattern.GetEnumerator();

            int i_index = -1;
            int pattern_length = 0;
            for (;;)
            {
                if (!i.MoveNext())
                {
                    if (!j.MoveNext())
                    {
                        i_index++;
                        found = true;
                        break;
                    }
                    found = false;
                    break;
                }
                i_index++;
                if (!j.MoveNext())
                {
                    found = true;
                    break;
                }
                pattern_length++;
                if (!i.Current.Equals(j.Current))
                {
                    j = pattern.GetEnumerator();
                    pattern_length = 0;
                }
            }
            if (!found)
                return -1;
            return i_index - pattern_length;
        }

        private byte HexToByte(string hex)
        {
            if (hex.Length > 2 || hex.Length <= 0)
                throw new ArgumentException("hex must be 1 or 2 characters in length");
            byte newByte = byte.Parse(hex, NumberStyles.HexNumber);
            return newByte;
        }

        private bool IsHexDigit(char c)
        {
            int numA = Convert.ToInt32('A');
            int num1 = Convert.ToInt32('0');
            c = Char.ToUpper(c);
            int numChar = Convert.ToInt32(c);
            if (numChar >= numA && numChar < (numA + 6))
                return true;
            if (numChar >= num1 && numChar < (num1 + 10))
                return true;
            return false;
        }

        private byte[] GetBytes(string hexString)
        {
            string newString = hexString.Where(IsHexDigit).Aggregate(string.Empty, (current, c) => current + c);
            if (newString.Length%2 != 0)
                newString = newString.Substring(0, newString.Length - 1);

            int byteLength = newString.Length/2;
            var bytes = new byte[byteLength];
            int j = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                var hex = new String(new[] {newString[j], newString[j + 1]});
                bytes[i] = HexToByte(hex);
                j = j + 2;
            }
            return bytes;
        }
    }
}