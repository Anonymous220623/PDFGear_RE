// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.NavigationView
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.DependencyInjection;
using pdfeditor.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls;

[TemplatePart(Name = "PART_LayoutRoot")]
[TemplatePart(Name = "PART_ResizeDragger")]
public partial class NavigationView : ListBox
{
  private const string LayoutRootName = "PART_LayoutRoot";
  private const string ResizeDraggerName = "PART_ResizeDragger";
  public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (NavigationView), new PropertyMetadata((object) null, new PropertyChangedCallback(NavigationView.OnHeaderPropertyChanged)));
  public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (NavigationView), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(nameof (Footer), typeof (object), typeof (NavigationView), new PropertyMetadata((object) null, new PropertyChangedCallback(NavigationView.OnFooterPropertyChanged)));
  public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register(nameof (FooterTemplate), typeof (DataTemplate), typeof (NavigationView), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty NavigationListWidthProperty = DependencyProperty.Register(nameof (NavigationListWidth), typeof (double), typeof (NavigationView), new PropertyMetadata((object) 32.0));
  public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof (Content), typeof (object), typeof (NavigationView), new PropertyMetadata((object) null, new PropertyChangedCallback(NavigationView.OnContentPropertyChanged)));
  public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(nameof (ContentTemplate), typeof (DataTemplate), typeof (NavigationView), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty PaneBackgroundProperty = DependencyProperty.Register(nameof (PaneBackground), typeof (Brush), typeof (NavigationView), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MinContentWidthProperty = DependencyProperty.Register(nameof (MinContentWidth), typeof (double), typeof (NavigationView), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty MaxContentWidthProperty = DependencyProperty.Register(nameof (MaxContentWidth), typeof (double), typeof (NavigationView), new PropertyMetadata((object) double.MaxValue));
  public static readonly DependencyProperty ContentWidthProperty = DependencyProperty.Register(nameof (ContentWidth), typeof (double), typeof (NavigationView), new PropertyMetadata((object) 240.0));
  public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register(nameof (IsClosed), typeof (bool), typeof (NavigationView), new PropertyMetadata((object) true, new PropertyChangedCallback(NavigationView.OnIsClosedPropertyChanged)));
  public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(nameof (Direction), typeof (NavigationViewDirection), typeof (NavigationView), new PropertyMetadata((object) NavigationViewDirection.Left, new PropertyChangedCallback(NavigationView.OnDirectionPropertyChanged)));
  public static readonly DependencyProperty IsAnimationEnabledProperty = DependencyProperty.Register(nameof (IsAnimationEnabled), typeof (bool), typeof (NavigationView), new PropertyMetadata((object) true));
  private Thumb resizeDragger;
  private double oldWidth = -1.0;

  static NavigationView()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (NavigationView), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (NavigationView)));
  }

  public NavigationView() => VisualStateManager.GoToState((FrameworkElement) this, "Close", true);

  public object Header
  {
    get => this.GetValue(NavigationView.HeaderProperty);
    set => this.SetValue(NavigationView.HeaderProperty, value);
  }

  public DataTemplate HeaderTemplate
  {
    get => (DataTemplate) this.GetValue(NavigationView.HeaderTemplateProperty);
    set => this.SetValue(NavigationView.HeaderTemplateProperty, (object) value);
  }

  public object Footer
  {
    get => this.GetValue(NavigationView.FooterProperty);
    set => this.SetValue(NavigationView.FooterProperty, value);
  }

  public DataTemplate FooterTemplate
  {
    get => (DataTemplate) this.GetValue(NavigationView.FooterTemplateProperty);
    set => this.SetValue(NavigationView.FooterTemplateProperty, (object) value);
  }

  public double NavigationListWidth
  {
    get => (double) this.GetValue(NavigationView.NavigationListWidthProperty);
    set => this.SetValue(NavigationView.NavigationListWidthProperty, (object) value);
  }

  public object Content
  {
    get => this.GetValue(NavigationView.ContentProperty);
    set => this.SetValue(NavigationView.ContentProperty, value);
  }

  public DataTemplate ContentTemplate
  {
    get => (DataTemplate) this.GetValue(NavigationView.ContentTemplateProperty);
    set => this.SetValue(NavigationView.ContentTemplateProperty, (object) value);
  }

  public Brush PaneBackground
  {
    get => (Brush) this.GetValue(NavigationView.PaneBackgroundProperty);
    set => this.SetValue(NavigationView.PaneBackgroundProperty, (object) value);
  }

  public double MinContentWidth
  {
    get => (double) this.GetValue(NavigationView.MinContentWidthProperty);
    set => this.SetValue(NavigationView.MinContentWidthProperty, (object) value);
  }

  public double MaxContentWidth
  {
    get => (double) this.GetValue(NavigationView.MaxContentWidthProperty);
    set => this.SetValue(NavigationView.MaxContentWidthProperty, (object) value);
  }

  public double ContentWidth
  {
    get => (double) this.GetValue(NavigationView.ContentWidthProperty);
    set => this.SetValue(NavigationView.ContentWidthProperty, (object) value);
  }

  public bool IsClosed
  {
    get => (bool) this.GetValue(NavigationView.IsClosedProperty);
    set => this.SetValue(NavigationView.IsClosedProperty, (object) value);
  }

  public NavigationViewDirection Direction
  {
    get => (NavigationViewDirection) this.GetValue(NavigationView.DirectionProperty);
    set => this.SetValue(NavigationView.DirectionProperty, (object) value);
  }

  public bool IsAnimationEnabled
  {
    get => (bool) this.GetValue(NavigationView.IsAnimationEnabledProperty);
    set => this.SetValue(NavigationView.IsAnimationEnabledProperty, (object) value);
  }

  private static void OnHeaderPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is NavigationView navigationView))
      return;
    navigationView.RemoveLogicalChild(e.OldValue);
    if (!(e.NewValue is DependencyObject newValue))
      return;
    navigationView.AddLogicalChild((object) newValue);
  }

  private static void OnFooterPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is NavigationView navigationView))
      return;
    navigationView.RemoveLogicalChild(e.OldValue);
    if (!(e.NewValue is DependencyObject newValue))
      return;
    navigationView.AddLogicalChild((object) newValue);
  }

  private static void OnContentPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is NavigationView navigationView))
      return;
    navigationView.RemoveLogicalChild(e.OldValue);
    if (!(e.NewValue is DependencyObject newValue))
      return;
    navigationView.AddLogicalChild((object) newValue);
  }

  private static void OnIsClosedPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is NavigationView navigationView) || object.Equals(e.NewValue, e.OldValue))
      return;
    navigationView.UpdateInlineState();
  }

  private static void OnDirectionPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is NavigationView navigationView) || object.Equals(e.NewValue, e.OldValue))
      return;
    navigationView.UpdateInlineState();
  }

  private Thumb ResizeDragger
  {
    get => this.resizeDragger;
    set
    {
      if (this.resizeDragger == value)
        return;
      if (this.resizeDragger != null)
      {
        this.resizeDragger.DragStarted -= new DragStartedEventHandler(this.ResizeDragger_DragStarted);
        this.resizeDragger.DragDelta -= new DragDeltaEventHandler(this.ResizeDragger_DragDelta);
        this.resizeDragger.DragCompleted -= new DragCompletedEventHandler(this.ResizeDragger_DragCompleted);
      }
      this.resizeDragger = value;
      if (this.resizeDragger == null)
        return;
      this.resizeDragger.DragStarted += new DragStartedEventHandler(this.ResizeDragger_DragStarted);
      this.resizeDragger.DragDelta += new DragDeltaEventHandler(this.ResizeDragger_DragDelta);
      this.resizeDragger.DragCompleted += new DragCompletedEventHandler(this.ResizeDragger_DragCompleted);
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.ResizeDragger = this.GetTemplateChild("PART_ResizeDragger") as Thumb;
    this.UpdateInlineState();
  }

  private void ResizeDragger_DragStarted(object sender, DragStartedEventArgs e)
  {
    this.oldWidth = this.ContentWidth;
  }

  private void ResizeDragger_DragDelta(object sender, DragDeltaEventArgs e)
  {
    if (this.oldWidth == -1.0)
      return;
    double num = e.HorizontalChange;
    if (this.Direction == NavigationViewDirection.Right)
      num = -num;
    this.oldWidth += num;
    if (this.oldWidth > this.MaxContentWidth)
      this.oldWidth = this.MaxContentWidth;
    if (this.oldWidth < this.MinContentWidth)
      this.oldWidth = this.MinContentWidth;
    this.ContentWidth = this.oldWidth;
    Ioc.Default.GetRequiredService<MainViewModel>().SetPageStyle();
  }

  private void ResizeDragger_DragCompleted(object sender, DragCompletedEventArgs e)
  {
    this.oldWidth = -1.0;
  }

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new NavigationViewItem();
  }

  protected override bool IsItemItsOwnContainerOverride(object item) => item is NavigationViewItem;

  protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
  {
    base.PrepareContainerForItemOverride(element, item);
    if (!(element is NavigationViewItem navigationViewItem))
      return;
    navigationViewItem.ItemClicked += new EventHandler<NavigationViewItemClickEventArgs>(this.Container_ItemClicked);
  }

  protected override void ClearContainerForItemOverride(DependencyObject element, object item)
  {
    if (element is NavigationViewItem navigationViewItem)
      navigationViewItem.ItemClicked -= new EventHandler<NavigationViewItemClickEventArgs>(this.Container_ItemClicked);
    base.ClearContainerForItemOverride(element, item);
  }

  private async void Container_ItemClicked(object sender, NavigationViewItemClickEventArgs e)
  {
    if (!(sender is NavigationViewItem element))
      element = (NavigationViewItem) null;
    else if (!element.IsSelected)
    {
      element = (NavigationViewItem) null;
    }
    else
    {
      await Task.Yield();
      element.IsSelected = false;
      element = (NavigationViewItem) null;
    }
  }

  protected override void OnSelectionChanged(SelectionChangedEventArgs e)
  {
    base.OnSelectionChanged(e);
    this.IsClosed = this.SelectedItem == null;
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    if (requiredService.DocumentWrapper.Document == null)
      return;
    requiredService.SetPageStyle();
  }

  private void UpdateInlineState()
  {
    if (this.IsClosed)
      VisualStateManager.GoToState((FrameworkElement) this, "Close", this.IsAnimationEnabled);
    else if (this.Direction == NavigationViewDirection.Left)
      VisualStateManager.GoToState((FrameworkElement) this, "LeftInline", this.IsAnimationEnabled);
    else
      VisualStateManager.GoToState((FrameworkElement) this, "RightInline", this.IsAnimationEnabled);
  }
}
