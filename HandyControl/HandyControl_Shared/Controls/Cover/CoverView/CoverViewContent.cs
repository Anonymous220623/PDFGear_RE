// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.CoverViewContent
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;
using HandyControl.Tools.Extension;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_Triangle", Type = typeof (FrameworkElement))]
[TemplatePart(Name = "PART_Content", Type = typeof (FrameworkElement))]
public class CoverViewContent : ContentControl
{
  private const string ElementTriangle = "PART_Triangle";
  private const string ElementContent = "PART_Content";
  private FrameworkElement _triangle;
  private FrameworkElement _content;
  private int _index;
  private int _groups;
  private double _itemWidth;
  private bool _isOpen;
  internal static readonly DependencyProperty ManualHeightProperty = DependencyProperty.Register(nameof (ManualHeight), typeof (double), typeof (CoverViewContent), new PropertyMetadata(ValueBoxes.Double0Box), new ValidateValueCallback(ValidateHelper.IsInRangeOfPosDoubleIncludeZero));
  public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register(nameof (ContentHeight), typeof (double), typeof (CoverViewContent), new PropertyMetadata(ValueBoxes.Double300Box), new ValidateValueCallback(ValidateHelper.IsInRangeOfPosDoubleIncludeZero));

  internal bool WaitForUpdate { get; set; }

  internal bool CanSwitch { get; set; } = true;

  internal bool IsOpen
  {
    get => this._isOpen;
    set
    {
      if (this._isOpen == value)
        return;
      this._isOpen = value;
      this.OpenSwitch(value);
    }
  }

  internal double ManualHeight
  {
    get => (double) this.GetValue(CoverViewContent.ManualHeightProperty);
    set => this.SetValue(CoverViewContent.ManualHeightProperty, (object) value);
  }

  public double ContentHeight
  {
    get => (double) this.GetValue(CoverViewContent.ContentHeightProperty);
    set => this.SetValue(CoverViewContent.ContentHeightProperty, (object) value);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this._triangle = this.GetTemplateChild("PART_Triangle") as FrameworkElement;
    this._content = this.GetTemplateChild("PART_Content") as FrameworkElement;
    if (!this.WaitForUpdate)
      return;
    this._triangle.BeginAnimation(FrameworkElement.MarginProperty, (AnimationTimeline) AnimationHelper.CreateAnimation(new Thickness(((double) (this._index % this._groups) + 0.5) * this._itemWidth - this._triangle.Width / 2.0, 0.0, 0.0, 0.0)));
    this.OpenSwitch(this._isOpen);
    this.WaitForUpdate = false;
  }

  internal void UpdatePosition(int index, int groups, double itemWidth)
  {
    if (this._triangle == null)
    {
      this._index = index;
      this._groups = groups;
      this._itemWidth = itemWidth;
      this.WaitForUpdate = true;
    }
    else
    {
      this._triangle.BeginAnimation(FrameworkElement.MarginProperty, (AnimationTimeline) AnimationHelper.CreateAnimation(new Thickness(((double) (index % groups) + 0.5) * itemWidth - this._triangle.Width / 2.0, 0.0, 0.0, 0.0)));
      if (!this.IsOpen)
        return;
      if (this.ManualHeight > 0.0 && !MathHelper.AreClose(this.ManualHeight, this.ContentHeight))
        this._content.BeginAnimation(FrameworkElement.HeightProperty, (AnimationTimeline) AnimationHelper.CreateAnimation(this.ManualHeight));
      else
        this._content.BeginAnimation(FrameworkElement.HeightProperty, (AnimationTimeline) AnimationHelper.CreateAnimation(this.ContentHeight));
    }
  }

  private void OpenSwitch(bool isOpen)
  {
    if (this._content == null)
      return;
    DoubleAnimation animation = AnimationHelper.CreateAnimation(isOpen ? (this.ManualHeight > 0.0 ? this.ManualHeight : this.ContentHeight) : 0.0);
    this._triangle.Show(false);
    this.Show(true);
    animation.Completed += (EventHandler) ((s, e) =>
    {
      this.CanSwitch = true;
      this.Show(this.IsOpen);
      if (!this.IsOpen)
        return;
      this._triangle.Show(true);
    });
    this.CanSwitch = false;
    this._content.BeginAnimation(FrameworkElement.HeightProperty, (AnimationTimeline) animation);
  }
}
