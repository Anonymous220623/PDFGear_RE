// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.OverviewCustomPanel
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class OverviewCustomPanel : Panel
{
  internal static readonly DependencyProperty OverviewProperty = DependencyProperty.Register(nameof (Overview), typeof (Overview), typeof (OverviewCustomPanel), new PropertyMetadata((PropertyChangedCallback) null));

  internal Overview Overview
  {
    get => (Overview) this.GetValue(OverviewCustomPanel.OverviewProperty);
    set => this.SetValue(OverviewCustomPanel.OverviewProperty, (object) value);
  }

  public OverviewCustomPanel()
  {
    this.SetBinding(OverviewCustomPanel.OverviewProperty, (BindingBase) new Binding()
    {
      RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof (Overview), 1)
    });
    this.Loaded += new RoutedEventHandler(this.OverviewCustomPanel_Loaded);
  }

  private void OverviewCustomPanel_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.Overview == null)
      return;
    this.InvalidateMeasure();
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    Size size = new Size(0.0, 0.0);
    foreach (FrameworkElement child in this.Children)
    {
      if (child is OverviewResizer)
        child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      else
        child.Measure(availableSize);
      switch (child)
      {
        case OverviewResizer _:
        case Grid _:
          continue;
        default:
          size.Width = Math.Max(size.Width, double.IsNaN(child.DesiredSize.Width) || double.IsInfinity(child.DesiredSize.Width) ? 0.0 : child.DesiredSize.Width);
          size.Height = Math.Max(size.Height, double.IsNaN(child.DesiredSize.Height) || double.IsInfinity(child.DesiredSize.Height) ? 0.0 : child.DesiredSize.Height);
          continue;
      }
    }
    return this.Overview != null && this.Overview.ScrollSource != null ? availableSize.GetUniformSize(new Size(this.Overview.ScrollSource.ExtentWidth, this.Overview.ScrollSource.ExtentHeight)) : size;
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    Size size = new Size(0.0, 0.0);
    foreach (FrameworkElement child in this.Children)
    {
      if (child is OverviewResizer)
        child.Arrange(new Rect(new Point(0.0, 0.0), child.DesiredSize));
      else
        child.Arrange(new Rect(new Point(0.0, 0.0), finalSize));
      switch (child)
      {
        case OverviewResizer _:
        case Grid _:
          continue;
        default:
          size.Width = Math.Max(size.Width, double.IsNaN(child.ActualWidth) || double.IsInfinity(child.ActualWidth) ? 0.0 : child.ActualWidth);
          size.Height = Math.Max(size.Height, double.IsNaN(child.ActualHeight) || double.IsInfinity(child.ActualHeight) ? 0.0 : child.ActualHeight);
          continue;
      }
    }
    return this.Overview != null && this.Overview.ScrollSource != null ? finalSize.GetUniformSize(new Size(this.Overview.ScrollSource.ExtentWidth, this.Overview.ScrollSource.ExtentHeight)) : size;
  }
}
