// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfBookmark
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents the bookmark into a PDF document.</summary>
public class PdfBookmark
{
  private PdfDocument _doc;
  private PdfDestination _destination;
  private PdfAction _action;
  private PdfTypeDictionary _dictionary;

  /// <summary>
  /// Gets the Pdfium SDK handle that the bookmark is bound to
  /// </summary>
  public IntPtr Handle { get; private set; }

  /// <summary>Gets the bookmark's dictionary</summary>
  public PdfTypeDictionary Dictionary
  {
    get
    {
      if (this._dictionary == null || this._dictionary.IsDisposed || this._dictionary.Handle != this.Handle)
        this._dictionary = new PdfTypeDictionary(this.Handle);
      return this._dictionary;
    }
  }

  /// <summary>Gets the title of bookmark</summary>
  public string Title
  {
    get => Pdfium.FPDFBookmark_GetTitle(this.Handle);
    set => this.Dictionary[nameof (Title)] = (PdfTypeBase) PdfTypeString.Create(value ?? "", true);
  }

  /// <summary>Gets the parent bookmark of the current bookmark.</summary>
  public PdfBookmark Parent { get; private set; }

  /// <summary>
  /// Gets the collection of PdfBookmark objects assigned to the current bookmark.
  /// </summary>
  public PdfBookmarkCollections Childs { get; private set; }

  /// <summary>
  /// Gets or sets the PdfDestination object associated with this bookmark
  /// </summary>
  public PdfDestination Destination
  {
    get
    {
      if (this._destination != null)
        return this._destination;
      IntPtr dest = Pdfium.FPDFBookmark_GetDest(this._doc.Handle, this.Handle);
      if (dest == IntPtr.Zero)
        return (PdfDestination) null;
      this._destination = new PdfDestination(this._doc, dest);
      return this._destination;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("Dest"))
        this.Dictionary.Remove("Dest");
      else if (value != null)
        this.Dictionary["Dest"] = value.GetForInsert(this._doc);
      this._destination = value;
    }
  }

  /// <summary>
  /// Gets or sets the PdfAction object associated with this bookmark or null if no action.
  /// In this case, the application should try <see cref="P:Patagames.Pdf.Net.PdfBookmark.Destination" />
  /// </summary>
  public PdfAction Action
  {
    get
    {
      if (this._action != null)
        return this._action;
      IntPtr action = Pdfium.FPDFBookmark_GetAction(this.Handle);
      if (action == IntPtr.Zero)
        return (PdfAction) null;
      this._action = PdfAction.FromHandle(this._doc, action);
      return this._action;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("A"))
        this.Dictionary.Remove("A");
      else if (value != null)
      {
        if (value.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(value.Dictionary.Handle) != IntPtr.Zero)
          throw new ArgumentException(string.Format(Error.err0067, (object) "action", (object) "object"));
        PdfIndirectList list = PdfIndirectList.FromPdfDocument(this._doc);
        list.Add((PdfTypeBase) value.Dictionary);
        this.Dictionary.SetIndirectAt("A", list, (PdfTypeBase) value.Dictionary);
      }
      this._action = value;
    }
  }

  /// <summary>Initializes a new instance of the PdfBookmark class.</summary>
  /// <param name="document">Document which contains this collection of destinations.</param>
  /// <param name="handle">Pdfium SDK handle that the bookmark is bound to</param>
  /// <param name="parent">Parent bookmark or null for top-level bookmark</param>
  internal PdfBookmark(PdfDocument document, IntPtr handle, PdfBookmark parent)
  {
    this._doc = document;
    this.Handle = handle;
    this.Parent = parent;
    this.Childs = new PdfBookmarkCollections(this._doc, this);
  }
}
