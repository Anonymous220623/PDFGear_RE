// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfGoToAction
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Actions;

/// <summary>
/// Represents a go-to action. A go-to action changes the view to a specified destination (page, location, and magnification factor).
/// </summary>
public class PdfGoToAction : PdfAction
{
  private PdfDestination _destination;

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
        return (PdfDestination) null;
      this._destination = new PdfDestination(this.Document, dest);
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
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfGoToAction" /> class with the document and the destination.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="destination">The destination to jump to.</param>
  public PdfGoToAction(PdfDocument document, PdfDestination destination)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create("GoTo");
    this.Destination = destination;
  }

  /// <summary>Initializes a new instance of the PdfAction class.</summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="handle">Pdfium SDK handle that the action is bound to</param>
  public PdfGoToAction(PdfDocument document, IntPtr handle)
    : base(document, handle)
  {
  }
}
