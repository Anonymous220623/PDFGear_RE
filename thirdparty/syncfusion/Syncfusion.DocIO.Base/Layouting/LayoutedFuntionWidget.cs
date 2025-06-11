// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedFuntionWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Office;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal abstract class LayoutedFuntionWidget : ILayoutedFuntionWidget
{
  private RectangleF m_bounds = RectangleF.Empty;
  private IOfficeMathFunctionBase m_widget;
  private LayoutedOMathWidget m_owner;

  internal LayoutedFuntionWidget(IOfficeMathFunctionBase widget) => this.m_widget = widget;

  internal LayoutedFuntionWidget(LayoutedFuntionWidget srcWidget)
  {
    this.Bounds = srcWidget.Bounds;
    this.m_widget = srcWidget.Widget;
  }

  public IOfficeMathFunctionBase Widget => this.m_widget;

  public RectangleF Bounds
  {
    get => this.m_bounds;
    set => this.m_bounds = value;
  }

  public LayoutedOMathWidget Owner
  {
    get => this.m_owner;
    set => this.m_owner = value;
  }

  public abstract void ShiftXYPosition(float xPosition, float yPosition);

  public virtual void Dispose()
  {
    this.m_owner = (LayoutedOMathWidget) null;
    this.m_owner = (LayoutedOMathWidget) null;
  }
}
