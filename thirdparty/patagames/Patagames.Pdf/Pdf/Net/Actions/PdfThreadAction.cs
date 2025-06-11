// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfThreadAction
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
/// Represents a thread action. A thread action jumps to a specified bead on an article thread, in either the current document or a different one.
/// </summary>
public class PdfThreadAction : PdfAction
{
  private PdfFileSpecification _fileSpec;
  private PdfIndirectList _list;

  /// <summary>
  /// Gets or sets the file containing the thread. If this property is null, the thread is in the current file.
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
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfThreadAction" /> class with the document, index of the thread and the file specification.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="index">The zero-based index of the thread within the Threads array of its document’s catalog.</param>
  /// <param name="fileSpec">The file containing the thread. If this entry is absent, the thread is in the current file.</param>
  public PdfThreadAction(PdfDocument document, int index, PdfFileSpecification fileSpec = null)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create("Thread");
    this.Dictionary["D"] = (PdfTypeBase) PdfTypeNumber.Create(index);
    this.FileSpecification = fileSpec;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfThreadAction" /> class with the document, title of the thread and the file specification.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="title">The title of the thread as specified in its thread information dictionary. If two or more threads have the same title, the one appearing first in the document catalog’s Threads array is used.</param>
  /// <param name="fileSpec">The file containing the thread. If this entry is absent, the thread is in the current file.</param>
  public PdfThreadAction(PdfDocument document, string title, PdfFileSpecification fileSpec = null)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create("Thread");
    this.Dictionary["D"] = (PdfTypeBase) PdfTypeString.Create(title, true, true);
    this.FileSpecification = fileSpec;
  }

  /// <summary>Initializes a new instance of the PdfAction class.</summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="handle">Pdfium SDK handle that the action is bound to</param>
  public PdfThreadAction(PdfDocument document, IntPtr handle)
    : base(document, handle)
  {
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
  }
}
