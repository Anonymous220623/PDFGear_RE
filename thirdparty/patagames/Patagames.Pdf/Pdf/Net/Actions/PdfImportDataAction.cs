// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfImportDataAction
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Actions;

/// <summary>
/// An import-data action imports Forms Data Format (FDF) data into the document’s interactive form from a specified file.
/// </summary>
public class PdfImportDataAction : PdfAction
{
  private PdfFileSpecification _fileSpec;
  private PdfIndirectList _list;

  /// <summary>
  /// Gets or sets the FDF file from which to import the data.
  /// </summary>
  public PdfFileSpecification FileSpecification
  {
    get
    {
      if (!this.Dictionary.ContainsKey("F"))
        return (PdfFileSpecification) null;
      if ((PdfWrapper) this._fileSpec == (PdfWrapper) null || this._fileSpec.Dictionary.IsDisposed)
        this._fileSpec = this.Dictionary["F"].Is<PdfTypeDictionary>() ? new PdfFileSpecification(this.Document, this.Dictionary["F"]) : (PdfFileSpecification) null;
      return this._fileSpec;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey("F"))
        this.Dictionary.Remove("F");
      else if ((PdfWrapper) value != (PdfWrapper) null)
      {
        if (value.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(value.Dictionary.Handle) != IntPtr.Zero)
          throw new ArgumentException(string.Format(Error.err0067, (object) "file specification", (object) "object"));
        this._list.Add((PdfTypeBase) value.Dictionary);
        this.Dictionary.SetIndirectAt("F", this._list, (PdfTypeBase) value.Dictionary);
      }
      this._fileSpec = value;
    }
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfImportDataAction" /> class with the document and a file specification.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="fileSpec">The FDF file from which to import the data. </param>
  public PdfImportDataAction(PdfDocument document, PdfFileSpecification fileSpec)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    if ((PdfWrapper) fileSpec == (PdfWrapper) null)
      throw new ArgumentNullException(nameof (fileSpec));
    if (fileSpec.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(fileSpec.Dictionary.Handle) != IntPtr.Zero)
      throw new ArgumentException(string.Format(Error.err0067, (object) nameof (fileSpec), (object) "object"));
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create("ImportData");
    this.FileSpecification = fileSpec;
  }

  /// <summary>Initializes a new instance of the PdfAction class.</summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="handle">Pdfium SDK handle that the action is bound to</param>
  public PdfImportDataAction(PdfDocument document, IntPtr handle)
    : base(document, handle)
  {
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
  }
}
