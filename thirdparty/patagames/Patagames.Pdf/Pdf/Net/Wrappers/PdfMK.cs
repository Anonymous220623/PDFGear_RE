// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Wrappers.PdfMK
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Collections;

#nullable disable
namespace Patagames.Pdf.Net.Wrappers;

/// <summary>
/// Represents the MK entry used to provide additional information for constructing the annotation’s appearance stream.
/// </summary>
public class PdfMK : PdfWrapper
{
  private PdfPage _page;
  private PdfIconFit _iconFit;
  private PdfPageObjectsCollection _normalIcon;
  private PdfPageObjectsCollection _rolloverIcon;
  private PdfPageObjectsCollection _downIcon;

  /// <summary>
  /// Gets or sets a value indicating how the widget annotation is rotated counterclockwise relative to the page.
  /// </summary>
  public PageRotate Rotation
  {
    get
    {
      if (!this.IsExists("R"))
        return PageRotate.Normal;
      int rotation = this.Dictionary["R"].As<PdfTypeNumber>().IntValue / 90 % 4;
      if (rotation < 0)
        rotation = 4 + rotation;
      return (PageRotate) rotation;
    }
    set
    {
      if (value == PageRotate.Normal && this.Dictionary.ContainsKey("R"))
      {
        this.Dictionary.Remove("R");
      }
      else
      {
        if (value == PageRotate.Normal)
          return;
        this.Dictionary["R"] = (PdfTypeBase) PdfTypeNumber.Create((int) value * 90);
      }
    }
  }

  /// <summary>
  /// Gets or sets the color of the widget annotation’s border.
  /// </summary>
  public FS_COLOR BorderColor
  {
    get
    {
      return !this.IsExists("BC") ? new FS_COLOR(0) : new FS_COLOR(this.Dictionary["BC"].As<PdfTypeArray>());
    }
    set
    {
      if (value.A == 0 && this.Dictionary.ContainsKey("BC"))
      {
        this.Dictionary.Remove("BC");
      }
      else
      {
        if (value.A == 0)
          return;
        this.Dictionary["BC"] = (PdfTypeBase) value.ToArray();
      }
    }
  }

  /// <summary>
  /// Gets or sets the color of the widget annotation’s background.
  /// </summary>
  public FS_COLOR BackgroundColor
  {
    get
    {
      return !this.IsExists("BG") ? new FS_COLOR(0) : new FS_COLOR(this.Dictionary["BG"].As<PdfTypeArray>());
    }
    set
    {
      if (value.A == 0 && this.Dictionary.ContainsKey("BG"))
      {
        this.Dictionary.Remove("BG");
      }
      else
      {
        if (value.A == 0)
          return;
        this.Dictionary["BG"] = (PdfTypeBase) value.ToArray();
      }
    }
  }

