// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.TabPanel
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class TabPanel : Panel
{
  private int _itemCount;
  internal bool CanUpdate = true;
  internal Dictionary<int, TabItem> ItemDic = new Dictionary<int, TabItem>();
  public static readonly DependencyPropertyKey FluidMoveDurationPropertyKey = DependencyProperty.RegisterReadOnly(nameof (FluidMoveDuration), typeof (Duration), typeof (TabPanel), new PropertyMetadata((object) new Duration(TimeSpan.FromMilliseconds(0.0))));
  public static readonly DependencyProperty FluidMoveDurationProperty = TabPanel.FluidMoveDurationPropertyKey.DependencyProperty;
  public static readonly DependencyProperty IsTabFillEnabledProperty = DependencyProperty.Register(nameof (IsTabFillEnabled), typeof (bool), typeof (TabPanel), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty TabItemWidthProperty = DependencyProperty.Register(nameof (TabItemWidth), typeof (double), typeof (TabPanel), new PropertyMetadata((object) 200.0));
  public static readonly DependencyProperty TabItemHeightProperty = DependencyProperty.Register(nameof (TabItemHeight), typeof (double), typeof (TabPanel), new PropertyMetadata((object) 30.0));
  internal bool ForceUpdate;
  private Size _oldSize;
  private bool _isLoaded;

  public Duration FluidMoveDuration
  {
    get => (Duration) this.GetValue(TabPanel.FluidMoveDurationProperty);
    set => this.SetValue(TabPanel.FluidMoveDurationProperty, (object) value);
  }

  public bool IsTabFillEnabled
  {
    get => (bool) this.GetValue(TabPanel.IsTabFillEnabledProperty);
    set => this.SetValue(TabPanel.IsTabFillEnabledProperty, ValueBoxes.BooleanBox(value));
  }

  public double TabItemWidth
  {
    get => (double) this.GetValue(TabPanel.TabItemWidthProperty);
    set => this.SetValue(TabPanel.TabItemWidthProperty, (object) value);
  }

  public double TabItemHeight
  {
    get => (double) this.GetValue(TabPanel.TabItemHeightProperty);
    set => this.SetValue(TabPanel.TabItemHeightProperty, (object) value);
  }

  protected override Size MeasureOverride(Size constraint)
  {
    if ((this._itemCount == this.InternalChildren.Count || !this.CanUpdate) && !this.ForceUpdate && !this.IsTabFillEnabled)
      return this._oldSize;
    constraint.Height = this.TabItemHeight;
    this._itemCount = this.InternalChildren.Count;
    Size size = new Size();
    this.ItemDic.Clear();
    int count = this.InternalChildren.Count;
    if (count == 0)
    {
      this._oldSize = new Size();
      return this._oldSize;
    }
    constraint.Width += (double) this.InternalChildren.Count;
    double num1 = 0.0;
    int[] numArray = new int[count];
    if (!this.IsTabFillEnabled)
      num1 = this.TabItemWidth;
    else if (this.TemplatedParent is TabControl templatedParent)
      numArray = ArithmeticHelper.DivideInt2Arr((int) templatedParent.ActualWidth + this.InternalChildren.Count, count);
    for (int index = 0; index < count; ++index)
    {
      if (this.IsTabFillEnabled)
        num1 = (double) numArray[index];
      if (this.InternalChildren[index] is TabItem internalChild)
      {
        internalChild.RenderTransform = (Transform) new TranslateTransform();
        internalChild.MaxWidth = num1;
        Rect rect = new Rect();
        ref Rect local = ref rect;
        double width = size.Width;
        Thickness borderThickness = internalChild.BorderThickness;
        double left1 = borderThickness.Left;
        double num2 = width - left1;
        local.X = num2;
        rect.Width = num1;
        rect.Height = this.TabItemHeight;
        Rect finalRect = rect;
        internalChild.Arrange(finalRect);
        TabItem tabItem = internalChild;
        double num3 = num1;
        borderThickness = internalChild.BorderThickness;
        double left2 = borderThickness.Left;
        double num4 = num3 - left2;
        tabItem.ItemWidth = num4;
        internalChild.CurrentIndex = index;
        internalChild.TargetOffsetX = 0.0;
        this.ItemDic[index] = internalChild;
        size.Width += internalChild.ItemWidth;
      }
    }
    size.Height = constraint.Height;
    this._oldSize = size;
    return this._oldSize;
  }

  public TabPanel()
  {
    this.Loaded += (RoutedEventHandler) ((s, e) =>
    {
      if (this._isLoaded)
        return;
      this.ForceUpdate = true;
      this.Measure(new Size(this.DesiredSize.Width, this.ActualHeight));
      this.ForceUpdate = false;
      foreach (TabItem tabItem in this.ItemDic.Values)
        tabItem.TabPanel = this;
      this._isLoaded = true;
    });
  }
}
