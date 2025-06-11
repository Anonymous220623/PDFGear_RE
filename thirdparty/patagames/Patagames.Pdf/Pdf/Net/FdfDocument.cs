// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.FdfDocument
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using System;
using System.IO;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Represents a FDF (Forms Data Format) document. FDF is a file format for representing form data and annotations that are contained in a PDF form.
/// </summary>
/// <threadsafety>Any public static (Shared in Visual Basic) members of this type are thread safe. Any instance members are not guaranteed to be thread safe.</threadsafety>
public class FdfDocument : IDisposable
{
  private PdfTypeDictionary _root;

  /// <summary>
  /// Gets a value indicating whether the object has been disposed of.
  /// <value>true if the control has been disposed of; otherwise, false.</value>
  /// </summary>
  public bool IsDisposed { get; private set; }

  /// <summary>
  /// Gets the Pdfium SDK handle that the FDF document is bound to
  /// </summary>
  public IntPtr Handle { get; private set; }

  /// <summary>Gets FDF content as a string</summary>
  public string Content
  {
    get => this.Handle == IntPtr.Zero ? (string) null : Pdfium.FFDF_GetDocumentContent(this.Handle);
  }

  /// <summary>
  /// Gets a path to the PDF file spicified inside FDF document
  /// </summary>
  /// <returns>The source file or target file: the PDF document file that this FDF file was exported from or is intended to be imported into.</returns>
  public string PathToPdf
  {
    get => this.Handle == IntPtr.Zero ? (string) null : Pdfium.FFDF_GetPath(this.Handle);
  }

