// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.PdfKeywords
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal static class PdfKeywords
{
  internal const string True = "true";
  internal const string False = "false";
  internal const string IndirectReference = "R";
  internal const string Null = "null";
  internal const string StartXRef = "startxref";
  internal const string XRef = "xref";
  internal const string Trailer = "trailer";
  internal const string StreamStart = "stream";
  internal const string StreamEnd = "endstream";
  internal const string IndirectObjectStart = "obj";
  internal const string IndirectObjectEnd = "endobj";
  internal const string PdfHeader = "%PDF-";
  internal const string BinaryMarker = "%âãÏÓ";
  internal const string EndOfFile = "%%EOF";
  internal const string DictionaryStart = "<<";
  internal const string DictionaryEnd = ">>";
  internal const string EndOfInlineImage = "EI";
  internal const string StandardEncoding = "StandardEncoding";
  internal const string ISOLatin1Encoding = "ISOLatin1Encoding";

  public static bool IsKeyword(string str)
  {
    switch (str)
    {
      case null:
        return false;
      case "StandardEncoding":
        return true;
      default:
        return str == "ISOLatin1Encoding";
    }
  }

  internal static object GetValue(string keyword)
  {
    switch (keyword)
    {
      case "StandardEncoding":
        return (object) new PostScriptArray((object[]) PresettedEncodings.StandardEncoding);
      default:
        return (object) null;
    }
  }
}
