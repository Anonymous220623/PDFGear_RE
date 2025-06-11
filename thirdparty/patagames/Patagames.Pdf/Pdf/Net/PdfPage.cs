// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfPage
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Actions;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.EventArguments;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Net.Wrappers;
using Patagames.Pdf.Properties;
using System;
using System.Drawing;
using System.Text;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents a PDF page inside document.</summary>
public class PdfPage : IDisposable
{
  private int _pageIndex;
  private IntPtr _handle = IntPtr.Zero;
  private IFSDK_PAUSE _pause;
  private PdfText _text;
  private PdfPageObjectsCollection _pageObjects;
  private PdfLinkCollections _links;
  private bool _fromHandle;
  private PdfTypeDictionary _dictionary;
  private PdfAnnotationCollection _annots;
  private ProgressiveStatus _progressiveStatus;
  private PdfPageLevelActions _aactions;

  /// <summary>
  /// SDK fire this event during rendering process initiated by <see cref="O:Patagames.Pdf.Net.PdfPage.StartProgressiveRender" /> method.
  /// </summary>
  public event EventHandler<ProgressiveRenderEventArgs> ProgressiveRender;

  /// <summary>
  /// This event is triggered when the page is loaded into memory.
  /// </summary>
  public event EventHandler Loaded;

  /// <summary>
  /// This event is triggered when the page is unloaded from the memory.
  /// </summary>
  public event EventHandler Disposed;

  /// <summary>
  /// Raises the <see cref="E:Patagames.Pdf.Net.PdfPage.ProgressiveRender" /> event.
  /// </summary>
  /// <param name="e">An ProgressiveRenderEventArgs that contains the event data.</param>
  protected virtual void OnProgressiveRender(ProgressiveRenderEventArgs e)
  {
    if (this.ProgressiveRender == null)
      return;
    this.ProgressiveRender((object) this, e);
  }

  /// <summary>
  /// Raises the <see cref="E:Patagames.Pdf.Net.PdfPage.Loaded" /> event.
  /// </summary>
  protected virtual void OnLoaded()
  {
    if (this.Loaded == null)
      return;
    this.Loaded((object) this, EventArgs.Empty);
  }

  /// <summary>
  /// Raises the <see cref="E:Patagames.Pdf.Net.PdfPage.Disposed" /> event.
  /// </summary>
  protected virtual void OnDisposed()
  {
    if (this.Disposed == null)
      return;
    this.Disposed((object) this, EventArgs.Empty);
  }

  /// <summary>
  /// Gets a value indicating whether the page has been loaded.
  /// </summary>
  public bool IsLoaded => this._handle != IntPtr.Zero;

  /// <summary>
  /// Gets a value indicating whether the page content has been parsed.
  /// </summary>
  public bool IsParsed { get; private set; }

  /// <summary>Gets the index of the page in the document.</summary>
  public int PageIndex
  {
    get => this._pageIndex;
    internal set => this._pageIndex = value;
  }

  /// <summary>Gets annotations associated with the page</summary>
  public PdfAnnotationCollection Annots
  {
    get
    {
      if (this._annots != null && !this._annots.IsDisposed)
        return this._annots;
      this._annots = PdfAnnotationCollection.GetAnnotations(this);
      return this._annots;
    }
  }

  /// <summary>
  /// Gets or sets the object that contains data about the page.
  /// </summary>
  /// <remarks>
  /// Any type derived from the Object class can be assigned to this property.
  /// A common use for the Tag property is to store data that is closely associated with the page.
  /// </remarks>
  public object Tag { get; set; }

