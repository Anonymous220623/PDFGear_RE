// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.GradientShading
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class GradientShading : Shading
{
  private PdfArray m_coordinate;
  private PdfArray m_extented;
  private Function m_function;
  private PdfArray m_domain;
  private Color m_background;

  internal Color Background
  {
    get => this.m_background;
    set => this.m_background = value;
  }

  internal PdfArray Coordinate
  {
    get => this.m_coordinate;
    set => this.m_coordinate = value;
  }

  internal PdfArray Extented
  {
    get => this.m_extented;
    set => this.m_extented = value;
  }

  internal PdfArray Domain
  {
    get
    {
      this.m_domain = this.Function.Domain;
      return this.m_domain;
    }
    set => this.m_domain = this.Function.Domain;
  }

  internal Function Function
  {
    get => this.m_function;
    set => this.m_function = value;
  }

  public GradientShading(PdfDictionary dic)
    : base(dic)
  {
    this.m_coordinate = dic.Items.ContainsKey(new PdfName("Coords")) ? dic.Items[new PdfName("Coords")] as PdfArray : (PdfArray) null;
    this.m_extented = dic.Items.ContainsKey(new PdfName("Extend")) ? dic.Items[new PdfName("Extend")] as PdfArray : (PdfArray) null;
    this.m_function = Function.CreateFunction(this.GetFunction(dic));
    this.m_background = this.GetBackgroundColor(dic);
  }

  public GradientShading()
  {
  }

  private PdfDictionary GetFunction(PdfDictionary dictionary)
  {
    if (dictionary.Items.ContainsKey(new PdfName("Function")))
    {
      if (dictionary.Items[new PdfName("Function")] is PdfDictionary function1)
        return function1;
      if ((dictionary.Items[new PdfName("Function")] as PdfReferenceHolder).Object is PdfDictionary function2)
        return function2;
    }
    return (PdfDictionary) null;
  }

  protected Color GetColor(double data)
  {
    return this.AlternateColorspace.GetColor(this.AlternateColorspace.ToParams(this.Function.ColorTransferFunction(new double[1]
    {
      data
    })));
  }

  private Color GetBackgroundColor(PdfDictionary dictionary)
  {
    if (!dictionary.Items.ContainsKey(new PdfName("Background")))
      return Color.Transparent;
    PdfArray pdfArray = dictionary.Items[new PdfName("Background")] as PdfArray;
    return Color.FromArgb((int) byte.MaxValue, (int) ((double) (pdfArray[0] as PdfNumber).FloatValue * (double) byte.MaxValue), (int) ((double) (pdfArray[1] as PdfNumber).FloatValue * (double) byte.MaxValue), (int) (pdfArray[2] as PdfNumber).FloatValue * (int) byte.MaxValue);
  }
}
