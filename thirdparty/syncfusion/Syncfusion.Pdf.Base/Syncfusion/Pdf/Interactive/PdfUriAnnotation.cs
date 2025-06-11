// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfUriAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfUriAnnotation : PdfActionLinkAnnotation
{
  private PdfUriAction m_uriAction = new PdfUriAction();

  public string Uri
  {
    get => this.m_uriAction.Uri;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Uri));
      if (!(this.m_uriAction.Uri != value))
        return;
      this.m_uriAction.Uri = value;
    }
  }

  public override PdfAction Action
  {
    get => base.Action;
    set
    {
      base.Action = value;
      this.m_uriAction.Next = value;
    }
  }

  public PdfUriAnnotation(RectangleF rectangle)
    : base(rectangle)
  {
  }

  public PdfUriAnnotation(RectangleF rectangle, string uri)
    : base(rectangle)
  {
    this.Uri = uri != null ? uri : throw new ArgumentNullException(nameof (Uri));
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Link"));
    this.Dictionary.SetProperty("A", (IPdfWrapper) this.m_uriAction);
  }
}
