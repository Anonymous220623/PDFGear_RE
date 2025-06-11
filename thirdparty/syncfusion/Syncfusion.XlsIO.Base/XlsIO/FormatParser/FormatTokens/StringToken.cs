// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.StringToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class StringToken : FormatTokenBase
{
  private static readonly Regex DayRegex = new Regex("\"[^\"]*\"", RegexOptions.Compiled);

  public override int TryParse(string strFormat, int iIndex)
  {
    int regex = this.TryParseRegex(StringToken.DayRegex, strFormat, iIndex);
    if (regex != iIndex)
      this.m_strFormat = strFormat.Substring(iIndex + 1, this.m_strFormat.Length - 2);
    return regex;
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    return this.m_strFormat;
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => this.m_strFormat;

  public override TokenType TokenType => TokenType.String;
}
