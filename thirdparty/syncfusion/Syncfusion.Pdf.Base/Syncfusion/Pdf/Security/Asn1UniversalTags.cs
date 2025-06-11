// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1UniversalTags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

[Flags]
internal enum Asn1UniversalTags
{
  ReservedBER = 0,
  Boolean = 1,
  Integer = 2,
  BitString = Integer | Boolean, // 0x00000003
  OctetString = 4,
  Null = OctetString | Boolean, // 0x00000005
  ObjectIdentifier = OctetString | Integer, // 0x00000006
  ObjectDescriptor = ObjectIdentifier | Boolean, // 0x00000007
  External = 8,
  Real = External | Boolean, // 0x00000009
  Enumerated = External | Integer, // 0x0000000A
  EmbeddedPDV = Enumerated | Boolean, // 0x0000000B
  UTF8String = External | OctetString, // 0x0000000C
  RelativeOid = UTF8String | Boolean, // 0x0000000D
  Sequence = 16, // 0x00000010
  Set = Sequence | Boolean, // 0x00000011
  NumericString = Sequence | Integer, // 0x00000012
  PrintableString = NumericString | Boolean, // 0x00000013
  TeletexString = Sequence | OctetString, // 0x00000014
  VideotexString = TeletexString | Boolean, // 0x00000015
  IA5String = TeletexString | Integer, // 0x00000016
  UTFTime = IA5String | Boolean, // 0x00000017
  GeneralizedTime = Sequence | External, // 0x00000018
  GraphicsString = GeneralizedTime | Boolean, // 0x00000019
  VisibleString = GeneralizedTime | Integer, // 0x0000001A
  GeneralString = VisibleString | Boolean, // 0x0000001B
  UniversalString = GeneralizedTime | OctetString, // 0x0000001C
  CharacterString = UniversalString | Boolean, // 0x0000001D
  BMPString = UniversalString | Integer, // 0x0000001E
  Constructed = 32, // 0x00000020
  Application = 64, // 0x00000040
  Tagged = 128, // 0x00000080
}
