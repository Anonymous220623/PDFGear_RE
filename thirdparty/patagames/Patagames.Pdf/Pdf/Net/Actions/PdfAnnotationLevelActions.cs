// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfAnnotationLevelActions
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;

#nullable disable
namespace Patagames.Pdf.Net.Actions;

/// <summary>Represents the additional actions in a PDF document.</summary>
public class PdfAnnotationLevelActions : PdfAAction
{
  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.PdfAction" /> to be performed when the cursor enters the annotation’s active area.
  /// </summary>
  public PdfAction AnnotationEnter
  {
    get => this.GetActionAt("E");
    set => this.SetActionAt("E", value);
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.PdfAction" /> to be performed when the cursor exits the annotation’s active area.
  /// </summary>
  public PdfAction AnnotationExit
  {
    get => this.GetActionAt("X");
    set => this.SetActionAt("X", value);
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.PdfAction" /> to be performed when the mouse button is pressed inside the annotation’s active area.
  /// </summary>
  public PdfAction AnnotationDown
  {
    get => this.GetActionAt("D");
    set => this.SetActionAt("D", value);
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.PdfAction" /> to be performed when the mouse button is released inside the annotation’s active area.
  /// </summary>
  /// <remarks>For backward compatibility, the <see cref="P:Patagames.Pdf.Net.Annotations.PdfWidgetAnnotation.Action" /> property in the <see cref="T:Patagames.Pdf.Net.Annotations.PdfWidgetAnnotation" /> class,
  /// if present, takes precedence over this property.</remarks>
  public PdfAction AnnotationUp
  {
    get => this.GetActionAt("U");
    set => this.SetActionAt("U", value);
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.PdfAction" /> to be performed when the annotation receives the input focus.
  /// </summary>
  public PdfAction AnnotationFocused
  {
    get => this.GetActionAt("Fo");
    set => this.SetActionAt("Fo", value);
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.PdfAction" /> to be performed when the annotation loses the input focus.
  /// </summary>
  public PdfAction AnnotationBlurred
  {
    get => this.GetActionAt("Bl");
    set => this.SetActionAt("Bl", value);
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.PdfAction" /> to be performed when the page containing the annotation is opened
  /// (for example, when the user navigates to it from the next or previous page or by means of a link annotation or bookmark item).
  /// The action is executed after the <see cref="P:Patagames.Pdf.Net.Actions.PdfPageLevelActions.PageOpened" /> action and the <see cref="P:Patagames.Pdf.Net.PdfDocument.OpenAction" /> in the <see cref="T:Patagames.Pdf.Net.PdfDocument" /> class, if such actions are present.
  /// </summary>
  public PdfAction AnnotationPageOpen
  {
    get => this.GetActionAt("PO");
    set => this.SetActionAt("PO", value);
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.PdfAction" /> to be performed when the page containing the annotation is closed
  /// (for example, when the user navigates to the next or previous page, or follows a link annotation or bookmark item).
  /// The action is executed before the <see cref="P:Patagames.Pdf.Net.Actions.PdfPageLevelActions.PageClosed" /> action, if present.
  /// </summary>
  public PdfAction AnnotationPageClose
  {
    get => this.GetActionAt("PC");
    set => this.SetActionAt("PC", value);
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.PdfAction" /> to be performed when the page containing the annotation becomes visible in the viewer application’s user interface.
  /// </summary>
  public PdfAction AnnotationPageVisible
  {
    get => this.GetActionAt("PV");
    set => this.SetActionAt("PV", value);
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.PdfAction" /> to be performed when the page containing the annotation is no longer visible in the viewer application’s user interface.
  /// </summary>
  public PdfAction AnnotationPageInvisible
  {
    get => this.GetActionAt("PI");
    set => this.SetActionAt("PI", value);
  }

  /// <summary>
  /// Creates new instance of <see cref="T:Patagames.Pdf.Net.Actions.PdfAnnotationLevelActions" />.
  /// </summary>
  /// <param name="document">Pdf document.</param>
  public PdfAnnotationLevelActions(PdfDocument document)
    : base(document)
  {
  }

  /// <summary>
  /// Creates a new instance of <see cref="T:Patagames.Pdf.Net.Actions.PdfAnnotationLevelActions" /> and initialize it with specified dictionary
  /// </summary>
  /// <param name="document">Pdf document.</param>
  /// <param name="dictionary">The dictionary or indirect dictionary</param>
  public PdfAnnotationLevelActions(PdfDocument document, PdfTypeBase dictionary)
    : base(document, dictionary)
  {
  }
}
