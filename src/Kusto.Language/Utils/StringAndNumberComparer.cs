using System;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    /// <summary>
    /// Compares strings with embedded numbers using the numbers numeric order, not the text order.
    /// </summary>
    internal class StringAndNumberComparer : IComparer<string>
    {
        private readonly StringComparison _comparison;

        private StringAndNumberComparer(StringComparison comparison)
        {
            _comparison = comparison;
        }

        public static StringAndNumberComparer Ordinal = 
            new StringAndNumberComparer(StringComparison.Ordinal);

        public static StringAndNumberComparer OrdinalIgnoreCase = 
            new StringAndNumberComparer(StringComparison.OrdinalIgnoreCase);

        public int Compare(string xText, string yText)
        {
            int xStart = 0;
            int yStart = 0;

            while (xStart < xText.Length && yStart < yText.Length)
            {
                var xSegmentIsNumber = char.IsDigit(xText[xStart]);
                var ySegmentIsNumber = char.IsDigit(yText[yStart]);
                
                if (xSegmentIsNumber && ySegmentIsNumber)
                {
                    // both segments are numbers
                    int xNumberLength = LengthOfNumber(xText, xStart);
                    int yNumberLength = LengthOfNumber(yText, yStart);

                    var comp = CompareNumberSegment(xText, xStart, xNumberLength, yText, yStart, yNumberLength);                   
                    if (comp != 0)
                        return comp;
                    
                    xStart += xNumberLength;
                    yStart += yNumberLength;
                }
                else if (!xSegmentIsNumber && !ySegmentIsNumber)
                {
                    // neither segments are numbers
                    var commonLength = GetCommonLengthWithoutDigits(xText, xStart, yText, yStart);

                    var comp = string.Compare(xText, xStart, yText, yStart, commonLength, _comparison);
                    if (comp != 0)
                        return comp;

                    xStart += commonLength;
                    yStart += commonLength;
                }
                else
                {
                    // one segment is a number and not the other, so the result is just comparison of first character
                    // since text characters can either sort before or after numbers
                    return xText[xStart] - yText[yStart];
                }
            }

            // everything was identical until one ran out of characters
            // therefore the one without remaining text orders first
            if (xText.Length - xStart > 0)
            {
                // x has more characters, so x orders after y
                return 1;
            }
            else if (yText.Length - yStart > 0)
            {
                // y has more characters, so x orders before y
                return -1;
            }
            else
            {
                // both are the same
                return 0;
            }
        }

        /// <summary>
        /// Gets the largest length the two strings share without digits.
        /// </summary>
        private int GetCommonLengthWithoutDigits(string xText, int xStart, string yText, int yStart)
        {
            int length = 0;
            
            while (xStart + length < xText.Length && !char.IsDigit(xText[xStart + length])
                && yStart + length < yText.Length && !char.IsDigit(yText[yStart + length]))
            {
                length++;
            }

            return length;
        }

        private int CompareNumberSegment(string xText, int xStart, int xLength, string yText, int yStart, int yLength)
        {
            // if x is longer, skip leading zeros up to length of y
            while (xStart < xText.Length && xLength > yLength && xText[xStart] == '0')
                xStart++;

            // if y is longer, skip leading zeros up to length of x
            while (yStart < yText.Length && yLength > xLength && yText[yStart] == '0')
                yStart++;

            // if both numbers have same length use text comparsison
            if (xLength > 0 && xLength == yLength)
                return string.Compare(xText, xStart, yText, yStart, xLength);

            // longer wins because it has more digits
            return xLength - yLength;
        }

        private static int LengthOfNumber(string text, int startIndex)
        {
            int index = startIndex;
            for (; index < text.Length; index++)
            {
                if (!char.IsDigit(text[index]))
                    break;
            }
            return index - startIndex;
        }
    }
}