// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.PlaceReservedDigitToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class PlaceReservedDigitToken : DigitToken
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