  /// <summary>
  /// Gets or sets the widget annotation’s normal caption, displayed when it is not interacting with the user.
  /// </summary>
  /// <remarks>Apply only to widget annotations associated with pushbutton fields, check boxes and radio buttons.</remarks>
  public string NormalCaption
  {
    get
    {
      return !this.IsExists("CA") ? (string) null : this.Dictionary["CA"].As<PdfTypeString>().UnicodeString;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("CA"))
      {
        this.Dictionary.Remove("CA");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["CA"] = (PdfTypeBase) PdfTypeString.Create(value, true);
      }
    }
  }

  /// <summary>
  /// Gets or sets the widget annotation’s rollover caption, displayed when the user rolls the cursor into its active area without pressing the mouse button.
  /// </summary>
  /// <remarks>Apply only to widget annotations associated with pushbutton fields.</remarks>
  public string RolloverCaption
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
  /// Gets or sets the widget annotation’s alternate (down) caption, displayed when the mouse button is pressed within its active area.
  /// </summary>
  /// <remarks>Apply only to widget annotations associated with pushbutton fields.</remarks>
  public string DownCaption
  {
    get
    {
      return !this.IsExists("AC") ? (string) null : this.Dictionary["AC"].As<PdfTypeString>().UnicodeString;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("AC"))
      {
        this.Dictionary.Remove("AC");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["AC"] = (PdfTypeBase) PdfTypeString.Create(value, true);
      }
    }
  }

  /// <summary>
  /// Gets or sets a code indicating where to position the text of the widget annotation’s caption relative to its icon.
  /// </summary>
  /// <remarks>Apply only to widget annotations associated with pushbutton fields.</remarks>
  public TextPositions CaptionPosition
  {
    get
    {
      return !this.IsExists("TP") ? TextPositions.TEXTPOS_CAPTION : (TextPositions) this.Dictionary["TP"].As<PdfTypeNumber>().IntValue;
    }
    set
    {
      if (value == TextPositions.TEXTPOS_CAPTION && this.Dictionary.ContainsKey("TP"))
      {
        this.Dictionary.Remove("TP");
      }
      else
      {
        if (value == TextPositions.TEXTPOS_CAPTION)
          return;
        this.Dictionary["TP"] = (PdfTypeBase) PdfTypeNumber.Create((int) value);
      }
    }
  }

  /// <summary>
  /// Gets or sets an icon fit specifying how to display the widget annotation’s icon within its annotation rectangle.
  /// If present, the icon fit dictionary applies to all of the annotation’s icons (<see cref="P:Patagames.Pdf.Net.Wrappers.PdfMK.NormalIcon" />, <see cref="P:Patagames.Pdf.Net.Wrappers.PdfMK.RolloverIcon" />, and <see cref="P:Patagames.Pdf.Net.Wrappers.PdfMK.DownIcon" />)
  /// </summary>
  /// <remarks>Apply only to widget annotations associated with pushbutton fields.</remarks>
  public PdfIconFit IconFit
  {
    get
    {
      if (!this.IsExists("IF"))
        return (PdfIconFit) null;
      if ((PdfWrapper) this._iconFit == (PdfWrapper) null || this._iconFit.Dictionary.IsDisposed)
        this._iconFit = new PdfIconFit(this.Dictionary["IF"]);
      return this._iconFit;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey("IF"))
        this.Dictionary.Remove("IF");
      else if ((PdfWrapper) value != (PdfWrapper) null)
        this.Dictionary["IF"] = (PdfTypeBase) value.Dictionary;
      this._iconFit = value;
    }
  }

  /// <summary>
  /// Gets a <see cref="T:Patagames.Pdf.Net.PdfPageObjectsCollection" /> defining the widget annotation’s normal icon, displayed when it is not interacting with the user.
  /// </summary>
  /// <remarks>Apply only to widget annotations associated with pushbutton fields.</remarks>
  public PdfPageObjectsCollection NormalIcon
  {
    get
    {
      if (this._normalIcon != null)
        return this._normalIcon;
      IntPtr handle = this.Dictionary["I"].As<PdfTypeStream>().Handle;
      if (handle == IntPtr.Zero)
        return (PdfPageObjectsCollection) null;
      IntPtr resDict = IntPtr.Zero;
      if (this._page.Dictionary.ContainsKey("Resources"))
        resDict = this._page.Dictionary["Resources"].Handle;
      this._normalIcon = new PdfPageObjectsCollection(this._page.Document, resDict, handle);
      return this._normalIcon;
    }
  }

  /// <summary>
  /// Gets a <see cref="T:Patagames.Pdf.Net.PdfPageObjectsCollection" /> defining the widget annotation’s rollover icon, displayed when the user rolls the cursor into its active area without pressing the mouse button.
  /// </summary>
  /// <remarks>Apply only to widget annotations associated with pushbutton fields.</remarks>
  public PdfPageObjectsCollection RolloverIcon
  {
    get
    {
      if (this._rolloverIcon != null)
        return this._rolloverIcon;
      IntPtr handle = this.Dictionary["RI"].As<PdfTypeStream>().Handle;
      if (handle == IntPtr.Zero)
        return (PdfPageObjectsCollection) null;
      IntPtr resDict = IntPtr.Zero;
      if (this._page.Dictionary.ContainsKey("Resources"))
        resDict = this._page.Dictionary["Resources"].Handle;
      this._rolloverIcon = new PdfPageObjectsCollection(this._page.Document, resDict, handle);
      return this._rolloverIcon;
    }
  }

  /// <summary>
  /// Gets a <see cref="T:Patagames.Pdf.Net.PdfPageObjectsCollection" /> defining the widget annotation’s alternate (down) icon, displayed when the mouse button is pressed within its active area.
  /// </summary>
  /// <remarks>Apply only to widget annotations associated with pushbutton fields.</remarks>
  public PdfPageObjectsCollection DownIcon
  {
    get
    {
      if (this._downIcon != null)
        return this._downIcon;
      IntPtr handle = this.Dictionary["IX"].As<PdfTypeStream>().Handle;
      if (handle == IntPtr.Zero)
        return (PdfPageObjectsCollection) null;
      IntPtr resDict = IntPtr.Zero;
      if (this._page.Dictionary.ContainsKey("Resources"))
        resDict = this._page.Dictionary["Resources"].Handle;
      this._downIcon = new PdfPageObjectsCollection(this._page.Document, resDict, handle);
      return this._downIcon;
    }
  }

  /// <summary>
  /// Creates new instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfMK" />.
  /// </summary>
  /// <param name="page">The PDF page to which the corresponding widget annotation is assigned.</param>
  public PdfMK(PdfPage page)
  {
    this._page = page != null ? page : throw new ArgumentNullException(nameof (page));
  }

  /// <summary>
  /// Creates a new instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfMK" /> and initialize it with specified dictionary
  /// </summary>
  /// <param name="page">The PDF page to which the corresponding widget annotation is assigned.</param>
  /// <param name="dictionary">The dictionary or indirect dictionary</param>
  public PdfMK(PdfPage page, PdfTypeBase dictionary)
    : base(dictionary)
  {
    this._page = page != null ? page : throw new ArgumentNullException(nameof (page));
  }

  /// <summary>
  /// Releases the unmanaged resources used by the <see cref="T:Patagames.Pdf.Net.Wrappers.PdfMK" /> class and optionally releases the managed resources.
  /// </summary>
  /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      if (this._normalIcon != null)
        this._normalIcon.Dispose();
      this._normalIcon = (PdfPageObjectsCollection) null;
      if (this._downIcon != null)
        this._downIcon.Dispose();
      this._downIcon = (PdfPageObjectsCollection) null;
      if (this._rolloverIcon != null)
        this._rolloverIcon.Dispose();
      this._rolloverIcon = (PdfPageObjectsCollection) null;
    }
    base.Dispose(disposing);
  }

  /// <summary>Creates empty icon stream</summary>
  /// <param name="mode">A type of icon appearance that should be created.</param>
  /// <exception cref="T:System.ArgumentException" />
  public void CreateEmptyAppearance(AppearanceStreamModes mode)
  {
    if (mode != AppearanceStreamModes.Normal && mode != AppearanceStreamModes.Down && mode != AppearanceStreamModes.Rollover)
      throw new ArgumentException();
    PdfTypeStream indirectObject = PdfTypeStream.Create();
    indirectObject.InitEmpty();
    PdfIndirectList list = PdfIndirectList.FromPdfDocument(this._page.Document);
    int objectNumber = list.Add((PdfTypeBase) indirectObject);
    switch (mode)
    {
      case AppearanceStreamModes.Normal:
        this.Dictionary.SetIndirectAt("I", list, objectNumber);
        break;
      case AppearanceStreamModes.Rollover:
        this.Dictionary.SetIndirectAt("RI", list, objectNumber);
        break;
      case AppearanceStreamModes.Down:
        this.Dictionary.SetIndirectAt("IX", list, objectNumber);
        break;
    }
  }

  /// <summary>Generate content of specified icon stream.</summary>
  /// <param name="mode">Specifies appearance stream that should be generated.</param>
  /// <returns>true if content was successfully generated; false otherwise.</returns>
  public bool GenerateAppearance(AppearanceStreamModes mode)
  {
    IntPtr zero = IntPtr.Zero;
    PdfPageObjectsCollection collection;
    IntPtr handle;
    switch (mode)
    {
      case AppearanceStreamModes.Normal:
        if (this.NormalIcon == null)
          return false;
        collection = this.NormalIcon;
        handle = this.Dictionary["I"].As<PdfTypeStream>().Handle;
        break;
      case AppearanceStreamModes.Rollover:
        if (this.RolloverIcon == null)
          return false;
        collection = this.RolloverIcon;
        handle = this.Dictionary["RI"].As<PdfTypeStream>().Handle;
        break;
      case AppearanceStreamModes.Down:
        if (this.DownIcon == null)
          return false;
        collection = this.DownIcon;
        handle = this.Dictionary["IX"].As<PdfTypeStream>().Handle;
        break;
      default:
        throw new ArgumentException();
    }
    int num = Pdfium.FPDF_GenerateContentToStream(this._page.Document.Handle, collection.Handle, handle, IntPtr.Zero) ? 1 : 0;
    PdfTypeDictionary pdfTypeDictionary = PdfTypeDictionary.Create(Pdfium.FPDFOBJ_GetDict(handle));
    pdfTypeDictionary["BBox"] = (PdfTypeBase) AnnotDrawing.CalcBBox((IEnumerable) collection).ToArray();
    pdfTypeDictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("XObject");
    pdfTypeDictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("Form");
    pdfTypeDictionary["FormType"] = (PdfTypeBase) PdfTypeNumber.Create(1);
    pdfTypeDictionary["Matrix"] = (PdfTypeBase) new FS_MATRIX(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f).ToArray();
    return num != 0;
  }
}
