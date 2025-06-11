// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfUnknownAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represent the unsupported annotation type</summary>
/// <summary>
/// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfUnknownAnnotation" />.
/// </summary>
/// <param name="page">The PDF page associated with annotation.</param>
/// <param name="annot">The annotation dictionary or indirect dictionary.</param>
public class PdfUnknownAnnotation(PdfPage page, PdfTypeBase annot) : PdfAnnotation(page, annot)
{
}
