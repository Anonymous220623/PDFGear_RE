// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfCustomLoader
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.EventArguments;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Represents a custom access descriptor for loading PDF document.
/// </summary>
public class PdfCustomLoader : IDisposable
{
  private FPDF_FILEACCESS _fileAccess;

  /// <summary>Gets or sets a form fill environment object.</summary>
  public PdfForms FormFill { get; set; }

  /// <summary>
  /// Gets or sets a string used as the password for PDF file. If no password needed, empty or NULL can be used.
  /// </summary>
  public string Password { get; set; }

  /// <summary>Gets or sets user data.</summary>
  /// <remarks>
  /// Any type derived from the Object class can be assigned to this property.
  /// A common use for the Tag property is to store data that is closely associated with the loader.
  /// </remarks>
  public object Tag { get; set; }

  /// <summary>
  ///  SDK fire this event when it need to receive the next data block of PDF document.
  /// </summary>
  public event EventHandler<CustomLoadEventArgs> LoadBlock;

  /// <summary>Open and load a PDF document from a file.</summary>
  /// <returns>Instance of PDFDocument class</returns>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnknownErrorException">unknown error</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfFileNotFoundException">file not found or could not be opened</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.BadFormatException">file not in PDF format or corrupted</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.InvalidPasswordException">password required or incorrect password</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnsupportedSecuritySchemeException">unsupported security scheme</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfiumException">Error occured in PDFium. See ErrorCode for detail</exception>
  /// <remarks>The application should maintain the file resources being valid until the PDF document close.</remarks>
  public PdfDocument Load()
  {
    IntPtr DocHandle = Pdfium.FPDF_LoadCustomDocument(this._fileAccess, this.Password);
    PdfDocument document = !(DocHandle == IntPtr.Zero) ? new PdfDocument(DocHandle, (PdfAvailabilityProvider) null, this) : throw Pdfium.ProcessLastError();
    if (this.FormFill != null)
    {
      document.FormFill = this.FormFill;
      this.FormFill.Init(document);
    }
    return document;
  }

  /// <summary>Construct SetTimerEventArgs object</summary>
  /// <param name="fileLength">File length, in bytes.</param>
  /// <param name="userData">A custom object for all implementation specific data.</param>
  public PdfCustomLoader(uint fileLength, byte[] userData = null)
  {
    this._fileAccess = new FPDF_FILEACCESS(fileLength, userData);
    this._fileAccess.GetBlock = new GetBlockCallback(this.GetBlockData);
  }

  /// <summary>
  /// Releases all resources used by the <see cref="T:Patagames.Pdf.Net.PdfCustomLoader" />.
  /// </summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>
  /// Releases all resources used by the <see cref="T:Patagames.Pdf.Net.PdfCustomLoader" />.
  /// </summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    this._fileAccess.Dispose();
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  private bool GetBlockData(byte[] param, uint position, byte[] buf, uint size)
  {
    if (this.LoadBlock == null)
      return false;
    CustomLoadEventArgs e = new CustomLoadEventArgs(param, position, buf);
    this.LoadBlock((object) this, e);
    return e.ReturnValue;
  }
}
