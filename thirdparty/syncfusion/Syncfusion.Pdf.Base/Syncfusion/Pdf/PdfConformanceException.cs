// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfConformanceException
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

[Serializable]
public class PdfConformanceException : PdfDocumentException
{
  private const string ErrorMessage = "PDF Conformance-level exception.";

  public PdfConformanceException()
    : this("PDF Conformance-level exception.")
  {
  }

  public PdfConformanceException(Exception innerException)
    : this("PDF Conformance-level exception.", innerException)
  {
  }

  public PdfConformanceException(string message)
    : base(message)
  {
  }

  public PdfConformanceException(string message, Exception innerException)
    : base(message, innerException)
  {
  }
}
