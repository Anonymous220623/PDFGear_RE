// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Wrappers.PdfBorderStyle
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Wrappers;

/// <summary>Represents border style.</summary>
/// <remarks>
/// An annotation may optionally be surrounded by a border when displayed or
/// printed. If present, the border is drawn completely inside the annotation rectangle.
/// In PDF 1.1, the characteristics of the border are specified by the Border
/// entry in the annotation dictionary. Beginning with
/// PDF 1.2, some types of annotations may instead specify their border characteristics
/// in a <strong>BorderStyle</strong> property.
/// <note type="note">If neither the Border nor the BS entry is present, the
/// border is drawn as a solid line with a width of 1 point.</note>
/// </remarks>
public class PdfBorderStyle : PdfWrapper
{
  /// <summary>Gets or sets the border width in points.</summary>
  /// <remarks>
  /// If this value is 0, no border is drawn. Default value: 1.
  /// </remarks>
  public float Width
  {
    get => !this.IsExists("W") ? 1f : this.Dictionary["W"].As<PdfTypeNumber>().FloatValue;
    set => this.Dictionary["W"] = (PdfTypeBase) PdfTypeNumber.Create(value);
  }

  /// <summary>Gets or sets border style.</summary>
  public BorderStyles Style
  {
    get
    {
      if (!this.IsExists("S"))
        return BorderStyles.Solid;
      BorderStyles result;
      if (Pdfium.GetEnumDescription<BorderStyles>(this.Dictionary["S"].As<PdfTypeName>().Value, out result))
        return result;
      throw new PdfParserException(string.Format(Error.err0045, (object) "S"));
    }
    set
    {
      string enumDescription = Pdfium.GetEnumDescription((Enum) value);
      this.Dictionary["S"] = !((enumDescription ?? "").Trim() == "") ? (PdfTypeBase) PdfTypeName.Create(enumDescription) : throw new ArgumentException(string.Format(Error.err0047, (object) "BorderStyle", (object) "are BorderStyles.Solid, BorderStyles.Dashed, BorderStyles.Beveled, BorderStyles.Inset, and BorderStyles.Underline"));
    }
  }

  /// <summary>
  /// Gets or sets an array defining a pattern of dashes and gaps to be used in drawing a dashed border.
  /// </summary>
  /// <remarks>
  /// The dash array is specified in the same format as in the line dash pattern parameter of the graphics state.
  /// The dash phase is not specified and is assumed to be 0. For example, a DashPattern
  /// property of[3 2] specifies a border drawn with 3-point dashes alternating with 2-point
  /// gaps. Default value: [3].
  /// </remarks>
  public float[] DashPattern
  {
    get
    {
      if (!this.IsExists("D"))
        return (float[]) null;
      PdfTypeArray pdfTypeArray = this.Dictionary["D"].As<PdfTypeArray>();
      float[] dashPattern = new float[pdfTypeArray.Count];
      for (int index = 0; index < dashPattern.Length; ++index)
      {
        if (pdfTypeArray[index] == null)
          throw new PdfParserException(string.Format(Error.err0045, (object) "D"));
        dashPattern[index] = pdfTypeArray[index].As<PdfTypeNumber>().FloatValue;
      }
      return dashPattern;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("D"))
      {
        this.Dictionary.Remove("D");
      }
      else
      {
        if (value == null)
          return;
        PdfTypeArray pdfTypeArray = PdfTypeArray.Create();
        for (int index = 0; index < value.Length; ++index)
          pdfTypeArray.Add((PdfTypeBase) PdfTypeNumber.Create(value[index]));
        this.Dictionary["D"] = (PdfTypeBase) pdfTypeArray;
      }
    }
  }

  /// <summary>
  /// Creates new instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfBorderStyle" />.
  /// </summary>
  public PdfBorderStyle() => this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Border");

  /// <summary>
  /// Creates a new instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfBorderStyle" /> and initialize it with specified dictionary
  /// </summary>
  /// <param name="dictionary">The dictionary or indirect dictionary</param>
  public PdfBorderStyle(PdfTypeBase dictionary)
    : base(dictionary)
  {
  }
}