  /// <summary>Gets the root catalog of the FDF document</summary>
  public PdfTypeDictionary Root
  {
    get
    {
      if (this.Handle == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      IntPtr root = Pdfium.FFDF_GetRoot(this.Handle);
      if (root == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      if (this._root == null || this._root.IsDisposed || this._root.Handle != root)
        this._root = new PdfTypeDictionary(root);
      return this._root;
    }
  }

  /// <summary>
  /// Gets an array of <see cref="T:Patagames.Pdf.Net.FdfFieldItem" /> in this FDF document.
  /// </summary>
  public FdfFieldItem[] Fields
  {
    get
    {
      if (!this.Root.ContainsKey("FDF") || !this.Root["FDF"].Is<PdfTypeDictionary>())
        return (FdfFieldItem[]) null;
      PdfTypeDictionary pdfTypeDictionary1 = this.Root["FDF"].As<PdfTypeDictionary>();
      if (!pdfTypeDictionary1.ContainsKey(nameof (Fields)) || !pdfTypeDictionary1[nameof (Fields)].Is<PdfTypeArray>())
        return (FdfFieldItem[]) null;
      PdfTypeArray pdfTypeArray1 = pdfTypeDictionary1[nameof (Fields)].As<PdfTypeArray>();
      FdfFieldItem[] fields = new FdfFieldItem[pdfTypeArray1.Count];
      for (int index1 = 0; index1 < pdfTypeArray1.Count; ++index1)
      {
        if (pdfTypeArray1[index1].Is<PdfTypeDictionary>())
        {
          PdfTypeDictionary pdfTypeDictionary2 = pdfTypeArray1[index1].As<PdfTypeDictionary>();
          string name = (string) null;
          if (pdfTypeDictionary2.ContainsKey("T") && pdfTypeDictionary2["T"].Is<PdfTypeName>())
            name = pdfTypeDictionary2["T"].As<PdfTypeName>().Value;
          else if (pdfTypeDictionary2.ContainsKey("T") && pdfTypeDictionary2["T"].Is<PdfTypeString>())
            name = pdfTypeDictionary2["T"].As<PdfTypeString>().UnicodeString;
          string[] values = (string[]) null;
          if (pdfTypeDictionary2.ContainsKey("V") && pdfTypeDictionary2["V"].Is<PdfTypeName>())
            values = new string[1]
            {
              pdfTypeDictionary2["V"].As<PdfTypeName>().Value
            };
          else if (pdfTypeDictionary2.ContainsKey("V") && pdfTypeDictionary2["V"].Is<PdfTypeString>())
            values = new string[1]
            {
              pdfTypeDictionary2["V"].As<PdfTypeString>().UnicodeString
            };
          else if (pdfTypeDictionary2.ContainsKey("V") && pdfTypeDictionary2["V"].Is<PdfTypeArray>())
          {
            PdfTypeArray pdfTypeArray2 = pdfTypeDictionary2["V"].As<PdfTypeArray>();
            values = new string[pdfTypeArray2.Count];
            for (int index2 = 0; index2 < pdfTypeArray2.Count; ++index2)
            {
              if (pdfTypeArray2[index2].Is<PdfTypeName>())
                values[index2] = pdfTypeArray2[index2].As<PdfTypeName>().Value;
              else if (pdfTypeArray2[index2].Is<PdfTypeString>())
                values[index2] = pdfTypeArray2[index2].As<PdfTypeString>().UnicodeString;
            }
          }
          fields[index1] = new FdfFieldItem(name, values);
        }
      }
      return fields;
    }
  }

  /// <summary>Initializes a new instance of the PdfDocument class.</summary>
  private FdfDocument(IntPtr FdfHandle) => this.Handle = FdfHandle;

  /// <summary>Releases all resources used by the PdfDocument.</summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>Releases all resources used by the PdfImageObject.</summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !PdfCommon.IsInitialize)
      return;
    if (this.Handle != IntPtr.Zero)
      Pdfium.FFDF_CloseDocument(this.Handle);
    this.Handle = IntPtr.Zero;
    if (this._root != null)
      this._root.Dispose();
    this._root = (PdfTypeDictionary) null;
    this.IsDisposed = true;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  /// <summary>Finalize object</summary>
  ~FdfDocument()
  {
    if (PdfCommon.IsCheckForMemoryLeaks && !this.IsDisposed)
      throw new MemoryLeakException(nameof (FdfDocument));
  }

  /// <summary>Create empty FDF document.</summary>
  /// <returns>Instance of FDFDocument class.</returns>
  public static FdfDocument CreateNew()
  {
    IntPtr Handle = Pdfium.FFDF_CreateNew();
    return !(Handle == IntPtr.Zero) ? FdfDocument.FromHandle(Handle) : throw Pdfium.ProcessLastError();
  }

  /// <summary>
  /// Crreate an instance of <see cref="T:Patagames.Pdf.Net.FdfDocument" /> class from handle.
  /// </summary>
  /// <param name="Handle">Handle to Fdf Document</param>
  /// <returns>Instance of FDFDocument class</returns>
  public static FdfDocument FromHandle(IntPtr Handle) => new FdfDocument(Handle);

  /// <summary>Loads a FDF document from a file.</summary>
  /// <param name="path">Path to the FDF file (including extension)</param>
  /// <returns>Instance of FDFDocument class</returns>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">Error occured in Pdfium. See ErrorCode for detail</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.NoLicenseException">This exception thrown only in trial mode if document cannot be opened due to a license restrictions"</exception>
  public static FdfDocument Load(string path)
  {
    IntPtr FdfHandle = Pdfium.FFDF_LoadDocument(path);
    return !(FdfHandle == IntPtr.Zero) ? new FdfDocument(FdfHandle) : throw Pdfium.ProcessLastError();
  }

  /// <summary>Loads a fDF document from a memory.</summary>
  /// <param name="content">Pointer to a buffer containing the FDF document</param>
  /// <returns>Instance of FDFDocument class</returns>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">Error occured in Pdfium. See ErrorCode for detail</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.NoLicenseException">This exception thrown only in trial mode if document cannot be opened due to a license restrictions"</exception>
  public static FdfDocument Load(byte[] content)
  {
    IntPtr FdfHandle = Pdfium.FFDF_LoadMemDocument(content);
    return !(FdfHandle == IntPtr.Zero) ? new FdfDocument(FdfHandle) : throw Pdfium.ProcessLastError();
  }

  /// <summary>Loads the FDF document from the specified stream.</summary>
  /// <param name="stream">The stream containing the FDF document to load.</param>
  /// <param name="length">The length to read from stream. If not specified then entire stream will be readed.</param>
  /// <returns>Instance of PDFDocument class</returns>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">Error occured in PDFium. See ErrorCode for detail</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.NoLicenseException">This exception thrown only in trial mode if document cannot be opened due to a license restrictions"</exception>
  public static FdfDocument Load(Stream stream, int length = 0)
  {
    byte[] numArray = new byte[length > 0 ? checked ((IntPtr) length) : checked ((IntPtr) stream.Length)];
    stream.Read(numArray, 0, numArray.Length);
    return FdfDocument.Load(numArray);
  }
}
