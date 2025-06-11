// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.ReservedPlaceToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class ReservedPlaceToken : FormatTokenBase
{
  private const char DEF_START = '_';
  private const string DEF_SPACE = " ";
  private const string DEF_EN_QUAD_SPACE = " ";

  public override int TryParse(string strFormat, int iIndex)
  {
    int num = strFormat != null ? strFormat.Length : throw new ArgumentNullException(nameof (strFormat));
    if (num == 0)
      throw new ArgumentException("strFormat - string cannot be empty.");
    bool flag = iIndex + 1 >= num;
    if (strFormat[iIndex] == '_' && !flag)
    {
      this.m_strFormat = strFormat[iIndex + 1].ToString();
      iIndex += 2;
    }
    return iIndex;
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    return this.ApplyFormat(string.Empty, bShowHiddenSymbols);
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols)
  {
    if (bShowHiddenSymbols)
      return this.m_strFormat;
    return this.m_strFormat == "-" ? " " : " ";
  }

  public override TokenType TokenType => TokenType.ReservedPlace;
}
