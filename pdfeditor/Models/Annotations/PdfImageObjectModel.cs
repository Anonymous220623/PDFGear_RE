// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Annotations.PdfImageObjectModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace pdfeditor.Models.Annotations;

public class PdfImageObjectModel : IEquatable<PdfImageObjectModel>, IDisposable
{
  private bool IsDisposed;

  public PdfImageObjectModel(PdfImageObject imageObject)
  {
    this.Bitmap = imageObject != null ? imageObject.Bitmap.Clone() : throw new ArgumentNullException(nameof (imageObject));
    PdfTypeStream pdfTypeStream = (PdfTypeStream) null;
    if (imageObject.SoftMask != null && imageObject.SoftMask.Is<PdfTypeStream>())
    {
      pdfTypeStream = imageObject.SoftMask.As<PdfTypeStream>();
    }
    else
    {
      PdfTypeBase pdfTypeBase;
      if (imageObject.Stream.Dictionary != null && imageObject.Stream.Dictionary.TryGetValue("SMask", out pdfTypeBase) && pdfTypeBase != null && pdfTypeBase.Is<PdfTypeStream>())
        pdfTypeStream = pdfTypeBase.As<PdfTypeStream>();
    }
    if (pdfTypeStream == null)
      return;
    this.SoftMask = pdfTypeStream.Clone();
  }

  public PdfImageObjectModel(PdfBitmap bitmap) => this.Bitmap = bitmap.Clone();

  public PdfBitmap Bitmap { get; }

  public PdfTypeBase SoftMask { get; }

  public bool Equals(PdfImageObjectModel other) => other.Bitmap == this.Bitmap;

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  protected void Dispose(bool Disposing)
  {
    if (!this.IsDisposed)
    {
      int num = Disposing ? 1 : 0;
      this.Bitmap.Dispose();
      this.SoftMask?.Dispose();
    }
    this.IsDisposed = true;
  }

  ~PdfImageObjectModel() => this.Dispose(false);
}
