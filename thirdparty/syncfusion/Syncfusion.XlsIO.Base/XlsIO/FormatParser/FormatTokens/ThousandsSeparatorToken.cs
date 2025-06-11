// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.ThousandsSeparatorToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class ThousandsSeparatorToken : SingleCharToken
{
  private const char DEF_FORMAT = ',';
  private bool m_bAfterDigits;

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    return string.Empty;
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => string.Empty;

  public override char FormatChar => ',';

  public override TokenType TokenType => TokenType.ThousandsSeparator;

  public bool IsAfterDigits
  {
    get => this.m_bAfterDigits;
    set => this.m_bAfterDigits = value;
  }

  public double PreprocessValue(double value)
  {
    if (this.m_bAfterDigits)
      value /= 1000.0;
    return value;
  }
}
