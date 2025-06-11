// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Lists.PdfOrderedList
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;

#nullable disable
namespace Syncfusion.Pdf.Lists;

public class PdfOrderedList : PdfList
{
  private PdfOrderedMarker m_marker;
  private bool m_useHierarchy;

  public PdfOrderedMarker Marker
  {
    get => this.m_marker;
    set => this.m_marker = value != null ? value : throw new ArgumentNullException("marker");
  }

  public bool MarkerHierarchy
  {
    get => this.m_useHierarchy;
    set => this.m_useHierarchy = value;
  }

  public PdfOrderedList()
    : this(PdfOrderedList.CreateMarker(PdfNumberStyle.Numeric))
  {
  }

  public PdfOrderedList(PdfFont font)
    : base(font)
  {
    PdfOrderedList.CreateMarker(PdfNumberStyle.Numeric);
  }

  public PdfOrderedList(PdfNumberStyle style) => this.Marker = PdfOrderedList.CreateMarker(style);

  public PdfOrderedList(PdfListItemCollection items)
    : this(items, PdfOrderedList.CreateMarker(PdfNumberStyle.Numeric))
  {
  }

  public PdfOrderedList(PdfOrderedMarker marker) => this.Marker = marker;

  public PdfOrderedList(PdfListItemCollection items, PdfOrderedMarker marker)
    : base(items)
  {
    this.Marker = marker;
  }

  public PdfOrderedList(string text)
    : this(text, PdfOrderedList.CreateMarker(PdfNumberStyle.Numeric))
  {
  }

  public PdfOrderedList(string text, PdfOrderedMarker marker)
    : this(PdfList.CreateItems(text), marker)
  {
  }

  private static PdfOrderedMarker CreateMarker(PdfNumberStyle style)
  {
    return new PdfOrderedMarker(style, (PdfFont) null);
  }
}
