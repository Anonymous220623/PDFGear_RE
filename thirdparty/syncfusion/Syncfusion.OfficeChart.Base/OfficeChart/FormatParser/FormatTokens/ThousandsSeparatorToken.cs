// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.ThousandsSeparatorToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal class ThousandsSeparatorToken : SingleCharToken
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
