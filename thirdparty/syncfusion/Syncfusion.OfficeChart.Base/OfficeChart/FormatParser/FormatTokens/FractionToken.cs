// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.FractionToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal class FractionToken : SingleCharToken
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
    return section == null || section.FormatType != OfficeFormatType.DateTime ? this.m_strFormat : DateTimeFormatInfo.CurrentInfo.DateSeparator;
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => this.m_strFormat;
}
