// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal enum TokenType
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
}
