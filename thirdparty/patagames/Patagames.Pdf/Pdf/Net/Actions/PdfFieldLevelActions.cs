// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfFieldLevelActions
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;

#nullable disable
namespace Patagames.Pdf.Net.Actions;

/// <summary>Represents the additional actions in a PDF document.</summary>
public class PdfFieldLevelActions : PdfAAction
{
  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.Actions.PdfJavaScriptAction" /> to be performed when the user types a keystroke into a text field or combo box or modifies the selection
  /// in a scrollable list box. This action can check the keystroke for validity and reject or modify it.
  /// </summary>
  public PdfJavaScriptAction FieldKeystroke
  {
    get => this.GetActionAt("K") as PdfJavaScriptAction;
    set => this.SetActionAt("K", (PdfAction) value);
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.Actions.PdfJavaScriptAction" /> to be performed before the field is formatted to display its current value. This action can modify the field’s value before formatting.
  /// </summary>
  public PdfJavaScriptAction FieldFormat
  {
    get => this.GetActionAt("F") as PdfJavaScriptAction;
    set => this.SetActionAt("F", (PdfAction) value);
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.Actions.PdfJavaScriptAction" /> to be performed when the field’s value is changed. This action can check the new value for validity.
  /// </summary>
  public PdfJavaScriptAction FieldValidate
  {
    get => this.GetActionAt("V") as PdfJavaScriptAction;
    set => this.SetActionAt("V", (PdfAction) value);
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.Actions.PdfJavaScriptAction" /> to be performed to recalculate the value of this field when that of another field changes.
  /// </summary>
  public PdfJavaScriptAction FieldCalculate
  {
    get => this.GetActionAt("C") as PdfJavaScriptAction;
    set => this.SetActionAt("C", (PdfAction) value);
  }

  /// <summary>
  /// Creates new instance of <see cref="T:Patagames.Pdf.Net.Actions.PdfFieldLevelActions" />.
  /// </summary>
  /// <param name="document">Pdf document.</param>
  public PdfFieldLevelActions(PdfDocument document)
    : base(document)
  {
  }

  /// <summary>
  /// Creates a new instance of <see cref="T:Patagames.Pdf.Net.Actions.PdfFieldLevelActions" /> and initialize it with specified dictionary
  /// </summary>
  /// <param name="document">Pdf document.</param>
  /// <param name="dictionary">The dictionary or indirect dictionary</param>
  public PdfFieldLevelActions(PdfDocument document, PdfTypeBase dictionary)
    : base(document, dictionary)
  {
  }
}
