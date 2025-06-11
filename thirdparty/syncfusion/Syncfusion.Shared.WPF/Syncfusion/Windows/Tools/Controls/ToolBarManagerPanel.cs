// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.ToolBarManagerPanel
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Shared;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class ToolBarManagerPanel : Panel
{
  private ToolBarManager manager;

  public ToolBarManager Manager
  {
    get
    {
      if (this.manager == null)
        this.manager = VisualUtils.FindAncestor((Visual) this, typeof (ToolBarManager)) as ToolBarManager;
      return this.manager;
    }
  }

  internal void Arrange(Size finalSize)
  {
    double size1 = 0.0;
    double size2 = 0.0;
    if (this.Manager.TopToolBarTray != null)
      this.CalculateSize(this.Manager.TopToolBarTray, ref size1);
    Rect rect1 = new Rect(0.0, 0.0, finalSize.Width, size1);
    double size3 = 0.0;
    if (this.Manager.BottomToolBarTray != null)
      this.CalculateSize(this.Manager.BottomToolBarTray, ref size3);
    Rect rect2 = new Rect(0.0, Math.Max(0.0, finalSize.Height - size3), finalSize.Width, size3);
    if (this.Manager.LeftToolBarTray != null)
      this.CalculateSize(this.Manager.LeftToolBarTray, ref size2);
    double num = finalSize.Height - (rect1.Height + rect2.Height);
    double height1 = num < 0.0 ? 20.0 : num;
    Rect rect3 = new Rect(0.0, rect1.Height, size2, height1);
    double size4 = 0.0;
    if (this.Manager.RightToolBarTray != null)
      this.CalculateSize(this.Manager.RightToolBarTray, ref size4);
    Rect rect4 = new Rect(Math.Max(0.0, finalSize.Width - size4), rect1.Height, size4, height1);
    if (this.Manager.TopToolBarTray != null)
      this.Manager.TopToolBarTray.ArrangeCall(rect1);
    if (this.Manager.LeftToolBarTray != null)
      this.Manager.LeftToolBarTray.ArrangeCall(rect3);
    if (this.Manager.RightToolBarTray != null)
      this.Manager.RightToolBarTray.ArrangeCall(rect4);
    if (this.Manager.BottomToolBarTray != null)
      this.Manager.BottomToolBarTray.ArrangeCall(rect2);
    double width = Math.Max(0.0, finalSize.Width - (rect3.Width + rect4.Width));
    double height2 = Math.Max(0.0, finalSize.Height - (rect1.Height + rect2.Height));
    this.Manager.content.Arrange(new Rect(rect3.Right, rect1.Bottom, width, height2));
    if (this.Manager.Content == null)
      return;
    this.Manager.Content.Arrange(new Rect(0.0, 0.0, width, height2));
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    this.Arrange(finalSize);
    return finalSize;
  }

  private void CalculateSize(ToolBarTrayAdv tray, ref double size)
  {
    foreach (ToolBarBand band in tray.Bands)
      size += band.Size;
  }
}
