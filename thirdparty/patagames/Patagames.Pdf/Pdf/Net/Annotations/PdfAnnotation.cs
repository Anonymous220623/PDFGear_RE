// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfAnnotation
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
using System.Collections;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>
/// Abstract class representing entries that are common to all annotations.
/// </summary>
/// <remarks>
/// The annotation may contain additional entries specific to a particular annotation type; see the descriptions of individual annotation type.
/// <note>
/// A annotation may be referenced from the <see cref="T:Patagames.Pdf.Net.Annotations.PdfAnnotationCollection" /> that assigned with
/// one page only. Attempting to share an annotation among multiple pages
/// produces unpredictable behavior.This requirement applies only to the annotation
/// itself, not to subsidiary objects, which can be shared among multiple annotations without causing any difficulty
/// </note>
/// </remarks>
public abstract class PdfAnnotation : PdfWrapper
{
  private PdfPageObjectsCollection _normalAppearance;
  private PdfPageObjectsCollection _rolloverAppearance;
  private PdfPageObjectsCollection _downAppearance;

  /// <summary>Gets list of indirect objects</summary>
  protected PdfIndirectList ListOfIndirectObjects { get; private set; }

  /// <summary>Gets PDF page associated with this annotation.</summary>
  public PdfPage Page { get; private set; }

  /// <summary>
  /// Gets or sets the annotation rectangle, defining the location of the annotation on the page in default user space units.
  /// </summary>
  public virtual FS_RECTF Rectangle
  {
    get => new FS_RECTF(this.Dictionary["Rect"]);
    set => this.Dictionary["Rect"] = (PdfTypeBase) value.ToArray();
  }

