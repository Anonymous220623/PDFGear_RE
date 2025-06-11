// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfPopupAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represents a pop-up annotation</summary>
/// <remarks>
/// A pop-up annotation displays text in a pop-up window for entry and
/// editing. It typically does not appear alone but is associated with a markup annotation,
/// its parent annotation, and is used for editing the parent’s text.It has no
/// appearance stream or associated actions of its own and is identified by the Popup
/// property in the parent’s annotation.
/// </remarks>
public class PdfPopupAnnotation : PdfAnnotation
{
  private PdfAnnotation _parent;

  /// <summary>
  /// Gets or sets the parent annotation with which this pop-up annotation is associated.
  /// </summary>
  /// <remarks>
  /// <note>
  /// If this Property is notnull, the parent annotation’s
  /// <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.Contents" />, <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.ModificationDate" />, <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.Color" />, and <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.Text" /> entries override those of the pop-up annotation itself.
  /// </note>
  /// </remarks>
  public PdfAnnotation Parent
  {
    get
    {
      if (!this.IsExists(nameof (Parent)))
        return (PdfAnnotation) null;
      if ((PdfWrapper) this._parent == (PdfWrapper) null || this._parent.Dictionary.IsDisposed)
      {
        PdfAnnotationCollection annots = this.Page.Annots;
        this._parent = annots == null ? (PdfAnnotation) null : annots.GetByDictionary(this.Dictionary[nameof (Parent)]);
        if ((PdfWrapper) this._parent == (PdfWrapper) null)
          this._parent = PdfAnnotation.Create(this.Dictionary[nameof (Parent)], this.Page);
      }
      return this._parent;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey(nameof (Parent)))
        this.Dictionary.Remove(nameof (Parent));
      else if ((PdfWrapper) value != (PdfWrapper) null)
      {
        this.ListOfIndirectObjects.Add((PdfTypeBase) value.Dictionary);
        this.Dictionary.SetIndirectAt(nameof (Parent), this.ListOfIndirectObjects, (PdfTypeBase) value.Dictionary);
      }
      this._parent = value;
    }
  }

  /// <summary>
  /// Gets or sets a flag specifying whether the pop-up annotation should initially be displayed open.
  /// </summary>
  /// <remarks>Default value: <strong>false</strong></remarks>
  public bool IsOpen
  {
    get => this.IsExists("Open") && this.Dictionary["Open"].As<PdfTypeBoolean>().Value;
    set => this.Dictionary["Open"] = (PdfTypeBase) PdfTypeBoolean.Create(value);
  }

  /// <summary>
  /// If <see cref="P:Patagames.Pdf.Net.Annotations.PdfPopupAnnotation.Parent" /> property is not null, the parent annotation’s  <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.Contents" /> property override of the pop-up annotation itself.
  /// </summary>
  public override string Contents
  {
    get
    {
      PdfAnnotation parent = this.Parent;
      return (PdfWrapper) parent != (PdfWrapper) null ? parent.Contents : base.Contents;
    }
    set
    {
      PdfAnnotation parent = this.Parent;
      if ((PdfWrapper) parent != (PdfWrapper) null)
        parent.Contents = value;
      else
        base.Contents = value;
    }
  }

  /// <summary>
  /// If <see cref="P:Patagames.Pdf.Net.Annotations.PdfPopupAnnotation.Parent" /> property is not null, the parent annotation’s  <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.ModificationDate" /> property override of the pop-up annotation itself.
  /// </summary>
  public override string ModificationDate
  {
    get
    {
      PdfAnnotation parent = this.Parent;
      return (PdfWrapper) parent != (PdfWrapper) null ? parent.ModificationDate : base.ModificationDate;
    }
    set
    {
      PdfAnnotation parent = this.Parent;
      if ((PdfWrapper) parent != (PdfWrapper) null)
        parent.ModificationDate = value;
      else
        base.ModificationDate = value;
    }
  }

  /// <summary>
  /// If <see cref="P:Patagames.Pdf.Net.Annotations.PdfPopupAnnotation.Parent" /> property is not null, the parent annotation’s  <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.Color" /> property override of the pop-up annotation itself.
  /// </summary>
  public override FS_COLOR Color
  {
    get
    {
      PdfAnnotation parent = this.Parent;
      return (PdfWrapper) parent != (PdfWrapper) null ? parent.Color : base.Color;
    }
    set
    {
      PdfAnnotation parent = this.Parent;
      if ((PdfWrapper) parent != (PdfWrapper) null)
        parent.Color = value;
      else
        base.Color = value;
    }
  }

  /// <summary>
  /// If <see cref="P:Patagames.Pdf.Net.Annotations.PdfPopupAnnotation.Parent" /> property is <see cref="T:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation" />, the parent annotation’s  <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.Text" /> property override of the pop-up annotation itself.
  /// </summary>
  public string Text
  {
    get
    {
      if (this.Parent is PdfMarkupAnnotation)
        return (this.Parent as PdfMarkupAnnotation).Text;
      return !this.Dictionary.ContainsKey("T") ? (string) null : (this.Dictionary["T"] as PdfTypeString).UnicodeString;
    }
    set
    {
      if (this.Parent is PdfMarkupAnnotation)
        (this.Parent as PdfMarkupAnnotation).Text = value;
      else if (value == null && this.Dictionary.ContainsKey("T"))
      {
        this.Dictionary.Remove("T");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["T"] = (PdfTypeBase) PdfTypeString.Create(value, true);
      }
    }
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfPopupAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfPopupAnnotation(PdfPage page)
    : base(page)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("Popup");
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfPopupAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="parent">The parent annotation with which this pop-up annotation is associated.</param>
  /// <param name="isOpen">A flag specifying whether the pop-up annotation should initially be displayed open.</param>
  public PdfPopupAnnotation(PdfPage page, PdfAnnotation parent, bool isOpen)
    : this(page)
  {
    this.Parent = parent;
    this.IsOpen = isOpen;
    float width = page.Width;
    float height = page.Height;
    this.Rectangle = new FS_RECTF(width - 180f, height, width, height - 140f);
    if (!(parent is PdfMarkupAnnotation))
      return;
    (parent as PdfMarkupAnnotation).Popup = this;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfPopupAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfPopupAnnotation(PdfPage page, PdfTypeBase dictionary)
    : base(page, dictionary)
  {
  }
}
