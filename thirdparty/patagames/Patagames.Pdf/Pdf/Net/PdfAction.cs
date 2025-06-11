// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfAction
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Actions;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents the PDF action in a PDF document.</summary>
public class PdfAction
{
  private PdfDocument _doc;
  private PdfTypeDictionary _dictionary;
  private PdfActionCollection _next;

  /// <summary>
  /// Gets the Pdfium SDK handle that the action is bound to
  /// </summary>
  public IntPtr Handle { get; private set; }

  /// <summary>Gets the action's dictionary.</summary>
  public PdfTypeDictionary Dictionary
  {
    get
    {
      if (this.Handle == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      if (!Pdfium.IsFullAPI)
        throw new NoLicenseException(536870917U /*0x20000005*/, Error.errLicense3);
      if (this._dictionary == null || this._dictionary.IsDisposed || this._dictionary.Handle != this.Handle)
        this._dictionary = new PdfTypeDictionary(this.Handle);
      return this._dictionary;
    }
  }

  /// <summary>
  /// Gets type of current action. See <see cref="T:Patagames.Pdf.Enums.ActionTypes" /> for details.
  /// </summary>
  public ActionTypes ActionType => Pdfium.FPDFAction_GetType(this.Handle);

  /// <summary>Gets the document associated with this action.</summary>
  public PdfDocument Document => this._doc;

  /// <summary>
  /// Gets the sequence of actions to be performed after the action represented by this class.
  /// </summary>
  public PdfActionCollection Next
  {
    get
    {
      if (this._next == null)
        this._next = new PdfActionCollection(this);
      return this._next;
    }
  }

  /// <summary>Initializes a new instance of the PdfAction class.</summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="handle">Pdfium SDK handle that the action is bound to</param>
  protected PdfAction(PdfDocument document, IntPtr handle)
  {
    this._doc = document != null ? document : throw new ArgumentNullException(nameof (document));
    this.Handle = handle;
  }

  /// <summary>
  /// Create a new instance of the action class depending on its type.
  /// </summary>
  /// <param name="document">Pdf document</param>
  /// <param name="handle">A Pdfium handle of an action.</param>
  /// <returns>An instance of the children class, or <see cref="T:Patagames.Pdf.Net.PdfAction" />, if the action could not be mapped to a more specific type.</returns>
  public static PdfAction FromHandle(PdfDocument document, IntPtr handle)
  {
    long num = (long) (Pdfium.FPDFAction_GetType(handle) - 1UL);
    if ((ulong) num <= 17UL)
    {
      switch ((uint) num)
      {
        case 0:
          return (PdfAction) new PdfGoToAction(document, handle);
        case 1:
          return (PdfAction) new PdfGoToRAction(document, handle);
        case 2:
          return (PdfAction) new PdfUriAction(document, handle);
        case 3:
          return (PdfAction) new PdfLaunchAction(document, handle);
        case 4:
          return (PdfAction) new PdfThreadAction(document, handle);
        case 5:
          return (PdfAction) new PdfGoToEAction(document, handle);
        case 6:
          return (PdfAction) new PdfSoundAction(document, handle);
        case 7:
          return (PdfAction) new PdfMovieAction(document, handle);
        case 8:
          return (PdfAction) new PdfHideAction(document, handle);
        case 9:
          return (PdfAction) new PdfNamedAction(document, handle);
        case 10:
          return (PdfAction) new PdfSubmitFormAction(document, handle);
        case 11:
          return (PdfAction) new PdfResetFormAction(document, handle);
        case 12:
          return (PdfAction) new PdfImportDataAction(document, handle);
        case 13:
          return (PdfAction) new PdfJavaScriptAction(document, handle);
        case 14:
          return (PdfAction) new PdfOCGStateAction(document, handle);
        case 15:
          return (PdfAction) new PdfRenditionAction(document, handle);
        case 16 /*0x10*/:
          return (PdfAction) new PdfTransitionAction(document, handle);
        case 17:
          return (PdfAction) new Pdf3DViewAction(document, handle);
      }
    }
    return new PdfAction(document, handle);
  }
}
