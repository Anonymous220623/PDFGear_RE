// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IO.IPdfWriter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.IO;

internal interface IPdfWriter
{
  long Position { get; set; }

  long Length { get; }

  PdfDocumentBase Document { get; set; }

  void Write(IPdfPrimitive pdfObject);

  void Write(long number);

  void Write(float number);

  void Write(string text);

  void Write(char[] text);

  void Write(byte[] data);
}
