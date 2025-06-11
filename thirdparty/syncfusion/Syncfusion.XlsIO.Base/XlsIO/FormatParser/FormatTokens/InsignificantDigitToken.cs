// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.InsignificantDigitToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class InsignificantDigitToken : DigitToken
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
