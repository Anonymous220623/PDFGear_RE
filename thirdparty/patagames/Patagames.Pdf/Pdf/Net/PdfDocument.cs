// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfDocument
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Actions;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.EventArguments;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Net.Wrappers;
using Patagames.Pdf.Properties;
using System;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents a PDF document.</summary>
/// <threadsafety>Any public static (Shared in Visual Basic) members of this type are thread safe. Any instance members are not guaranteed to be thread safe.</threadsafety>
/// <seealso href="https://pdfium.patagames.com/c-pdf-library/">Create PDF in C#</seealso>
public class PdfDocument : IDisposable
{
  private PdfCustomLoader _loader;
  private PdfPageCollection _pages;
  private PdfBookmarkCollections _bookmarks;
  private PdfAttachmentCollection _attachments;
  private PdfDestination _openDestination;
  private PdfAction _openAction;
  private PdfDocumentLevelActions _aactions;
  private object _syncReadFromFile = new object();
  /// <summary>For prevent GC collet</summary>
  private byte[] _bytePdf;
  /// <summary>For prevent GC collect</summary>
  private Stream _streamPdf;
  private bool _leaveOpen;
  private IntPtr _handle;
  private PdfTypeDictionary _trailer;
  private PdfTypeDictionary _root;
  private PdfTypeDictionary _info;

  /// <summary>
  /// SDK fire this event for transmission of the next data block of PDF document from <see cref="M:Patagames.Pdf.Net.PdfDocument.Save(Patagames.Pdf.Enums.SaveFlags,System.Int32)" /> method.
  /// </summary>
  public event EventHandler<WriteFileBlockEventArgs> WriteBlock;

  /// <summary>
  /// Gets or sets the object that contains data about the document.
  /// </summary>
  /// <remarks>
  /// Any type derived from the Object class can be assigned to this property.
  /// A common use for the Tag property is to store data that is closely associated with the document.
  /// </remarks>
  public object Tag { get; set; }

