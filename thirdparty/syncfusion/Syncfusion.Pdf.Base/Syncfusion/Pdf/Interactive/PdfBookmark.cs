// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfBookmark
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfBookmark : PdfBookmarkBase
{
  private PdfDestination m_destination;
  private PdfNamedDestination m_namedDestination;
  private PdfColor m_color;
  private PdfTextStyle m_textStyle;
  private PdfBookmark m_previous;
  private PdfBookmark m_next;
  private PdfBookmarkBase m_parent;
  private PdfAction m_action;

  internal PdfBookmark(
    string title,
    PdfBookmarkBase parent,
    PdfBookmark previous,
    PdfBookmark next)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    if (title == null)
      throw new ArgumentNullException(nameof (title));
    this.m_parent = parent;
    this.Dictionary.SetProperty(nameof (Parent), (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) parent));
    this.Previous = previous;
    this.Next = next;
    this.Title = title;
  }

  internal PdfBookmark(
    string title,
    PdfBookmarkBase parent,
    PdfBookmark previous,
    PdfBookmark next,
    PdfDestination dest)
    : this(title, parent, previous, next)
  {
    this.Destination = dest != null ? dest : throw new ArgumentNullException("destination");
  }

  internal PdfBookmark(PdfDictionary dictionary, PdfCrossTable crossTable)
    : base(dictionary, crossTable)
  {
  }

  public virtual PdfDestination Destination
  {
    get => this.m_destination;
    set
    {
      this.m_destination = value != null ? value : throw new ArgumentNullException(nameof (Destination));
      this.Dictionary.SetProperty("Dest", (IPdfWrapper) value);
    }
  }

  public virtual PdfNamedDestination NamedDestination
  {
    get => this.m_namedDestination;
    set
    {
      if (this.m_namedDestination == value)
        return;
      this.m_namedDestination = value;
      PdfDictionary pdfDictionary = new PdfDictionary();
      pdfDictionary.SetProperty("D", (IPdfPrimitive) new PdfString(this.m_namedDestination.Title));
      pdfDictionary.SetProperty("S", (IPdfPrimitive) new PdfName("GoTo"));
      this.Dictionary.SetProperty("A", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary));
    }
  }

  public virtual string Title
  {
    get
    {
      PdfString pdfString = this.Dictionary[nameof (Title)] as PdfString;
      string title = (string) null;
      if (pdfString != null)
        title = pdfString.Value;
      return title;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Title));
      this.Dictionary.SetString(nameof (Title), value);
    }
  }

  public virtual PdfColor Color
  {
    get => this.m_color;
    set
    {
      if (!(this.m_color != value))
        return;
      this.m_color = value;
      this.UpdateColor();
    }
  }

  public virtual PdfTextStyle TextStyle
  {
    get => this.m_textStyle;
    set
    {
      if (this.m_textStyle == value)
        return;
      this.m_textStyle = value;
      this.UpdateTextStyle();
    }
  }

  public PdfAction Action
  {
    get => this.m_action;
    set
    {
      if (this.m_action == value)
        return;
      this.m_action = value;
      this.Dictionary.SetProperty("A", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_action.Dictionary));
    }
  }

  public new bool IsExpanded
  {
    get => base.IsExpanded;
    set => base.IsExpanded = value;
  }

  internal virtual PdfBookmark Previous
  {
    get => this.m_previous;
    set
    {
      if (this.m_previous == value)
        return;
      this.m_previous = value;
      this.Dictionary.SetProperty("Prev", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) value));
    }
  }

  internal virtual PdfBookmarkBase Parent => this.m_parent;

  internal virtual PdfBookmark Next
  {
    get => this.m_next;
    set
    {
      if (this.m_next == value)
        return;
      this.m_next = value;
      this.Dictionary.SetProperty(nameof (Next), (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) value));
    }
  }

  internal void SetParent(PdfBookmarkBase parent) => this.m_parent = parent;

  private void UpdateColor()
  {
    PdfDictionary dictionary = this.Dictionary;
    if (dictionary["C"] is PdfArray && this.m_color.IsEmpty)
      dictionary.Remove("C");
    else
      dictionary["C"] = (IPdfPrimitive) this.m_color.ToArray();
  }

  private void UpdateTextStyle()
  {
    if (this.m_textStyle == PdfTextStyle.Regular)
      this.Dictionary.Remove("F");
    else
      this.Dictionary.SetNumber("F", (int) this.m_textStyle);
  }
}
