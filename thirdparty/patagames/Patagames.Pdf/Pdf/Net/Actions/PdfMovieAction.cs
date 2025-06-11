// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfMovieAction
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Actions;

/// <summary>
/// Represents a movie action. A movie action can be used to play a movie in a floating window or within the annotation rectangle of a movie annotation.
/// </summary>
public class PdfMovieAction : PdfAction
{
  private PdfIndirectList _list;

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfMovieAction" /> class.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  public PdfMovieAction(PdfDocument document)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create("Movie");
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfMovieAction" /> class with the movie annotation.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="movie">A movie annotation identifying the movie to be played.</param>
  public PdfMovieAction(PdfDocument document, PdfMovieAnnotation movie)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    if ((PdfWrapper) movie != (PdfWrapper) null && movie.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(movie.Dictionary.Handle) != IntPtr.Zero)
      throw new ArgumentException(string.Format(Error.err0067, (object) nameof (movie), (object) "object"));
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create("Movie");
    if (!((PdfWrapper) movie != (PdfWrapper) null))
      return;
    this._list.Add((PdfTypeBase) movie.Dictionary);
    this.Dictionary.SetIndirectAt("Annotation", this._list, (PdfTypeBase) movie.Dictionary);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfMovieAction" /> class with the moview title.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="title">The title of a movie annotation identifying the movie to be played.</param>
  public PdfMovieAction(PdfDocument document, string title)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create("Movie");
    if (title == null)
      return;
    this.Dictionary["T"] = (PdfTypeBase) PdfTypeString.Create(title, true, true);
  }

  /// <summary>Initializes a new instance of the PdfAction class.</summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="handle">Pdfium SDK handle that the action is bound to</param>
  public PdfMovieAction(PdfDocument document, IntPtr handle)
    : base(document, handle)
  {
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
  }
}
