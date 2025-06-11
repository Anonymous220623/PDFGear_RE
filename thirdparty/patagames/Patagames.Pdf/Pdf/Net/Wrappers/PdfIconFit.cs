// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Wrappers.PdfIconFit
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

/// <summary>
/// Represents an icon fit dictionary specifying how to display the button’s icon within the annotation rectangle of its widget annotation.
/// </summary>
public class PdfIconFit : PdfWrapper
{
  /// <summary>
  /// Gets or sets the circumstances under which the icon should be scaled inside the annotation rectangle.
  /// </summary>
  public IconScaleModes ScaleMode
  {
    get
    {
      if (!this.IsExists("SW"))
        return IconScaleModes.Always;
      IconScaleModes result;
      if (Pdfium.GetEnumDescription<IconScaleModes>(this.Dictionary["SW"].As<PdfTypeName>().Value, out result))
        return result;
      throw new PdfParserException(string.Format(Error.err0045, (object) "SW"));
    }
    set
    {
      string enumDescription = Pdfium.GetEnumDescription((Enum) value);
      this.Dictionary["SW"] = !((enumDescription ?? "").Trim() == "") ? (PdfTypeBase) PdfTypeName.Create(enumDescription) : throw new ArgumentException(string.Format(Error.err0047, (object) nameof (ScaleMode), (object) "are IconScaleModes.Always, IconScaleModes.Bigger, IconScaleModes.Smaller, and IconScaleModes.Never"));
    }
  }

  /// <summary>Gets or sets the type of scaling to use.</summary>
  public IconScaleTypes ScaleType
  {
    get
    {
      if (!this.IsExists("S"))
        return IconScaleTypes.Proportional;
      IconScaleTypes result;
      if (Pdfium.GetEnumDescription<IconScaleTypes>(this.Dictionary["S"].As<PdfTypeName>().Value, out result))
        return result;
      throw new PdfParserException(string.Format(Error.err0045, (object) "S"));
    }
    set
    {
      string enumDescription = Pdfium.GetEnumDescription((Enum) value);
      this.Dictionary["S"] = !((enumDescription ?? "").Trim() == "") ? (PdfTypeBase) PdfTypeName.Create(enumDescription) : throw new ArgumentException(string.Format(Error.err0047, (object) nameof (ScaleType), (object) "are IconScaleTypes.Anamorphic and IconScaleTypes.Proportional"));
    }
  }

  /// <summary>
  /// Gets or sets the number between 0.0 and 1.0 indicating the fraction of leftover space to allocate at the left of the icon.
  /// A value of 0.0 positions the icon at the left corner of the annotation rectangle.
  /// A value of 0.5 centers it within the horizontal direction of rectangle.
  /// This Property is used only if the <see cref="P:Patagames.Pdf.Net.Wrappers.PdfIconFit.ScaleType" /> is <see cref="F:Patagames.Pdf.Enums.IconScaleTypes.Proportional" />.
  /// </summary>
  public float HorizontalOffset
  {
    get
    {
      return !this.IsExists("A") || !this.Dictionary["A"].Is<PdfTypeArray>() || this.Dictionary["A"].As<PdfTypeArray>().Count < 1 || !this.Dictionary["A"].As<PdfTypeArray>()[0].Is<PdfTypeNumber>() ? 0.5f : this.Dictionary["A"].As<PdfTypeArray>()[0].As<PdfTypeNumber>().FloatValue;
    }
    set
    {
      if (!this.Dictionary.ContainsKey("A"))
        this.Dictionary["A"] = (PdfTypeBase) PdfTypeArray.Create();
      float verticalOffset = this.VerticalOffset;
      this.Dictionary["A"].As<PdfTypeArray>().Clear();
      this.Dictionary["A"].As<PdfTypeArray>().AddReal(value);
      this.Dictionary["A"].As<PdfTypeArray>().AddReal(verticalOffset);
    }
  }

  /// <summary>
  /// Gets or sets the number between 0.0 and 1.0 indicating the fraction of leftover space to allocate at the bottom of the icon.
  /// A value of 0.0 positions the icon at the bottom corner of the annotation rectangle.
  /// A value of 0.5 centers it within the vertical direction of rectangle.
  /// This Property is used only if the <see cref="P:Patagames.Pdf.Net.Wrappers.PdfIconFit.ScaleType" /> is <see cref="F:Patagames.Pdf.Enums.IconScaleTypes.Proportional" />.
  /// </summary>
  public float VerticalOffset
  {
    get
    {
      return !this.IsExists("A") || !this.Dictionary["A"].Is<PdfTypeArray>() || this.Dictionary["A"].As<PdfTypeArray>().Count < 2 || !this.Dictionary["A"].As<PdfTypeArray>()[1].Is<PdfTypeNumber>() ? 0.5f : this.Dictionary["A"].As<PdfTypeArray>()[1].As<PdfTypeNumber>().FloatValue;
    }
    set
    {
      if (!this.Dictionary.ContainsKey("A"))
        this.Dictionary["A"] = (PdfTypeBase) PdfTypeArray.Create();
      float horizontalOffset = this.HorizontalOffset;
      this.Dictionary["A"].As<PdfTypeArray>().Clear();
      this.Dictionary["A"].As<PdfTypeArray>().AddReal(horizontalOffset);
      this.Dictionary["A"].As<PdfTypeArray>().AddReal(value);
    }
  }

  /// <summary>
  ///  If true, indicates that the button appearance should be scaled to fit fully within the bounds of the annotation
  ///  without taking into consideration the line width of the border.
  /// </summary>
  public bool FitToBounds
  {
    get => this.IsExists("FB") && this.Dictionary["FB"].As<PdfTypeBoolean>().Value;
    set
    {
      if (!value && this.Dictionary.ContainsKey("FB"))
      {
        this.Dictionary.Remove("FB");
      }
      else
      {
        if (!value)
          return;
        this.Dictionary["FB"] = (PdfTypeBase) PdfTypeBoolean.Create(value);
      }
    }
  }

  /// <summary>
  /// Creates new instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfIconFit" /> class.
  /// </summary>
  public PdfIconFit()
  {
  }

  /// <summary>
  /// Creates a new instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfIconFit" /> and initialize it with specified dictionary
  /// </summary>
  /// <param name="dictionary">The dictionary or indirect dictionary</param>
  public PdfIconFit(PdfTypeBase dictionary)
    : base(dictionary)
  {
  }
}
