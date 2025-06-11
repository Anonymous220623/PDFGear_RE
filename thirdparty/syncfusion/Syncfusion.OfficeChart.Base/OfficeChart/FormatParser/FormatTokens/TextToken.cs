// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.TextToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal class TextToken : SingleCharToken
{
  private const char DEF_FORMAT_CHAR = '@';

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    return value.ToString((IFormatProvider) culture);
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols)
  {
    return this.m_strFormat != null ? value : (string) null;
  }

  public override TokenType TokenType => TokenType.Text;

  public override char FormatChar => '@';
}
