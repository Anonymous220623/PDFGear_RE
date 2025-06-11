// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LeafEmtyWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.Rendering;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LeafEmtyWidget : ILeafWidget, IWidget
{
  private SizeF m_size;
  private Syncfusion.Layouting.LayoutInfo m_layoutInfo;

  public LeafEmtyWidget(SizeF size)
  {
    this.m_layoutInfo = new Syncfusion.Layouting.LayoutInfo(ChildrenLayoutDirection.Horizontal);
    this.m_size = size;
  }

  public SizeF Measure(DrawingContext dc) => this.m_size;

  public ILayoutInfo LayoutInfo => (ILayoutInfo) this.m_layoutInfo;

  public void Draw(DrawingContext dc, LayoutedWidget layoutedWidget)
  {
  }

  public void InitLayoutInfo() => this.m_layoutInfo = (Syncfusion.Layouting.LayoutInfo) null;

  public void InitLayoutInfo(IWidget widget)
  {
  }
}