  /// <summary>
  /// Gets or sets the text to be displayed for the annotation or, if this type of annotation does not display text, an alternate description of the annotation’s contents in human-readable form.
  /// </summary>
  /// <remarks>
  /// <para><see cref="T:Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation" /> annotations display text directly on the page. The annotation’s Contents property specifies the displayed text.</para>
  /// <para>Most other markup annotations have an associated pop-up window that may contain text. The annotation’s Contents property specifies the text to be displayed
  /// when the pop-up window is opened. These include <see cref="T:Patagames.Pdf.Net.Annotations.PdfTextAnnotation" />, <see cref="T:Patagames.Pdf.Net.Annotations.PdfLineAnnotation" />, <see cref="T:Patagames.Pdf.Net.Annotations.PdfSquareAnnotation" />, <see cref="T:Patagames.Pdf.Net.Annotations.PdfCircleAnnotation" />,
  /// <see cref="T:Patagames.Pdf.Net.Annotations.PdfPolygonAnnotation" />, <see cref="T:Patagames.Pdf.Net.Annotations.PdfPolylineAnnotation" />, <see cref="T:Patagames.Pdf.Net.Annotations.PdfHighlightAnnotation" />, <see cref="T:Patagames.Pdf.Net.Annotations.PdfUnderlineAnnotation" />,
  /// <see cref="T:Patagames.Pdf.Net.Annotations.PdfSquigglyAnnotation" />, <see cref="T:Patagames.Pdf.Net.Annotations.PdfStrikeoutAnnotation" />, <see cref="T:Patagames.Pdf.Net.Annotations.PdfStampAnnotation" />, <see cref="T:Patagames.Pdf.Net.Annotations.PdfCaretAnnotation" />, <see cref="T:Patagames.Pdf.Net.Annotations.PdfInkAnnotation" />,
  /// and <see cref="T:Patagames.Pdf.Net.Annotations.PdfFileAttachmentAnnotation" /> annotations.</para>
  /// <para><see cref="T:Patagames.Pdf.Net.Annotations.PdfSoundAnnotation" /> annotations do not have a pop-up window but may also have associated text specified by the Contents property.</para>
  /// <note type="note">When separating text into paragraphs, a carriage return should be used (and not, for example, a line feed character).</note>
  /// <para>For all other annotation types (<see cref="T:Patagames.Pdf.Net.Annotations.PdfLinkAnnotation" />, <see cref="T:Patagames.Pdf.Net.Annotations.PdfMovieAnnotation" />, <see cref="T:Patagames.Pdf.Net.Annotations.PdfWidgetAnnotation" />, <see cref="T:Patagames.Pdf.Net.Annotations.PdfPrinterMarkAnnotation" />, and <see cref="T:Patagames.Pdf.Net.Annotations.PdfTrapNetAnnotation" />),
  /// the Contents property provides an alternate representation of the annotation’s contents in human-readable form, which is useful when extracting the document’s
  /// contents in support of accessibility to users with disabilities or for other purposes.</para>
  /// <para>The <see cref="T:Patagames.Pdf.Net.Annotations.PdfPopupAnnotation" /> type typically does not appear by itself; it is associated with a markup annotation that uses it to display text.
  /// <note type="note"> The Contents property for a pop-up annotation is relevant only if it has no parent; in that case, it represents the text of the annotation.</note>
  /// </para>
  /// </remarks>
  public virtual string Contents
  {
    get
    {
      return !this.IsExists(nameof (Contents)) ? (string) null : this.Dictionary[nameof (Contents)].As<PdfTypeString>().UnicodeString;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey(nameof (Contents)))
      {
        this.Dictionary.Remove(nameof (Contents));
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary[nameof (Contents)] = (PdfTypeBase) PdfTypeString.Create(value, true);
      }
    }
  }

  /// <summary>
  /// Gets or sets the annotation name, a text string uniquely identifying it among all the annotations on its page.
  /// </summary>
  public virtual string Name
  {
    get
    {
      return !this.IsExists("NM") ? (string) null : this.Dictionary["NM"].As<PdfTypeString>().AnsiString;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("NM"))
      {
        this.Dictionary.Remove("NM");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["NM"] = (PdfTypeBase) PdfTypeString.Create(value);
      }
    }
  }

  /// <summary>
  /// Gets or sets the date and time when the annotation was most recently modified.
  /// The preferred format is a date string as described in remarks,
  /// but viewer applications should be prepared to accept and display a string in any format.
  /// </summary>
  /// <remarks>
  /// PDF defines a standard date format, which closely follows that of the
  /// international standard ASN.1 (Abstract Syntax Notation One), defined in ISO/
  /// IEC 8824 (see the Bibliography). A date is an ASCII string of the form
  /// <para>(D:YYYYMMDDHHmmSSOHH'mm')</para>
  /// where
  /// <list type="bullet">
  /// <item>YYYY is the year</item>
  /// <item>MM is the month</item>
  /// <item>DD is the day (01–31)</item>
  /// <item>HH is the hour (00–23)</item>
  /// <item>mm is the minute (00–59)</item>
  /// <item>SS is the second (00–59)</item>
  /// <item>O is the relationship of local time to Universal Time (UT), denoted by one of the characters +, −, or Z (see below)</item>
  /// <item>HH followed by ' is the absolute value of the offset from UT in hours (00–23)</item>
  /// <item>mm followed by ' is the absolute value of the offset from UT in minutes (00–59)</item>
  /// </list>
  /// The apostrophe character (') after HH and mm is part of the syntax. All fields after
  /// the year are optional. (The prefix D:, although also optional, is strongly
  /// recommended.) The default values for MM and DD are both 01; all other
  /// numerical fields default to zero values. A plus sign (+) as the value of the O field
  /// signifies that local time is later than UT, a minus sign(−) signifies that local time
  /// is earlier than UT, and the letter Z signifies that local time is equal to UT.If no UT
  /// information is specified, the relationship of the specified time to UT is considered
  /// to be unknown.Regardless of whether the time zone is known, the rest of the date
  /// should be specified in local time.
  /// <para>For example, December 23, 1998, at 7:52 PM, U.S.Pacific Standard Time, is
  /// represented by the string</para>
  /// D:199812231952−08'00'
  /// </remarks>
  public virtual string ModificationDate
  {
    get
    {
      return !this.IsExists("M") ? (string) null : this.Dictionary["M"].As<PdfTypeString>().UnicodeString;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("M"))
      {
        this.Dictionary.Remove("M");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["M"] = (PdfTypeBase) PdfTypeString.Create(value, true);
      }
    }
  }

  /// <summary>
  ///  Gets or sets a set of flags specifying various characteristics of the annotation <see cref="T:Patagames.Pdf.Enums.AnnotationFlags" />.
  /// </summary>
  public virtual AnnotationFlags Flags
  {
    get
    {
      return !this.IsExists("F") ? AnnotationFlags.None : (AnnotationFlags) this.Dictionary["F"].As<PdfTypeNumber>().IntValue;
    }
    set
    {
      if (value == AnnotationFlags.None && this.Dictionary.ContainsKey("F"))
      {
        this.Dictionary.Remove("F");
      }
      else
      {
        if (value == AnnotationFlags.None)
          return;
        this.Dictionary["F"] = (PdfTypeBase) PdfTypeNumber.Create((int) value);
      }
    }
  }

  /// <summary>
  /// Gets or sets the color of annotation.Please see remarks section for more details
  /// </summary>
  /// <remarks>
  /// The color used for the following purposes:
  /// <list type="bullet">
  /// <item>The background of the annotation’s icon when closed</item>
  /// <item>The title bar of the annotation’s pop-up window</item>
  /// <item>The border of a link annotation</item>
  /// </list>
  /// </remarks>
  public virtual FS_COLOR Color
  {
    get
    {
      return !this.IsExists("C") ? new FS_COLOR(0) : new FS_COLOR(this.Dictionary["C"].As<PdfTypeArray>());
    }
    set
    {
      if (value.A == 0 && this.Dictionary.ContainsKey("C"))
      {
        this.Dictionary.Remove("C");
      }
      else
      {
        if (value.A == 0)
          return;
        this.Dictionary["C"] = (PdfTypeBase) value.ToArray();
      }
    }
  }

  /// <summary>The annotation’s normal appearance.</summary>
  /// <remarks>
  /// An appearance dictionary specifying how the annotation is presented visually on the page.
  /// <para>The normal appearance is used when the annotation is not interacting with the user.This appearance is also used for printing the annotation.</para>
  /// </remarks>
  public virtual PdfPageObjectsCollection NormalAppearance
  {
    get
    {
      if (this._normalAppearance != null)
        return this._normalAppearance;
      IntPtr appearanceStream = Pdfium.FPDFAnnot_GetAppearanceStream(this.Dictionary.Handle, AppearanceStreamModes.Normal);
      if (appearanceStream == IntPtr.Zero)
        return (PdfPageObjectsCollection) null;
      IntPtr resDict = IntPtr.Zero;
      if (this.Page.Dictionary.ContainsKey("Resources"))
        resDict = this.Page.Dictionary["Resources"].Handle;
      this._normalAppearance = new PdfPageObjectsCollection(this.Page.Document, resDict, appearanceStream);
      return this._normalAppearance;
    }
  }

  /// <summary>The annotation’s rollover appearance.</summary>
  /// <remarks>
  /// An appearance dictionary specifying how the annotation is presented visually on the page.
  /// <para>The rollover appearance is used when the user moves the cursor into the annotation’s active area without pressing the mouse button.</para>
  /// </remarks>
  public virtual PdfPageObjectsCollection RolloverAppearance
  {
    get
    {
      if (this._rolloverAppearance != null)
        return this._rolloverAppearance;
      IntPtr appearanceStream = Pdfium.FPDFAnnot_GetAppearanceStream(this.Dictionary.Handle, AppearanceStreamModes.Rollover);
      if (appearanceStream == IntPtr.Zero)
        return (PdfPageObjectsCollection) null;
      IntPtr resDict = IntPtr.Zero;
      if (this.Page.Dictionary.ContainsKey("Resources"))
        resDict = this.Page.Dictionary["Resources"].Handle;
      this._rolloverAppearance = new PdfPageObjectsCollection(this.Page.Document, resDict, appearanceStream);
      return this._rolloverAppearance;
    }
  }

  /// <summary>The annotation’s down appearance.</summary>
  /// <remarks>
  /// An appearance dictionary specifying how the annotation is presented visually on the page.
  /// <para>The down appearance is used when the mouse button is pressed or held down within the annotation’s active area.</para>
  /// </remarks>
  public virtual PdfPageObjectsCollection DownAppearance
  {
    get
    {
      if (this._downAppearance != null)
        return this._downAppearance;
      IntPtr appearanceStream = Pdfium.FPDFAnnot_GetAppearanceStream(this.Dictionary.Handle, AppearanceStreamModes.Down);
      if (appearanceStream == IntPtr.Zero)
        return (PdfPageObjectsCollection) null;
      IntPtr resDict = IntPtr.Zero;
      if (this.Page.Dictionary.ContainsKey("Resources"))
        resDict = this.Page.Dictionary["Resources"].Handle;
      this._downAppearance = new PdfPageObjectsCollection(this.Page.Document, resDict, appearanceStream);
      return this._downAppearance;
    }
  }

  /// <summary>Creates a new annotation.</summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfAnnotation(PdfPage page)
  {
    this.Page = page;
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Annot");
    this.Rectangle = new FS_RECTF();
    this.Name = Guid.NewGuid().ToString();
    this.ListOfIndirectObjects = PdfIndirectList.FromPdfDocument(this.Page.Document);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfAnnotation(PdfPage page, PdfTypeBase dictionary)
    : base(dictionary)
  {
    this.Page = page;
    this.ListOfIndirectObjects = PdfIndirectList.FromPdfDocument(this.Page.Document);
  }

  /// <summary>
  /// Creates a new annotation based on its type in the specified dictionary.
  /// </summary>
  /// <param name="dictionary">Annotation dictionary or indirect dictionary.</param>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <returns>A new instance of the derived annotation class or <see cref="T:Patagames.Pdf.Net.Annotations.PdfUnknownAnnotation" /> if type is not recognized.</returns>
  /// <exception cref="T:System.ArgumentNullException" />
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfParserException" />
  public static PdfAnnotation Create(PdfTypeBase dictionary, PdfPage page)
  {
    if (dictionary == null)
      throw new ArgumentNullException(nameof (dictionary));
    PdfTypeDictionary pdfTypeDictionary = (PdfTypeDictionary) null;
    if (dictionary is PdfTypeDictionary)
      pdfTypeDictionary = dictionary as PdfTypeDictionary;
    else if (dictionary is PdfTypeIndirect)
    {
      PdfTypeBase direct = (dictionary as PdfTypeIndirect).Direct;
      if (direct is PdfTypeDictionary)
        pdfTypeDictionary = direct as PdfTypeDictionary;
    }
    if (pdfTypeDictionary == null)
      throw new PdfParserException(Error.err0038);
    if (!pdfTypeDictionary.ContainsKey("Subtype") || !(pdfTypeDictionary["Subtype"] is PdfTypeName))
      throw new PdfParserException(Error.err0038);
    switch ((pdfTypeDictionary["Subtype"] as PdfTypeName).Value)
    {
      case "3D":
        return (PdfAnnotation) new Pdf3DAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "Caret":
        return (PdfAnnotation) new PdfCaretAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "Circle":
        return (PdfAnnotation) new PdfCircleAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "FileAttachment":
        return (PdfAnnotation) new PdfFileAttachmentAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "FreeText":
        return (PdfAnnotation) new PdfFreeTextAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "Highlight":
        return (PdfAnnotation) new PdfHighlightAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "Ink":
        return (PdfAnnotation) new PdfInkAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "Line":
        return (PdfAnnotation) new PdfLineAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "Link":
        return (PdfAnnotation) new PdfLinkAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "Movie":
        return (PdfAnnotation) new PdfMovieAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "PolyLine":
        return (PdfAnnotation) new PdfPolylineAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "Polygon":
        return (PdfAnnotation) new PdfPolygonAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "Popup":
        return (PdfAnnotation) new PdfPopupAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "PrinterMark":
        return (PdfAnnotation) new PdfPrinterMarkAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "Screen":
        return (PdfAnnotation) new PdfScreenAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "Sound":
        return (PdfAnnotation) new PdfSoundAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "Square":
        return (PdfAnnotation) new PdfSquareAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "Squiggly":
        return (PdfAnnotation) new PdfSquigglyAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "Stamp":
        return (PdfAnnotation) new PdfStampAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "StrikeOut":
        return (PdfAnnotation) new PdfStrikeoutAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "Text":
        return (PdfAnnotation) new PdfTextAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "TrapNet":
        return (PdfAnnotation) new PdfTrapNetAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "Underline":
        return (PdfAnnotation) new PdfUnderlineAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "Watermark":
        return (PdfAnnotation) new PdfWatermarkAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      case "Widget":
        return (PdfAnnotation) new PdfWidgetAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
      default:
        return (PdfAnnotation) new PdfUnknownAnnotation(page, (PdfTypeBase) pdfTypeDictionary);
    }
  }

  /// <summary>
  /// Releases the unmanaged resources used by the Annotation class and optionally releases the managed resources.
  /// </summary>
  /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      if (this._normalAppearance != null)
        this._normalAppearance.Dispose();
      this._normalAppearance = (PdfPageObjectsCollection) null;
      if (this._downAppearance != null)
        this._downAppearance.Dispose();
      this._downAppearance = (PdfPageObjectsCollection) null;
      if (this._rolloverAppearance != null)
        this._rolloverAppearance.Dispose();
      this._rolloverAppearance = (PdfPageObjectsCollection) null;
    }
    base.Dispose(disposing);
  }

  /// <summary>Creates empty appearance stream</summary>
  /// <param name="mode">A type of annotation appearance that should be created.</param>
  /// <exception cref="T:System.ArgumentException" />
  public void CreateEmptyAppearance(AppearanceStreamModes mode)
  {
    if (mode != AppearanceStreamModes.Normal && mode != AppearanceStreamModes.Down && mode != AppearanceStreamModes.Rollover)
      throw new ArgumentException();
    if (!this.IsExists("AP"))
      this.Dictionary["AP"] = (PdfTypeBase) PdfTypeDictionary.Create();
    PdfTypeDictionary pdfTypeDictionary = this.Dictionary["AP"].As<PdfTypeDictionary>();
    PdfTypeStream indirectObject = PdfTypeStream.Create();
    indirectObject.InitEmpty();
    int objectNumber = this.ListOfIndirectObjects.Add((PdfTypeBase) indirectObject);
    switch (mode)
    {
      case AppearanceStreamModes.Normal:
        pdfTypeDictionary.SetIndirectAt("N", this.ListOfIndirectObjects, objectNumber);
        break;
      case AppearanceStreamModes.Rollover:
        pdfTypeDictionary.SetIndirectAt("R", this.ListOfIndirectObjects, objectNumber);
        break;
      case AppearanceStreamModes.Down:
        pdfTypeDictionary.SetIndirectAt("D", this.ListOfIndirectObjects, objectNumber);
        break;
    }
  }

  /// <summary>Generate content of specified appearance stream.</summary>
  /// <param name="mode">Specifies appearance stream that should be generated.</param>
  /// <returns>true if content was successfully generated; false otherwise.</returns>
  public bool GenerateAppearance(AppearanceStreamModes mode)
  {
    PdfPageObjectsCollection collection;
    switch (mode)
    {
      case AppearanceStreamModes.Normal:
        if (this.NormalAppearance == null)
          return false;
        collection = this.NormalAppearance;
        break;
      case AppearanceStreamModes.Rollover:
        if (this.RolloverAppearance == null)
          return false;
        collection = this.RolloverAppearance;
        break;
      case AppearanceStreamModes.Down:
        if (this.DownAppearance == null)
          return false;
        collection = this.DownAppearance;
        break;
      default:
        throw new ArgumentException();
    }
    IntPtr appearanceStream = Pdfium.FPDFAnnot_GetAppearanceStream(this.Dictionary.Handle, mode);
    int num = Pdfium.FPDF_GenerateContentToStream(this.Page.Document.Handle, collection.Handle, appearanceStream, IntPtr.Zero) ? 1 : 0;
    PdfTypeDictionary pdfTypeDictionary = PdfTypeDictionary.Create(Pdfium.FPDFOBJ_GetDict(appearanceStream));
    FS_RECTF fsRectf = AnnotDrawing.CalcBBox((IEnumerable) collection);
    pdfTypeDictionary["BBox"] = (PdfTypeBase) fsRectf.ToArray();
    pdfTypeDictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("XObject");
    pdfTypeDictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("Form");
    pdfTypeDictionary["FormType"] = (PdfTypeBase) PdfTypeNumber.Create(1);
    pdfTypeDictionary["Matrix"] = (PdfTypeBase) new FS_MATRIX(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f).ToArray();
    this.Rectangle = new FS_RECTF(fsRectf.left, fsRectf.top, fsRectf.right, fsRectf.bottom);
    return num != 0;
  }
}
