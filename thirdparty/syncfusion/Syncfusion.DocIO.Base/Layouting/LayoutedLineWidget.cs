// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedLineWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedLineWidget
{
  private PointF m_point1;
  private PointF m_point2;
  private Color m_color;
  private float m_width;
  private bool m_skip;

  internal LayoutedLineWidget()
  {
  }

  internal LayoutedLineWidget(LayoutedLineWidget srcWidget)
  {
    this.Point1 = srcWidget.Point1;
    this.Point2 = srcWidget.Point2;
    this.Width = srcWidget.Width;
    this.Skip = srcWidget.Skip;
  }

  internal PointF Point1
  {
    get => this.m_point1;
    set => this.m_point1 = value;
  }

  internal PointF Point2
  {
    get => this.m_point2;
    set => this.m_point2 = value;
  }

  internal Color Color
  {
    get => this.m_color;
    set => this.m_color = value;
  }

  internal float Width
  {
    get => this.m_width;
    set => this.m_width = value;
  }

  internal bool Skip
  {
    get => this.m_skip;
    set => this.m_skip = value;
  }

  internal void ShiftXYPosition(float xPosition, float yPosition)
  {
    this.Point1 = new PointF(this.Point1.X + xPosition, this.Point1.Y + yPosition);
    this.Point2 = new PointF(this.Point2.X + xPosition, this.Point2.Y + yPosition);
  }

  public void Dispose()
  {
  }
}
