// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfAutomaticFieldInfo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

internal class PdfAutomaticFieldInfo
{
  private PointF m_location = PointF.Empty;
  private PdfAutomaticField m_field;
  private float m_scalingX = 1f;
  private float m_scalingY = 1f;

  public PdfAutomaticFieldInfo(PdfAutomaticField field, PointF location)
  {
    this.m_field = field != null ? field : throw new ArgumentNullException(nameof (field));
    this.m_location = location;
  }

  public PdfAutomaticFieldInfo(
    PdfAutomaticField field,
    PointF location,
    float scalingX,
    float scalingY)
  {
    this.m_field = field != null ? field : throw new ArgumentNullException(nameof (field));
    this.m_location = location;
    this.m_scalingX = scalingX;
    this.m_scalingY = scalingY;
  }

  public PdfAutomaticFieldInfo(PdfAutomaticFieldInfo fieldInfo)
  {
    this.m_field = fieldInfo != null ? fieldInfo.Field : throw new ArgumentNullException(nameof (fieldInfo));
    this.m_location = fieldInfo.Location;
    this.m_scalingX = fieldInfo.ScalingX;
    this.m_scalingY = fieldInfo.ScalingY;
  }

  public PointF Location
  {
    get => this.m_location;
    set => this.m_location = value;
  }

  public PdfAutomaticField Field
  {
    get => this.m_field;
    set => this.m_field = value != null ? value : throw new ArgumentNullException(nameof (Field));
  }

  public float ScalingX
  {
    get => this.m_scalingX;
    set => this.m_scalingX = value;
  }

  public float ScalingY
  {
    get => this.m_scalingY;
    set => this.m_scalingY = value;
  }
}
