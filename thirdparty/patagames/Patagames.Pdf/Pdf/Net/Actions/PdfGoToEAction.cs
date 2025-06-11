// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfGoToEAction
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
/// Represents an embedded go-to action. An embedded go-to action is similar to a remote go-to action but allows jumping to or from a PDF file that is embedded in another PDF file.
/// </summary>
public class PdfGoToEAction : PdfAction
{
  private PdfDestination _destination;
  private PdfFileSpecification _fileSpec;
  private PdfIndirectList _list;

  /// <summary>
  /// Gets or sets the PdfDestination object associated with this action
  /// </summary>
  public PdfDestination Destination
  {
    get
    {
      if (this._destination != null)
        return this._destination;
      IntPtr dest = Pdfium.FPDFAction_GetDest(this.Document.Handle, this.Handle);
      if (dest == IntPtr.Zero)
      {
        string remoteDest = Pdfium.FPDFAction_GetRemoteDest(this.Handle);
        if (remoteDest == null)
          return (PdfDestination) null;
        this._destination = new PdfDestination((PdfDocument) null, remoteDest);
        return this._destination;
      }
      this._destination = new PdfDestination((PdfDocument) null, dest);
      return this._destination;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("D"))
        this.Dictionary.Remove("D");
      else if (value != null)
        this.Dictionary["D"] = value.GetForInsert(this.Document);
      this._destination = value;
    }
  }

  /// <summary>
  /// Gets or sets the root document of the target relative to the root document of
  /// the source. If this property is null, the source and target share the same root document.
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
  /// Gets a flag specifying whether to open the destination document in a new window.
  /// If this flag is false, the destination document replaces
  /// the current document in the same window. If this property is null, the viewer
  /// application should behave in accordance with the current user preference.
  /// </summary>
  public bool? NewWindow
  {
    get
    {
      if (!this.Dictionary.ContainsKey(nameof (NewWindow)))
        return new bool?();
      return !this.Dictionary[nameof (NewWindow)].Is<PdfTypeBoolean>() ? new bool?() : new bool?(this.Dictionary[nameof (NewWindow)].As<PdfTypeBoolean>().Value);
    }
    set
    {
      if (!value.HasValue && this.Dictionary.ContainsKey(nameof (NewWindow)))
      {
        this.Dictionary.Remove(nameof (NewWindow));
      }
      else
      {
        if (!value.HasValue)
          return;
        this.Dictionary[nameof (NewWindow)] = (PdfTypeBase) PdfTypeBoolean.Create(value.Value);
      }
    }
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfGoToEAction" /> class with the document, file specification and the destination.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="fileSpec">The file in which the destination is located.</param>
  /// <param name="destination">The destination to jump to.</param>
  public PdfGoToEAction(
    PdfDocument document,
    PdfFileSpecification fileSpec,
    PdfDestination destination)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create("GoToE");
    this.Destination = destination;
    this.FileSpecification = fileSpec;
  }

  /// <summary>Initializes a new instance of the PdfAction class.</summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="handle">Pdfium SDK handle that the action is bound to</param>
  public PdfGoToEAction(PdfDocument document, IntPtr handle)
    : base(document, handle)
  {
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
  }
}
