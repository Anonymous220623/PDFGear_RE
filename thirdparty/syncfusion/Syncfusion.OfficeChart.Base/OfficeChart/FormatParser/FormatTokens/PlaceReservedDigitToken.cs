// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.PlaceReservedDigitToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal class PlaceReservedDigitToken : DigitToken
{
  private const char DEF_FORMAT_CHAR = '?';
  private const string DEF_EMPTY_DIGIT = " ";

  protected internal override string GetDigitString(
    double value,
    int iDigit,
    bool bShowHiddenSymbols)
  {
    return iDigit != 0 || bShowHiddenSymbols || value >= 1.0 ? base.GetDigitString(value, iDigit, bShowHiddenSymbols) : " ";
  }

  public override TokenType TokenType => TokenType.PlaceReservedDigit;

  public override char FormatChar => '?';
}
