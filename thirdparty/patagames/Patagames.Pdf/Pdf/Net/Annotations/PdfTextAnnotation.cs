// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfTextAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Properties;
using System;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represents a text annotation</summary>
/// <remarks>
/// A text annotation represents a “sticky note” attached to a point in the PDF document.
/// When closed, the annotation appears as an icon; when open, it displays a
/// pop-up window containing the text of the note in a font and size chosen by the
/// viewer application. Text annotations do not scale and rotate with the page; they
/// behave as if the NoZoom and NoRotate annotation flags (see <see cref="T:Patagames.Pdf.Enums.AnnotationFlags" />)
/// were always set.
/// </remarks>
public class PdfTextAnnotation : PdfMarkupAnnotation
{
  /// <summary>
  /// Gets or sets a flag specifying whether the annotation should initially be displayed open.
  /// </summary>
  /// <remarks>
  /// Default value: <strong>false</strong> (closed).
  /// </remarks>
  public bool IsOpen
  {
    get => this.IsExists("Open") && this.Dictionary["Open"].As<PdfTypeBoolean>().Value;
    set => this.Dictionary["Open"] = (PdfTypeBase) PdfTypeBoolean.Create(value);
  }

  /// <summary>
  /// The name of an icon to be used in displaying the annotation.
  /// </summary>
  /// <remarks>
  /// Additional names may be supported as well. In this case please use <see cref="P:Patagames.Pdf.Net.Annotations.PdfTextAnnotation.ExtendedIconName" /> property.
  /// <note type="note">he annotation dictionary’s AP entry, if present,
  /// takes precedence over the StandardIconName and <see cref="P:Patagames.Pdf.Net.Annotations.PdfTextAnnotation.ExtendedIconName" /> properties.</note>
  /// <para>Default value: <strong>Note</strong>.</para>
  /// </remarks>
  public IconNames StandardIconName
  {
    get
    {
      if (!this.IsExists("Name"))
        return IconNames.Note;
      IconNames result;
      return Pdfium.GetEnumDescription<IconNames>(this.Dictionary["Name"].As<PdfTypeName>().Value, out result) ? result : IconNames.Extended;
    }
    set
    {
      string initialVal = value != IconNames.Extended ? Pdfium.GetEnumDescription((Enum) value) : throw new ArgumentException(Error.err0043);
      this.Dictionary["Name"] = !((initialVal ?? "").Trim() == "") ? (PdfTypeBase) PdfTypeName.Create(initialVal) : throw new ArgumentException(string.Format(Error.err0047, (object) nameof (StandardIconName), (object) "is one of the IconNames enumerator"));
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
  /// Gets orsets the state model corresponding to <see cref="P:Patagames.Pdf.Net.Annotations.PdfTextAnnotation.State" />;
  /// </summary>
  public StateModels StateModel
  {
    get
    {
      if (!this.IsExists(nameof (StateModel)))
        return StateModels.Marked;
      StateModels result;
      if (Pdfium.GetEnumDescription<StateModels>(this.Dictionary[nameof (StateModel)].As<PdfTypeString>().UnicodeString, out result))
        return result;
      throw new PdfParserException(string.Format(Error.err0045, (object) nameof (StateModel)));
    }
    set
    {
      string enumDescription = Pdfium.GetEnumDescription((Enum) value);
      this.Dictionary[nameof (StateModel)] = !((enumDescription ?? "").Trim() == "") ? (PdfTypeBase) PdfTypeString.Create(enumDescription, true) : throw new PdfParserException(string.Format(Error.err0045, (object) nameof (StateModel)));
    }
  }

  /// <summary>
  /// Gets orsets the state to which the original annotation should be set;
  /// </summary>
  public AnnotationStates State
  {
    get
    {
      if (!this.IsExists(nameof (State)))
      {
        switch (this.StateModel)
        {
          case StateModels.Marked:
            return AnnotationStates.Unmarked;
          case StateModels.Review:
            return AnnotationStates.None;
          default:
            throw new NotImplementedException($"The type {this.StateModel} of StateModel is not mplemented");
        }
      }
      else
      {
        AnnotationStates result;
        if (Pdfium.GetEnumDescription<AnnotationStates>(this.Dictionary[nameof (State)].As<PdfTypeString>().UnicodeString, out result))
          return result;
        throw new PdfParserException(string.Format(Error.err0045, (object) nameof (State)));
      }
    }
    set
    {
      string enumDescription = Pdfium.GetEnumDescription((Enum) value);
      this.Dictionary[nameof (State)] = !((enumDescription ?? "").Trim() == "") ? (PdfTypeBase) PdfTypeString.Create(enumDescription, true) : throw new ArgumentException(string.Format(Error.err0047, (object) nameof (State), (object) "is one of the AnnotationStates enumerator"));
    }
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfTextAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfTextAnnotation(PdfPage page)
    : base(page)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("Text");
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfTextAnnotation" /> with specified parameters.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="icon">The name of a standard icon to be used in displaying the annotation.</param>
  /// <param name="color">Annotation color.</param>
  /// <param name="text">Annotation text.</param>
  /// <param name="subject">Annotation subject.</param>
  /// <param name="x">The x-coordinate of the left bottom corner of the annotation rectangle.</param>
  /// <param name="y">The y-coordinate of the left-bottom cornere of the annotation rectangle.</param>
  public PdfTextAnnotation(
    PdfPage page,
    IconNames icon,
    FS_COLOR color,
    string text,
    string subject,
    float x,
    float y)
    : this(page)
  {
    this.Color = color;
    this.Opacity = (float) color.A / (float) byte.MaxValue;
    this.Flags = AnnotationFlags.Print | AnnotationFlags.NoZoom | AnnotationFlags.NoRotate;
    this.Contents = text;
    this.Subject = subject;
    if (icon == IconNames.Extended)
      this.ExtendedIconName = "Star";
    else
      this.StandardIconName = icon;
    if ((text ?? "").Trim() != "")
      this.RichText = $"<?xml version=\"1.0\"?><body xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:xfa=\"http://www.xfa.org/schema/xfa-data/1.0/\" xfa:APIVersion=\"Pdfium.NEt SDK:3.0.0\" xfa:spec=\"2.0.2\"><p dir=\"ltr\"><span style=\"text-align:left;font-size:13pt;font-style:normal;font-weight:normal;color:#000000;font-family:Arial\">{text}</span></p></body>";
    float num1 = 20f;
    float num2 = 20f;
    switch (icon)
    {
      case IconNames.Note:
        num2 = 17.696f;
        num1 = 20.836f;
        break;
      case IconNames.Comment:
        num2 = 19.414f;
        num1 = 19.414f;
        break;
      case IconNames.Key:
        num2 = 11.208f;
        num1 = 20.036f;
        break;
      case IconNames.Help:
        num2 = 21.71f;
        num1 = 21.712f;
        break;
      case IconNames.NewParagraph:
        num2 = 15.612f;
        num1 = 21.175f;
        break;
      case IconNames.Paragraph:
        num2 = 21.324f;
        num1 = 21.766f;
        break;
      case IconNames.Insert:
        num2 = 19.802f;
        num1 = 22.031f;
        break;
    }
    this.Rectangle = new FS_RECTF(x, y + num1, x + num2, y);
    this.RegenerateAppearances();
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfTextAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="icon">The name of a standard icon to be used in displaying the annotation.</param>
  /// <param name="text">Annotation text.</param>
  /// <param name="subject">Annotation subject.</param>
  /// <param name="x">The x-coordinate of this annotation.</param>
  /// <param name="y">The y-coordinate of this annotation.</param>
  public PdfTextAnnotation(
    PdfPage page,
    IconNames icon,
    string text,
    string subject,
    float x,
    float y)
    : this(page, icon, new FS_COLOR((int) byte.MaxValue, 0, 192 /*0xC0*/, (int) byte.MaxValue), text, subject, x, y)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfTextAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfTextAnnotation(PdfPage page, PdfTypeBase dictionary)
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
    float left = this.Rectangle.left;
    float top = this.Rectangle.top;
    FS_COLOR fillColor = new FS_COLOR(this.Opacity, this.Color);
    FS_COLOR strokeColor = new FS_COLOR(this.Opacity, 0.0f, 0.0f, 0.0f);
    List<PdfPathObject> pdfPathObjectList = (List<PdfPathObject>) null;
    switch (this.StandardIconName)
    {
      case IconNames.Note:
        pdfPathObjectList = AnnotDrawing.CreateNote(fillColor, strokeColor);
        break;
      case IconNames.Comment:
        pdfPathObjectList = AnnotDrawing.CreateComment(fillColor, strokeColor);
        break;
      case IconNames.Key:
        pdfPathObjectList = AnnotDrawing.CreateKey(fillColor, strokeColor);
        break;
      case IconNames.Help:
        pdfPathObjectList = AnnotDrawing.CreateHelp(fillColor, strokeColor);
        break;
      case IconNames.NewParagraph:
        pdfPathObjectList = AnnotDrawing.CreateNewParagraph(fillColor, strokeColor);
        break;
      case IconNames.Paragraph:
        pdfPathObjectList = AnnotDrawing.CreateParagraph(fillColor, strokeColor);
        break;
      case IconNames.Insert:
        pdfPathObjectList = AnnotDrawing.CreateInsert(fillColor, strokeColor);
        break;
      case IconNames.Extended:
        pdfPathObjectList = AnnotDrawing.CreateStar(fillColor, strokeColor);
        break;
    }
    if (pdfPathObjectList != null)
    {
      foreach (PdfPageObject pdfPageObject in pdfPathObjectList)
        this.NormalAppearance.Add(pdfPageObject);
    }
    this.GenerateAppearance(AppearanceStreamModes.Normal);
    this.Rectangle = new FS_RECTF(left, top, left + this.Rectangle.Width, top - this.Rectangle.Height);
  }
}
