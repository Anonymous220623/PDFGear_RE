// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public enum TokenType
{
  Unknown,
  Section,
  Hour,
  Hour24,
  Minute,
  MinuteTotal,
  Second,
  SecondTotal,
  Year,
  Month,
  Day,
  String,
  ReservedPlace,
  Character,
  AmPm,
  Color,
  Condition,
  Text,
  SignificantDigit,
  InsignificantDigit,
  PlaceReservedDigit,
  Percent,
  Scientific,
  General,
  ThousandsSeparator,
  DecimalPoint,
  Asterix,
  Fraction,
  MilliSecond,
  Culture,
  Dollar,
}
