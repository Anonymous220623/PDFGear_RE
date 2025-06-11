// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarRadioButton
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace pdfeditor.Controls.Menus;

public class ToolbarRadioButton : KeyedRadioButton
{
  private ContentPresenter headerPresenter;
  private Window window;
  public static readonly DependencyProperty HeaderProperty = ToolbarButtonHelper.HeaderProperty.AddOwner(typeof (ToolbarRadioButton));
  public static readonly DependencyProperty HeaderTemplateProperty = ToolbarButtonHelper.HeaderTemplateProperty.AddOwner(typeof (ToolbarRadioButton));
  public static readonly DependencyProperty OrientationProperty = ToolbarButtonHelper.OrientationProperty.AddOwner(typeof (ToolbarRadioButton), new PropertyMetadata((object) Orientation.Vertical, new PropertyChangedCallback(ToolbarRadioButton.OnOrientationPropertyChanged)));
  public static readonly DependencyProperty IsToggleEnabledProperty = DependencyProperty.Register(nameof (IsToggleEnabled), typeof (bool), typeof (ToolbarRadioButton), new PropertyMetadata((object) true));
  public static readonly DependencyProperty IsCheckableProperty = DependencyProperty.Register(nameof (IsCheckable), typeof (bool), typeof (ToolbarRadioButton), new PropertyMetadata((object) true, new PropertyChangedCallback(ToolbarRadioButton.OnIsCheckablePropertyChanged)));

  static ToolbarRadioButton()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ToolbarRadioButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ToolbarRadioButton)));
  }

  public ToolbarRadioButton()
  {
    this.Loaded += new RoutedEventHandler(this.ToolbarButton_Loaded);
    this.Unloaded += new RoutedEventHandler(this.ToolbarButton_Unloaded);
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
    get => this.GetValue(ToolbarRadioButton.HeaderProperty);
    set => this.SetValue(ToolbarRadioButton.HeaderProperty, value);
  }

  public DataTemplate HeaderTemplate
  {
    get => (DataTemplate) this.GetValue(ToolbarRadioButton.HeaderTemplateProperty);
    set => this.SetValue(ToolbarRadioButton.HeaderTemplateProperty, (object) value);
  }

  public Orientation Orientation
  {
    get => (Orientation) this.GetValue(ToolbarRadioButton.OrientationProperty);
    set => this.SetValue(ToolbarRadioButton.OrientationProperty, (object) value);
  }

  private static void OnOrientationPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if ((Orientation) e.NewValue == (Orientation) e.OldValue || !(d is ToolbarRadioButton button))
      return;
    ToolbarButtonHelper.UpdateHeaderStates((ButtonBase) button);
  }

  public bool IsToggleEnabled
  {
    get => (bool) this.GetValue(ToolbarRadioButton.IsToggleEnabledProperty);
    set => this.SetValue(ToolbarRadioButton.IsToggleEnabledProperty, (object) value);
  }

  public bool IsCheckable
  {
    get => (bool) this.GetValue(ToolbarRadioButton.IsCheckableProperty);
    set => this.SetValue(ToolbarRadioButton.IsCheckableProperty, (object) value);
  }

  private static void OnIsCheckablePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if ((bool) e.NewValue == (bool) e.OldValue || !(d is ToolbarRadioButton toolbarRadioButton) || (bool) e.NewValue)
      return;
    bool? isChecked = toolbarRadioButton.IsChecked;
    bool flag = false;
    if (isChecked.GetValueOrDefault() == flag & isChecked.HasValue)
      return;
    toolbarRadioButton.IsChecked = new bool?(false);
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

  protected override void OnMouseEnter(MouseEventArgs e) => base.OnMouseEnter(e);

  protected override void OnClick() => base.OnClick();

  protected override void OnToggle()
  {
    if (this.IsCheckable && this.IsToggleEnabled)
      this.IsChecked = new bool?(!this.IsChecked.GetValueOrDefault());
    else
      base.OnToggle();
  }
}
