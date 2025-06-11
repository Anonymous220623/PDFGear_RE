// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.DecimalPointToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal class DecimalPointToken : SingleCharToken
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
