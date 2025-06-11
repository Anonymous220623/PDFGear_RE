// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.Pdf3DViewAction
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Actions;

/// <summary>
/// A go-to-3D-view action identifies a 3D annotation and specifies a view for the annotation to use
/// </summary>
public class Pdf3DViewAction : PdfAction
{
  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.Pdf3DViewAction" /> class.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  public Pdf3DViewAction(PdfDocument document)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create("GoTo3DView");
  }

  /// <summary>Initializes a new instance of the PdfAction class.</summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="handle">Pdfium SDK handle that the action is bound to</param>
  public Pdf3DViewAction(PdfDocument document, IntPtr handle)
    : base(document, handle)
  {
  }
}
