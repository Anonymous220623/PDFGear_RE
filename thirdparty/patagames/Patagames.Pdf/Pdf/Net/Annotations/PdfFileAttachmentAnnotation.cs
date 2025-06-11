// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfFileAttachmentAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using Patagames.Pdf.Properties;
using System;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represents a file attachment annotation</summary>
/// <remarks>
/// A file attachment annotation (PDF 1.3) contains a reference to a file, which typically
/// is embedded in the PDF file; For example, a table of data might
/// use a file attachment annotation to link to a spreadsheet file based on that data;
/// activating the annotation extracts the embedded file and gives the user an opportunity
/// to view it or store it in the file system.
/// <para>
/// The Contents entry of the annotation dictionary may specify descriptive text relating
/// to the attached file.
/// </para>
/// </remarks>
public class PdfFileAttachmentAnnotation : PdfMarkupAnnotation
{
  private PdfFileSpecification _fileSpecification;

  /// <summary>
  /// The name of an icon to be used in displaying the annotation.
  /// </summary>
  /// <remarks>
  /// Additional names may be supported as well. In this case please use <see cref="P:Patagames.Pdf.Net.Annotations.PdfFileAttachmentAnnotation.ExtendedIconName" /> property.
  /// <note type="note">The annotation dictionary’s AP entry, if present, takes precedence over
  /// the StandardIconName and <see cref="P:Patagames.Pdf.Net.Annotations.PdfFileAttachmentAnnotation.ExtendedIconName" /> properties.</note>
  /// <para>Default value: <strong>PushPin</strong>.</para>
  /// </remarks>
  public FileIconNames StandardIconName
  {
    get
    {
      if (!this.IsExists("Name"))
        return FileIconNames.PushPin;
      FileIconNames result;
      return Pdfium.GetEnumDescription<FileIconNames>(this.Dictionary["Name"].As<PdfTypeName>().Value, out result) ? result : FileIconNames.Extended;
    }
    set
    {
      string initialVal = value != FileIconNames.Extended ? Pdfium.GetEnumDescription((Enum) value) : throw new ArgumentException(Error.err0043);
      this.Dictionary["Name"] = !((initialVal ?? "").Trim() == "") ? (PdfTypeBase) PdfTypeName.Create(initialVal) : throw new ArgumentException(string.Format(Error.err0047, (object) nameof (StandardIconName), (object) "is one of the FileIconNames enumerator"));
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
  /// Gets the string representation of the file specification, if any.
  /// </summary>
  /// <remarks>
  /// A simple file specification gives just the name of the target file in a standard format, independent of the naming conventions of any particular file system.
  /// </remarks>
  public string SimpleFileSpec
  {
    get
    {
      if (!this.IsExists("FS"))
        return (string) null;
      return this.Dictionary["FS"] is PdfTypeString ? this.Dictionary["FS"].As<PdfTypeString>().UnicodeString : (string) null;
    }
  }

  /// <summary>
  /// Gets or sets a file specification described the file associated with this annotation.
  /// </summary>
  public PdfFileSpecification FileSpecification
  {
    get
    {
      if (!this.IsExists("FS"))
        return (PdfFileSpecification) null;
      if ((PdfWrapper) this._fileSpecification == (PdfWrapper) null || this._fileSpecification.Dictionary.IsDisposed)
        this._fileSpecification = this.Dictionary["FS"].Is<PdfTypeDictionary>() ? new PdfFileSpecification(this.Page.Document, this.Dictionary["FS"]) : (PdfFileSpecification) null;
      return this._fileSpecification;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey("FS"))
        this.Dictionary.Remove("FS");
      else if ((PdfWrapper) value != (PdfWrapper) null)
      {
        this.ListOfIndirectObjects.Add((PdfTypeBase) value.Dictionary);
        this.Dictionary.SetIndirectAt("FS", this.ListOfIndirectObjects, (PdfTypeBase) value.Dictionary);
      }
      this._fileSpecification = value;
    }
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfFileAttachmentAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfFileAttachmentAnnotation(PdfPage page)
    : base(page)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("FileAttachment");
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfFileAttachmentAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="icon">The name of a standard icon to be used in displaying the annotation.</param>
  /// <param name="filename">The name of the attached file.</param>
  /// <param name="fileData">The contents of the attached file.</param>
  /// <param name="color">Annotation color.</param>
  /// <param name="x">The x-coordinate of this annotation.</param>
  /// <param name="y">The y-coordinate of this annotation.</param>
  public PdfFileAttachmentAnnotation(
    PdfPage page,
    FileIconNames icon,
    string filename,
    byte[] fileData,
    FS_COLOR color,
    float x,
    float y)
    : this(page)
  {
    this.Color = color;
    this.Opacity = (float) color.A / (float) byte.MaxValue;
    this.Flags = AnnotationFlags.Print | AnnotationFlags.NoZoom | AnnotationFlags.NoRotate;
    if (icon == FileIconNames.Extended)
      this.ExtendedIconName = "Star";
    else
      this.StandardIconName = icon;
    this.FileSpecification = new PdfFileSpecification(page.Document);
    this.FileSpecification.FileName = filename;
    this.FileSpecification.EmbeddedFile = new PdfFile(fileData);
    this.Rectangle = new FS_RECTF(x, y + 20f, x + 20f, y);
    this.RegenerateAppearances();
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfFileAttachmentAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="icon">The name of a standard icon to be used in displaying the annotation.</param>
  /// <param name="filename">The name of the attached file.</param>
  /// <param name="fileData">The contents of the attached file.</param>
  /// <param name="x">The x-coordinate of this annotation.</param>
  /// <param name="y">The y-coordinate of this annotation.</param>
  public PdfFileAttachmentAnnotation(
    PdfPage page,
    FileIconNames icon,
    string filename,
    byte[] fileData,
    float x,
    float y)
    : this(page, icon, filename, fileData, new FS_COLOR((int) byte.MaxValue, 0, 192 /*0xC0*/, (int) byte.MaxValue), x, y)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfFileAttachmentAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfFileAttachmentAnnotation(PdfPage page, PdfTypeBase dictionary)
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
    float bottom = this.Rectangle.bottom;
    FS_COLOR fillColor = new FS_COLOR(this.Opacity, this.Color);
    FS_COLOR strokeColor = new FS_COLOR(this.Opacity, 0.0f, 0.0f, 0.0f);
    List<PdfPathObject> pdfPathObjectList = (List<PdfPathObject>) null;
    switch (this.StandardIconName)
    {
      case FileIconNames.PushPin:
        pdfPathObjectList = AnnotDrawing.CreatePushpin(fillColor, strokeColor);
        break;
      case FileIconNames.Graph:
        pdfPathObjectList = AnnotDrawing.CreateGraph(fillColor, strokeColor);
        break;
      case FileIconNames.Paperclip:
        pdfPathObjectList = AnnotDrawing.CreatePaperclip(fillColor, strokeColor);
        break;
      case FileIconNames.Tag:
        pdfPathObjectList = AnnotDrawing.CreateTag(fillColor, strokeColor);
        break;
      case FileIconNames.Extended:
        pdfPathObjectList = AnnotDrawing.CreateStar(fillColor, strokeColor);
        break;
    }
    if (pdfPathObjectList != null)
    {
      foreach (PdfPageObject pdfPageObject in pdfPathObjectList)
        this.NormalAppearance.Add(pdfPageObject);
    }
    this.GenerateAppearance(AppearanceStreamModes.Normal);
    double l = (double) left;
    double num1 = (double) bottom;
    FS_RECTF rectangle = this.Rectangle;
    double height = (double) rectangle.Height;
    double t = num1 + height;
    double num2 = (double) left;
    rectangle = this.Rectangle;
    double width = (double) rectangle.Width;
    double r = num2 + width;
    double b = (double) bottom;
    this.Rectangle = new FS_RECTF((float) l, (float) t, (float) r, (float) b);
  }
}
