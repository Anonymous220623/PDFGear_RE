// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartColorBar
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartColorBar : ItemsControl
{
  private ChartDock internalDockPosition = ChartDock.Top;
  public static readonly DependencyProperty ShowLabelProperty = DependencyProperty.Register(nameof (ShowLabel), typeof (bool), typeof (ChartColorBar), new PropertyMetadata((object) true));
  public static readonly DependencyProperty DockPositionProperty = DependencyProperty.Register(nameof (DockPosition), typeof (ChartDock), typeof (ChartColorBar), new PropertyMetadata((object) ChartDock.Top, new PropertyChangedCallback(ChartColorBar.OnDockPositionChanged)));

  public ChartColorBar()
  {
    this.DefaultStyleKey = (object) typeof (ChartColorBar);
    this.Loaded += new RoutedEventHandler(this.ColorBar_Loaded);
  }

  internal SfSurfaceChart Area { get; set; }

  internal Rect ArrangeRect { get; set; }

  internal ChartDock InternalDockPosition
  {
    get => this.internalDockPosition;
    set => this.internalDockPosition = value;
  }

  public bool ShowLabel
  {
    get => (bool) this.GetValue(ChartColorBar.ShowLabelProperty);
    set => this.SetValue(ChartColorBar.ShowLabelProperty, (object) value);
  }

  public ChartDock DockPosition
  {
    get => (ChartDock) this.GetValue(ChartColorBar.DockPositionProperty);
    set => this.SetValue(ChartColorBar.DockPositionProperty, (object) value);
  }

  private static void OnDockPositionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartColorBar).OnDockPositionChanged();
  }

  private void OnDockPositionChanged()
  {
    this.InternalDockPosition = this.DockPosition;
    if (this.Area != null)
      this.Area.UpdateColorBarArrangeRect();
    this.ChangeOrientation();
    if (ChartDockPanel.GetDock((UIElement) this) == this.InternalDockPosition)
      return;
    ChartDockPanel.SetDock((UIElement) this, this.InternalDockPosition);
  }

  private void ColorBar_Loaded(object sender, RoutedEventArgs e)
  {
    this.ChangeOrientation();
    if (this.DockPosition != ChartDock.Top && this.DockPosition != ChartDock.Bottom)
      return;
    this.Area.DockPanel.InvalidateMeasure();
  }

  private void ChangeOrientation()
  {
    ItemsPresenter visualChild = ChartLayoutUtils.GetVisualChild<ItemsPresenter>((DependencyObject) this);
    if (visualChild == null || VisualTreeHelper.GetChildrenCount((DependencyObject) visualChild) <= 0 || !(VisualTreeHelper.GetChild((DependencyObject) visualChild, 0) is StackPanel child))
      return;
    if (this.DockPosition != ChartDock.Left && this.DockPosition != ChartDock.Right)
      child.Orientation = Orientation.Horizontal;
    else
      child.Orientation = Orientation.Vertical;
  }

  protected internal virtual void UpdateColorBarItemsSource()
  {
    ObservableCollection<ColorBarItem> source = new ObservableCollection<ColorBarItem>();
    List<Brush> brushes = this.Area.ColorModel.GetBrushes(this.Area.Palette);
    if (brushes == null || brushes.Count == 0)
    {
      this.ItemsSource = (IEnumerable) source;
    }
    else
    {
      List<Brush> brushList = this.Area.Palette == ChartColorPalette.Custom ? brushes : (this.Area.BrushCount > 10 ? MeshGenerator.GetBrushRange(this.Area.BrushCount - 10, brushes) : brushes.GetRange(0, this.Area.BrushCount));
      double num = this.Area.YRange.Delta / (double) brushList.Count;
      double d1 = this.Area.YRange.Start;
      for (int index = 0; index < brushList.Count; ++index)
      {
        Brush brush = brushList[index];
        ColorBarItem colorBarItem = new ColorBarItem()
        {
          Background = brush,
          IconHeight = 12.0,
          IconWidth = 12.0,
          ColorBar = this,
          ShowLabel = this.ShowLabel,
          Orientation = Orientation.Horizontal
        };
        double d2 = d1 + num;
        colorBarItem.Label = double.IsNaN(d1) || double.IsNaN(d2) ? "" : $"{d1.ToString(this.Area.LegendLabelFormat, (IFormatProvider) CultureInfo.InvariantCulture)} - {d2.ToString(this.Area.LegendLabelFormat, (IFormatProvider) CultureInfo.InvariantCulture)}";
        d1 = d2;
        source.Add(colorBarItem);
      }
      if (this.Items != null && this.Items.Count > 0)
      {
        this.ItemsSource = (IEnumerable) null;
        this.Items.Clear();
      }
      if (this.DockPosition == ChartDock.Bottom || this.DockPosition == ChartDock.Top)
        this.ItemsSource = (IEnumerable) source;
      else
        this.ItemsSource = (IEnumerable) source.Reverse<ColorBarItem>();
    }
  }
}
