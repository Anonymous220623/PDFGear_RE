// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.OPicture
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class OPicture : OParagraphItem
{
  private float m_widthScale = 100f;
  private float m_heightScale = 100f;
  private float m_height;
  private float m_width;
  private string m_name;
  private float m_horizontalPosition;
  private float m_verticalPosition;
  private int m_orderIndex;
  private int m_spid;
  private string m_oPictureHRef;
  private TextWrappingStyle m_wrappingStyle;

  internal float Height
  {
    get => this.m_height;
    set => this.m_height = value;
  }

  internal float Width
  {
    get => this.m_width;
    set => this.m_width = value;
  }

  internal float HeightScale
  {
    get => this.m_heightScale;
    set => this.m_heightScale = value;
  }

  internal float WidthScale
  {
    get => this.m_widthScale;
    set => this.m_widthScale = value;
  }

  public string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal float HorizontalPosition
  {
    get => this.m_horizontalPosition;
    set => this.m_horizontalPosition = value;
  }

  internal float VerticalPosition
  {
    get => this.m_verticalPosition;
    set => this.m_verticalPosition = value;
  }

  internal int OrderIndex
  {
    get => this.m_orderIndex;
    set => this.m_orderIndex = value;
  }

  internal int ShapeId
  {
    get => this.m_spid;
    set => this.m_spid = value;
  }

  internal string OPictureHRef
  {
    get => this.m_oPictureHRef;
    set => this.m_oPictureHRef = value;
  }

  internal TextWrappingStyle TextWrappingStyle
  {
    get => this.m_wrappingStyle;
    set => this.m_wrappingStyle = value;
  }

  internal OPicture()
  {
  }

  internal void SetWidthScaleValue(float value) => this.m_widthScale = value;

  internal void SetHeightScaleValue(float value) => this.m_heightScale = value;
}
