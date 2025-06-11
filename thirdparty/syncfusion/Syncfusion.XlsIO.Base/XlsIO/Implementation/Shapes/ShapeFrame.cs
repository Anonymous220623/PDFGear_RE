// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Shapes.ShapeFrame
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Shapes;

internal class ShapeFrame
{
  private long m_offsetX;
  private long m_offsetY;
  private long m_offsetCX;
  private long m_offsetCY;
  private int m_rotation;
  private long m_chOffsetX;
  private long m_chOffsetY;
  private long m_chOffsetCX;
  private long m_chOffsetCY;
  private ShapeImpl m_baseShape;

  internal ShapeFrame(ShapeImpl shape) => this.m_baseShape = shape;

  internal long OffsetX
  {
    get => this.m_offsetX;
    set => this.m_offsetX = value;
  }

  internal long OffsetY
  {
    get => this.m_offsetY;
    set => this.m_offsetY = value;
  }

  internal long OffsetCX
  {
    get => this.m_offsetCX;
    set => this.m_offsetCX = value;
  }

  internal long OffsetCY
  {
    get => this.m_offsetCY;
    set => this.m_offsetCY = value;
  }

  internal long ChOffsetX
  {
    get => this.m_chOffsetX;
    set => this.m_chOffsetX = value;
  }

  internal long ChOffsetY
  {
    get => this.m_chOffsetY;
    set => this.m_chOffsetY = value;
  }

  internal long ChOffsetCX
  {
    get => this.m_chOffsetCX;
    set => this.m_chOffsetCX = value;
  }

  internal long ChOffsetCY
  {
    get => this.m_chOffsetCY;
    set => this.m_chOffsetCY = value;
  }

  internal int Rotation
  {
    get => this.m_rotation;
    set => this.m_rotation = value;
  }

  internal void SetAnchor(int rotation, long offsetX, long offsetY, long offsetCx, long offsetCy)
  {
    this.m_rotation = rotation;
    this.m_offsetX = offsetX;
    this.m_offsetY = offsetY;
    this.m_offsetCX = offsetCx;
    this.m_offsetCY = offsetCy;
  }

  internal void SetChildAnchor(
    long childOffsetX,
    long childOffsetY,
    long childOffsetCx,
    long childOffsetCy)
  {
    this.m_chOffsetCX = childOffsetCx;
    this.m_chOffsetCY = childOffsetCy;
    this.m_chOffsetX = childOffsetX;
    this.m_chOffsetY = childOffsetY;
  }

  internal ShapeFrame Clone(object parent)
  {
    ShapeFrame shapeFrame = (ShapeFrame) this.MemberwiseClone();
    shapeFrame.m_baseShape = (ShapeImpl) parent;
    return shapeFrame;
  }

  internal void SetParent(ShapeImpl shape) => this.m_baseShape = shape;

  internal void Close() => this.m_baseShape = (ShapeImpl) null;
}
