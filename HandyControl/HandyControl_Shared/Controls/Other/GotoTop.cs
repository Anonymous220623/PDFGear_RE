// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.GotoTop
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Extension;
using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class GotoTop : Button
{
  private Action _gotoTopAction;
  private System.Windows.Controls.ScrollViewer _scrollViewer;
  public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(nameof (Target), typeof (DependencyObject), typeof (GotoTop), new PropertyMetadata((object) null));
  public static readonly DependencyProperty AnimatedProperty = DependencyProperty.Register(nameof (Animated), typeof (bool), typeof (GotoTop), new PropertyMetadata(ValueBoxes.TrueBox));
  public static readonly DependencyProperty AnimationTimeProperty = DependencyProperty.Register(nameof (AnimationTime), typeof (double), typeof (GotoTop), new PropertyMetadata(ValueBoxes.Double200Box));
  public static readonly DependencyProperty HidingHeightProperty = DependencyProperty.Register(nameof (HidingHeight), typeof (double), typeof (GotoTop), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty AutoHidingProperty = DependencyProperty.Register(nameof (AutoHiding), typeof (bool), typeof (GotoTop), new PropertyMetadata(ValueBoxes.TrueBox));

  public DependencyObject Target
  {
    get => (DependencyObject) this.GetValue(GotoTop.TargetProperty);
    set => this.SetValue(GotoTop.TargetProperty, (object) value);
  }

  public GotoTop()
  {
    this.Loaded += (RoutedEventHandler) ((s, e) => this.CreateGotoAction(this.Target));
  }

  public virtual void CreateGotoAction(DependencyObject obj)
  {
    if (this._scrollViewer != null)
      this._scrollViewer.ScrollChanged -= new ScrollChangedEventHandler(this.ScrollViewer_ScrollChanged);
    this._scrollViewer = VisualHelper.GetChild<System.Windows.Controls.ScrollViewer>(obj);
    if (this._scrollViewer == null)
      return;
    this._scrollViewer.ScrollChanged += new ScrollChangedEventHandler(this.ScrollViewer_ScrollChanged);
    ScrollViewer scrollViewerHandy = this._scrollViewer as ScrollViewer;
    if (scrollViewerHandy != null && this.Animated && scrollViewerHandy.IsInertiaEnabled)
      this._gotoTopAction = (Action) (() => scrollViewerHandy.ScrollToTopInternal(this.AnimationTime));
    else
      this._gotoTopAction = (Action) (() => this._scrollViewer.ScrollToTop());
  }

  private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
  {
    if (!this.AutoHiding)
      return;
    this.Show(e.VerticalOffset >= this.HidingHeight);
  }

  public bool Animated
  {
    get => (bool) this.GetValue(GotoTop.AnimatedProperty);
    set => this.SetValue(GotoTop.AnimatedProperty, ValueBoxes.BooleanBox(value));
  }

  public double AnimationTime
  {
    get => (double) this.GetValue(GotoTop.AnimationTimeProperty);
    set => this.SetValue(GotoTop.AnimationTimeProperty, (object) value);
  }

  public double HidingHeight
  {
    get => (double) this.GetValue(GotoTop.HidingHeightProperty);
    set => this.SetValue(GotoTop.HidingHeightProperty, (object) value);
  }

  public bool AutoHiding
  {
    get => (bool) this.GetValue(GotoTop.AutoHidingProperty);
    set => this.SetValue(GotoTop.AutoHidingProperty, ValueBoxes.BooleanBox(value));
  }

  protected override void OnClick()
  {
    base.OnClick();
    Action gotoTopAction = this._gotoTopAction;
    if (gotoTopAction == null)
      return;
    gotoTopAction();
  }
}
