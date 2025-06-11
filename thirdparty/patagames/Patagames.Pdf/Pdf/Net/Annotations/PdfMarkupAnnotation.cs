// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Net.Wrappers;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represents the Markup annotations.</summary>
/// <remarks>
/// Many annotation types are defined as markup annotations because they are used
/// primarily to mark up PDF documents. These annotations have
/// text that appears as part of the annotation and may be displayed in other ways by
/// a viewer application, such as in a Comments pane.
/// </remarks>
public abstract class PdfMarkupAnnotation : PdfAnnotation
{
  private PdfPopupAnnotation _popup;
  private PdfAnnotation _rel;

  /// <summary>
  /// Gets or sets the text label to be displayed in the title bar of the annotation’s pop-up window when open and active.
  /// </summary>
  /// <remarks>
  /// By convention, this entry identifies the user who added the annotation.
  /// </remarks>
  public virtual string Text
  {
    get
    {
      return !this.IsExists("T") ? (string) null : this.Dictionary["T"].As<PdfTypeString>().UnicodeString;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("T"))
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
  /// Gets or sets a pop-up annotation for entering or editing the text associated with this annotation.
  /// </summary>
  public virtual PdfPopupAnnotation Popup
  {
    get
    {
      if (!this.IsExists(nameof (Popup)))
        return (PdfPopupAnnotation) null;
      if ((PdfWrapper) this._popup == (PdfWrapper) null || this._popup.Dictionary.IsDisposed)
      {
        PdfAnnotationCollection annots = this.Page.Annots;
        PdfAnnotation byDictionary = annots == null ? (PdfAnnotation) null : annots.GetByDictionary(this.Dictionary[nameof (Popup)]);
        if ((PdfWrapper) byDictionary != (PdfWrapper) null && byDictionary is PdfPopupAnnotation)
        {
          this._popup = byDictionary as PdfPopupAnnotation;
        }
        else
        {
          PdfAnnotation pdfAnnotation = PdfAnnotation.Create(this.Dictionary[nameof (Popup)], this.Page);
          this._popup = pdfAnnotation is PdfPopupAnnotation ? pdfAnnotation as PdfPopupAnnotation : throw new PdfParserException(Error.err0040);
        }
      }
      return this._popup;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey(nameof (Popup)))
        this.Dictionary.Remove(nameof (Popup));
      else if ((PdfWrapper) value != (PdfWrapper) null)
      {
        this.ListOfIndirectObjects.Add((PdfTypeBase) value.Dictionary);
        this.Dictionary.SetIndirectAt(nameof (Popup), this.ListOfIndirectObjects, (PdfTypeBase) value.Dictionary);
      }
      this._popup = value;
    }
  }

  /// <summary>
  /// Gets or sets the constant opacity value to be used in painting the annotation.
  /// </summary>
  /// <remarks>
  /// Opacity vary from 0.0 (no contribution) to 1.0 (maximum contribution)
  /// <para>This value applies to all visible elements of the annotation in its closed state (including its background and border) but not to the popup window that appears when the annotation is opened.</para>
  /// <para>The specified value is not used if the annotation has an appearance stream;
  /// in that case, the appearance stream must
  /// specify any transparency. (However, if the viewer regenerates the annotation’s
  /// appearance stream, it may incorporate the Opacity value into the stream’s content.)</para>
  /// <para>The implicit blend mode is Normal.
  /// Default value: <strong>1.0</strong>.</para>
  /// <note type="note">If no explicit appearance stream is defined for the annotation, it is painted by
  /// implementation-dependent means that do not necessarily conform to the Adobe
  /// imaging model; in this case, the effect of this entry is implementation-dependent as well.</note>
  /// </remarks>
  public virtual float Opacity
  {
    get => !this.IsExists("CA") ? 1f : this.Dictionary["CA"].As<PdfTypeNumber>().FloatValue;
    set => this.Dictionary["CA"] = (PdfTypeBase) PdfTypeNumber.Create(value);
  }

