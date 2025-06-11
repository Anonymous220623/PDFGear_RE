// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfAvailabilityProvider
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.EventArguments;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents PDF document availability provider.</summary>
public class PdfAvailabilityProvider : IDisposable
{
  private FX_FILEAVAIL _avail = new FX_FILEAVAIL();
  private FPDF_FILEACCESS _file = new FPDF_FILEACCESS(0U);
  private FX_DOWNLOADHINTS _hints = new FX_DOWNLOADHINTS();

  /// <summary>
  /// Gets a value indicating whether the object has been disposed of.
  /// <value>true if the control has been disposed of; otherwise, false.</value>
  /// </summary>
  public bool IsDisposed { get; private set; }

  /// <summary>Gets the Pdfium SDK handle that the page is bound to</summary>
  public IntPtr Handle { get; private set; }

  /// <summary>
  /// Gets the PDF document associated with the current availability provider.
  /// </summary>
  public PdfDocument Document { get; private set; }

  /// <summary>
  /// Check whether the document is ready for loading, if not, fires AddSegment event.
  /// </summary>
  /// <remarks>
  /// The application should read this property whenever new data arrived, and process all the
  /// generated download hints if any, while the property is false. Then the application can call <see cref="M:Patagames.Pdf.Net.PdfAvailabilityProvider.CreateDocument(System.String)" /> to get a document.
  /// </remarks>
  public bool IsDocumentAvailable => Pdfium.FPDFAvail_IsDocAvail(this.Handle, this._hints);

  /// <summary>
  /// Gets zero-based index for the first available page in a linearized PDF
  /// </summary>
  /// <remarks>For most linearized PDFs, the first available page would be just the first page, however,
  /// some PDFs might make other page to be the first available page.
  /// For non-linearized PDF, this property will always return zero.</remarks>
  public int FirstAvailablePageIndex
  {
    get => this.Document == null ? -1 : Pdfium.FPDFAvail_GetFirstPageNum(this.Document.Handle);
  }

  /// <summary>
  /// To check whether a document is Linearized PDF file. <see cref="T:Patagames.Pdf.Enums.IsLinearizedResults" /> for more details.
  /// </summary>
  /// <remarks>
  /// return IsLinearized means the document is linearized PDF else not.
  /// It return IsLinearized/NotLinearized state as soon as we have first 1K data. If the file's size less than
  /// 1K,we don't known whether the PDF is a linearized file.</remarks>
  public IsLinearizedResults IsLinearized => Pdfium.FPDFAvail_IsLinearized(this.Handle);

  /// <summary>
  /// Check whether Form data is ready for init, if not, get download hints. <see cref="T:Patagames.Pdf.Enums.IsFormAvailableResults" /> for details.
  /// </summary>
  /// <remarks>
  /// This property is valid only if <see cref="M:Patagames.Pdf.Net.PdfAvailabilityProvider.CreateDocument(System.String)" /> was called.
  /// The application should read this property whenever new data arrived, and process all the
  /// generated download hints if any, until the function property sets to true. Then the
  /// application can create an instance of <see cref="T:Patagames.Pdf.Net.PdfForms" /> class, call <see cref="M:Patagames.Pdf.Net.PdfAvailabilityProvider.InitDocument(Patagames.Pdf.Net.PdfForms)" /> method and perform page loading
  /// after the property returns true.
  /// </remarks>
  public IsFormAvailableResults IsFormAvailable
  {
    get => Pdfium.FPDFAvail_IsFormAvail(this.Handle, this._hints);
  }

  /// <summary>
  /// Called by SDK to check whether the data section is ready.
  /// <note type="note">Required: Yes. </note>
  /// </summary>
  public event EventHandler<IsSegmentAvailableEventArgs> IsSegmentAvailable;

  /// <summary>
  /// Called by SDK to report some downloading hints for download manager.
  /// <note type="note">Required: Yes.</note>
  /// </summary>
  public event EventHandler<AddSegmentEventArgs> AddSegment;

  /// <summary>
  /// Called by SDK to read some data from download manager.
  /// <note type="note">Required: Yes.</note>
  /// </summary>
  public event EventHandler<ReadSegmentEventArgs> ReadSegment;

  private bool IsDataAvailableCallback(FX_FILEAVAIL pThis, IntPtr offset, IntPtr size)
  {
    IsSegmentAvailableEventArgs e = new IsSegmentAvailableEventArgs(offset.ToInt32(), size.ToInt32());
    if (this.IsSegmentAvailable != null)
      this.IsSegmentAvailable((object) this, e);
    return e.IsSegmentAvailable;
  }

