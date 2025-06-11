// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Wrappers.PdfBorderEffect
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

/// <summary>Represents a border effects.</summary>
/// <remarks>
/// Beginning with PDF 1.5, some annotations (<see cref="T:Patagames.Pdf.Net.Annotations.PdfFigureAnnotation" />, <see cref="T:Patagames.Pdf.Net.Annotations.PdfCircleAnnotation" />, and <see cref="T:Patagames.Pdf.Net.Annotations.PdfPolygonAnnotation" />) can have
/// a <see cref="T:Patagames.Pdf.Net.Wrappers.PdfBorderEffect" /> that specifies an effect to be applied
/// to the border of the annotations. Beginning with PDF 1.6, the <see cref="T:Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation" />
/// can also have a <see cref="T:Patagames.Pdf.Net.Wrappers.PdfBorderEffect" />.
/// </remarks>
public class PdfBorderEffect : PdfWrapper
{
  /// <summary>Gets or sets the border effect to apply.</summary>
  public BorderEffects Effect
  {
    get
    {
      if (!this.IsExists("S"))
        return BorderEffects.None;
      string str = this.Dictionary["S"].As<PdfTypeName>().Value;
      if (str.Equals("S", StringComparison.OrdinalIgnoreCase))
        return BorderEffects.None;
      if (str.Equals("C", StringComparison.OrdinalIgnoreCase))
        return BorderEffects.Cloudy;
      throw new PdfParserException(string.Format(Error.err0045, (object) "S"));
    }
    set
    {
      if (value != BorderEffects.None)
      {
        if (value != BorderEffects.Cloudy)
          throw new ArgumentException(string.Format(Error.err0047, (object) nameof (Effect), (object) "are BorderEffect.None and BorderEffect.Cloudy"));
        this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create("C");
      }
      else
        this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create("S");
    }
  }

  /// <summary>
  /// A number describing the intensity of the effect. Suggested values range from 0 to 2. Default value: 0.
  /// </summary>
  /// <remarks>
  /// Valid only if the value of <see cref="P:Patagames.Pdf.Net.Wrappers.PdfBorderEffect.Effect" /> property is <see cref="F:Patagames.Pdf.Enums.BorderEffects.Cloudy" />.
  /// </remarks>
  public int Intensity
  {
    get => !this.IsExists("I") ? 0 : this.Dictionary["I"].As<PdfTypeNumber>().IntValue;
    set => this.Dictionary["I"] = (PdfTypeBase) PdfTypeNumber.Create(value);
  }

  /// <summary>
  /// Creates new instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfBorderEffect" />.
  /// </summary>
  public PdfBorderEffect()
  {
  }

  /// <summary>
  /// Creates a new instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfBorderEffect" /> and initialize it with specified dictionary
  /// </summary>
  /// <param name="dictionary">The dictionary or indirect dictionary</param>
  public PdfBorderEffect(PdfTypeBase dictionary)
    : base(dictionary)
  {
  }
}