  /// <summary>Gets the root catalog of the PDF document</summary>
  public PdfTypeDictionary Root
  {
    get
    {
      if (this.Handle == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      IntPtr root = Pdfium.FPDF_GetRoot(this.Handle);
      if (root == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      if (this._root == null || this._root.IsDisposed || this._root.Handle != root)
        this._root = new PdfTypeDictionary(root);
      return this._root;
    }
  }

  /// <summary>Gets the document’s information dictionary</summary>
  public PdfTypeDictionary Info
  {
    get
    {
      if (this.Handle == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      IntPtr info = Pdfium.FPDF_GetInfo(this.Handle);
      if (info == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      if (this._info == null || this._info.IsDisposed || this._info.Handle != info)
        this._info = new PdfTypeDictionary(info);
      return this._info;
    }
  }

  /// <summary>Gets the trailer of the PDF file.</summary>
  public PdfTypeDictionary Trailer
  {
    get
    {
      if (this.Handle == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      IntPtr trailer = Pdfium.FPDF_GetTrailer(this.Handle);
      if (trailer == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      if (this._trailer == null || this._trailer.IsDisposed || this._trailer.Handle != trailer)
        this._trailer = new PdfTypeDictionary(trailer);
      return this._trailer;
    }
  }

  /// <summary>
  /// Gets a value indicating whether the object has been disposed of.
  /// <value>true if the control has been disposed of; otherwise, false.</value>
  /// </summary>
  public bool IsDisposed { get; private set; }

  /// <summary>
  /// Gets <see cref="T:Patagames.Pdf.Net.PdfAvailabilityProvider" /> object if document was loaded from availability provider.
  /// </summary>
  public PdfAvailabilityProvider AvailabilityProvider { get; private set; }

  /// <summary>
  /// Gets the Pdfium SDK handle that the document is bound to
  /// </summary>
  public IntPtr Handle
  {
    get
    {
      if (this.IsDisposed)
        throw new ObjectDisposedException(nameof (PdfDocument));
      return this._handle;
    }
  }

  /// <summary>Gets a form fill environment object.</summary>
  public PdfForms FormFill { get; internal set; }

  /// <summary>
  /// Gets the collection of Pages found in the PDF document.
  /// </summary>
  /// <remarks>Page loaded into memory when you first access to any method or a property of an object and unloaded from memory when calling the Dispose method</remarks>
  public PdfPageCollection Pages
  {
    get
    {
      if (this._pages == null)
        this._pages = new PdfPageCollection(this);
      return this._pages;
    }
  }

  /// <summary>
  /// Gets the collection of bookmarks found in the PDF document.
  /// </summary>
  public PdfBookmarkCollections Bookmarks
  {
    get
    {
      if (this._bookmarks == null)
        this._bookmarks = new PdfBookmarkCollections(this, (PdfBookmark) null);
      return this._bookmarks;
    }
  }

  /// <summary>
  /// Gets the collection of attachments found in the PDF document.
  /// </summary>
  public PdfAttachmentCollection Attachments
  {
    get
    {
      if (this._attachments == null)
        this._attachments = new PdfAttachmentCollection(this);
      return this._attachments;
    }
  }

  /// <summary>
  /// Gets the collection of named destinations found in the PDF document.
  /// </summary>
  public PdfDestinationCollections NamedDestinations { get; private set; }

  /// <summary>The document’s title.</summary>
  public string Title
  {
    get => Pdfium.FPDF_GetMetaText(this.Handle, DocumentTags.Title);
    set => Pdfium.FPDF_SetMetaText(this.Handle, DocumentTags.Title, value);
  }

  /// <summary>The name of the person who created the document.</summary>
  public string Author
  {
    get => Pdfium.FPDF_GetMetaText(this.Handle, DocumentTags.Author);
    set => Pdfium.FPDF_SetMetaText(this.Handle, DocumentTags.Author, value);
  }

  /// <summary>The subject of the document.</summary>
  public string Subject
  {
    get => Pdfium.FPDF_GetMetaText(this.Handle, DocumentTags.Subject);
    set => Pdfium.FPDF_SetMetaText(this.Handle, DocumentTags.Subject, value);
  }

  /// <summary>Keywords associated with the document.</summary>
  public string Keywords
  {
    get => Pdfium.FPDF_GetMetaText(this.Handle, DocumentTags.Keywords);
    set => Pdfium.FPDF_SetMetaText(this.Handle, DocumentTags.Keywords, value);
  }

  /// <summary>
  /// If the document was converted to PDF from another format, the name of the application (for example, Adobe FrameMaker®) that created the original document from which it was converted.
  /// </summary>
  public string Creator
  {
    get => Pdfium.FPDF_GetMetaText(this.Handle, DocumentTags.Creator);
    set => Pdfium.FPDF_SetMetaText(this.Handle, DocumentTags.Creator, value);
  }

  /// <summary>
  /// If the document was converted to PDF from another format, the name of the application (for example, Acrobat Distiller) that converted it to PDF.
  /// </summary>
  public string Producer
  {
    get => Pdfium.FPDF_GetMetaText(this.Handle, DocumentTags.Producer);
    set => Pdfium.FPDF_SetMetaText(this.Handle, DocumentTags.Producer, value);
  }

  /// <summary>
  /// The date and time the document was created, in human-readable form (see Section 3.8.3, “Dates” in PDF Refernce 1.7).
  /// </summary>
  public string CreationDate
  {
    get => Pdfium.FPDF_GetMetaText(this.Handle, DocumentTags.CreationDate);
    set => Pdfium.FPDF_SetMetaText(this.Handle, DocumentTags.CreationDate, value);
  }

  /// <summary>
  /// The date and time the document was most recently modified, in human-readable form (see Section 3.8.3, “Dates”  in PDF Refernce 1.7).
  /// </summary>
  public string ModificationDate
  {
    get => Pdfium.FPDF_GetMetaText(this.Handle, DocumentTags.CreationDate);
    set => Pdfium.FPDF_SetMetaText(this.Handle, DocumentTags.ModificationDate, value);
  }

  /// <summary>
  /// A name object indicating whether the document has been modified to include trapping information (see Section 10.10.5, “Trapping Support” in PDF Reference 1.7).
  /// </summary>
  public string Trapped => Pdfium.FPDF_GetMetaText(this.Handle, DocumentTags.Trapped);

  /// <summary>
  /// Gets the file version of the specific PDF document. 14 for 1.4, 15 for 1.5, ...
  /// </summary>
  /// <remarks>If the document is created by method <see cref="M:Patagames.Pdf.Net.PdfDocument.CreateNew(Patagames.Pdf.Net.PdfForms)" />, then return value would always zero.</remarks>
  public int Version
  {
    get
    {
      int fileVersion = 0;
      return Pdfium.FPDF_GetFileVersion(this.Handle, out fileVersion) ? fileVersion : 0;
    }
  }

  /// <summary>
  /// Gets permission flags. Please refer to <see cref="T:Patagames.Pdf.Enums.PdfUserAccessPermission" /> for detailed description.
  /// </summary>
  public PdfUserAccessPermission Permission
  {
    get => (PdfUserAccessPermission) Pdfium.FPDF_GetDocPermissions(this.Handle);
  }

  /// <summary>
  /// Gets the security handler revision number. Please refer to <a href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference</a> for detailed description.
  /// </summary>
  /// <seealso href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference 1.7</seealso>
  public int SecurityRevision => Pdfium.FPDF_GetSecurityHandlerRevision(this.Handle);

  /// <summary>
  ///  Gets the document's PageMode(How the document should be displayed when opened)
  /// </summary>
  /// <returns>The flags for page mode.</returns>
  public PageModes PagesMode => Pdfium.FPDFDoc_GetPageMode(this.Handle);

  /// <summary>
  /// Gets or sets the document-level information for URI actions.
  /// </summary>
  public string BaseUri
  {
    get
    {
      if (!this.Root.ContainsKey("URI"))
        return (string) null;
      if (!this.Root["URI"].Is<PdfTypeDictionary>())
        return (string) null;
      PdfTypeDictionary pdfTypeDictionary = this.Root["URI"].As<PdfTypeDictionary>();
      if (!pdfTypeDictionary.ContainsKey("Base"))
        return (string) null;
      return !pdfTypeDictionary["Base"].Is<PdfTypeString>() ? (string) null : pdfTypeDictionary["Base"].As<PdfTypeString>().AnsiString;
    }
    internal set
    {
      if (value == null && this.Root.ContainsKey("URI"))
      {
        this.Root.Remove("URI");
      }
      else
      {
        if (value == null)
          return;
        if (!this.Root.ContainsKey("URI") || !this.Root["URI"].Is<PdfTypeDictionary>())
          this.Root["URI"] = (PdfTypeBase) PdfTypeDictionary.Create();
        this.Root["URI"].As<PdfTypeDictionary>()["Base"] = (PdfTypeBase) PdfTypeString.Create(value);
      }
    }
  }

  /// <summary>
  /// Gets or sets a value specifying a destination to be displayed when the document is opened.
  /// </summary>
  public PdfDestination OpenDestination
  {
    get
    {
      if (!this.Root.ContainsKey("OpenAction"))
        return (PdfDestination) null;
      if (this.Root["OpenAction"].Is<PdfTypeString>())
        return this.NamedDestinations[this.Root["OpenAction"].As<PdfTypeString>().UnicodeString];
      if (this.Root["OpenAction"].Is<PdfTypeName>())
        return this.NamedDestinations[this.Root["OpenAction"].As<PdfTypeName>().Value];
      if (!this.Root["OpenAction"].Is<PdfTypeArray>())
        return (PdfDestination) null;
      PdfTypeArray pdfTypeArray = this.Root["OpenAction"].As<PdfTypeArray>();
      if (this._openDestination == null || this._openDestination.Handle != pdfTypeArray.Handle)
        this._openDestination = new PdfDestination(this, pdfTypeArray.Handle);
      return this._openDestination;
    }
    set
    {
      if (value == null && this.Root.ContainsKey("OpenAction"))
        this.Root.Remove("OpenAction");
      else if (value != null)
        this.Root["OpenAction"] = value.GetForInsert(this);
      this._openDestination = value;
      this._openAction = (PdfAction) null;
    }
  }

  /// <summary>
  /// Gets or sets a value specifying an action to be performed when the document is opened.
  /// </summary>
  public PdfAction OpenAction
  {
    get
    {
      if (!this.Root.ContainsKey(nameof (OpenAction)))
        return (PdfAction) null;
      if (!this.Root[nameof (OpenAction)].Is<PdfTypeDictionary>())
        return (PdfAction) null;
      PdfTypeDictionary pdfTypeDictionary = this.Root[nameof (OpenAction)].As<PdfTypeDictionary>();
      if (this._openAction == null || this._openAction.Handle != pdfTypeDictionary.Handle)
        this._openAction = PdfAction.FromHandle(this, pdfTypeDictionary.Handle);
      return this._openAction;
    }
    set
    {
      if (value == null && this.Root.ContainsKey(nameof (OpenAction)))
        this.Root.Remove(nameof (OpenAction));
      else if (value != null)
      {
        if (value.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(value.Dictionary.Handle) != IntPtr.Zero)
          throw new ArgumentException(string.Format(Error.err0067, (object) "action", (object) "object"));
        PdfIndirectList list = PdfIndirectList.FromPdfDocument(this);
        list.Add((PdfTypeBase) value.Dictionary);
        this.Root.SetIndirectAt(nameof (OpenAction), list, (PdfTypeBase) value.Dictionary);
      }
      this._openDestination = (PdfDestination) null;
      this._openAction = value;
    }
  }

  /// <summary>
  /// Gets or sets the additional-actions defining the actions to be taken in response to various trigger events affecting the document as a whole.
  /// </summary>
  public PdfDocumentLevelActions AdditionalActions
  {
    get
    {
      if (!this.Root.ContainsKey("AA"))
      {
        this._aactions = (PdfDocumentLevelActions) null;
        return (PdfDocumentLevelActions) null;
      }
      if ((PdfWrapper) this._aactions == (PdfWrapper) null || this.Root["AA"].Is<PdfTypeDictionary>() && this._aactions.Dictionary.Handle != this.Root["AA"].As<PdfTypeDictionary>().Handle)
        this._aactions = new PdfDocumentLevelActions(this, (PdfTypeBase) this.Root["AA"].As<PdfTypeDictionary>());
      return this._aactions;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Root.ContainsKey("AA"))
        this.Root.Remove("AA");
      else if ((PdfWrapper) value != (PdfWrapper) null)
      {
        if (value.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(value.Dictionary.Handle) != IntPtr.Zero)
          throw new ArgumentException(string.Format(Error.err0067, (object) "additional actions", (object) "object"));
        PdfIndirectList list = PdfIndirectList.FromPdfDocument(this);
        list.Add((PdfTypeBase) value.Dictionary);
        this.Root.SetIndirectAt("AA", list, (PdfTypeBase) value.Dictionary);
      }
      this._aactions = value;
    }
  }

  /// <summary>Initializes a new instance of the PdfDocument class.</summary>
  internal PdfDocument(
    IntPtr DocHandle,
    PdfAvailabilityProvider availabilityProvider,
    PdfCustomLoader loader)
  {
    this._handle = DocHandle;
    this.AvailabilityProvider = availabilityProvider;
    this._loader = loader;
    this.NamedDestinations = new PdfDestinationCollections(this);
  }

  /// <summary>Releases all resources used by the PdfDocument.</summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>Releases all resources used by the PdfImageObject.</summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !PdfCommon.IsInitialize)
      return;
    if (this.Pages != null)
      this.Pages.Dispose();
    this._pages = (PdfPageCollection) null;
    this._bookmarks = (PdfBookmarkCollections) null;
    this._attachments = (PdfAttachmentCollection) null;
    this.NamedDestinations = (PdfDestinationCollections) null;
    if (this.FormFill != null)
      this.FormFill.Dispose();
    this.FormFill = (PdfForms) null;
    if (this._loader != null)
    {
      this._loader.LoadBlock -= new EventHandler<CustomLoadEventArgs>(PdfDocument.Loader_LoadBlock);
      this._loader.Dispose();
    }
    this._loader = (PdfCustomLoader) null;
    if (this.Handle != IntPtr.Zero)
      Pdfium.FPDF_CloseDocument(this.Handle);
    this._handle = IntPtr.Zero;
    this._bytePdf = (byte[]) null;
    if (this._streamPdf != null && !this._leaveOpen)
      this._streamPdf.Dispose();
    this._streamPdf = (Stream) null;
    if (this._root != null)
      this._root.Dispose();
    this._root = (PdfTypeDictionary) null;
    if (this._trailer != null)
      this._trailer.Dispose();
    this._trailer = (PdfTypeDictionary) null;
    if (this._info != null)
      this._info.Dispose();
    this._info = (PdfTypeDictionary) null;
    this.IsDisposed = true;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  /// <summary>Finalize object</summary>
  ~PdfDocument()
  {
    if (PdfCommon.IsCheckForMemoryLeaks && !this.IsDisposed)
      throw new MemoryLeakException(nameof (PdfDocument));
  }

  /// <summary>
  /// Crreate an instance of <see cref="T:Patagames.Pdf.Net.PdfDocument" /> class from handle.
  /// </summary>
  /// <param name="Handle">Handle to Pdf Document</param>
  /// <param name="forms">Instance of <see cref="T:Patagames.Pdf.Net.PdfForms" /> class that will be associated with loaded document</param>
  /// <returns>Instance of PDFDocument class</returns>
  public static PdfDocument FromHandle(IntPtr Handle, PdfForms forms = null)
  {
    PdfDocument document = new PdfDocument(Handle, (PdfAvailabilityProvider) null, (PdfCustomLoader) null);
    if (forms != null)
    {
      document.FormFill = forms;
      forms.Init(document);
    }
    return document;
  }

  /// <summary>Open and load a PDF document from a file.</summary>
  /// <param name="path">Path to the PDF file (including extension)</param>
  /// <param name="forms">Instance of <see cref="T:Patagames.Pdf.Net.PdfForms" /> class that will be associated with loaded document</param>
  /// <param name="password">A string used as the password for PDF file. If no password needed, empty or NULL can be used.</param>
  /// <returns>Instance of PDFDocument class</returns>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownErrorException">unknown error</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfFileNotFoundException">file not found or could not be opened</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.BadFormatException">file not in PDF format or corrupted</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.InvalidPasswordException">password required or incorrect password</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnsupportedSecuritySchemeException">unsupported security scheme</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">Error occured in PDFium. See ErrorCode for detail</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.NoLicenseException">This exception thrown only in trial mode if document cannot be opened due to a license restrictions"</exception>
  /// <remarks>
  /// <note type="note">
  /// With the trial version the documents which size is smaller than 1024 Kb, or greater than 10 Mb can be loaded without any restrictions. For other documents the allowed ranges is 1.5 - 2 Mb; 2.5 - 3 Mb; 3.5 - 4 Mb; 4.5 - 5 Mb and so on.
  /// </note>
  /// </remarks>
  /// <seealso href="https://pdfium.patagames.com/c-pdf-library/">Read PDF Document in C#</seealso>
  public static PdfDocument Load(string path, PdfForms forms = null, string password = null)
  {
    FileStream fileStream = File.OpenRead(path);
    try
    {
      if (fileStream != null)
        return PdfDocument.Load((Stream) fileStream, forms, password, false);
    }
    catch (MarshalDirectiveException ex)
    {
      fileStream.Dispose();
      forms?.Dispose();
    }
    IntPtr DocHandle = Pdfium.FPDF_LoadDocument(path, password);
    PdfDocument document = !(DocHandle == IntPtr.Zero) ? new PdfDocument(DocHandle, (PdfAvailabilityProvider) null, (PdfCustomLoader) null) : throw Pdfium.ProcessLastError();
    if (forms != null)
    {
      document.FormFill = forms;
      forms.Init(document);
    }
    return document;
  }

  /// <summary>Open and load a PDF document from a memory.</summary>
  /// <param name="content">Pointer to a buffer containing the PDF document</param>
  /// <param name="forms">Instance of <see cref="T:Patagames.Pdf.Net.PdfForms" /> class that will be associated with loaded document</param>
  /// <param name="password">A string used as the password for PDF file. If no password needed, empty or NULL can be used.</param>
  /// <returns>Instance of PDFDocument class</returns>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownErrorException">Unknown error</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfFileNotFoundException">File not found or could not be opened</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.BadFormatException">File not in PDF format or corrupted</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.InvalidPasswordException">Password required or incorrect password</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnsupportedSecuritySchemeException">unsupported security scheme</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">Error occured in Pdfium. See ErrorCode for detail</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.NoLicenseException">This exception thrown only in trial mode if document cannot be opened due to a license restrictions"</exception>
  /// <remarks>
  /// <note type="note">
  /// <para>The application should maintain the byte array being valid until the PDF document close.</para>
  /// <para>With the trial version the documents which size is smaller than 1024 Kb, or greater than 10 Mb can be loaded without any restrictions. For other documents the allowed ranges is 1.5 - 2 Mb; 2.5 - 3 Mb; 3.5 - 4 Mb; 4.5 - 5 Mb and so on.</para>
  /// </note>
  /// </remarks>
  /// <seealso href="https://pdfium.patagames.com/c-pdf-library/">Read PDF Document in C#</seealso>
  public static PdfDocument Load(byte[] content, PdfForms forms = null, string password = null)
  {
    MemoryStream memoryStream = new MemoryStream(content);
    if (memoryStream != null)
      return PdfDocument.Load((Stream) memoryStream, forms, password, false);
    IntPtr DocHandle = Pdfium.FPDF_LoadMemDocument(content, content.Length, password);
    PdfDocument document = !(DocHandle == IntPtr.Zero) ? new PdfDocument(DocHandle, (PdfAvailabilityProvider) null, (PdfCustomLoader) null) : throw Pdfium.ProcessLastError();
    document._bytePdf = content;
    if (forms != null)
    {
      document.FormFill = forms;
      forms.Init(document);
    }
    return document;
  }

  /// <summary>Loads the PDF document from the specified stream.</summary>
  /// <param name="stream">The stream containing the PDF document to load. The stream must support seeking.</param>
  /// <param name="forms">Instance of <see cref="T:Patagames.Pdf.Net.PdfForms" /> class that will be associated with loaded document</param>
  /// <param name="password">A string used as the password for PDF file. If no password needed, empty or NULL can be used.</param>
  /// <param name="leaveOpen">true if the application would like the stream to remain open after closing document.</param>
  /// <returns>Instance of PDFDocument class</returns>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownErrorException">unknown error</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfFileNotFoundException">file not found or could not be opened</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.BadFormatException">file not in PDF format or corrupted</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.InvalidPasswordException">password required or incorrect password</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnsupportedSecuritySchemeException">unsupported security scheme</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">Error occured in PDFium. See ErrorCode for detail</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.NoLicenseException">This exception thrown only in trial mode if document cannot be opened due to a license restrictions"</exception>
  /// <remarks>
  /// <note type="note">
  /// <para>The application should maintain the stream resources being valid until the PDF document close.</para>
  /// <para>With the trial version the documents which size is smaller than 1024 Kb, or greater than 10 Mb can be loaded without any restrictions. For other documents the allowed ranges is 1.5 - 2 Mb; 2.5 - 3 Mb; 3.5 - 4 Mb; 4.5 - 5 Mb and so on.</para>
  /// </note>
  /// </remarks>
  /// <seealso href="https://pdfium.patagames.com/c-pdf-library/">Read PDF Document in C#</seealso>
  public static PdfDocument Load(Stream stream, PdfForms forms = null, string password = null, bool leaveOpen = true)
  {
    PdfCustomLoader loader = new PdfCustomLoader((uint) stream.Length);
    loader.FormFill = forms;
    loader.Password = password;
    loader.Tag = (object) new object[2]
    {
      (object) stream,
      null
    };
    loader.LoadBlock += new EventHandler<CustomLoadEventArgs>(PdfDocument.Loader_LoadBlock);
    try
    {
      PdfDocument pdfDocument = PdfDocument.Load(loader);
      ((object[]) loader.Tag)[1] = pdfDocument._syncReadFromFile;
      pdfDocument._streamPdf = stream;
      pdfDocument._leaveOpen = leaveOpen;
      return pdfDocument;
    }
    catch
    {
      loader.LoadBlock -= new EventHandler<CustomLoadEventArgs>(PdfDocument.Loader_LoadBlock);
      loader.Dispose();
      if (!leaveOpen)
        stream.Dispose();
      throw;
    }
  }

  /// <summary>
  /// Loads the PDF document from the specified custom descriptor.
  /// </summary>
  /// <param name="loader">Custom access descriptor for loading PDF document.</param>
  /// <returns>Instance of PDFDocument class</returns>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownErrorException">unknown error</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfFileNotFoundException">file not found or could not be opened</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.BadFormatException">file not in PDF format or corrupted</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.InvalidPasswordException">password required or incorrect password</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnsupportedSecuritySchemeException">unsupported security scheme</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">Error occured in PDFium. See ErrorCode for detail</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.NoLicenseException">This exception thrown only in trial mode if document cannot be opened due to a license restrictions"</exception>
  /// <remarks>
  /// <note type="note">
  /// <para>The application should maintain the instance of PdfCustomLoader class being valid until the PDF document close.</para>
  /// <para>With the trial version the documents which size is smaller than 1024 Kb, or greater than 10 Mb can be loaded without any restrictions. For other documents the allowed ranges is 1.5 - 2 Mb; 2.5 - 3 Mb; 3.5 - 4 Mb; 4.5 - 5 Mb and so on.</para>
  /// </note>
  /// </remarks>
  /// <seealso href="https://pdfium.patagames.com/c-pdf-library/">Read PDF Document in C#</seealso>
  public static PdfDocument Load(PdfCustomLoader loader) => loader.Load();

  /// <summary>
  /// Saves a copy of PDF document to the specified file name
  /// </summary>
  /// <param name="path">Path to the new PDF file</param>
  /// <param name="flags">The creating flags.</param>
  /// <param name="version">The PDF file Version. File Version: 14 for 1.4, 15 for 1.5, ...</param>
  /// <seealso href="https://pdfium.patagames.com/c-pdf-library/">Generate PDF Document in C#</seealso>
  public void Save(string path, SaveFlags flags, int version = 0)
  {
    using (FileStream fileStream = File.Create(path))
      this.Save((Stream) fileStream, flags, version);
  }

  /// <summary>Saves a copy of PDF document to the stream</summary>
  /// <param name="stream">The stream to write to.</param>
  /// <param name="flags">The creating flags.</param>
  /// <param name="version">The PDF file Version. File Version: 14 for 1.4, 15 for 1.5, ...</param>
  /// <seealso href="https://pdfium.patagames.com/c-pdf-library/">Generate PDF Document in C#</seealso>
  public void Save(Stream stream, SaveFlags flags, int version = 0)
  {
    FPDF_FILEWRITE saveData = new FPDF_FILEWRITE();
    saveData.WriteBlock = (WriteBlockCallback) ((pThis, buffer, buflen) =>
    {
      stream.Write(buffer, 0, buffer.Length);
      return true;
    });
    try
    {
      if (!(version <= 0 ? Pdfium.FPDF_SaveAsCopy(this.Handle, saveData, flags) : Pdfium.FPDF_SaveWithVersion(this.Handle, saveData, flags, version)))
        throw Pdfium.ProcessLastError();
    }
    finally
    {
      GC.KeepAlive((object) saveData);
    }
  }

  /// <summary>Saves the copy of PDF document in custom way.</summary>
  /// <param name="flags">The creating flags.</param>
  /// <param name="version">The PDF file Version. File Version: 14 for 1.4, 15 for 1.5, ...</param>
  /// <remarks>Before calling this method the application should be subscribed to the WriteBlock event.
  /// SDK will fire this event for transmission of the next data block of PDF document.</remarks>
  /// <seealso href="https://pdfium.patagames.com/c-pdf-library/">Generate PDF Document in C#</seealso>
  public void Save(SaveFlags flags, int version = 0)
  {
    FPDF_FILEWRITE saveData = new FPDF_FILEWRITE();
    saveData.WriteBlock = (WriteBlockCallback) ((pThis, buffer, buflen) =>
    {
      WriteFileBlockEventArgs e = new WriteFileBlockEventArgs(ref buffer);
      if (this.WriteBlock != null)
        this.WriteBlock((object) this, e);
      return e.ReturnValue;
    });
    try
    {
      if (!(version <= 0 ? Pdfium.FPDF_SaveAsCopy(this.Handle, saveData, flags) : Pdfium.FPDF_SaveWithVersion(this.Handle, saveData, flags, version)))
        throw Pdfium.ProcessLastError();
    }
    finally
    {
      GC.KeepAlive((object) saveData);
    }
  }

  /// <summary>Get a text from metadata of the document.</summary>
  /// <param name="tag">The tag for the meta data. Currently, It can be "Title", "Author", "Subject",
  /// "Keywords", "Creator", "Producer", "CreationDate", or "ModDate". For
  /// detailed explanation of these tags and their respective values, please refer
  /// to <a href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf"> PDF Reference 1.6</a>, section 10.2.1, "Document Information Dictionary".
  /// </param>
  /// <returns>The text from metadata of the document.</returns>
  /// <seealso href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference 1.7</seealso>
  public string GetMetaText(string tag) => Pdfium.FPDF_GetMetaText(this.Handle, tag);

  /// <summary>Create a new PDF document.</summary>
  /// <param name="forms">Instance of <see cref="T:Patagames.Pdf.Net.PdfForms" /> class that will be associated with document</param>
  /// <returns>A instance of PDFDocument class represented a new PDF document. If failed, null is returned</returns>
  /// <seealso href="https://pdfium.patagames.com/c-pdf-library/">Create PDF in C#</seealso>
  public static PdfDocument CreateNew(PdfForms forms = null)
  {
    IntPtr newDocument = Pdfium.FPDF_CreateNewDocument();
    PdfDocument document = !(newDocument == IntPtr.Zero) ? new PdfDocument(newDocument, (PdfAvailabilityProvider) null, (PdfCustomLoader) null) : throw Pdfium.ProcessLastError();
    if (forms != null)
    {
      document.FormFill = forms;
      forms.Init(document);
    }
    return document;
  }

  /// <summary>
  /// Copy the viewer preferences from other PDF document to current.
  /// </summary>
  /// <param name="sourceDoc">PDF document with the viewer preferences.</param>
  /// <returns>True for success, False for failure.</returns>
  public bool CopyViewerPreferences(PdfDocument sourceDoc)
  {
    return Pdfium.FPDF_CopyViewerPreferences(this.Handle, sourceDoc.Handle);
  }

  /// <summary>Get the size of a page by index.</summary>
  /// <param name="pageIndex">Page index, zero for the first page.</param>
  /// <returns>True for success. False for error (document or page not found)</returns>
  public FS_SIZEF GetPageSizeByIndex(int pageIndex)
  {
    double width;
    double height;
    return !Pdfium.FPDF_GetPageSizeByIndex(this.Handle, pageIndex, out width, out height) ? new FS_SIZEF(0.0f, 0.0f) : new FS_SIZEF((float) width, (float) height);
  }

  /// <summary>
  /// This method is required for performing the document's additional-action.
  /// </summary>
  /// <param name="aaType">The type of the additional-actions. See <see cref="T:Patagames.Pdf.Enums.DocumentActionTypes" /> for detail</param>
  public void DoDocumentAAction(DocumentActionTypes aaType)
  {
    if (this.FormFill == null)
      return;
    Pdfium.FORM_DoDocumentAAction(this.FormFill.Handle, aaType);
  }

  /// <summary>Protect document with a password.</summary>
  /// <param name="openPassword">Set if require a password to open the document.</param>
  /// <param name="permissionsPassword">Set if require a password to change document restriction settings.</param>
  /// <param name="permissions">The permission flags.</param>
  /// <param name="encryptMetadata">Indicates whether the document-level metadata stream is to be encrypted.</param>
  /// <param name="algorithm">Defines data transformations that cannot be easily reversed by unauthorized users.</param>
  /// <remarks>
  /// <para>You can add a password to a PDF document to limit access and restrict certain features, such as printing, copying and editing.</para>
  /// <para>There are two kinds of passwords that could be applied to a PDF file: a Document Open password
  /// and a Permissions password. When you set a Document Open password, anyone who tries to open
  /// the PDF file must type in the password that you specified. When you set a Permissions password,
  /// anyone who wants to change the restrictions must type the Permissions password. Please note
  /// that if a PDF is secured with both types of passwords, it can be opened with either password, but
  /// only the Permissions password allows you to change the restrictions.</para>
  /// <para>Security settings will not be applied to the document until you save the document. You will be able to continue change security settings until you close the document.</para>
  /// <note type="note">RC4 is a copyrighted, proprietary algorithm of RSA Security, Inc. Independent
  /// software vendors may be required to license RC4 to develop software that encrypts
  /// or decrypts PDF documents.</note>
  /// </remarks>
  public void SetPasswordProtection(
    string openPassword,
    string permissionsPassword = null,
    PdfUserAccessPermission permissions = PdfUserAccessPermission.PermitAll,
    bool encryptMetadata = true,
    EncriptionAlgorithm algorithm = EncriptionAlgorithm.AES128)
  {
    if ((openPassword ?? "") == "" && (permissionsPassword ?? "") == "")
    {
      if (this.Root == null || !this.Root.ContainsKey("PTG-397EF413-659F-460D-95FE-78727C1CBBCF"))
        return;
      this.Root.Remove("PTG-397EF413-659F-460D-95FE-78727C1CBBCF");
    }
    else
    {
      PdfTypeDictionary pdfTypeDictionary1 = PdfTypeDictionary.Create();
      pdfTypeDictionary1["Filter"] = (PdfTypeBase) PdfTypeName.Create("Standard");
      pdfTypeDictionary1["P"] = (PdfTypeBase) PdfTypeNumber.Create((int) permissions);
      if (!encryptMetadata)
        pdfTypeDictionary1["EncryptMetadata"] = (PdfTypeBase) PdfTypeBoolean.Create(encryptMetadata);
      pdfTypeDictionary1["UserPassword"] = (PdfTypeBase) PdfTypeString.Create(openPassword);
      pdfTypeDictionary1["OwnerPassword"] = (PdfTypeBase) PdfTypeString.Create(permissionsPassword);
      switch (algorithm)
      {
        case EncriptionAlgorithm.AES128:
          pdfTypeDictionary1["V"] = (PdfTypeBase) PdfTypeNumber.Create(4);
          pdfTypeDictionary1["R"] = (PdfTypeBase) PdfTypeNumber.Create(4);
          pdfTypeDictionary1["Length"] = (PdfTypeBase) PdfTypeNumber.Create(128 /*0x80*/);
          pdfTypeDictionary1["StrF"] = (PdfTypeBase) PdfTypeName.Create("StdCF");
          pdfTypeDictionary1["StmF"] = (PdfTypeBase) PdfTypeName.Create("StdCF");
          pdfTypeDictionary1["CF"] = (PdfTypeBase) PdfTypeDictionary.Create();
          PdfTypeDictionary pdfTypeDictionary2 = PdfTypeDictionary.Create();
          pdfTypeDictionary1["CF"].As<PdfTypeDictionary>()["StdCF"] = (PdfTypeBase) pdfTypeDictionary2;
          pdfTypeDictionary2["Length"] = (PdfTypeBase) PdfTypeNumber.Create(16 /*0x10*/);
          pdfTypeDictionary2["CFM"] = (PdfTypeBase) PdfTypeName.Create("AESV2");
          pdfTypeDictionary2["AuthEvent"] = (PdfTypeBase) PdfTypeName.Create("DocOpen");
          break;
        case EncriptionAlgorithm.AES256:
          pdfTypeDictionary1["V"] = (PdfTypeBase) PdfTypeNumber.Create(5);
          pdfTypeDictionary1["R"] = (PdfTypeBase) PdfTypeNumber.Create(5);
          pdfTypeDictionary1["Length"] = (PdfTypeBase) PdfTypeNumber.Create(256 /*0x0100*/);
          pdfTypeDictionary1["StrF"] = (PdfTypeBase) PdfTypeName.Create("StdCF");
          pdfTypeDictionary1["StmF"] = (PdfTypeBase) PdfTypeName.Create("StdCF");
          pdfTypeDictionary1["CF"] = (PdfTypeBase) PdfTypeDictionary.Create();
          PdfTypeDictionary pdfTypeDictionary3 = PdfTypeDictionary.Create();
          pdfTypeDictionary1["CF"].As<PdfTypeDictionary>()["StdCF"] = (PdfTypeBase) pdfTypeDictionary3;
          pdfTypeDictionary3["Length"] = (PdfTypeBase) PdfTypeNumber.Create(32 /*0x20*/);
          pdfTypeDictionary3["CFM"] = (PdfTypeBase) PdfTypeName.Create("AESV3");
          pdfTypeDictionary3["AuthEvent"] = (PdfTypeBase) PdfTypeName.Create("DocOpen");
          break;
        case EncriptionAlgorithm.ARCFour128:
          pdfTypeDictionary1["V"] = (PdfTypeBase) PdfTypeNumber.Create(2);
          pdfTypeDictionary1["R"] = (PdfTypeBase) PdfTypeNumber.Create(3);
          pdfTypeDictionary1["Length"] = (PdfTypeBase) PdfTypeNumber.Create(128 /*0x80*/);
          break;
        default:
          throw new NotImplementedException();
      }
      this.Root["PTG-397EF413-659F-460D-95FE-78727C1CBBCF"] = (PdfTypeBase) pdfTypeDictionary1;
    }
  }

  private static void Loader_LoadBlock(object sender, CustomLoadEventArgs e)
  {
    Stream stream = (Stream) ((object[]) ((PdfCustomLoader) sender).Tag)[0];
    object obj = ((object[]) ((PdfCustomLoader) sender).Tag)[1];
    if (obj != null)
    {
      lock (obj)
      {
        stream.Seek((long) e.Position, SeekOrigin.Begin);
        stream.Read(e.Buffer, 0, e.Buffer.Length);
      }
    }
    else
    {
      stream.Seek((long) e.Position, SeekOrigin.Begin);
      stream.Read(e.Buffer, 0, e.Buffer.Length);
    }
    e.ReturnValue = true;
  }
}
