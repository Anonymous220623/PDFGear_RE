// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfActionLinkAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public abstract class PdfActionLinkAnnotation : PdfLinkAnnotation
{
  private PdfAction m_action;

  public virtual PdfAction Action
  {
    get => this.m_action;
    set => this.m_action = value != null ? value : throw new ArgumentNullException(nameof (Action));
  }

  public PdfActionLinkAnnotation(RectangleF rectangle)
    : base(rectangle)
  {
  }

  public PdfActionLinkAnnotation(RectangleF rectangle, PdfAction action)
    : base(rectangle)
  {
    this.m_action = action != null ? action : throw new ArgumentNullException(nameof (action));
  }
}
