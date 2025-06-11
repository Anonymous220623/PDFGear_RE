// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfOCGStateAction
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Actions;

/// <summary>
/// A set-OCG-state action (PDF 1.5) sets the state of one or more optional content groups.
/// </summary>
public class PdfOCGStateAction : PdfAction
{
  /// <summary>
  /// Gets or sets a flag indicating whether radio-button state relationships between optional content groups should be preserved  when the states in the State array are applied.
  /// </summary>
  /// <remarks>
  /// <para>If true, indicates that radio-button state relationships between optional content groups
  /// (as specified by the RBGroups entry in the current configuration dictionary) should be preserved
  /// when the states in the State array are applied.
  /// That is, if a group is set to ON(either by ON or Toggle) during processing of the State array,
  /// any other groups belonging to the same radio-button group are turned OFF.</para>
  /// <para>If a group is set to OFF, there is no effect on other group.</para>
  /// </remarks>
  /// <value>Default value: true.</value>
  public bool PreserveRB
  {
    get
    {
      return !this.Dictionary.ContainsKey(nameof (PreserveRB)) || !this.Dictionary[nameof (PreserveRB)].Is<PdfTypeBoolean>() || this.Dictionary[nameof (PreserveRB)].As<PdfTypeBoolean>().Value;
    }
    set => this.Dictionary[nameof (PreserveRB)] = (PdfTypeBase) PdfTypeBoolean.Create(value);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfMovieAction" /> class.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  public PdfOCGStateAction(PdfDocument document)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create("SetOCGState");
  }

  /// <summary>Initializes a new instance of the PdfAction class.</summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="handle">Pdfium SDK handle that the action is bound to</param>
  public PdfOCGStateAction(PdfDocument document, IntPtr handle)
    : base(document, handle)
  {
  }
}
