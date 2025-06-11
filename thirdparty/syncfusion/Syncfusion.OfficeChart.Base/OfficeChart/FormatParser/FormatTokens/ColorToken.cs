// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.ColorToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal class ColorToken : InBracketToken
{
  private const string DEF_COLOR = "Color";
  private const int DEF_MIN_COLOR_INDEX = 1;
  private const int DEF_MAX_COLOR_INDEX = 56;
  private const int DEF_COLOR_INCREMENT = 7;
  private static readonly Regex ColorRegex = new Regex("[Color [0-9]+]");
  private static readonly string[] DEF_KNOWN_COLORS = new string[8]
  {
    "Black",
    "White",
    "Red",
    "Green",
    "Blue",
    "Yellow",
    "Magenta",
    "Cyan"
  };
  private int m_iColorIndex = -1;

  public override int TryParse(string strFormat, int iStartIndex, int iIndex, int iEndIndex)
  {
    int num = strFormat != null ? strFormat.Length : throw new ArgumentNullException(nameof (strFormat));
    if (num == 0)
      throw new ArgumentException("strFormat - string cannot be empty.");
    this.m_iColorIndex = iIndex >= 0 && iIndex <= num - 1 ? this.FindColor(strFormat, iIndex) : throw new ArgumentOutOfRangeException(nameof (iIndex), "Value cannot be less than 0 and greater than than format length - 1.");
    if (this.m_iColorIndex < 0)
    {
      this.m_iColorIndex = this.TryDetectColorNumber(strFormat, iIndex, iEndIndex);
      if (this.m_iColorIndex < 0)
        return iStartIndex;
      this.m_iColorIndex += 7;
      iIndex = iEndIndex + 1;
    }
    else
    {
      string str = ColorToken.DEF_KNOWN_COLORS[this.m_iColorIndex];
      iIndex += str.Length;
      this.m_strFormat = str;
      if (iIndex != iEndIndex)
        return iStartIndex;
      ++iIndex;
    }
    return iIndex;
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    return string.Empty;
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => string.Empty;

  public override TokenType TokenType => TokenType.Color;

  private int FindColor(string strFormat, int iIndex)
  {
    return this.FindString(ColorToken.DEF_KNOWN_COLORS, strFormat, iIndex, true);
  }

  private int TryDetectColorNumber(string strFormat, int iIndex, int iEndIndex)
  {
    int num1 = strFormat != null ? strFormat.Length : throw new ArgumentNullException(nameof (strFormat));
    if (num1 == 0)
      throw new ArgumentException("strFormat - string cannot be empty.");
    if (iIndex < 0 || iIndex > num1 - 1)
      throw new ArgumentOutOfRangeException(nameof (iIndex), "Value cannot be less than 0 and greater than than format length - 1.");
    if (iEndIndex < 0 || iEndIndex > num1 - 1)
      throw new ArgumentOutOfRangeException(nameof (iEndIndex), "Value cannot be less than 0 and greater than than format length - 1.");
    int length = "Color".Length;
    if (string.Compare(strFormat, iIndex, "Color", 0, length, StringComparison.CurrentCultureIgnoreCase) == 0)
    {
      int startIndex = iIndex + length;
      double result;
      if (double.TryParse(strFormat.Substring(startIndex, iEndIndex - startIndex), NumberStyles.Integer, (IFormatProvider) null, out result))
      {
        int num2 = (int) result;
        return num2 >= 1 && num2 <= 56 ? num2 : throw new ArgumentOutOfRangeException("Color index");
      }
    }
    return -1;
  }
}
