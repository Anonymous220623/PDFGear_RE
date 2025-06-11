// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfCaretAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represents a caret annotation</summary>
/// <remarks>
/// A caret annotation (PDF 1.5) is a visual symbol that indicates the presence of text edits.
/// </remarks>
public class PdfCaretAnnotation : PdfMarkupAnnotation
{
  /// <summary>Gets or sets an inner rectangle of the annotation.</summary>
  /// <remarks>
  /// The inner rectangle is the actual boundaries of the of the underlying caret.
  /// Such a difference can occur, for example, when a paragraph symbol specified by <see cref="P:Patagames.Pdf.Net.Annotations.PdfCaretAnnotation.IsSymbol" /> property is displayed along with the caret.
  /// </remarks>
  public float[] InnerRectangle
  {
    get
    {
      if (!this.IsExists("RD"))
        return (float[]) null;
      float[] innerRectangle = new float[4];
      PdfTypeArray pdfTypeArray = this.Dictionary["RD"].As<PdfTypeArray>();
      int count = pdfTypeArray.Count;
      if (count > 0)
        innerRectangle[0] = (pdfTypeArray[0] as PdfTypeNumber).FloatValue;
      if (count > 1)
        innerRectangle[1] = (pdfTypeArray[1] as PdfTypeNumber).FloatValue;
      if (count > 2)
        innerRectangle[2] = (pdfTypeArray[2] as PdfTypeNumber).FloatValue;
      if (count > 3)
        innerRectangle[3] = (pdfTypeArray[3] as PdfTypeNumber).FloatValue;
      return innerRectangle;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("RD"))
      {
        this.Dictionary.Remove("RD");
      }
      else
      {
        if (value == null)
          return;
        PdfTypeArray pdfTypeArray = PdfTypeArray.Create();
        if (value.Length != 0)
          pdfTypeArray.Add((PdfTypeBase) PdfTypeNumber.Create(value[0]));
        if (value.Length > 1)
          pdfTypeArray.Add((PdfTypeBase) PdfTypeNumber.Create(value[1]));
        if (value.Length > 2)
          pdfTypeArray.Add((PdfTypeBase) PdfTypeNumber.Create(value[2]));
        if (value.Length > 3)
          pdfTypeArray.Add((PdfTypeBase) PdfTypeNumber.Create(value[3]));
        this.Dictionary["RD"] = (PdfTypeBase) pdfTypeArray;
      }
    }
  }

  /// <summary>
  /// Gets or sets a value indicating whether a new paragraph symbol (¶) is associated with the caret.
  /// </summary>
  /// <remarks>
  /// <list type="table">
  /// <item><term><strong>true</strong></term><description>A new paragraph symbol (¶) should be associated with the caret.</description></item>
  /// <item><term><strong>false</strong></term><description>No symbol should be associated with the caret.</description></item>
  /// </list>
  /// Default value: <strong>false</strong>.
  /// </remarks>
  public bool IsSymbol
  {
    get
    {
      return this.IsExists("Sy") && this.Dictionary["Sy"].As<PdfTypeName>().Value.Equals("P", StringComparison.OrdinalIgnoreCase);
    }
    set => this.Dictionary["Sy"] = (PdfTypeBase) PdfTypeName.Create(value ? "P" : "None");
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfCaretAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfCaretAnnotation(PdfPage page)
    : base(page)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("Caret");
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfCaretAnnotation" /> with parameters as specified.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="caretRect">The rectangle in which caret should be inscribed.</param>
  /// <param name="color">The caret color.</param>
  /// <param name="showSymbol">A flag indicating whether the symbol "new paragraph" should be displayed.</param>
  public PdfCaretAnnotation(PdfPage page, FS_RECTF caretRect, FS_COLOR color, bool showSymbol)
    : this(page)
  {
    this.Color = color;
    this.IsSymbol = showSymbol;
    if (showSymbol)
    {
      FS_RECTF fsRectf = AnnotDrawing.CalcBBox((IEnumerable) AnnotDrawing.CreateParagraph(new FS_COLOR(0), new FS_COLOR(0)));
      caretRect.right += fsRectf.Width;
      this.InnerRectangle = new float[4]
      {
        0.0f,
        0.0f,
        fsRectf.Width,
        0.0f
      };
    }
    this.Rectangle = caretRect;
    this.RegenerateAppearances();
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfCaretAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfCaretAnnotation(PdfPage page, PdfTypeBase dictionary)
    : base(page, dictionary)
  {
  }

  /// <summary>
  /// Re-creates the appearance of the annotation based on its properties
  /// </summary>
  /// <remarks>
  /// When the annotation does not have an Appearance stream (old style annotations),
  /// they are not rendered by the Pdfium engine.
  /// Calling this function creates an appearance stream based on the default parameters and the properties of this annotation.
  /// </remarks>
  public override void RegenerateAppearances()
  {
    this.CreateEmptyAppearance(AppearanceStreamModes.Normal);
    float[] innerRectangle1 = this.InnerRectangle;
    FS_RECTF rectangle = this.Rectangle;
    FS_RECTF innerRectangle2 = AnnotDrawing.GetInnerRectangle(rectangle, innerRectangle1);
    FS_COLOR fsColor = new FS_COLOR(this.Opacity, this.Color);
    FS_COLOR strokeColor = new FS_COLOR(this.Opacity, 0.0f, 0.0f, 0.0f);
    foreach (PdfPageObject pdfPageObject in AnnotDrawing.CreateCaret(fsColor, innerRectangle2))
      this.NormalAppearance.Add(pdfPageObject);
    if (this.IsSymbol)
    {
      List<PdfPathObject> paragraph = AnnotDrawing.CreateParagraph(fsColor, strokeColor);
      FS_RECTF fsRectf = AnnotDrawing.CalcBBox((IEnumerable) paragraph);
      foreach (PdfPathObject pdfPathObject in paragraph)
      {
        pdfPathObject.TransformPath(1f, 0.0f, 0.0f, 1f, rectangle.right - fsRectf.Width, rectangle.bottom + innerRectangle2.Height / 2f);
        pdfPathObject.CalcBoundingBox();
        this.NormalAppearance.Add((PdfPageObject) pdfPathObject);
      }
    }
    this.GenerateAppearance(AppearanceStreamModes.Normal);
    PdfTypeDictionary.Create(Pdfium.FPDFOBJ_GetDict(Pdfium.FPDFAnnot_GetAppearanceStream(this.Dictionary.Handle, AppearanceStreamModes.Normal))).SetAt("BBox", (PdfTypeBase) rectangle.ToArray());
    this.Rectangle = rectangle;
  }
}
