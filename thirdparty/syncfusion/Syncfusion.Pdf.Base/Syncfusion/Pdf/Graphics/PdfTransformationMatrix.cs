// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfTransformationMatrix
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class PdfTransformationMatrix : ICloneable
{
  private const double DegRadFactor = 0.017453292519943295;
  private const double RadDegFactor = 57.295779513082323;
  private Matrix m_matrix;

  public float OffsetX => this.m_matrix.OffsetX;

  public float OffsetY => this.m_matrix.OffsetY;

  protected internal Matrix Matrix
  {
    get => this.m_matrix;
    set
    {
      if (this.m_matrix == value)
        return;
      this.m_matrix = value;
    }
  }

  public PdfTransformationMatrix() => this.m_matrix = new Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);

  internal PdfTransformationMatrix(bool value)
  {
    this.m_matrix = new Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, 0.0f);
  }

  public void Translate(SizeF offsets) => this.Translate(offsets.Width, offsets.Height);

  public void Translate(float offsetX, float offsetY) => this.m_matrix.Translate(offsetX, offsetY);

  public void Scale(SizeF scales) => this.Scale(scales.Width, scales.Height);

  public void Scale(float scaleX, float scaleY) => this.m_matrix.Scale(scaleX, scaleY);

  public void Rotate(float angle) => this.m_matrix.Rotate(angle);

  public void Skew(SizeF angles) => this.Skew(angles.Width, angles.Height);

  public void Skew(float angleX, float angleY)
  {
    this.m_matrix.Multiply(new Matrix(1f, (float) Math.Tan(PdfTransformationMatrix.DegressToRadians(angleX)), (float) Math.Tan(PdfTransformationMatrix.DegressToRadians(angleY)), 1f, 0.0f, 0.0f));
  }

  public void Shear(float shearX, float shearY) => this.m_matrix.Shear(shearX, shearY);

  public void RotateAt(float angle, PointF point) => this.m_matrix.RotateAt(angle, point);

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    char ch = ' ';
    int index = 0;
    for (int length = this.m_matrix.Elements.Length; index < length; ++index)
    {
      stringBuilder.Append(PdfNumber.FloatToString(this.m_matrix.Elements[index]));
      stringBuilder.Append(ch);
    }
    return stringBuilder.ToString();
  }

  protected internal void Multiply(PdfTransformationMatrix matrix)
  {
    this.m_matrix.Multiply(matrix.Matrix);
  }

  public static double DegressToRadians(float degreesX) => Math.PI / 180.0 * (double) degreesX;

  public static double RadiansToDegress(float radians) => 180.0 / Math.PI * (double) radians;

  internal PdfTransformationMatrix Clone()
  {
    PdfTransformationMatrix transformationMatrix = this.MemberwiseClone() as PdfTransformationMatrix;
    transformationMatrix.m_matrix = this.m_matrix.Clone();
    return transformationMatrix;
  }

  object ICloneable.Clone() => (object) this.Clone();
}
