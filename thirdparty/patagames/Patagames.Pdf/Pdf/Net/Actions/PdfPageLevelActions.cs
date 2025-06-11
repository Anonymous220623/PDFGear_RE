// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfPageLevelActions
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;

#nullable disable
namespace Patagames.Pdf.Net.Actions;

/// <summary>Represents the additional actions in a PDF document.</summary>
public class PdfPageLevelActions : PdfAAction
{
  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.PdfAction" /> to be performed  when the page is opened
  /// (for example, when the user navigates to it from the next or previous page or by means of a link annotation or bookmark item).
  /// This action is independent of any that may be defined by the <see cref="P:Patagames.Pdf.Net.PdfDocument.OpenAction" /> property in the <see cref="T:Patagames.Pdf.Net.PdfDocument" /> class
  /// and is executed after such an action.
  /// </summary>
  public PdfAction PageOpened
  {
    get => this.GetActionAt("O");
    set => this.SetActionAt("O", value);
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.PdfAction" /> to be performed when the page is closed
  /// (for example, when the user navigates to the next or previous page or follows a link annotation or an bookmark item).
  /// This action applies to the page being closed and is executed before any other page is opened.
  /// </summary>
  public PdfAction PageClosed
  {
    get => this.GetActionAt("C");
    set => this.SetActionAt("C", value);
  }

  /// <summary>
  /// Creates new instance of <see cref="T:Patagames.Pdf.Net.Actions.PdfPageLevelActions" />.
  /// </summary>
  /// <param name="document">Pdf document.</param>
  public PdfPageLevelActions(PdfDocument document)
    : base(document)
  {
  }

  /// <summary>
  /// Creates a new instance of <see cref="T:Patagames.Pdf.Net.Actions.PdfPageLevelActions" /> and initialize it with specified dictionary
  /// </summary>
  /// <param name="document">Pdf document.</param>
  /// <param name="dictionary">The dictionary or indirect dictionary</param>
  public PdfPageLevelActions(PdfDocument document, PdfTypeBase dictionary)
    : base(document, dictionary)
  {
  }
}
