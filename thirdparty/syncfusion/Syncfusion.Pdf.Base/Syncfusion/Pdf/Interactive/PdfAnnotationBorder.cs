// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfAnnotationBorder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfAnnotationBorder : IPdfWrapper
{
  private float m_horizontalRadius;
  private float m_verticalRadius;
  private float m_borderWidth = 1f;
  private PdfArray m_array = new PdfArray();

  public float HorizontalRadius
  {
    get => this.m_horizontalRadius;
    set
    {
      if ((double) this.m_horizontalRadius == (double) value)
        return;
      this.m_horizontalRadius = value;
      this.SetNumber(0, value);
    }
  }

  public float VerticalRadius
  {
    get => this.m_verticalRadius;
    set
    {
      if ((double) this.m_verticalRadius == (double) value)
        return;
      this.m_verticalRadius = value;
      this.SetNumber(1, value);
    }
  }

  public float Width
  {
    get => this.m_borderWidth;
    set
    {
      if ((double) this.m_borderWidth == (double) value)
        return;
      this.m_borderWidth = value;
      this.SetNumber(2, value);
    }
  }

  public PdfAnnotationBorder()
  {
    this.Initialize(this.m_borderWidth, this.m_horizontalRadius, this.m_verticalRadius);
  }

  public PdfAnnotationBorder(float borderWidth)
  {
    this.Initialize(borderWidth, this.m_horizontalRadius, this.m_verticalRadius);
    this.Width = borderWidth;
  }

  public PdfAnnotationBorder(float borderWidth, float horizontalRadius, float verticalRadius)
  {
    this.Initialize(borderWidth, horizontalRadius, verticalRadius);
  }

  private void Initialize(float borderWidth, float horizontalRadius, float verticalRadius)
  {
    this.m_array.Add((IPdfPrimitive) new PdfNumber(horizontalRadius), (IPdfPrimitive) new PdfNumber(verticalRadius), (IPdfPrimitive) new PdfNumber(borderWidth));
  }

  private void SetNumber(int index, float value)
  {
    (this.m_array[index] as PdfNumber).FloatValue = value;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_array;
}
