// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.InsignificantDigitToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal class InsignificantDigitToken : DigitToken
{
  private const char DEF_FORMAT_CHAR = '#';
  private bool m_bHideIfZero;

  protected internal override string GetDigitString(
    double value,
    int iDigit,
    bool bShowHiddenSymbols)
  {
    if (iDigit != 0 || Math.Abs(value) >= 1.0 && !this.HideIfZero)
      return base.GetDigitString(value, iDigit, bShowHiddenSymbols);
    return this.IsCenterDigit ? base.GetDigitString(value, iDigit, bShowHiddenSymbols) : string.Empty;
  }

  public override TokenType TokenType => TokenType.InsignificantDigit;

  public override char FormatChar => '#';

  public bool HideIfZero
  {
    get => this.m_bHideIfZero;
    set => this.m_bHideIfZero = value;
  }
}
