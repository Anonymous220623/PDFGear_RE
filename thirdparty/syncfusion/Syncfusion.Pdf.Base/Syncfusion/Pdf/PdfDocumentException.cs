// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfDocumentException
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfDocumentException : PdfException
{
  private const string ErrorMessage = "Critical error on the document level.";

  public PdfDocumentException()
    : this("Critical error on the document level.")
  {
  }

  public PdfDocumentException(Exception innerException)
    : this("Critical error on the document level.", innerException)
  {
  }

  public PdfDocumentException(string message)
    : base(message)
  {
  }

  public PdfDocumentException(string message, Exception innerException)
    : base(message, innerException)
  {
  }
}
