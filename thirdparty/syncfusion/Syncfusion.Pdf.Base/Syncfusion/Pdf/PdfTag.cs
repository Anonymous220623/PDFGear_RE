// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfTag
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public abstract class PdfTag
{
  private int m_tagOrder;
  private RectangleF bounds;

  public virtual int Order
  {
    get => this.m_tagOrder;
    set => this.m_tagOrder = value;
  }

  internal RectangleF Bounds
  {
    get => this.bounds;
    set => this.bounds = value;
  }
}
