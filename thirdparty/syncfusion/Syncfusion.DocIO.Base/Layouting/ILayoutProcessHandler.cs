// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.ILayoutProcessHandler
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal interface ILayoutProcessHandler
{
  bool GetNextArea(
    out RectangleF rect,
    ref int columnIndex,
    ref bool isContinuousSection,
    bool isSplittedWidget,
    ref float topMargin,
    bool isFromDynmicLayout,
    ref IWidgetContainer curWidget);

  void PushLayoutedWidget(
    LayoutedWidget ltWidget,
    RectangleF layoutArea,
    bool isNeedToRestartFootnote,
    bool m_bisNeedToRestartEndnoteID,
    LayoutState state,
    bool isNeedToFindInterSectingPoint);

  bool HandleSplittedWidget(
    SplitWidgetContainer stWidgetContainer,
    LayoutState state,
    LayoutedWidget ltWidget,
    ref bool isLayoutedWidgetNeedToPushed);

  void HandleLayoutedWidget(LayoutedWidget ltWidget);
}
