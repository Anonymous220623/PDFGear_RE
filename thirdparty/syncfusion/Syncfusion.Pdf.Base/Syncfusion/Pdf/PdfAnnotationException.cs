// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfAnnotationException
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfAnnotationException : PdfDocumentException
{
  private const string ErrorMessage = "Annotation exception.";

  public PdfAnnotationException()
    : this("Annotation exception.")
  {
  }

  public PdfAnnotationException(Exception innerException)
    : this("Annotation exception.", innerException)
  {
  }

  public PdfAnnotationException(string message)
    : base(message)
  {
  }

  public PdfAnnotationException(string message, Exception innerException)
    : base(message, innerException)
  {
  }
}
