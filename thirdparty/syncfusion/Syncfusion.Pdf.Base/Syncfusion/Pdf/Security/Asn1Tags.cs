﻿// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1Tags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal sealed class Asn1Tags
{
  public const int Boolean = 1;
  public const int Integer = 2;
  public const int BitString = 3;
  public const int OctetString = 4;
  public const int Null = 5;
  public const int ObjectIdentifier = 6;
  public const int External = 8;
  public const int Enumerated = 10;
  public const int Sequence = 16 /*0x10*/;
  public const int SequenceOf = 16 /*0x10*/;
  public const int Set = 17;
  public const int SetOf = 17;
  public const int NumericString = 18;
  public const int PrintableString = 19;
  public const int TeleText = 20;
  public const int VideotexString = 21;
  public const int AsciiString = 22;
  public const int UtcTime = 23;
  public const int GeneralizedTime = 24;
  public const int GraphicString = 25;
  public const int VisibleString = 26;
  public const int GeneralString = 27;
  public const int UniversalString = 28;
  public const int BmpString = 30;
  public const int Utf8String = 12;
  public const int Constructed = 32 /*0x20*/;
  public const int Application = 64 /*0x40*/;
  public const int Tagged = 128 /*0x80*/;
}
