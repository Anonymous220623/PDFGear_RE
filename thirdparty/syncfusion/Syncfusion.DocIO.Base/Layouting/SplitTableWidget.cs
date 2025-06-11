// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.SplitTableWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.Rendering;
using System;

#nullable disable
namespace Syncfusion.Layouting;

internal class SplitTableWidget : IWidget
{
  private ITableWidget m_tableWidget;
  private int m_rowNumber;
  private int m_colNumber;
  private SplitWidgetContainer[] m_splittedCells;

  public SplitTableWidget(ITableWidget tableWidget, int rowNumber)
    : this(tableWidget, rowNumber, 0)
  {
  }

  public SplitTableWidget(
    ITableWidget tableWidget,
    int rowNumber,
    SplitWidgetContainer[] splittedCells)
    : this(tableWidget, rowNumber, 0)
  {
    this.m_splittedCells = splittedCells;
  }

  public SplitTableWidget(ITableWidget tableWidget, int rowNumber, int colNumber)
  {
    this.m_tableWidget = tableWidget;
    this.m_rowNumber = rowNumber;
    this.m_colNumber = colNumber;
  }

  public ITableWidget TableWidget => this.m_tableWidget;

  public int StartRowNumber => this.m_rowNumber;

  public int StartColumnNumber => this.m_colNumber;

  public SplitWidgetContainer[] SplittedCells => this.m_splittedCells;

  public ILayoutInfo LayoutInfo => (ILayoutInfo) null;

  public void Draw(DrawingContext dc, LayoutedWidget layoutedWidget)
  {
    throw new NotImplementedException();
  }

  public void InitLayoutInfo()
  {
  }

  public void InitLayoutInfo(IWidget widget)
  {
  }
}
