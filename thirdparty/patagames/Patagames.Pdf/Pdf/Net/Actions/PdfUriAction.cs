// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfUriAction
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Actions;

/// <summary>Represents a uniform resource identifier action.</summary>
public class PdfUriAction : PdfAction
{
  /// <summary>
  /// Gets the document-level information for <see cref="T:Patagames.Pdf.Net.Actions.PdfUriAction" />
  /// </summary>
  public string BaseUri => this.Document.BaseUri;

  /// <summary>
  /// Gets or sets the uniform resource identifier to resolve.
  /// </summary>
  public string Uri
  {
    get
    {
      if (!this.Dictionary.ContainsKey("URI"))
        return (string) null;
      return !this.Dictionary["URI"].Is<PdfTypeString>() ? (string) null : this.Dictionary["URI"].As<PdfTypeString>().AnsiString;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("URI"))
      {
        this.Dictionary.Remove("URI");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["URI"] = (PdfTypeBase) PdfTypeString.Create(value);
      }
    }
  }

  /// <summary>Gets URL assigned to the current action.</summary>
  public string ActionUrl => Pdfium.FPDFAction_GetURIPath(this.Document.Handle, this.Handle);

  /// <summary>
  /// Gets or sets a flag specifying whether to track the mouse position when the URI is resolved (see below).
  /// </summary>
  public bool IsMap
  {
    get
    {
      return this.Dictionary.ContainsKey(nameof (IsMap)) && this.Dictionary[nameof (IsMap)].Is<PdfTypeBoolean>() && this.Dictionary[nameof (IsMap)].As<PdfTypeBoolean>().Value;
    }
    set => this.Dictionary[nameof (IsMap)] = (PdfTypeBase) PdfTypeBoolean.Create(value);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfUriAction" /> class with the document and the uri.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="uri">The uniform resource identifier to resolve.</param>
  public PdfUriAction(PdfDocument document, string uri)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    if (uri == null)
      throw new ArgumentNullException(nameof (uri));
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create("URI");
    this.Uri = uri;
  }

  /// <summary>Initializes a new instance of the PdfAction class.</summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="handle">Pdfium SDK handle that the action is bound to</param>
  public PdfUriAction(PdfDocument document, IntPtr handle)
    : base(document, handle)
  {
  }
}
