// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfDocumentLevelActions
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;

#nullable disable
namespace Patagames.Pdf.Net.Actions;

/// <summary>Represents the additional actions in a PDF document.</summary>
public class PdfDocumentLevelActions : PdfAAction
{
  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.Actions.PdfJavaScriptAction" /> to be performed before closing a document.
  /// </summary>
  public PdfJavaScriptAction DocumentWillClose
  {
    get => this.GetActionAt("WC") as PdfJavaScriptAction;
    set => this.SetActionAt("WC", (PdfAction) value);
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.Actions.PdfJavaScriptAction" /> to be performed before saving a document.
  /// </summary>
  public PdfJavaScriptAction DocumentWillSave
  {
    get => this.GetActionAt("WS") as PdfJavaScriptAction;
    set => this.SetActionAt("WS", (PdfAction) value);
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.Actions.PdfJavaScriptAction" /> to be performed after saving a document.
  /// </summary>
  public PdfJavaScriptAction DocumentSaved
  {
    get => this.GetActionAt("DS") as PdfJavaScriptAction;
    set => this.SetActionAt("DS", (PdfAction) value);
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.Actions.PdfJavaScriptAction" /> to be performed before printing a document.
  /// </summary>
  public PdfJavaScriptAction DocumentWillPrint
  {
    get => this.GetActionAt("WP") as PdfJavaScriptAction;
    set => this.SetActionAt("WP", (PdfAction) value);
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.Actions.PdfJavaScriptAction" /> to be performed  after printing a document.
  /// </summary>
  public PdfJavaScriptAction DocumentPrinted
  {
    get => this.GetActionAt("DP") as PdfJavaScriptAction;
    set => this.SetActionAt("DP", (PdfAction) value);
  }

  /// <summary>
  /// Creates new instance of <see cref="T:Patagames.Pdf.Net.Actions.PdfDocumentLevelActions" />.
  /// </summary>
  /// <param name="document">Pdf document.</param>
  public PdfDocumentLevelActions(PdfDocument document)
    : base(document)
  {
  }

  /// <summary>
  /// Creates a new instance of <see cref="T:Patagames.Pdf.Net.Actions.PdfDocumentLevelActions" /> and initialize it with specified dictionary
  /// </summary>
  /// <param name="document">Pdf document.</param>
  /// <param name="dictionary">The dictionary or indirect dictionary</param>
  public PdfDocumentLevelActions(PdfDocument document, PdfTypeBase dictionary)
    : base(document, dictionary)
  {
  }
}
