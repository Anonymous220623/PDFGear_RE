// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.ToolBarTrayAdv
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

[DesignTimeVisible(false)]
[TemplatePart(Name = "PART_TrayPanel", Type = typeof (TrayPanel))]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Office2010Blue/Office2010BlueToolBarTrayAdvStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Office2010Black/Office2010BlackToolBarTrayAdvStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Office2010Silver/Office2010SilverToolBarTrayAdvStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Office2007Blue/Office2007BlueToolBarTrayAdvStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Office2007Black/Office2007BlackToolBarTrayAdvStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Office2007Silver/Office2007SilverToolBarTrayAdvStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Blend/BlendToolBarTrayAdvStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/VS2010/VS2010ToolBarTrayAdvStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (ToolBarManager), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/ToolBarAdv/Themes/Default/DefaultToolBarTrayAdvStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Metro/MetroToolBarTrayAdvStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Transparent/TransparentToolBarTrayAdvStyle.xaml")]
[ToolboxItem(false)]
[ContentProperty("ToolBars")]
public class ToolBarTrayAdv : Control
{
  internal List<ToolBarBand> Bands;
  private TrayPanel panel;
  private bool forceArrangeCall;
  private bool forceMeasureCall;
  internal ToolBarManager ToolBarManager;
  public static readonly DependencyProperty ToolBarsProperty = DependencyProperty.Register(nameof (ToolBars), typeof (ObservableCollection<ToolBarAdv>), typeof (ToolBarTrayAdv), (PropertyMetadata) null);
  public static readonly DependencyProperty IsLockedProperty = DependencyProperty.Register(nameof (IsLocked), typeof (bool), typeof (ToolBarTrayAdv), new PropertyMetadata((object) false));
  public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (System.Windows.Controls.Orientation), typeof (ToolBarTrayAdv), new PropertyMetadata((object) System.Windows.Controls.Orientation.Horizontal, new PropertyChangedCallback(ToolBarTrayAdv.OnOrientationChanged)));

  public bool IsHostedInToolBarManager => this.ToolBarManager != null;

  public ToolBarTrayAdv()
  {
    this.DefaultStyleKey = (object) typeof (ToolBarTrayAdv);
    this.ToolBars = new ObservableCollection<ToolBarAdv>();
    this.Bands = new List<ToolBarBand>();
    this.SubscribeEvents();
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public ObservableCollection<ToolBarAdv> ToolBars
  {
    get => (ObservableCollection<ToolBarAdv>) this.GetValue(ToolBarTrayAdv.ToolBarsProperty);
    internal set => this.SetValue(ToolBarTrayAdv.ToolBarsProperty, (object) value);
  }

  public bool IsLocked
  {
    get => (bool) this.GetValue(ToolBarTrayAdv.IsLockedProperty);
    set => this.SetValue(ToolBarTrayAdv.IsLockedProperty, (object) value);
  }

  public System.Windows.Controls.Orientation Orientation
  {
    get => (System.Windows.Controls.Orientation) this.GetValue(ToolBarTrayAdv.OrientationProperty);
    set => this.SetValue(ToolBarTrayAdv.OrientationProperty, (object) value);
  }

  private static void OnOrientationChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ToolBarTrayAdv toolBarTrayAdv = obj as ToolBarTrayAdv;
    foreach (ToolBarAdv toolBar in (Collection<ToolBarAdv>) toolBarTrayAdv.ToolBars)
      toolBar.Orientation = toolBarTrayAdv.Orientation;
  }

  internal new void UpdateVisualState()
  {
    foreach (ToolBarAdv toolBar in (Collection<ToolBarAdv>) this.ToolBars)
      toolBar.Orientation = this.Orientation;
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.panel = this.GetTemplateChild("PART_TrayPanel") as TrayPanel;
    this.InsertBars();
  }

  internal void ArrangeCall(Rect rect)
  {
    this.forceArrangeCall = true;
    this.Arrange(rect);
    if (!this.forceArrangeCall)
      return;
    this.ArrangeOverride(new Size(rect.Width, rect.Height));
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    double x = 0.0;
    double y = 0.0;
    double num1 = 0.0;
    double num2 = 0.0;
    foreach (ToolBarBand band in this.Bands)
    {
      Size size = band.ArrangeToolBars(x, y);
      if (this.Orientation == System.Windows.Controls.Orientation.Horizontal)
        y += size.Height;
      else
        x += size.Width;
      num2 += size.Width;
      num1 += size.Height;
    }
    if (this.Orientation == System.Windows.Controls.Orientation.Horizontal)
      finalSize.Height = num1;
    else
      finalSize.Width = num2;
    base.ArrangeOverride(finalSize);
    this.forceArrangeCall = false;
    this.Clip = (Geometry) new RectangleGeometry()
    {
      Rect = new Rect(new Point(), finalSize)
    };
    return finalSize;
  }

  internal void MeasureCall(bool inValidate, Size availableSize)
  {
    this.forceMeasureCall = true;
    this.Measure(availableSize);
    if (!this.forceMeasureCall)
      return;
    this.MeasureSize(availableSize);
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    this.forceMeasureCall = false;
    Size size = this.MeasureSize(availableSize);
    base.MeasureOverride(availableSize);
    return size;
  }

  private Size MeasureSize(Size availableSize)
  {
    Size size1 = new Size();
    this.CorrectBandNos();
    double num1 = double.IsInfinity(availableSize.Width) ? double.MaxValue : availableSize.Width;
    double num2 = double.IsInfinity(availableSize.Height) ? double.MaxValue : availableSize.Height;
    foreach (ToolBarBand band in this.Bands)
    {
      band.IsWindowResizing = true;
      Size size2 = band.Measure(new Size()
      {
        Width = this.Orientation == System.Windows.Controls.Orientation.Horizontal ? availableSize.Width : (this.Bands.Count > 1 ? band.ToolBars[0].MinWidth : availableSize.Width),
        Height = this.Orientation == System.Windows.Controls.Orientation.Horizontal ? (this.Bands.Count > 1 ? band.ToolBars[0].MinHeight : availableSize.Height) : availableSize.Height
      });
      size1.Width += this.Orientation == System.Windows.Controls.Orientation.Horizontal ? num1 : size2.Width;
      size1.Height += this.Orientation == System.Windows.Controls.Orientation.Horizontal ? size2.Height : num2;
      num1 = 0.0;
      num2 = 0.0;
    }
    return size1;
  }

  internal DockArea FindDockArea(Point point)
  {
    return this.IsHostedInToolBarManager ? this.ToolBarManager.FindDockArea(point) : DockArea.None;
  }

  internal void DockToolBar(ToolBarAdv toolBarAdv, DockArea area)
  {
    if (!this.IsHostedInToolBarManager || area == DockArea.None || area == this.ToolBarManager.GetDockArea(toolBarAdv.Tray))
      return;
    if (toolBarAdv.Tray != null && toolBarAdv.Tray.ToolBars.Contains(toolBarAdv))
    {
      toolBarAdv.Tray.Remove(toolBarAdv);
      this.ToolBarManager.Invalidate();
    }
    ToolBarTrayAdv tray = this.ToolBarManager.GetToolBarTray(area) ?? new ToolBarTrayAdv();
    tray.ToolBars.Add(toolBarAdv);
    toolBarAdv.Tray = tray;
    if (this.ToolBarManager.GetDockArea(toolBarAdv.Tray) == DockArea.None)
    {
      this.ToolBarManager.DockTray(tray, area);
    }
    else
    {
      toolBarAdv.Orientation = tray.Orientation;
      this.InvalidateMeasure();
      this.ToolBarManager.Invalidate();
    }
  }

  internal void Remove(ToolBarAdv toolBar)
  {
    this.CorrectBandNos();
    this.ToolBars.Remove(toolBar);
    ToolBarBand band = this.Bands[toolBar.Band];
    if (band.ToolBars.Contains(toolBar))
      band.ToolBars.Remove(toolBar);
    if (band.ToolBars.Count == 0)
      this.Bands.Remove(band);
    this.CorrectBandNos();
    if (!this.panel.Children.Contains((UIElement) toolBar))
      return;
    this.panel.Children.Remove((UIElement) toolBar);
  }

  private void SubscribeEvents()
  {
    this.ToolBars.CollectionChanged += new NotifyCollectionChangedEventHandler(this.ToolBarsCollectionChanged);
  }

  private void ToolBarsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.NewItems == null)
      return;
    for (int index = 0; index < e.NewItems.Count; ++index)
    {
      if (e.NewItems[0] is ToolBarAdv newItem && e.Action == NotifyCollectionChangedAction.Add)
      {
        newItem.Tray = this;
        this.InsertBand(newItem.Band, newItem);
        this.InsertToPanel(newItem);
      }
    }
    this.Bands.Sort(new Comparison<ToolBarBand>(ToolBarBand.CompareBand));
    this.InvalidateMeasure();
  }

  private void CorrectBandNos()
  {
    int num = 0;
    foreach (ToolBarBand band in this.Bands)
    {
      band.BandNo = num;
      foreach (ToolBarAdv toolBar in band.ToolBars)
        toolBar.Band = num;
      ++num;
    }
  }

  private void InsertToPanel(ToolBarAdv bar)
  {
    if (this.panel == null || this.panel.Children.Contains((UIElement) bar))
      return;
    if (bar.Parent != null && bar.Parent is TrayPanel)
      (bar.Parent as TrayPanel).Children.Remove((UIElement) bar);
    this.panel.Children.Add((UIElement) bar);
  }

  private void InsertBars()
  {
    foreach (ToolBarAdv toolBar in (Collection<ToolBarAdv>) this.ToolBars)
      this.InsertToPanel(toolBar);
  }

  internal ToolBarBand GetPreviousBand(ToolBarBand band)
  {
    if (this.Bands.Contains(band))
    {
      int num1 = this.Bands.IndexOf(band);
      if (num1 != 0)
      {
        int num2;
        return this.Bands[num2 = num1 - 1];
      }
    }
    return (ToolBarBand) null;
  }

  internal ToolBarBand GetNextBand(ToolBarBand band)
  {
    if (this.Bands.Contains(band))
    {
      int num1 = this.Bands.IndexOf(band);
      if (num1 != this.Bands.Count - 1)
      {
        int num2;
        return this.Bands[num2 = num1 + 1];
      }
    }
    return (ToolBarBand) null;
  }

  private void InsertBand(int bandNo, ToolBarAdv bar)
  {
    ToolBarBand toolBarBand1 = this.GetToolBarBand(bandNo);
    if (toolBarBand1 != null)
    {
      toolBarBand1.Insert(bar);
    }
    else
    {
      ToolBarBand toolBarBand2 = new ToolBarBand();
      toolBarBand2.BandNo = bar.Band;
      toolBarBand2.Insert(bar);
      this.Bands.Add(toolBarBand2);
    }
  }

  internal ToolBarBand GetBandFromPoint(Point point)
  {
    foreach (ToolBarBand band in this.Bands)
    {
      double orientedTopValue = OrientedValue.GetOrientedTopValue(band.BoundingRectangle, this.Orientation);
      double orientedBottomValue = OrientedValue.GetOrientedBottomValue(band.BoundingRectangle, this.Orientation);
      double orientedYvalue = OrientedValue.GetOrientedYValue(point, this.Orientation);
      if (orientedTopValue < orientedYvalue && orientedBottomValue > orientedYvalue)
        return band;
    }
    return (ToolBarBand) null;
  }

  internal ToolBarBand TryCreateNewBand(Point point)
  {
    this.CorrectBandNos();
    ToolBarBand lastBand = this.GetLastBand();
    if (lastBand != null && OrientedValue.GetOrientedBottomValue(lastBand.BoundingRectangle, this.Orientation) <= OrientedValue.GetOrientedYValue(point, this.Orientation))
    {
      ToolBarBand newBand = new ToolBarBand();
      newBand.BandNo = lastBand.BandNo + 1;
      this.Bands.Add(newBand);
      return newBand;
    }
    ToolBarBand firstBand = this.GetFirstBand();
    if (firstBand != null && OrientedValue.GetOrientedTopValue(firstBand.BoundingRectangle, this.Orientation) >= OrientedValue.GetOrientedYValue(point, this.Orientation))
    {
      ToolBarBand newBand = new ToolBarBand();
      newBand.BandNo = 0;
      this.Bands.Insert(0, newBand);
      return newBand;
    }
    this.CorrectBandNos();
    return (ToolBarBand) null;
  }

  internal ToolBarBand GetLastBand()
  {
    return this.Bands.Count > 0 ? this.Bands.Last<ToolBarBand>() : (ToolBarBand) null;
  }

  internal ToolBarBand GetFirstBand()
  {
    return this.Bands.Count > 0 ? this.Bands.First<ToolBarBand>() : (ToolBarBand) null;
  }

  internal void MoveBarToBand(ToolBarAdv bar, ToolBarBand band, double xPos)
  {
    if (bar == null || band == null || double.IsNaN(xPos))
      return;
    int position = band.GetPosition(xPos);
    bar.ToolBarBand.Remove(bar);
    if (bar.ToolBarBand.ToolBars.Count == 0)
      this.Bands.Remove(bar.ToolBarBand);
    band.InsertAt(position, bar);
    this.InvalidateLayout();
    if (this.ToolBarManager == null)
      return;
    this.ToolBarManager.Invalidate();
  }

  internal void InvalidateLayout()
  {
    this.InvalidateMeasure();
    this.InvalidateArrange();
    this.UpdateLayout();
  }

  private ToolBarBand GetToolBarBand(int bandNo)
  {
    foreach (ToolBarBand band in this.Bands)
    {
      if (band.BandNo == bandNo)
        return band;
    }
    return (ToolBarBand) null;
  }
}
