// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfBrush
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public abstract class PdfBrush : ICloneable
{
  internal abstract bool MonitorChanges(
    PdfBrush brush,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveChanges,
    PdfColorSpace currentColorSpace);

  internal abstract bool MonitorChanges(
    PdfBrush brush,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveChanges,
    PdfColorSpace currentColorSpace,
    bool check,
    bool iccbased,
    bool indexed);

  internal abstract bool MonitorChanges(
    PdfBrush brush,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveChanges,
    PdfColorSpace currentColorSpace,
    bool check);

  internal abstract bool MonitorChanges(
    PdfBrush brush,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveChanges,
    PdfColorSpace currentColorSpace,
    bool check,
    bool iccbased);

  internal abstract void ResetChanges(PdfStreamWriter streamWriter);

  object ICloneable.Clone() => (object) this.Clone();

  public abstract PdfBrush Clone();
}
