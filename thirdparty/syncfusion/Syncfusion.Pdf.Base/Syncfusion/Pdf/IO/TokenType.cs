// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IO.TokenType
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.IO;

internal enum TokenType
{
  Unknown,
  DictionaryStart,
  DictionaryEnd,
  StreamStart,
  StreamEnd,
  HexStringStart,
  HexStringEnd,
  String,
  UnicodeString,
  Number,
  Real,
  Name,
  ArrayStart,
  ArrayEnd,
  Reference,
  ObjectStart,
  ObjectEnd,
  Boolean,
  HexDigit,
  Eof,
  Trailer,
  StartXRef,
  XRef,
  Null,
  ObjectType,
  HexStringWeird,
  HexStringWeirdEscape,
  WhiteSpace,
}
