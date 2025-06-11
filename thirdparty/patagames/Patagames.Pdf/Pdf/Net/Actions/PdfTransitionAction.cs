// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfTransitionAction
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Actions;

/// <summary>
/// A transition action can be used to control drawing during a sequence of actions.
/// </summary>
public class PdfTransitionAction : PdfAction
{
  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfTransitionAction" /> class.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  public PdfTransitionAction(PdfDocument document)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create("Trans");
  }

  /// <summary>Initializes a new instance of the PdfAction class.</summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="handle">Pdfium SDK handle that the action is bound to</param>
  public PdfTransitionAction(PdfDocument document, IntPtr handle)
    : base(document, handle)
  {
  }
}
