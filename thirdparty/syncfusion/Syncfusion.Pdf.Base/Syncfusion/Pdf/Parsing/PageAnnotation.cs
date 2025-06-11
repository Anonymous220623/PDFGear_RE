// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PageAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class PageAnnotation
{
  private RectangleF m_rect = new RectangleF();
  private string m_uri = string.Empty;
  private float m_border = 1f;
  private string m_annotType;
  private PdfArray m_pageAnnotDestinations;

  public PageAnnotation(RectangleF rec, string uri, float border, string annotType)
  {
    this.m_rect = rec;
    this.m_uri = uri;
    this.m_border = border;
    this.m_annotType = annotType;
  }

  public PageAnnotation(
    RectangleF rec,
    string uri,
    float border,
    string annotType,
    PdfArray pageAnnotDestinations)
  {
    this.m_rect = rec;
    this.m_uri = uri;
    this.m_border = border;
    this.m_annotType = annotType;
    this.m_pageAnnotDestinations = pageAnnotDestinations;
  }

  internal RectangleF Rect
  {
    get => this.m_rect;
    set => this.m_rect = value;
  }

  internal string URI
  {
    get => this.m_uri;
    set => this.m_uri = value;
  }

  internal float Border
  {
    get => this.m_border;
    set => this.m_border = value;
  }

  internal string AnnotType
  {
    get => this.m_annotType;
    set => this.m_annotType = value;
  }

  internal PdfArray PageAnnotDestinations
  {
    get => this.m_pageAnnotDestinations;
    set => this.m_pageAnnotDestinations = value;
  }
}
