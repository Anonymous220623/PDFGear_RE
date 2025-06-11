// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfLink
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents the link annotation on a PDF page.</summary>
public class PdfLink
{
  private PdfPage _page;
  private PdfDestination _destination;
  private PdfAction _action;
  private PdfTypeDictionary _dictionary;

  /// <summary>Gets the Pdfium SDK handle that the link is bound to</summary>
  public IntPtr Handle { get; private set; }

  /// <summary>Gets the link dictionary.</summary>
  public PdfTypeDictionary Dictionary
  {
    get
    {
      if (this.Handle == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      IntPtr obj_handle = Pdfium.FPDFDICT_Create();
      if (obj_handle != IntPtr.Zero)
        Pdfium.FPDFOBJ_Release(obj_handle);
      if (this._dictionary == null || this._dictionary.IsDisposed || this._dictionary.Handle != this.Handle)
        this._dictionary = new PdfTypeDictionary(this.Handle);
      return this._dictionary;
    }
  }

  /// <summary>Gets or set the destination assigned to a link.</summary>
  /// <remarks>Return null if there is no destination associated with the link,
  /// in this case the application should try <see cref="P:Patagames.Pdf.Net.PdfLink.Action" /> property</remarks>
  public PdfDestination Destination
  {
    get
    {
      if (this._destination != null)
        return this._destination;
      IntPtr dest = Pdfium.FPDFLink_GetDest(this._page.Document.Handle, this.Handle);
      if (dest != IntPtr.Zero)
        this._destination = new PdfDestination(this._page.Document, dest);
      return this._destination;
    }
    set
    {
      if (value == null && Pdfium.FPDFDICT_KeyExist(this.Handle, "Dest"))
        Pdfium.FPDFDICT_RemoveAt(this.Handle, "Dest");
      else if (value != null)
        Pdfium.FPDFDICT_SetAt(this.Handle, "Dest", value.GetForInsert(this._page.Document).Handle);
      this._destination = value;
    }
  }

  /// <summary>Gets the PDF action assigned to a link.</summary>
  public PdfAction Action
  {
    get
    {
      if (this._action != null)
        return this._action;
      IntPtr action = Pdfium.FPDFLink_GetAction(this.Handle);
      if (action != IntPtr.Zero)
        this._action = PdfAction.FromHandle(this._page.Document, action);
      return this._action;
    }
    set
    {
      if (this._action == value)
        return;
      this._action = value;
      if (this._action == null)
      {
        Pdfium.FPDFDICT_RemoveAt(this.Handle, "A");
      }
      else
      {
        if (this._action.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(this._action.Handle) != IntPtr.Zero)
          throw new ArgumentException(string.Format(Error.err0067, (object) "action", (object) "object"));
        PdfIndirectList list = PdfIndirectList.FromPdfDocument(this._action.Document);
        list.Add((PdfTypeBase) this._action.Dictionary);
        this.Dictionary.SetIndirectAt("A", list, (PdfTypeBase) this._action.Dictionary);
      }
    }
  }

  /// <summary>
  /// Gets the annotation rectangle. (Specified by the entry of annotation dictionary).
  /// </summary>
  public FS_RECTF AnnotationRect
  {
    get
    {
      FS_RECTF rect;
      Pdfium.FPDFLink_GetAnnotRect(this.Handle, out rect);
      return rect;
    }
    set => this.Dictionary["Rect"] = (PdfTypeBase) value.ToArray();
  }

  /// <summary>
  /// Get the collection of quadrilaterals of the link annotation
  /// </summary>
  public PdfQuadPointsCollection QuadPoints { get; private set; }

  /// <summary>Initializes a new instance of the PdfLink class.</summary>
  /// <param name="page">PDF page containig this link</param>
  /// <param name="handle">&gt;Pdfium SDK handle that the link is bound to.</param>
  internal PdfLink(PdfPage page, IntPtr handle)
  {
    this._page = page;
    this.Handle = handle;
    this.QuadPoints = new PdfQuadPointsCollection(this);
  }
}
