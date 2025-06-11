// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfLaunchAction
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
/// Represents a launch action. A launch action launches an application or opens or prints a document.
/// </summary>
public class PdfLaunchAction : PdfAction
{
  private PdfFileSpecification _fileSpec;
  private PdfIndirectList _list;

  /// <summary>
  /// Gets or sets the application to be launched or the document to be opened or printed.
  /// If this property is null and the viewer application does not understand any of the alternative entries, it should do nothing.
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
  /// This property is ignored if the file designated by the <see cref="P:Patagames.Pdf.Net.Actions.PdfLaunchAction.FileSpecification" /> property is not a PDF document.
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
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfLaunchAction" /> class with the document and the file specification.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="fileSpec">The file in which the destination is located.</param>
  public PdfLaunchAction(PdfDocument document, PdfFileSpecification fileSpec)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create("Launch");
    this.FileSpecification = fileSpec;
  }

  /// <summary>Initializes a new instance of the PdfAction class.</summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="handle">Pdfium SDK handle that the action is bound to</param>
  public PdfLaunchAction(PdfDocument document, IntPtr handle)
    : base(document, handle)
  {
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
  }
}
