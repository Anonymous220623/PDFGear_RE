// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.SecondTotalToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal class SecondTotalToken : FormatTokenBase
{
  private static readonly Regex HourRegex = new Regex("\\[[sS]+\\]", RegexOptions.Compiled);

  public override int TryParse(string strFormat, int iIndex)
  {
    return this.TryParseRegex(SecondTotalToken.HourRegex, strFormat, iIndex);
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    return ((int) (value * 86400.0)).ToString();
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => string.Empty;

  public override TokenType TokenType => TokenType.SecondTotal;
}