  private bool GetBlockCallback(byte[] param, uint position, byte[] buf, uint size)
  {
    ReadSegmentEventArgs e = new ReadSegmentEventArgs(param, (int) position, buf, (int) size);
    if (this.ReadSegment != null)
      this.ReadSegment((object) this, e);
    return e.IsSuccess;
  }

  private void AddSegmentCallback(FX_DOWNLOADHINTS pThis, IntPtr offset, IntPtr size)
  {
    if (this.AddSegment == null)
      return;
    this.AddSegment((object) this, new AddSegmentEventArgs(offset.ToInt32(), size.ToInt32()));
  }

  /// <summary>
  /// Construct instance of <see cref="T:Patagames.Pdf.Net.PdfAvailabilityProvider" /> class with specified file size
  /// </summary>
  /// <param name="PdfSize">The size in bytes of downloadable document</param>
  public PdfAvailabilityProvider(int PdfSize)
  {
    this._avail.IsDataAvailable = new Patagames.Pdf.IsDataAvailableCallback(this.IsDataAvailableCallback);
    this._file.FileLen = (uint) PdfSize;
    this._file.GetBlock = new Patagames.Pdf.GetBlockCallback(this.GetBlockCallback);
    this._hints.AddSegment = new Patagames.Pdf.AddSegmentCallback(this.AddSegmentCallback);
    this.Handle = Pdfium.FPDFAvail_Create(this._avail, this._file);
    if (this.Handle == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
  }

  /// <summary>
  /// Release all resources allocated by PdfAvailabilityProvider
  /// </summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>Releases all resources used by the PdfImageObject.</summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !PdfCommon.IsInitialize)
      return;
    if (this.Document != null)
      this.Document.Dispose();
    this.Document = (PdfDocument) null;
    if (this.Handle != IntPtr.Zero)
      Pdfium.FPDFAvail_Destroy(this.Handle);
    this.Handle = IntPtr.Zero;
    this._file.Dispose();
    this.IsDisposed = true;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  /// <summary>Finalize object</summary>
  ~PdfAvailabilityProvider()
  {
    if (PdfCommon.IsCheckForMemoryLeaks && !this.IsDisposed)
      throw new MemoryLeakException(nameof (PdfAvailabilityProvider));
  }

  /// <summary>
  /// Check whether a page is ready for loading, if not, fires <see cref="E:Patagames.Pdf.Net.PdfAvailabilityProvider.AddSegment" /> event.
  /// </summary>
  /// <param name="index">Index number of the page. 0 for the first page.</param>
  /// <returns>True for page is fully available, False for page not yet available.</returns>
  /// <remarks>
  /// This function call be called only after <see cref="M:Patagames.Pdf.Net.PdfAvailabilityProvider.CreateDocument(System.String)" /> if called.
  /// The application should call this function whenever new data arrived, and process all the
  /// generated download hints if any, until the function returns True. Then the
  /// application can perform page loading.
  /// </remarks>
  public bool IsPageAvailable(int index)
  {
    return Pdfium.FPDFAvail_IsPageAvail(this.Handle, index, this._hints);
  }

  /// <summary>
  /// Get document from the availability provider and store it in <see cref="P:Patagames.Pdf.Net.PdfAvailabilityProvider.Document" /> property.
  /// </summary>
  /// <param name="password">Optional password for decrypting the PDF file.</param>
  /// <remarks>
  /// After <see cref="P:Patagames.Pdf.Net.PdfAvailabilityProvider.IsDocumentAvailable" /> returns TRUE, the application should call this function to
  /// get the document instance.
  /// </remarks>
  public void CreateDocument(string password = null)
  {
    if (!this.IsDocumentAvailable)
      throw new NotAvailableException(Error.err0009);
    IntPtr document = Pdfium.FPDFAvail_GetDocument(this.Handle, password);
    this.Document = !(document == IntPtr.Zero) ? new PdfDocument(document, this, (PdfCustomLoader) null) : throw Pdfium.ProcessLastError();
  }

  /// <summary>Initialize the created PDF document.</summary>
  /// <param name="forms">Instance of <see cref="T:Patagames.Pdf.Net.PdfForms" /> class that will be associated with loaded document</param>
  /// <remarks>After <see cref="P:Patagames.Pdf.Net.PdfAvailabilityProvider.IsFormAvailable" /> returns TRUE, the application should call this function to initialize the document instance.</remarks>
  public void InitDocument(PdfForms forms = null)
  {
    if (this.IsFormAvailable == IsFormAvailableResults.NotAvaialble)
      throw new NotAvailableException(Error.err0010);
    if (forms == null)
      return;
    this.Document.FormFill = forms;
    forms.Init(this.Document);
  }
}
