// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedMatrixWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Office;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedMatrixWidget : LayoutedFuntionWidget
{
  private List<List<LayoutedOMathWidget>> rows;

  internal LayoutedMatrixWidget(IOfficeMathFunctionBase widget)
    : base(widget)
  {
  }

  internal LayoutedMatrixWidget(LayoutedMatrixWidget srcWidget)
    : base((LayoutedFuntionWidget) srcWidget)
  {
    List<List<LayoutedOMathWidget>> layoutedOmathWidgetListList = new List<List<LayoutedOMathWidget>>();
    for (int index1 = 0; index1 < srcWidget.Rows.Count; ++index1)
    {
      List<LayoutedOMathWidget> layoutedOmathWidgetList = new List<LayoutedOMathWidget>();
      for (int index2 = 0; index2 < srcWidget.Rows[index1].Count; ++index2)
        layoutedOmathWidgetList.Add(new LayoutedOMathWidget(srcWidget.Rows[index1][index2]));
      layoutedOmathWidgetListList.Add(layoutedOmathWidgetList);
    }
    this.Rows = layoutedOmathWidgetListList;
  }

  internal List<List<LayoutedOMathWidget>> Rows
  {
    get => this.rows;
    set => this.rows = value;
  }

  public override void ShiftXYPosition(float xPosition, float yPosition)
  {
    this.Bounds = new RectangleF(this.Bounds.X + xPosition, this.Bounds.Y + yPosition, this.Bounds.Width, this.Bounds.Height);
    for (int index1 = 0; index1 < this.Rows.Count; ++index1)
    {
      for (int index2 = 0; index2 < this.Rows[index1].Count; ++index2)
        this.Rows[index1][index2].ShiftXYPosition(xPosition, yPosition);
    }
  }

  public override void Dispose()
  {
    if (this.rows != null)
    {
      for (int index1 = 0; index1 < this.rows.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.rows[index1].Count; ++index2)
        {
          this.rows[index1][index2].Dispose();
          this.rows[index1][index2] = (LayoutedOMathWidget) null;
        }
        this.rows[index1].Clear();
      }
      this.rows.Clear();
      this.rows = (List<List<LayoutedOMathWidget>>) null;
    }
    base.Dispose();
  }
}
