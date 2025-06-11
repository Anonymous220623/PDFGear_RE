// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarButton
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace pdfeditor.Controls.Menus;

public partial class ToolbarButton : Button
{
  private ContentPresenter headerPresenter;
  private Window window;
  public static readonly DependencyProperty HeaderProperty = ToolbarButtonHelper.HeaderProperty.AddOwner(typeof (ToolbarButton));
  public static readonly DependencyProperty HeaderTemplateProperty = ToolbarButtonHelper.HeaderTemplateProperty.AddOwner(typeof (ToolbarButton));
  public static readonly DependencyProperty OrientationProperty = ToolbarButtonHelper.OrientationProperty.AddOwner(typeof (ToolbarButton), new PropertyMetadata((object) Orientation.Vertical, new PropertyChangedCallback(ToolbarButton.OnOrientationPropertyChanged)));

  static ToolbarButton()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ToolbarButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ToolbarButton)));
  }

  public ToolbarButton()
  {
    if (!DesignerProperties.GetIsInDesignMode((DependencyObject) this))
    {
      this.Loaded += new RoutedEventHandler(this.ToolbarButton_Loaded);
      this.Unloaded += new RoutedEventHandler(this.ToolbarButton_Unloaded);
    }
    ToolbarButtonHelper.RegisterIsKeyboardFocused((ButtonBase) this);
  }

  private ContentPresenter HeaderPresenter
  {
    get => this.headerPresenter;
    set
    {
      if (this.headerPresenter == value)
        return;
      if (this.headerPresenter != null)
        this.headerPresenter.SizeChanged -= new SizeChangedEventHandler(this.HeaderPresenter_SizeChanged);
      this.headerPresenter = value;
      if (this.headerPresenter == null)
        return;
      this.headerPresenter.SizeChanged += new SizeChangedEventHandler(this.HeaderPresenter_SizeChanged);
    }
  }

  private void HeaderPresenter_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    ToolbarButtonHelper.UpdateHeaderStates((ButtonBase) this);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.HeaderPresenter = this.GetTemplateChild("HeaderPresenter") as ContentPresenter;
    ToolbarButtonHelper.UpdateContentStates((ButtonBase) this);
    ToolbarButtonHelper.UpdateHeaderStates((ButtonBase) this);
  }

  private void ToolbarButton_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.window != null)
    {
      this.window.Activated -= new EventHandler(this.Window_Activated);
      this.window.Deactivated -= new EventHandler(this.Window_Deactivated);
    }
    this.window = Window.GetWindow((DependencyObject) this);
    if (this.window == null)
      return;
    this.window.Activated += new EventHandler(this.Window_Activated);
    this.window.Deactivated += new EventHandler(this.Window_Deactivated);
  }

  private void ToolbarButton_Unloaded(object sender, RoutedEventArgs e)
  {
    if (this.window != null)
    {
      this.window.Activated -= new EventHandler(this.Window_Activated);
      this.window.Deactivated -= new EventHandler(this.Window_Deactivated);
    }
    this.window = (Window) null;
    this.IsMouseOverInternal = false;
  }

  private void Window_Activated(object sender, EventArgs e) => this.IsMouseOverInternal = false;

  private void Window_Deactivated(object sender, EventArgs e) => this.IsMouseOverInternal = false;

  protected override void OnMouseMove(MouseEventArgs e)
  {
    base.OnMouseMove(e);
    this.IsMouseOverInternal = ToolbarButtonHelper.IsContentMouseOver((ButtonBase) this, e);
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    base.OnMouseLeave(e);
    this.IsMouseOverInternal = ToolbarButtonHelper.IsContentMouseOver((ButtonBase) this, e);
  }

  public object Header
  {
    get => this.GetValue(ToolbarButton.HeaderProperty);
    set => this.SetValue(ToolbarButton.HeaderProperty, value);
  }

  public DataTemplate HeaderTemplate
  {
    get => (DataTemplate) this.GetValue(ToolbarButton.HeaderTemplateProperty);
    set => this.SetValue(ToolbarButton.HeaderTemplateProperty, (object) value);
  }

  public Orientation Orientation
  {
    get => (Orientation) this.GetValue(ToolbarButton.OrientationProperty);
    set => this.SetValue(ToolbarButton.OrientationProperty, (object) value);
  }

  private static void OnOrientationPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if ((Orientation) e.NewValue == (Orientation) e.OldValue || !(d is ToolbarButton button))
      return;
    ToolbarButtonHelper.UpdateHeaderStates((ButtonBase) button);
  }

  private bool IsMouseOverInternal
  {
    get => (bool) this.GetValue(ToolbarButtonHelper.IsMouseOverInternalProperty);
    set => this.SetValue(ToolbarButtonHelper.IsMouseOverInternalProperty, (object) value);
  }

  protected override void OnContentTemplateChanged(
    DataTemplate oldContentTemplate,
    DataTemplate newContentTemplate)
  {
    base.OnContentTemplateChanged(oldContentTemplate, newContentTemplate);
    ToolbarButtonHelper.UpdateContentStates((ButtonBase) this);
  }

  protected override void OnContentTemplateSelectorChanged(
    DataTemplateSelector oldContentTemplateSelector,
    DataTemplateSelector newContentTemplateSelector)
  {
    base.OnContentTemplateSelectorChanged(oldContentTemplateSelector, newContentTemplateSelector);
    ToolbarButtonHelper.UpdateContentStates((ButtonBase) this);
  }

  protected override void OnContentChanged(object oldContent, object newContent)
  {
    base.OnContentChanged(oldContent, newContent);
    ToolbarButtonHelper.UpdateContentStates((ButtonBase) this);
    ToolbarButtonHelper.UpdateHeaderStates((ButtonBase) this);
  }
}