  /// <summary>Gets the page's dictionary</summary>
  public PdfTypeDictionary Dictionary
  {
    get
    {
      if (this.Handle == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      IntPtr pageDictionary = Pdfium.FPDF_GetPageDictionary(this.Document.Handle, this._pageIndex);
      if (pageDictionary == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      if (this._dictionary == null || this._dictionary.IsDisposed || this._dictionary.Handle != pageDictionary)
        this._dictionary = new PdfTypeDictionary(pageDictionary);
      return this._dictionary;
    }
  }

  /// <summary>
  /// Gets a value indicating whether the object has been disposed of.
  /// <value>true if the page has been disposed of; otherwise, false.</value>
  /// </summary>
  public bool IsDisposed { get; private set; }

  /// <summary>
  /// Gets the PDF document associated with the current page.
  /// </summary>
  public PdfDocument Document { get; private set; }

  /// <summary>
  /// Getscurrent instance of <see cref="T:Patagames.Pdf.Net.PdfText" /> class that represents information about all characters in a page.
  /// </summary>
  public PdfText Text
  {
    get
    {
      if (this._text == null)
        this._text = new PdfText(this);
      return this._text;
    }
  }

  /// <summary>
  /// Gets the collection of PDF page objects found in the current page.
  /// </summary>
  public PdfPageObjectsCollection PageObjects
  {
    get
    {
      if (this._pageObjects == null)
        this._pageObjects = new PdfPageObjectsCollection(this);
      return this._pageObjects;
    }
  }

  /// <summary>
  /// Gets the collection of PDF links found in the current page.
  /// </summary>
  public PdfLinkCollections Links
  {
    get
    {
      if (this._links == null)
        this._links = new PdfLinkCollections(this);
      return this._links;
    }
  }

  /// <summary>Gets the Pdfium SDK handle that the page is bound to</summary>
  public IntPtr Handle
  {
    get
    {
      if (this._handle != IntPtr.Zero)
        return this._handle;
      this.InitByHandle(IntPtr.Zero);
      return this._handle;
    }
  }

  private void InitByHandle(IntPtr handle)
  {
    if (this._handle != IntPtr.Zero)
      return;
    if (this.Document.AvailabilityProvider != null && !this.Document.AvailabilityProvider.IsPageAvailable(this._pageIndex))
      throw new NotAvailableException(Error.err0011);
    this._handle = handle != IntPtr.Zero ? handle : Pdfium.FPDF_LoadPage(this.Document.Handle, this._pageIndex);
    if (this._handle == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
    this.IsDisposed = false;
    this.IsParsed = true;
    this.OriginalRotation = this.Rotation;
    if (this.Document.FormFill != null)
      Pdfium.FORM_OnAfterLoadPage(this._handle, this.Document.FormFill.Handle);
    this.DoPageAAction(PageActionTypes.FPDFPAGE_AACTION_OPEN);
    this.OnLoaded();
  }

  /// <summary>
  /// Gets page width (excluding non-displayable area) measured in points.One point is 1/72 inch (around 0.3528 mm)
  /// </summary>
  public float Width => (float) Pdfium.FPDF_GetPageWidth(this.Handle);

  /// <summary>
  /// Gets page height (excluding non-displayable area) measured in points.One point is 1/72 inch (around 0.3528 mm)
  /// </summary>
  public float Height => (float) Pdfium.FPDF_GetPageHeight(this.Handle);

  /// <summary>Gets or sets MediaBox of the page.</summary>
  /// <remarks>The media box defines the boundaries of the physical medium on which the
  /// page is to be printed. It may include any extended area surrounding the
  /// finished page for bleed, printing marks, or other such purposes. It may also
  /// include areas close to the edges of the medium that cannot be marked because
  /// of physical limitations of the output device. Content falling outside this boundary
  /// can safely be discarded without affecting the meaning of the PDF file.
  /// See <a href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference 1.7 SECTION 10.10 Prepress Support 963</a> for more details.
  /// </remarks>
  /// <seealso href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference 1.7 SECTION 10.10 Prepress Support 963</seealso>
  public FS_RECTF MediaBox
  {
    get
    {
      float left;
      float bottom;
      float right;
      float top;
      if (!Pdfium.FPDFPage_GetMediaBox(this.Handle, out left, out bottom, out right, out top))
        throw Pdfium.ProcessLastError();
      return new FS_RECTF()
      {
        left = left,
        bottom = bottom,
        right = right,
        top = top
      };
    }
    set
    {
      Pdfium.FPDFPage_SetMediaBox(this.Handle, value.left, value.bottom, value.right, value.top);
    }
  }

  /// <summary>Gets or sets CropBox of the page.</summary>
  /// <remarks>The crop box defines the region to which the contents of the page are to be
  /// clipped (cropped) when displayed or printed. Unlike the other boxes, the crop
  /// box has no defined meaning in terms of physical page geometry or intended
  /// use; it merely imposes clipping on the page contents. However, in the absence
  /// of additional information (such as imposition instructions specified in a JDF or
  /// PJTF job ticket), the crop box determines how the page’s contents are to be positioned
  /// on the output medium. The default value is the page’s <see cref="P:Patagames.Pdf.Net.PdfPage.MediaBox" />.
  /// See <a href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF reference 1.7.pdf</a> for more details
  /// </remarks>
  /// <seealso href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference 1.7</seealso>
  public FS_RECTF CropBox
  {
    get
    {
      float left;
      float bottom;
      float right;
      float top;
      if (!Pdfium.FPDFPage_GetCropBox(this.Handle, out left, out bottom, out right, out top))
        throw new EntryNotFoundException(nameof (CropBox));
      return new FS_RECTF()
      {
        left = left,
        bottom = bottom,
        right = right,
        top = top
      };
    }
    set
    {
      Pdfium.FPDFPage_SetCropBox(this.Handle, value.left, value.bottom, value.right, value.top);
    }
  }

  /// <summary>Gets or sets BleedBox of the page.</summary>
  /// <remarks> A rectangle, expressed in default user space units, defining the region to which the contents of the page should be clipped
  /// when output in a production environment. Default value: the value of <see cref="P:Patagames.Pdf.Net.PdfPage.CropBox" />.
  /// </remarks>
  public FS_RECTF BleedBox
  {
    get
    {
      float left;
      float bottom;
      float right;
      float top;
      if (!Pdfium.FPDFPage_GetBleedBox(this.Handle, out left, out bottom, out right, out top))
        throw new EntryNotFoundException(nameof (BleedBox));
      return new FS_RECTF()
      {
        left = left,
        bottom = bottom,
        right = right,
        top = top
      };
    }
    set
    {
      Pdfium.FPDFPage_SetBleedBox(this.Handle, value.left, value.bottom, value.right, value.top);
    }
  }

  /// <summary>Gets or sets TrimBox of the page.</summary>
  /// <remarks> A rectangle, expressed in default user space units, defining the intended dimensions of the finished page after trimming.
  /// Default value: the value of <see cref="P:Patagames.Pdf.Net.PdfPage.CropBox" />.
  /// </remarks>
  public FS_RECTF TrimBox
  {
    get
    {
      float left;
      float bottom;
      float right;
      float top;
      if (!Pdfium.FPDFPage_GetTrimBox(this.Handle, out left, out bottom, out right, out top))
        throw new EntryNotFoundException(nameof (TrimBox));
      return new FS_RECTF()
      {
        left = left,
        bottom = bottom,
        right = right,
        top = top
      };
    }
    set
    {
      Pdfium.FPDFPage_SetTrimBox(this.Handle, value.left, value.bottom, value.right, value.top);
    }
  }

  /// <summary>Gets or sets ArtBox of the page.</summary>
  /// <remarks> A rectangle, expressed in default user space units, defining the extent of the page’s meaningful content(including potential white space) as intended by the page’s creator.
  /// Default value: the value of <see cref="P:Patagames.Pdf.Net.PdfPage.CropBox" />.
  /// </remarks>
  public FS_RECTF ArtBox
  {
    get
    {
      float left;
      float bottom;
      float right;
      float top;
      if (!Pdfium.FPDFPage_GetArtBox(this.Handle, out left, out bottom, out right, out top))
        throw new EntryNotFoundException(nameof (ArtBox));
      return new FS_RECTF()
      {
        left = left,
        bottom = bottom,
        right = right,
        top = top
      };
    }
    set => Pdfium.FPDFPage_SetArtBox(this.Handle, value.left, value.bottom, value.right, value.top);
  }

  /// <summary>
  /// Gets the rotation angle of the page in a saved document.
  /// </summary>
  public PageRotate OriginalRotation { get; private set; }

  /// <summary>Gets or sets the page rotation.</summary>
  /// <remarks>The PDF page rotates clockwise. See <see cref="T:Patagames.Pdf.Enums.PageRotate" /> for details.</remarks>
  public PageRotate Rotation
  {
    get => Pdfium.FPDFPage_GetRotation(this.Handle);
    set => Pdfium.FPDFPage_SetRotation(this.Handle, value);
  }

  /// <summary>
  /// Gets a value that represents that whether the content of the current PDF page contains transparency.
  /// </summary>
  public bool IsTransparency => Pdfium.FPDFPage_HasTransparency(this.Handle);

  /// <summary>
  /// Gets a value that represents that whether the content of the current PDF page is available.
  /// </summary>
  public bool IsAvailable
  {
    get
    {
      if (this.Document == null)
        return false;
      if (this.Document.AvailabilityProvider == null)
        return true;
      return this._pageIndex >= 0 && this.Document.AvailabilityProvider.IsPageAvailable(this._pageIndex);
    }
  }

  /// <summary>
  /// Gets or sets the additional-actions defining actions to be performed when the page is opened or closed.
  /// </summary>
  public PdfPageLevelActions AdditionalActions
  {
    get
    {
      if (!this.Dictionary.ContainsKey("AA"))
      {
        this._aactions = (PdfPageLevelActions) null;
        return (PdfPageLevelActions) null;
      }
      if ((PdfWrapper) this._aactions == (PdfWrapper) null || this.Dictionary["AA"].Is<PdfTypeDictionary>() && this._aactions.Dictionary.Handle != this.Dictionary["AA"].As<PdfTypeDictionary>().Handle)
        this._aactions = new PdfPageLevelActions(this.Document, (PdfTypeBase) this.Dictionary["AA"].As<PdfTypeDictionary>());
      return this._aactions;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey("AA"))
        this.Dictionary.Remove("AA");
      else if ((PdfWrapper) value != (PdfWrapper) null)
      {
        if (value.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(value.Dictionary.Handle) != IntPtr.Zero)
          throw new ArgumentException(string.Format(Error.err0067, (object) "additional actions", (object) "object"));
        PdfIndirectList list = PdfIndirectList.FromPdfDocument(this.Document);
        list.Add((PdfTypeBase) value.Dictionary);
        this.Dictionary.SetIndirectAt("AA", list, (PdfTypeBase) value.Dictionary);
      }
      this._aactions = value;
    }
  }

  /// <summary>construct PdfPage object. Do not load actualy it.</summary>
  /// <param name="document">Document containing this page</param>
  /// <param name="page_index">Page index inside document</param>
  internal PdfPage(PdfDocument document, int page_index)
  {
    this.Document = document;
    this._pageIndex = page_index;
  }

  /// <summary>
  /// Creates new instance of PdfPage class from page's hadle.
  /// </summary>
  /// <param name="doc">PdfDocument what contains this page.</param>
  /// <param name="handle">Handle to page.</param>
  /// <param name="pageIndex">Page index in a given document.</param>
  /// <param name="isParsed">A value indicating whether the page content has been parsed (true) or not (false).</param>
  /// <returns>New instance of PdfPage class</returns>
  /// <remarks>When the instance of PdfPage class created by this method will be disposed the page will not actually unloaded.
  /// You should call the <see cref="M:Patagames.Pdf.Pdfium.FPDF_ClosePage(System.IntPtr)" /> method to unload page from the memory.</remarks>
  public static PdfPage FromHandle(PdfDocument doc, IntPtr handle, int pageIndex, bool isParsed = true)
  {
    PdfPage pdfPage = new PdfPage(doc, pageIndex)
    {
      _handle = handle
    };
    pdfPage.OriginalRotation = pdfPage.Rotation;
    pdfPage._fromHandle = true;
    pdfPage.IsParsed = isParsed;
    return pdfPage;
  }

  /// <summary>Dispose PDFPage object.</summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>Releases all resources used by the PdfPage object.</summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !PdfCommon.IsInitialize)
      return;
    if (!this._fromHandle && this._handle != IntPtr.Zero && this.Document.FormFill != null)
      Pdfium.FORM_DoPageAAction(this._handle, this.Document.FormFill.Handle, PageActionTypes.FPDFPAGE_AACTION_CLOSE);
    if (!this._fromHandle && this._handle != IntPtr.Zero && this.Document.FormFill != null)
      Pdfium.FORM_OnBeforeClosePage(this._handle, this.Document.FormFill.Handle);
    if (this._text != null)
      this._text.Dispose();
    this._text = (PdfText) null;
    this._links = (PdfLinkCollections) null;
    if (this._pageObjects != null)
      this._pageObjects.Dispose();
    this._pageObjects = (PdfPageObjectsCollection) null;
    if (this._annots != null)
      this._annots.Dispose();
    this._annots = (PdfAnnotationCollection) null;
    if (this._progressiveStatus != ProgressiveStatus.Ready)
      this.CancelProgressiveRender();
    if (this._pause != null)
      this._pause.Dispose();
    this._pause = (IFSDK_PAUSE) null;
    if (!this._fromHandle && this._handle != IntPtr.Zero)
    {
      Pdfium.FPDF_ClosePage(this._handle);
      this._handle = IntPtr.Zero;
      this.IsParsed = false;
    }
    if (this._dictionary != null)
      this._dictionary.Dispose();
    this._dictionary = (PdfTypeDictionary) null;
    this.IsDisposed = true;
    if (disposing)
      GC.SuppressFinalize((object) this);
    this.OnDisposed();
  }

  /// <summary>Finalize object</summary>
  ~PdfPage()
  {
    if (!this._fromHandle && PdfCommon.IsCheckForMemoryLeaks && this.IsLoaded && !this.IsDisposed)
      throw new MemoryLeakException(nameof (PdfPage));
  }

  /// <summary>
  /// Render contents in a page to the device context specified by a coordinate pair, a width, and a height.
  /// </summary>
  /// <param name="hdc">Device context</param>
  /// <param name="x">Left pixel position of the display area in the device coordinate</param>
  /// <param name="y">Top pixel position of the display area in the device coordinate.</param>
  /// <param name="width">Horizontal size (in pixels) for displaying the page</param>
  /// <param name="height">Vertical size (in pixels) for displaying the page.</param>
  /// <param name="rotate">Page orientation. <see cref="T:Patagames.Pdf.Enums.PageRotate" /> for details.</param>
  /// <param name="flags">RenderFlags.None for normal display, or combination of <see cref="T:Patagames.Pdf.Enums.RenderFlags" /></param>
  public void Render(
    IntPtr hdc,
    int x,
    int y,
    int width,
    int height,
    PageRotate rotate,
    RenderFlags flags)
  {
    Pdfium.FPDF_RenderPage(hdc, this.Handle, x, y, width, height, rotate, flags);
  }

  /// <summary>
  /// Start to render page contents to a device independent bitmap progressively specified by a coordinate pair, a width, and a height.
  /// </summary>
  /// <param name="bitmap">Instance of <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> class.</param>
  /// <param name="x">Left pixel position of the display area in the bitmap coordinate.</param>
  /// <param name="y">Top pixel position of the display area in the bitmap coordinate.</param>
  /// <param name="width">Horizontal size (in pixels) for displaying the page.</param>
  /// <param name="height">Vertical size (in pixels) for displaying the page.</param>
  /// <param name="rotate">Page orientation: 0 (normal), 1 (rotated 90 degrees clockwise), 2 (rotated 180 degrees), 3 (rotated 90 degrees counter-clockwise).</param>
  /// <param name="flags">0 for normal display, or combination of flags defined above.</param>
  /// <param name="userData">A user defined data pointer, used by user's application. Can be NULL.</param>
  /// <returns>Rendering Status. See <see cref="T:Patagames.Pdf.Enums.ProgressiveRenderingStatuses" /> for the details.</returns>
  public ProgressiveStatus StartProgressiveRender(
    PdfBitmap bitmap,
    int x,
    int y,
    int width,
    int height,
    PageRotate rotate,
    RenderFlags flags,
    byte[] userData)
  {
    if (!this.IsParsed)
      return ProgressiveStatus.Failed;
    if (this._pause != null)
      this._pause.Dispose();
    this._pause = new IFSDK_PAUSE(userData);
    this._pause.needToPauseNowCallback = new Patagames.Pdf.NeedToPauseNowCallback(this.NeedToPauseNowCallback);
    this._progressiveStatus = Pdfium.FPDF_RenderPageBitmap_Start(bitmap.Handle, this.Handle, x, y, width, height, rotate, flags, this._pause);
    return this._progressiveStatus;
  }

  /// <summary>Continue rendering a PDF page.</summary>
  /// <returns>The rendering status. See <see cref="T:Patagames.Pdf.Enums.ProgressiveRenderingStatuses" /> for the details.</returns>
  public ProgressiveStatus ContinueProgressiveRender()
  {
    this._progressiveStatus = Pdfium.FPDF_RenderPage_Continue(this.Handle, this._pause);
    return this._progressiveStatus;
  }

  /// <summary>
  /// Release the resource allocate during page rendering. Need to be called after finishing rendering or cancel the rendering.
  /// </summary>
  public void CancelProgressiveRender()
  {
    Pdfium.FPDF_RenderPage_Close(this.Handle);
    this._progressiveStatus = ProgressiveStatus.Ready;
    if (this._pause != null)
      this._pause.Dispose();
    this._pause = (IFSDK_PAUSE) null;
  }

  /// <summary>
  /// Render contents in a page to a device-independent bitmap specified by a coordinate pair, a width, and a height.
  /// </summary>
  /// <param name="bitmap">Instance of <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> class.</param>
  /// <param name="x">Left pixel position of the display area in the device coordinate</param>
  /// <param name="y">Top pixel position of the display area in the device coordinate</param>
  /// <param name="width">Horizontal size (in pixels) for displaying the page.</param>
  /// <param name="height">Vertical size (in pixels) for displaying the page.</param>
  /// <param name="rotate">Page orientation. See <see cref="T:Patagames.Pdf.Enums.PageRotate" /> for details.</param>
  /// <param name="flags">RenderFlags.None for normal display, or combination of <see cref="T:Patagames.Pdf.Enums.RenderFlags" /></param>
  public void Render(
    PdfBitmap bitmap,
    int x,
    int y,
    int width,
    int height,
    PageRotate rotate,
    RenderFlags flags)
  {
    Pdfium.FPDF_RenderPageBitmap(bitmap.Handle, this.Handle, x, y, width, height, rotate, flags);
  }

  /// <summary>
  /// Render contents in a page to a device-independent bitmap specified by a coordinate pair, a width, and a height.
  /// </summary>
  /// <param name="bitmap">Instance of <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> class.</param>
  /// <param name="x">Left pixel position of the display area in the device coordinate</param>
  /// <param name="y">Top pixel position of the display area in the device coordinate</param>
  /// <param name="width">Horizontal size (in pixels) for displaying the page.</param>
  /// <param name="height">Vertical size (in pixels) for displaying the page.</param>
  /// <param name="rotate">Page orientation. See <see cref="T:Patagames.Pdf.Enums.PageRotate" /> for details.</param>
  /// <param name="flags">RenderFlags.None for normal display, or combination of <see cref="T:Patagames.Pdf.Enums.RenderFlags" /></param>
  public void RenderEx(
    PdfBitmap bitmap,
    int x,
    int y,
    int width,
    int height,
    PageRotate rotate,
    RenderFlags flags)
  {
    Pdfium.FPDF_RenderPageBitmap(bitmap.Handle, this.Handle, x, y, width, height, rotate, flags);
  }

  /// <summary>
  /// Render FormFeilds on a page to a device independent bitmap.
  /// </summary>
  /// <param name="bitmap">Instance of <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> class (as the output buffer).</param>
  /// <param name="x">Left pixel position of the display area in the device coordinate.</param>
  /// <param name="y">Top pixel position of the display area in the device coordinate.</param>
  /// <param name="width">Horizontal size (in pixels) for displaying the page.</param>
  /// <param name="height">Vertical size (in pixels) for displaying the page.</param>
  /// <param name="rotate">Page orientation: 0 (normal), 1 (rotated 90 degrees clockwise), 2 (rotated 180 degrees), 3 (rotated 90 degrees counter-clockwise).</param>
  /// <param name="flags">0 for normal display, or combination of flags defined above.</param>
  /// <remarks>
  /// This method is designed to only render annotations and FormFields on the page.
  /// Without FPDF_ANNOT specified for flags, Rendering functions such as <see cref="M:Patagames.Pdf.Net.PdfPage.Render(Patagames.Pdf.Net.PdfBitmap,System.Int32,System.Int32,System.Int32,System.Int32,Patagames.Pdf.Enums.PageRotate,Patagames.Pdf.Enums.RenderFlags)" /> or <see cref="M:Patagames.Pdf.Net.PdfPage.StartProgressiveRender(Patagames.Pdf.Net.PdfBitmap,System.Int32,System.Int32,System.Int32,System.Int32,Patagames.Pdf.Enums.PageRotate,Patagames.Pdf.Enums.RenderFlags,System.Byte[])" /> will only render page contents(without annotations) to a bitmap.
  /// In order to implement the FormFill functions,Implementation should call this method after rendering functions finish rendering the page contents.
  /// </remarks>
  public void RenderForms(
    PdfBitmap bitmap,
    int x,
    int y,
    int width,
    int height,
    PageRotate rotate,
    RenderFlags flags)
  {
    if (this.Document.FormFill == null)
      return;
    Pdfium.FPDF_FFLDraw(this.Document.FormFill.Handle, bitmap.Handle, this.Handle, x, y, width, height, rotate, flags);
  }

  /// <summary>Creates an annotation collection for the page.</summary>
  /// <remarks>
  /// <para>To access created annotatios the <see cref="P:Patagames.Pdf.Net.PdfPage.Annots" /> property is used.</para>
  /// <para>This method does nothing if the page already contains annotations.</para>
  /// </remarks>
  public void CreateAnnotations() => this._annots = PdfAnnotationCollection.Create(this);

  /// <summary>
  /// Flat a pdf page, annotations or form fields will become part of the page contents.
  /// </summary>
  /// <param name="flag">the flag for the use of flatten result. FlattenFlags.NormalDisplay for normal display, FlattenFlags.FlatPrint for print.</param>
  /// <returns>The result flag of the function, See flags <see cref="T:Patagames.Pdf.Enums.FlattenResults" />.</returns>
  /// <remarks>Current Version on all fails return zero.</remarks>
  public FlattenResults FlattenPage(FlattenFlags flag)
  {
    return Pdfium.FPDFPage_Flatten(this.Handle, flag);
  }

  /// <summary>
  /// Transform the whole page with a specified matrix, then clip the page content region.
  /// </summary>
  /// <param name="matrix">The transform matrix.</param>
  /// <param name="clipRect">A rectangle page area to be clipped.</param>
  /// <returns>True if success,else false.</returns>
  /// <remarks>This function will transform the whole page, and would take effect to all the objects in the page.</remarks>
  public bool TransformWithClip(FS_MATRIX matrix, FS_RECTF clipRect)
  {
    return Pdfium.FPDFPage_TransFormWithClip(this.Handle, matrix, clipRect);
  }

  /// <summary>
  /// Clip the page content, the page content that outside the clipping region become invisible.
  /// </summary>
  /// <param name="clipPath">A instance of <see cref="T:Patagames.Pdf.Net.PdfClipPath" /> class.</param>
  /// <remarks>A clip path will be inserted before the page content stream or content array.
  /// In this way, the page content will be clipped by this clip path.</remarks>
  public void InsertClipPath(PdfClipPath clipPath)
  {
    Pdfium.FPDFPage_InsertClipPath(this.Handle, clipPath.Handle);
  }

  /// <summary>Generate PDF Page content.</summary>
  /// <param name="imagesOnly">If true images only will be generated.</param>
  /// <returns>True if successful, false otherwise.</returns>
  /// <remarks>Before you save the page to a file, or reload the page, you must call the GenerateContent function. Or the changed information will be lost.
  /// <note type="note">This feature needs a full license. However, if you set the imagesOnly flag to true, then the LITE license will also be suitable.</note>
  /// </remarks>
  public bool GenerateContent(bool imagesOnly = false)
  {
    return imagesOnly ? Pdfium.FPDFPage_GenerateContent(this.Handle) : Pdfium.FPDFPage_GenerateContentEx(this.Handle);
  }

  /// <summary>
  /// Transform (scale, rotate, shear, move) all annots in a page.
  /// </summary>
  /// <param name="matrix">The transform matrix</param>
  public void TransformAnnots(FS_MATRIX matrix)
  {
    Pdfium.FPDFPage_TransformAnnots(this.Handle, (double) matrix.a, (double) matrix.b, (double) matrix.c, (double) matrix.d, (double) matrix.e, (double) matrix.f);
  }

  /// <summary>
  /// Convert the screen coordinate of a point to page coordinate.
  /// </summary>
  /// <param name="x">Left pixel position of the display area in the device coordinate</param>
  /// <param name="y">Top pixel position of the display area in the device coordinate</param>
  /// <param name="width">Horizontal size (in pixels) for displaying the page</param>
  /// <param name="height">Vertical size (in pixels) for displaying the page</param>
  /// <param name="rotate">Page orientation: 0 (normal), 1 (rotated 90 degrees clockwise),2 (rotated 180 degrees), 3 (rotated 90 degrees counter-clockwise).</param>
  /// <param name="deviceX">X value in device coordinate, for the point to be converted</param>
  /// <param name="deviceY">Y value in device coordinate, for the point to be converted</param>
  /// <returns><see cref="T:Patagames.Pdf.FS_POINTF" /> structure that represents the point in page coordinate</returns>
  /// <remarks>The page coordinate system has its origin at left-bottom corner of the page, with X axis goes
  /// along the bottom side to the right, and Y axis goes along the left side upward. NOTE: this
  /// coordinate system can be altered when you zoom, scroll, or rotate a page, however, a point on
  /// the page should always have the same coordinate values in the page coordinate system.
  /// The device coordinate system is device dependant. For screen device, its origin is at left-top
  /// corner of the window. However this origin can be altered by Windows coordinate
  /// transformation utilities. You must make sure the x, y, width, height and rotate
  /// parameters have exactlysame values as you used in <see cref="O:Patagames.Pdf.Net.PdfPage.Render" /> methods call.
  /// </remarks>
  public FS_POINTF DeviceToPage(
    int x,
    int y,
    int width,
    int height,
    PageRotate rotate,
    int deviceX,
    int deviceY)
  {
    double page_x;
    double page_y;
    Pdfium.FPDF_DeviceToPage(this.Handle, x, y, width, height, rotate, deviceX, deviceY, out page_x, out page_y);
    return new FS_POINTF(page_x, page_y);
  }

  /// <summary>
  /// Convert the screen coordinate of a point to page coordinate.
  /// </summary>
  /// <param name="x">Left pixel position of the display area in the device coordinate</param>
  /// <param name="y">Top pixel position of the display area in the device coordinate</param>
  /// <param name="width">Horizontal size (in pixels) for displaying the page</param>
  /// <param name="height">Vertical size (in pixels) for displaying the page</param>
  /// <param name="rotate">Page orientation: 0 (normal), 1 (rotated 90 degrees clockwise),2 (rotated 180 degrees), 3 (rotated 90 degrees counter-clockwise).</param>
  /// <param name="deviceX">X value in device coordinate, for the point to be converted</param>
  /// <param name="deviceY">Y value in device coordinate, for the point to be converted</param>
  /// <param name="pageX">The X value of the point in page coordinate</param>
  /// <param name="pageY">The Y value of the point in page coordinate</param>
  /// <remarks>The page coordinate system has its origin at left-bottom corner of the page, with X axis goes
  /// along the bottom side to the right, and Y axis goes along the left side upward. NOTE: this
  /// coordinate system can be altered when you zoom, scroll, or rotate a page, however, a point on
  /// the page should always have the same coordinate values in the page coordinate system.
  /// The device coordinate system is device dependant. For screen device, its origin is at left-top
  /// corner of the window. However this origin can be altered by Windows coordinate
  /// transformation utilities. You must make sure the x, y, width, height and rotate
  /// parameters have exactlysame values as you used in <see cref="O:Patagames.Pdf.Net.PdfPage.Render" /> methods call.
  /// </remarks>
  public void DeviceToPage(
    int x,
    int y,
    int width,
    int height,
    PageRotate rotate,
    int deviceX,
    int deviceY,
    out double pageX,
    out double pageY)
  {
    Pdfium.FPDF_DeviceToPage(this.Handle, x, y, width, height, rotate, deviceX, deviceY, out pageX, out pageY);
  }

  /// <summary>
  /// Convert the page coordinate of a point to screen coordinate
  /// </summary>
  /// <param name="x">Left pixel position of the display area in the device coordinate</param>
  /// <param name="y">Top pixel position of the display area in the device coordinate</param>
  /// <param name="width">Horizontal size (in pixels) for displaying the page</param>
  /// <param name="height">Vertical size (in pixels) for displaying the page</param>
  /// <param name="rotate">Page orientation: 0 (normal), 1 (rotated 90 degrees clockwise),2 (rotated 180 degrees), 3 (rotated 90 degrees counter-clockwise).</param>
  /// <param name="pageX">X value in page coordinate, for the point to be converted</param>
  /// <param name="pageY">Y value in page coordinate, for the point to be converted</param>
  /// <param name="deviceX">The X value of the point in device coordinate</param>
  /// <param name="deviceY">The Y value of the point in device coordinate</param>
  /// <remarks>See remarks of <see cref="M:Patagames.Pdf.Net.PdfPage.DeviceToPage(System.Int32,System.Int32,System.Int32,System.Int32,Patagames.Pdf.Enums.PageRotate,System.Int32,System.Int32)" /> function</remarks>
  public void PageToDevice(
    int x,
    int y,
    int width,
    int height,
    PageRotate rotate,
    float pageX,
    float pageY,
    out int deviceX,
    out int deviceY)
  {
    Pdfium.FPDF_PageToDevice(this.Handle, x, y, width, height, rotate, (double) pageX, (double) pageY, out deviceX, out deviceY);
  }

  /// <summary>
  /// This method is required for performing the page object's additional-action when opened or closed.
  /// </summary>
  /// <param name="aaType">The type of the page object's additional-actions. See <see cref="T:Patagames.Pdf.Enums.PageActionTypes" /> for details.</param>
  public void DoPageAAction(PageActionTypes aaType)
  {
    if (this.Document.FormFill == null)
      return;
    Pdfium.FORM_DoPageAAction(this.Handle, this.Document.FormFill.Handle, aaType);
  }

  /// <summary>
  /// You can call this member function when the mouse cursor moves.
  /// </summary>
  /// <param name="modifier">Indicates whether various virtual keys are down. </param>
  /// <param name="x">Specifies the x-coordinate of the cursor in PDF user space.</param>
  /// <param name="y">Specifies the y-coordinate of the cursor in PDF user space.</param>
  /// <returns>TRUE indicates success; otherwise false.</returns>
  public bool OnMouseMove(int modifier, float x, float y)
  {
    return this.Document.FormFill != null && Pdfium.FORM_OnMouseMove(this.Document.FormFill.Handle, this.Handle, modifier, (double) x, (double) y);
  }

  /// <summary>
  /// You can call this member function when the user presses the left mouse button.
  /// </summary>
  /// <param name="modifier">Indicates whether various virtual keys are down. </param>
  /// <param name="x">Specifies the x-coordinate of the cursor in PDF user space.</param>
  /// <param name="y">Specifies the y-coordinate of the cursor in PDF user space.</param>
  /// <returns>TRUE indicates success; otherwise false.</returns>
  public bool OnLButtonDown(int modifier, float x, float y)
  {
    return this.Document.FormFill != null && Pdfium.FORM_OnLButtonDown(this.Document.FormFill.Handle, this.Handle, modifier, (double) x, (double) y);
  }

  /// <summary>
  /// You can call this member function when the user releases the left mouse button.
  /// </summary>
  /// <param name="modifier">Indicates whether various virtual keys are down. </param>
  /// <param name="x">Specifies the x-coordinate of the cursor in PDF user space.</param>
  /// <param name="y">Specifies the y-coordinate of the cursor in PDF user space.</param>
  /// <returns>TRUE indicates success; otherwise false.</returns>
  public bool OnLButtonUp(int modifier, float x, float y)
  {
    return this.Document.FormFill != null && Pdfium.FORM_OnLButtonUp(this.Document.FormFill.Handle, this.Handle, modifier, (double) x, (double) y);
  }

  /// <summary>
  /// You can call this member function when a key is pressed.
  /// </summary>
  /// <param name="keyCode">Indicates whether various virtual keys are down. </param>
  /// <param name="modifier">Contains the scan code, key-transition code, previous key state, and context code.</param>
  /// <returns>TRUE indicates success; otherwise false.</returns>
  public bool OnKeyDown(FWL_VKEYCODE keyCode, KeyboardModifiers modifier)
  {
    if (this.Document.FormFill == null)
      return false;
    char unicodeCharacter = PdfPage.GetUnicodeCharacter((int) keyCode);
    if (unicodeCharacter == char.MinValue)
      return Pdfium.FORM_OnKeyDown(this.Document.FormFill.Handle, this.Handle, keyCode, modifier);
    Pdfium.FORM_OnKeyDown(this.Document.FormFill.Handle, this.Handle, keyCode, modifier);
    return Pdfium.FORM_OnChar(this.Document.FormFill.Handle, this.Handle, (int) unicodeCharacter, modifier);
  }

  /// <summary>
  /// You can call this member function when a key is released.
  /// </summary>
  /// <param name="keyCode">Indicates whether various virtual keys are up. </param>
  /// <param name="modifier">Contains the scan code, key-transition code, previous key state, and context code.</param>
  /// <returns>TRUE indicates success; otherwise false.</returns>
  public bool OnKeyUp(FWL_VKEYCODE keyCode, KeyboardModifiers modifier)
  {
    return this.Document.FormFill != null && Pdfium.FORM_OnKeyUp(this.Document.FormFill.Handle, this.Handle, keyCode, modifier);
  }

  /// <summary>Check the form field position by point.</summary>
  /// <param name="x">X position in PDF "user space".</param>
  /// <param name="y">Y position in PDF "user space".</param>
  /// <returns>Return the type of the formfiled; -1 indicates no fields.</returns>
  public FormFieldTypes GetFormFieldAtPoint(float x, float y)
  {
    if (this.Document.FormFill == null)
      throw new Exception(Error.err0007);
    return Pdfium.FPDPage_HasFormFieldAtPoint(this.Document.FormFill.Handle, this.Handle, (double) x, (double) y);
  }

  /// <summary>Get the field's widget at specified position.</summary>
  /// <param name="x">X position in PDF "user space".</param>
  /// <param name="y">Y position in PDF "user space".</param>
  /// <param name="zOrder">The z order of found widget</param>
  /// <returns>Return <see cref="T:Patagames.Pdf.Net.PdfControl" /> object; null indicates no widget.</returns>
  public PdfControl GetControlAtPoint(float x, float y, out int zOrder)
  {
    if (this.Document.FormFill == null)
      throw new Exception(Error.err0007);
    return this.Document.FormFill.InterForm.Controls.GetByHandle(Pdfium.FPDFInterForm_GetControlAtPoint(this.Document.FormFill.InterForm.Handle, this.Handle, x, y, out zOrder));
  }

  private bool NeedToPauseNowCallback(IFSDK_PAUSE pThis)
  {
    ProgressiveRenderEventArgs e = new ProgressiveRenderEventArgs(pThis.userData);
    this.OnProgressiveRender(e);
    return e.NeedPause;
  }

  private static char GetAsciiCharacter(int uVirtKey)
  {
    byte[] numArray = new byte[(int) byte.MaxValue];
    Platform.GetKeyboardState(numArray);
    byte[] lpChar = new byte[1];
    return Platform.ToAscii(uVirtKey, 0, numArray, lpChar, 0) == 1 ? Convert.ToChar(lpChar[0]) : char.MinValue;
  }

  private static char GetUnicodeCharacter(int uVirtKey)
  {
    byte[] numArray1 = new byte[(int) byte.MaxValue];
    Platform.GetKeyboardState(numArray1);
    byte[] numArray2 = new byte[2];
    return Platform.ToUnicode(uVirtKey, 0, numArray1, numArray2, 2, 0) == 1 ? Encoding.Unicode.GetChars(numArray2, 0, 2)[0] : char.MinValue;
  }

  /// <summary>Start to load a page inside the document.</summary>
  /// <remarks>The page can be continue load using <see cref="M:Patagames.Pdf.Net.PdfPage.ContinueProgressiveLoad" /> method.</remarks>
  public void StartProgressiveLoad()
  {
    if (this._handle != IntPtr.Zero)
      return;
    if (this.Document.AvailabilityProvider != null && !this.Document.AvailabilityProvider.IsPageAvailable(this._pageIndex))
      throw new NotAvailableException(Error.err0011);
    this._handle = Pdfium.FPDF_StartLoadPage(this.Document.Handle, this._pageIndex);
    if (this._handle == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
    this.IsDisposed = false;
    this.IsParsed = false;
    this.OriginalRotation = this.Rotation;
    if (this.Document.FormFill != null)
      Pdfium.FORM_OnAfterLoadPage(this._handle, this.Document.FormFill.Handle);
    this.DoPageAAction(PageActionTypes.FPDFPAGE_AACTION_OPEN);
    this.OnLoaded();
    if (this._pause != null)
      this._pause.Dispose();
    this._pause = new IFSDK_PAUSE();
    this._pause.needToPauseNowCallback = new Patagames.Pdf.NeedToPauseNowCallback(this.NeedToPauseNowCallback);
  }

  /// <summary>Continue loading a PDF page.</summary>
  /// <returns>The loading status.</returns>
  public ProgressiveStatus ContinueProgressiveLoad()
  {
    if (this._handle == IntPtr.Zero)
      return ProgressiveStatus.Failed;
    int num = (int) Pdfium.FPDF_ContinueLoadPage(this._handle, this._pause);
    if (num != 2)
      return (ProgressiveStatus) num;
    this.IsParsed = true;
    if (this._pause != null)
      this._pause.Dispose();
    this._pause = (IFSDK_PAUSE) null;
    return (ProgressiveStatus) num;
  }

  /// <summary>Dispose and reload this page.</summary>
  public void ReloadPage()
  {
    this.Dispose();
    IntPtr handle = this.Handle;
  }

  /// <summary>
  /// Render contents in a page to a drawing surface specified by a coordinate pair, a width, and a height.
  /// </summary>
  /// <param name="graphics">GDI+ drawing surface</param>
  /// <param name="x">Left pixel position of the display area in the device coordinate</param>
  /// <param name="y">Top pixel position of the display area in the device coordinate.</param>
  /// <param name="width">Horizontal size (in pixels) for displaying the page</param>
  /// <param name="height">Vertical size (in pixels) for displaying the page.</param>
  /// <param name="rotate">Page orientation. <see cref="T:Patagames.Pdf.Enums.PageRotate" /> for details.</param>
  /// <param name="flags">RenderFlags.None for normal display, or combination of <see cref="T:Patagames.Pdf.Enums.RenderFlags" /></param>
  public void Render(
    Graphics graphics,
    int x,
    int y,
    int width,
    int height,
    PageRotate rotate,
    RenderFlags flags)
  {
    IntPtr hdc = IntPtr.Zero;
    try
    {
      hdc = graphics.GetHdc();
      this.Render(hdc, x, y, width, height, rotate, flags);
    }
    finally
    {
      if (hdc != IntPtr.Zero)
        graphics.ReleaseHdc(hdc);
    }
  }

  /// <summary>
  /// Render contents in a page to a drawing surface specified by a location and size.
  /// </summary>
  /// <param name="graphics">GDI+ drawing surface</param>
  /// <param name="location">A <see cref="T:System.Drawing.Point" /> structure that represents the Top-Left corner of the display area in the device coordinate.</param>
  /// <param name="size">A <see cref="T:System.Drawing.Size" /> structure that represents the horizontal and vertical size (in pixels) for displaying the page.</param>
  /// <param name="rotate">Page orientation. <see cref="T:Patagames.Pdf.Enums.PageRotate" /> for details.</param>
  /// <param name="flags">RenderFlags.None for normal display, or combination of <see cref="T:Patagames.Pdf.Enums.RenderFlags" /></param>
  public void Render(
    Graphics graphics,
    Point location,
    Size size,
    PageRotate rotate,
    RenderFlags flags)
  {
    this.Render(graphics, location.X, location.Y, size.Width, size.Height, rotate, flags);
  }

  /// <summary>
  /// Render contents in a page to a drawing surface specified by a <see cref="T:System.Drawing.Rectangle" /> structure.
  /// </summary>
  /// <param name="graphics">GDI+ drawing surface</param>
  /// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle in the device coordinate. </param>
  /// <param name="rotate">Page orientation. <see cref="T:Patagames.Pdf.Enums.PageRotate" /> for details.</param>
  /// <param name="flags">RenderFlags.None for normal display, or combination of <see cref="T:Patagames.Pdf.Enums.RenderFlags" /></param>
  public void Render(Graphics graphics, Rectangle rect, PageRotate rotate, RenderFlags flags)
  {
    this.Render(graphics, rect.X, rect.Y, rect.Width, rect.Height, rotate, flags);
  }

  /// <summary>
  /// Render contents in a page to the device context specified by a location and size.
  /// </summary>
  /// <param name="hdc">Device context</param>
  /// <param name="location">A <see cref="T:System.Drawing.Point" /> structure that represents the Top-Left corner of the display area in the device coordinate.</param>
  /// <param name="size">A <see cref="T:System.Drawing.Size" /> structure that represents the horizontal and vertical size (in pixels) for displaying the page.</param>
  /// <param name="rotate">Page orientation. <see cref="T:Patagames.Pdf.Enums.PageRotate" /> for details.</param>
  /// <param name="flags">RenderFlags.None for normal display, or combination of <see cref="T:Patagames.Pdf.Enums.RenderFlags" /></param>
  public void Render(IntPtr hdc, Point location, Size size, PageRotate rotate, RenderFlags flags)
  {
    this.Render(hdc, location.X, location.Y, size.Width, size.Height, rotate, flags);
  }

  /// <summary>
  /// Render contents in a page to the device context specified by a <see cref="T:System.Drawing.Rectangle" /> structure.
  /// </summary>
  /// <param name="hdc">Device context</param>
  /// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle in the device coordinate. </param>
  /// <param name="rotate">Page orientation. <see cref="T:Patagames.Pdf.Enums.PageRotate" /> for details.</param>
  /// <param name="flags">RenderFlags.None for normal display, or combination of <see cref="T:Patagames.Pdf.Enums.RenderFlags" /></param>
  public void Render(IntPtr hdc, Rectangle rect, PageRotate rotate, RenderFlags flags)
  {
    this.Render(hdc, rect.X, rect.Y, rect.Width, rect.Height, rotate, flags);
  }

  /// <summary>
  /// Start to render page contents to a device independent bitmap progressively specified by a location and size.
  /// </summary>
  /// <param name="bitmap">Instance of <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> class.</param>
  /// <param name="location">A <see cref="T:System.Drawing.Point" /> structure that represents the Top-Left corner of the display area in the bitmap coordinate.</param>
  /// <param name="size">A <see cref="T:System.Drawing.Size" /> structure that represents the horizontal and vertical size (in pixels) for displaying the page.</param>
  /// <param name="rotate">Page orientation: 0 (normal), 1 (rotated 90 degrees clockwise), 2 (rotated 180 degrees), 3 (rotated 90 degrees counter-clockwise).</param>
  /// <param name="flags">0 for normal display, or combination of flags defined above.</param>
  /// <param name="userData">A user defined data pointer, used by user's application. Can be NULL.</param>
  /// <returns>Rendering Status. See <see cref="T:Patagames.Pdf.Enums.ProgressiveRenderingStatuses" /> for the details.</returns>
  public ProgressiveStatus StartProgressiveRender(
    PdfBitmap bitmap,
    Point location,
    Size size,
    PageRotate rotate,
    RenderFlags flags,
    byte[] userData)
  {
    return this.StartProgressiveRender(bitmap, location.X, location.Y, size.Width, size.Height, rotate, flags, userData);
  }

  /// <summary>
  /// Start to render page contents to a device independent bitmap progressively specified by a <see cref="T:System.Drawing.Rectangle" /> structure.
  /// </summary>
  /// <param name="bitmap">Instance of <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> class.</param>
  /// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle in the device coordinate. </param>
  /// <param name="rotate">Page orientation: 0 (normal), 1 (rotated 90 degrees clockwise), 2 (rotated 180 degrees), 3 (rotated 90 degrees counter-clockwise).</param>
  /// <param name="flags">0 for normal display, or combination of flags defined above.</param>
  /// <param name="userData">A user defined data pointer, used by user's application. Can be NULL.</param>
  /// <returns>Rendering Status. See <see cref="T:Patagames.Pdf.Enums.ProgressiveRenderingStatuses" /> for the details.</returns>
  public ProgressiveStatus StartProgressiveRender(
    PdfBitmap bitmap,
    Rectangle rect,
    PageRotate rotate,
    RenderFlags flags,
    byte[] userData)
  {
    return this.StartProgressiveRender(bitmap, rect.X, rect.Y, rect.Width, rect.Height, rotate, flags, userData);
  }

  /// <summary>
  /// Render contents in a page to a device-independent bitmap specified by a location and size.
  /// </summary>
  /// <param name="bitmap">Instance of <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> class.</param>
  /// <param name="location">A <see cref="T:System.Drawing.Point" /> structure that represents the Top-Left corner of the display area in the bitmap coordinate.</param>
  /// <param name="size">A <see cref="T:System.Drawing.Size" /> structure that represents the horizontal and vertical size (in pixels) for displaying the page.</param>
  /// <param name="rotate">Page orientation. See <see cref="T:Patagames.Pdf.Enums.PageRotate" /> for details.</param>
  /// <param name="flags">RenderFlags.None for normal display, or combination of <see cref="T:Patagames.Pdf.Enums.RenderFlags" /></param>
  public void Render(
    PdfBitmap bitmap,
    Point location,
    Size size,
    PageRotate rotate,
    RenderFlags flags)
  {
    this.Render(bitmap, location.X, location.Y, size.Width, size.Height, rotate, flags);
  }

  /// <summary>
  /// Render contents in a page to a device-independent bitmap specified by a <see cref="T:System.Drawing.Rectangle" /> structure.
  /// </summary>
  /// <param name="bitmap">Instance of <see cref="T:Patagames.Pdf.Net.PdfBitmap" /> class.</param>
  /// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle in the device coordinate. </param>
  /// <param name="rotate">Page orientation. See <see cref="T:Patagames.Pdf.Enums.PageRotate" /> for details.</param>
  /// <param name="flags">RenderFlags.None for normal display, or combination of <see cref="T:Patagames.Pdf.Enums.RenderFlags" /></param>
  public void Render(PdfBitmap bitmap, Rectangle rect, PageRotate rotate, RenderFlags flags)
  {
    this.Render(bitmap, rect.X, rect.Y, rect.Width, rect.Height, rotate, flags);
  }

  /// <summary>
  /// Convert the screen coordinate of a point to page coordinate.
  /// </summary>
  /// <param name="x">Left pixel position of the display area in the device coordinate</param>
  /// <param name="y">Top pixel position of the display area in the device coordinate</param>
  /// <param name="width">Horizontal size (in pixels) for displaying the page</param>
  /// <param name="height">Vertical size (in pixels) for displaying the page</param>
  /// <param name="rotate">Page orientation: 0 (normal), 1 (rotated 90 degrees clockwise),2 (rotated 180 degrees), 3 (rotated 90 degrees counter-clockwise).</param>
  /// <param name="deviceX">X value in device coordinate, for the point to be converted</param>
  /// <param name="deviceY">Y value in device coordinate, for the point to be converted</param>
  /// <returns><see cref="T:System.Drawing.PointF" /> structure that represents the point in page coordinate</returns>
  /// <remarks>See remarks of <see cref="M:Patagames.Pdf.Net.PdfPage.DeviceToPage(System.Int32,System.Int32,System.Int32,System.Int32,Patagames.Pdf.Enums.PageRotate,System.Int32,System.Int32)" /> function</remarks>
  public PointF DeviceToPageEx(
    int x,
    int y,
    int width,
    int height,
    PageRotate rotate,
    int deviceX,
    int deviceY)
  {
    double page_x;
    double page_y;
    Pdfium.FPDF_DeviceToPage(this.Handle, x, y, width, height, rotate, deviceX, deviceY, out page_x, out page_y);
    return new PointF((float) page_x, (float) page_y);
  }

  /// <summary>
  /// Convert the page coordinate of a point to screen coordinate
  /// </summary>
  /// <param name="x">Left pixel position of the display area in the device coordinate</param>
  /// <param name="y">Top pixel position of the display area in the device coordinate</param>
  /// <param name="width">Horizontal size (in pixels) for displaying the page</param>
  /// <param name="height">Vertical size (in pixels) for displaying the page</param>
  /// <param name="rotate">Page orientation: 0 (normal), 1 (rotated 90 degrees clockwise),2 (rotated 180 degrees), 3 (rotated 90 degrees counter-clockwise).</param>
  /// <param name="pageX">X value in page coordinate, for the point to be converted</param>
  /// <param name="pageY">Y value in page coordinate, for the point to be converted</param>
  /// <returns><see cref="T:System.Drawing.Point" /> structure that represents the point in device coordinate</returns>
  /// <remarks>See remarks of <see cref="M:Patagames.Pdf.Net.PdfPage.DeviceToPage(System.Int32,System.Int32,System.Int32,System.Int32,Patagames.Pdf.Enums.PageRotate,System.Int32,System.Int32)" /> function</remarks>
  public Point PageToDeviceEx(
    int x,
    int y,
    int width,
    int height,
    PageRotate rotate,
    float pageX,
    float pageY)
  {
    int device_x;
    int device_y;
    Pdfium.FPDF_PageToDevice(this.Handle, x, y, width, height, rotate, (double) pageX, (double) pageY, out device_x, out device_y);
    return new Point(device_x, device_y);
  }
}
