// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.Hour24Token
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class Hour24Token : FormatTokenBase
{
  private static readonly Regex HourRegex = new Regex("\\[[hH]+\\]", RegexOptions.Compiled);

  public override int TryParse(string strFormat, int iIndex)
  {
    return this.TryParseRegex(Hour24Token.HourRegex, strFormat, iIndex);
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    double d = value;
    if (d <= 60.0)
      --d;
    DateTime dateTime = DateTime.FromOADate(d);
    double num1 = d * 24.0;
    double num2 = value > 0.0 ? (Math.Ceiling(num1 % 24.0) == (double) dateTime.Hour ? Math.Ceiling(num1) : Math.Floor(num1)) : Math.Ceiling(num1);
    if (num2 < 24.0)
      num2 = (double) dateTime.Hour;
    return ((int) num2).ToString();
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => string.Empty;

  public override TokenType TokenType => TokenType.Hour24;
}