  /// <summary>
  /// A rich text string to be displayed in the pop-up window when the annotation is opened.
  /// </summary>
  /// <remarks>
  /// <note type="note">This property is ignored when regenerates the annotation’s appearance stream.</note>
  /// </remarks>
  public virtual string RichText
  {
    get
    {
      return !this.IsExists("RC") ? (string) null : this.Dictionary["RC"].As<PdfTypeString>().UnicodeString;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("RC"))
      {
        this.Dictionary.Remove("RC");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["RC"] = (PdfTypeBase) PdfTypeString.Create(value, true);
      }
    }
  }

  /// <summary>
  /// Gets or sets the date and time when the annotation was created.
  /// </summary>
  /// <remarks>
  /// Please see remarks section at <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.ModificationDate" /> for more details.
  /// </remarks>
  public virtual string CreationDate
  {
    get
    {
      return !this.IsExists(nameof (CreationDate)) ? (string) null : this.Dictionary[nameof (CreationDate)].As<PdfTypeString>().AnsiString;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey(nameof (CreationDate)))
      {
        this.Dictionary.Remove(nameof (CreationDate));
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary[nameof (CreationDate)] = (PdfTypeBase) PdfTypeString.Create(value);
      }
    }
  }

  /// <summary>
  /// Gets or sets annotation that this annotation is “in reply to.”
  /// </summary>
  /// <remarks>
  /// Both annotations must be on the same page of the document. The relationship between the two annotations is specified by the <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.Relationship" /> property.
  /// <para>If this entry is present in an FDF file,
  /// its type is not a dictionary but a text string containing the contents of the <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.Name" /> property of the annotation being replied to, to allow for a situation where the annotation being replied to is not in the same FDF file.
  /// An <see cref="T:Patagames.Pdf.Net.Exceptions.PdfParserException" /> will be thrown in this case, but you may use the <see cref="P:Patagames.Pdf.Net.Wrappers.PdfWrapper.Dictionary" /> property to get access to this entry.
  /// </para>
  /// </remarks>
  public virtual PdfAnnotation RelationshipAnnotation
  {
    get
    {
      if (!this.IsExists("IRT"))
        return (PdfAnnotation) null;
      if ((PdfWrapper) this._rel == (PdfWrapper) null || this._rel.Dictionary.IsDisposed)
      {
        PdfAnnotationCollection annots = this.Page.Annots;
        this._rel = annots == null ? (PdfAnnotation) null : annots.GetByDictionary(this.Dictionary["IRT"]);
        if ((PdfWrapper) this._rel == (PdfWrapper) null)
          this._rel = PdfAnnotation.Create(this.Dictionary["IRT"], this.Page);
      }
      return this._rel;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey("IRT"))
        this.Dictionary.Remove("IRT");
      else if ((PdfWrapper) value != (PdfWrapper) null)
      {
        this.ListOfIndirectObjects.Add((PdfTypeBase) value.Dictionary);
        this.Dictionary.SetIndirectAt("IRT", this.ListOfIndirectObjects, (PdfTypeBase) value.Dictionary);
      }
      this._rel = value;
    }
  }

  /// <summary>
  /// Gets or sets text representing a short description of the subject being addressed by the annotation.
  /// </summary>
  public virtual string Subject
  {
    get
    {
      return !this.IsExists("Subj") ? (string) null : this.Dictionary["Subj"].As<PdfTypeString>().UnicodeString;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("Subj"))
      {
        this.Dictionary.Remove("Subj");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["Subj"] = (PdfTypeBase) PdfTypeString.Create(value, true);
      }
    }
  }

  /// <summary>
  /// Gets or sets a value specifying the relationship (the “reply type”) between this annotation and one specified by <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.RelationshipAnnotation" />.
  /// </summary>
  /// <remarks>
  /// Please see the remark section at <see cref="T:Patagames.Pdf.Enums.RelationTypes" />
  /// </remarks>
  public virtual RelationTypes Relationship
  {
    get
    {
      if (!this.IsExists("RT"))
        return RelationTypes.NonSpecified;
      string str = this.Dictionary["RT"].As<PdfTypeName>().Value;
      if (str.Equals("R", StringComparison.OrdinalIgnoreCase))
        return RelationTypes.Reply;
      if (str.Equals("Group", StringComparison.OrdinalIgnoreCase))
        return RelationTypes.Group;
      throw new PdfParserException(string.Format(Error.err0041, (object) "val"));
    }
    set
    {
      switch (value)
      {
        case RelationTypes.NonSpecified:
          if (!this.Dictionary.ContainsKey("RT"))
            break;
          this.Dictionary.Remove("RT");
          break;
        case RelationTypes.Reply:
          this.Dictionary["RT"] = (PdfTypeBase) PdfTypeName.Create("R");
          break;
        case RelationTypes.Group:
          this.Dictionary["RT"] = (PdfTypeBase) PdfTypeName.Create("Group");
          break;
        default:
          throw new ArgumentException(string.Format(Error.err0047, (object) nameof (Relationship), (object) "are RelationTypes.Reply, RelationTypes.Group and RelationTypes.NonSpecified"));
      }
    }
  }

  /// <summary>
  /// Gets or sets a string describing the intent of the markup annotation.
  /// </summary>
  /// <remarks>
  /// Intents allow viewer applications to distinguish between different uses and behaviors of a single markup annotation type.
  /// If this property is not present (null) or its value is
  /// the same as the annotation type, the annotation has no explicit intent and should
  /// behave in a generic manner in a viewer application.
  /// <para>
  /// <see cref="T:Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation" />, <see cref="T:Patagames.Pdf.Net.Annotations.PdfLineAnnotation" />, <see cref="T:Patagames.Pdf.Net.Annotations.PdfPolygonAnnotation" /> annotations, and (in PDF 1.7) <see cref="T:Patagames.Pdf.Net.Annotations.PdfPolylineAnnotation" />
  /// have defined intents, whose values are enumerated in the corresponding topics.
  /// </para>
  /// </remarks>
  protected virtual AnnotationIntent InternalIntent
  {
    get
    {
      if (!this.IsExists("IT"))
        return AnnotationIntent.None;
      AnnotationIntent result = AnnotationIntent.None;
      return Pdfium.GetEnumDescription<AnnotationIntent>(this.Dictionary["IT"].As<PdfTypeName>().Value, out result) ? result : AnnotationIntent.Unknown;
    }
    set
    {
      if (value == AnnotationIntent.None && this.Dictionary.ContainsKey("IT"))
      {
        this.Dictionary.Remove("IT");
      }
      else
      {
        if (value == AnnotationIntent.None)
          return;
        this.Dictionary["IT"] = (PdfTypeBase) PdfTypeName.Create(Pdfium.GetEnumDescription((Enum) value));
      }
    }
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfMarkupAnnotation(PdfPage page)
    : base(page)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfMarkupAnnotation(PdfPage page, PdfTypeBase dictionary)
    : base(page, dictionary)
  {
  }

  /// <summary>
  /// Re-creates the appearance of the annotation based on its properties.
  /// </summary>
  /// <remarks>
  /// When the annotation does not have an Appearance stream (old style annotations),
  /// they are not rendered by the Pdfium engine.
  /// Calling this function creates an appearance stream based on the default parameters and the properties of this annotation.
  /// <note type="note">Overridden in derived classes. The basic implementation do nothing.</note>
  /// </remarks>
  public virtual void RegenerateAppearances()
  {
  }
}
