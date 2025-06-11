// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfBarcodeException
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public class PdfBarcodeException : ApplicationException
{
  public PdfBarcodeException()
  {
  }

  public PdfBarcodeException(string message)
    : base(message)
  {
  }

  public PdfBarcodeException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  protected PdfBarcodeException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
