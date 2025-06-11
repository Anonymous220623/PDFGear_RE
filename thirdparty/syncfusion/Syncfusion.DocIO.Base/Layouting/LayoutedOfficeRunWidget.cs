// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedOfficeRunWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Office;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedOfficeRunWidget : LayoutedWidget, ILayoutedFuntionWidget
{
  private LayoutedWidget m_ltWidget;
  private IOfficeMathFunctionBase m_widget;
  private LayoutedOMathWidget m_owner;

  internal LayoutedOfficeRunWidget(IOfficeMathFunctionBase widget) => this.m_widget = widget;

  internal LayoutedOfficeRunWidget(ILayoutedFuntionWidget srcWidget)
    : base((LayoutedWidget) (srcWidget as LayoutedOfficeRunWidget))
  {
    this.m_widget = srcWidget.Widget;
    this.LayoutedWidget = new LayoutedWidget((srcWidget as LayoutedOfficeRunWidget).LayoutedWidget);
  }

  public IOfficeMathFunctionBase Widget => this.m_widget;

  internal LayoutedWidget LayoutedWidget
  {
    get => this.m_ltWidget;
    set => this.m_ltWidget = value;
  }

  public LayoutedOMathWidget Owner
  {
    get => this.m_owner;
    set => this.m_owner = value;
  }

  public void ShiftXYPosition(float xPosition, float yPosition)
  {
    this.Bounds = new RectangleF(this.Bounds.X + xPosition, this.Bounds.Y + yPosition, this.Bounds.Width, this.Bounds.Height);
    this.LayoutedWidget.Bounds = new RectangleF(this.LayoutedWidget.Bounds.X + xPosition, this.LayoutedWidget.Bounds.Y + yPosition, this.LayoutedWidget.Bounds.Width, this.LayoutedWidget.Bounds.Height);
  }

  public void Dispose()
  {
    if (this.m_ltWidget != null)
      this.m_ltWidget.InitLayoutInfoAll();
    this.m_widget = (IOfficeMathFunctionBase) null;
    this.m_owner = (LayoutedOMathWidget) null;
  }
}
