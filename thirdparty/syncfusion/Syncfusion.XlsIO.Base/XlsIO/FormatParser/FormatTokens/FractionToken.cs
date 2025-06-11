// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.FractionToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class FractionToken : SingleCharToken
{
  private const char DEF_FORMAT_CHAR = '/';

  public override char FormatChar => '/';

  public override TokenType TokenType => TokenType.Fraction;

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    return section == null || section.FormatType != ExcelFormatType.DateTime ? this.m_strFormat : DateTimeFormatInfo.CurrentInfo.DateSeparator;
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => this.m_strFormat;
}
