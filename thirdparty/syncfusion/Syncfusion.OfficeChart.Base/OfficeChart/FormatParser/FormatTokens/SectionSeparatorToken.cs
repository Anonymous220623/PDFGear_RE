// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.SectionSeparatorToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal class SectionSeparatorToken : FormatTokenBase
{
  private const string DEF_SEPARATOR = ";";

  public SectionSeparatorToken() => this.m_strFormat = ";";

  public override int TryParse(string strFormat, int iIndex)
  {
    int num = strFormat != null ? strFormat.Length : throw new ArgumentNullException(nameof (strFormat));
    if (num == 0)
      throw new ArgumentException("strFormat - string cannot be empty");
    if (iIndex < 0 || iIndex > num - 1)
      throw new ArgumentOutOfRangeException(nameof (iIndex), "Value cannot be less than 0 and greater than than format length - 1.");
    int length = this.m_strFormat.Length;
    if (string.Compare(strFormat, iIndex, this.m_strFormat, 0, length) == 0)
      iIndex += length;
    return iIndex;
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    throw new NotSupportedException();
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols)
  {
    throw new NotSupportedException();
  }

  public override TokenType TokenType => TokenType.Section;
}
