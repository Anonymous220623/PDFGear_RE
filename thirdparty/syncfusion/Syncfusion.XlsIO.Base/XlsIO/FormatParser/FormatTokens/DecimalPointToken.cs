// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.DecimalPointToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class DecimalPointToken : SingleCharToken
{
  private const char DEF_FORMAT = '.';

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    return this.m_strFormat == null || this.m_strFormat != CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator ? CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator : this.m_strFormat;
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => string.Empty;

  public override char FormatChar => '.';

  public override TokenType TokenType => TokenType.DecimalPoint;
}
