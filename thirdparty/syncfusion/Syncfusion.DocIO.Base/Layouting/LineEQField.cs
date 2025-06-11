// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LineEQField
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LineEQField : LayoutedEQFields
{
  private PointF m_point1;
  private PointF m_point2;

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
}
