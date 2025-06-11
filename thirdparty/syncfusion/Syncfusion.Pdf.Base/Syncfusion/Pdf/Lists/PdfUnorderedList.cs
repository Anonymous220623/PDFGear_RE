// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Lists.PdfUnorderedList
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;

#nullable disable
namespace Syncfusion.Pdf.Lists;

public class PdfUnorderedList : PdfList
{
  private PdfUnorderedMarker m_marker;

  public PdfUnorderedMarker Marker
  {
    get => this.m_marker;
    set => this.m_marker = value != null ? value : throw new ArgumentNullException("marker");
  }

  public PdfUnorderedList()
    : this(PdfUnorderedList.CreateMarker(PdfUnorderedMarkerStyle.Disk))
  {
  }

  public PdfUnorderedList(PdfListItemCollection items)
    : this(items, PdfUnorderedList.CreateMarker(PdfUnorderedMarkerStyle.Disk))
  {
  }

  public PdfUnorderedList(PdfFont font)
    : base(font)
  {
    PdfUnorderedList.CreateMarker(PdfUnorderedMarkerStyle.Disk);
  }

  public PdfUnorderedList(PdfUnorderedMarker marker) => this.Marker = marker;

  public PdfUnorderedList(PdfListItemCollection items, PdfUnorderedMarker marker)
    : base(items)
  {
    this.Marker = marker;
  }

  public PdfUnorderedList(string text)
    : this(text, PdfUnorderedList.CreateMarker(PdfUnorderedMarkerStyle.Disk))
  {
  }

  public PdfUnorderedList(string text, PdfUnorderedMarker marker)
    : this(PdfList.CreateItems(text), marker)
  {
  }

  private static PdfUnorderedMarker CreateMarker(PdfUnorderedMarkerStyle style)
  {
    return new PdfUnorderedMarker(style);
  }
}
