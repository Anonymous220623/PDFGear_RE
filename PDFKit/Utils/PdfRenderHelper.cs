// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfRenderHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using System;
using System.Buffers;

#nullable disable
namespace PDFKit.Utils;

internal static class PdfRenderHelper
{
  internal static IDisposable CreateHideFlagContext(PdfPage page, bool isAnnotationVisible)
  {
    return (IDisposable) new PdfRenderHelper.AnnotationFlagContext(page, isAnnotationVisible);
  }

  internal static bool CanAnnotationHide(PdfAnnotation annot)
  {
    return !((PdfWrapper) annot == (PdfWrapper) null) && !(annot is PdfWidgetAnnotation);
  }

  internal struct AnnotationFlagContext : IDisposable
  {
    private bool isAnnotationVisible;
    private bool[] visibleAnnotDict;
    private readonly int dictLength;

    public PdfPage Page { get; set; }

    public AnnotationFlagContext(PdfPage page, bool isAnnotationVisible)
    {
      this.Page = page;
      this.isAnnotationVisible = isAnnotationVisible;
      if (page.Annots == null)
        this.isAnnotationVisible = true;
      this.visibleAnnotDict = (bool[]) null;
      this.dictLength = 0;
      if (this.isAnnotationVisible)
        return;
      this.dictLength = page.Annots.Count;
      this.visibleAnnotDict = ArrayPool<bool>.Shared.Rent(this.dictLength);
      for (int index = 0; index < this.dictLength; ++index)
      {
        if (PdfRenderHelper.CanAnnotationHide(page.Annots[index]))
        {
          this.visibleAnnotDict[index] = (page.Annots[index].Flags & AnnotationFlags.Hidden) == AnnotationFlags.None;
          page.Annots[index].Flags |= AnnotationFlags.Hidden;
        }
      }
    }

    public void Dispose()
    {
      if (this.isAnnotationVisible || this.Page?.Annots == null)
        return;
      for (int index = 0; index < this.dictLength; ++index)
      {
        if (this.visibleAnnotDict[index] && PdfRenderHelper.CanAnnotationHide(this.Page.Annots[index]))
          this.Page.Annots[index].Flags &= ~AnnotationFlags.Hidden;
      }
      this.isAnnotationVisible = true;
      ArrayPool<bool>.Shared.Return(this.visibleAnnotDict);
      this.visibleAnnotDict = (bool[]) null;
    }
  }
}
