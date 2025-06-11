// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfStampAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represents a rubber stamp annotation</summary>
/// <remarks>
/// A rubber stamp annotation (PDF 1.3) displays text or graphics intended to look as
/// if they were stamped on the page with a rubber stamp. When opened, it displays a
/// pop-up window containing the text of the associated note.
/// </remarks>
public class PdfStampAnnotation : PdfMarkupAnnotation
{
  /// <summary>
  /// The name of an icon to be used in displaying the annotation.
  /// </summary>
  /// <remarks>
  /// Additional names may be supported as well. In this case please use <see cref="P:Patagames.Pdf.Net.Annotations.PdfStampAnnotation.ExtendedIconName" /> property.
  /// <note type="note">The annotation dictionary’s AP entry, if present, takes precedence over
  /// the StandardIconName and <see cref="P:Patagames.Pdf.Net.Annotations.PdfStampAnnotation.ExtendedIconName" /> properties.</note>
  /// <para>Default value: <strong>Draft</strong>.</para>
  /// </remarks>
  public StampIconNames StandardIconName
  {
    get
    {
      if (!this.IsExists("Name"))
        return StampIconNames.Draft;
      StampIconNames result;
      return Pdfium.GetEnumDescription<StampIconNames>(this.Dictionary["Name"].As<PdfTypeName>().Value, out result) ? result : StampIconNames.Extended;
    }
    set
    {
      string initialVal = value != StampIconNames.Extended ? Pdfium.GetEnumDescription((Enum) value) : throw new ArgumentException(Error.err0043);
      this.Dictionary["Name"] = !((initialVal ?? "").Trim() == "") ? (PdfTypeBase) PdfTypeName.Create(initialVal) : throw new ArgumentException(string.Format(Error.err0047, (object) nameof (StandardIconName), (object) "is one of the StampIconNames enumerator"));
    }
  }

  /// <summary>
  /// The name of an icon to be used in displaying the annotation.
  /// </summary>
  public string ExtendedIconName
  {
    get => !this.IsExists("Name") ? (string) null : this.Dictionary["Name"].As<PdfTypeName>().Value;
    set
    {
      if (value == null && this.Dictionary.ContainsKey("Name"))
      {
        this.Dictionary.Remove("Name");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["Name"] = (PdfTypeBase) PdfTypeName.Create(value);
      }
    }
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfStampAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfStampAnnotation(PdfPage page)
    : base(page)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("Stamp");
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfStampAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="icon">The icon to be used in displaying the annotation.</param>
  /// <param name="x">The x-coordinate of the left bottom corner of the annotation rectangle.</param>
  /// <param name="y">The y-coordinate of the left-bottom cornere of the annotation rectangle.</param>
  /// <param name="color">The color of this annotation.</param>
  public PdfStampAnnotation(PdfPage page, StampIconNames icon, float x, float y, FS_COLOR color)
    : this(page)
  {
    this.Color = color;
    this.StandardIconName = icon;
    this.Rectangle = new FS_RECTF(x, y + 30f, x + 150f, y);
    this.RegenerateAppearances();
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfStampAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="text">The icon to be used in displaying the annotation.</param>
  /// <param name="x">The x-coordinate of the left bottom corner of the annotation rectangle.</param>
  /// <param name="y">The y-coordinate of the left-bottom cornere of the annotation rectangle.</param>
  /// <param name="color">The color of this annotation.</param>
  public PdfStampAnnotation(PdfPage page, string text, float x, float y, FS_COLOR color)
    : this(page)
  {
    this.Color = color;
    this.ExtendedIconName = text;
    this.Contents = text;
    this.Rectangle = new FS_RECTF(x, y + 30f, x + 150f, y);
    this.RegenerateAppearances();
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfStampAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfStampAnnotation(PdfPage page, PdfTypeBase dictionary)
    : base(page, dictionary)
  {
  }

  /// <summary>
  /// Re-creates the appearance of the annotation based on its properties.
  /// </summary>
  /// <remarks>
  /// When the annotation does not have an Appearance stream (old style of annotations),
  /// they are not rendered by the Pdfium engine.
  /// Calling this function creates an appearance stream based on the default parameters and the properties of this annotation.
  /// </remarks>
  public override void RegenerateAppearances()
  {
    this.CreateEmptyAppearance(AppearanceStreamModes.Normal);
    FS_RECTF rectangle1 = this.Rectangle;
    FS_RECTF rectangle2 = this.Rectangle;
    FS_COLOR color = new FS_COLOR(this.Opacity, this.Color);
    string text = Resources.RuberStampDraft;
    switch (this.StandardIconName)
    {
      case StampIconNames.Approved:
        text = Resources.RuberStampApproved;
        break;
      case StampIconNames.Experimental:
        text = Resources.RuberStampExperimental;
        break;
      case StampIconNames.NotApproved:
        text = Resources.RuberStampNotApproved;
        break;
      case StampIconNames.AsIs:
        text = Resources.RuberStampAsIs;
        break;
      case StampIconNames.Expired:
        text = Resources.RuberStampExpired;
        break;
      case StampIconNames.NotForPublicRelease:
        text = Resources.RuberStampNotForPublicRelease;
        break;
      case StampIconNames.Confidential:
        text = Resources.RuberStampConfidential;
        break;
      case StampIconNames.Final:
        text = Resources.RuberStampFinal;
        break;
      case StampIconNames.Sold:
        text = Resources.RuberStampSold;
        break;
      case StampIconNames.Departmental:
        text = Resources.RuberStampDepartmental;
        break;
      case StampIconNames.ForComment:
        text = Resources.RuberStampForComment;
        break;
      case StampIconNames.TopSecret:
        text = Resources.RuberStampTopSecret;
        break;
      case StampIconNames.ForPublicRelease:
        text = Resources.RuberStampForPublicRelease;
        break;
      case StampIconNames.Extended:
        text = this.Contents;
        break;
    }
    PdfFont stock = PdfFont.CreateStock(this.Page.Document, FontStockNames.HelveticaBoldOblique);
    foreach (PdfPageObject pdfPageObject in AnnotDrawing.CreateRuberStamp(text, color, this.Rectangle, stock))
      this.NormalAppearance.Add(pdfPageObject);
    this.GenerateAppearance(AppearanceStreamModes.Normal);
  }
}
