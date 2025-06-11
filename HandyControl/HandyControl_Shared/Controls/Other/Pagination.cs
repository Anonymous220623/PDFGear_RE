// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Pagination
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;
using HandyControl.Tools.Extension;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_ButtonLeft", Type = typeof (Button))]
[TemplatePart(Name = "PART_ButtonRight", Type = typeof (Button))]
[TemplatePart(Name = "PART_ButtonFirst", Type = typeof (RadioButton))]
[TemplatePart(Name = "PART_MoreLeft", Type = typeof (FrameworkElement))]
[TemplatePart(Name = "PART_PanelMain", Type = typeof (Panel))]
[TemplatePart(Name = "PART_MoreRight", Type = typeof (FrameworkElement))]
[TemplatePart(Name = "PART_ButtonLast", Type = typeof (RadioButton))]
[TemplatePart(Name = "PART_ButtonLast", Type = typeof (NumericUpDown))]
public class Pagination : Control
{
  private const string ElementButtonLeft = "PART_ButtonLeft";
  private const string ElementButtonRight = "PART_ButtonRight";
  private const string ElementButtonFirst = "PART_ButtonFirst";
  private const string ElementMoreLeft = "PART_MoreLeft";
  private const string ElementPanelMain = "PART_PanelMain";
  private const string ElementMoreRight = "PART_MoreRight";
  private const string ElementButtonLast = "PART_ButtonLast";
  private const string ElementJump = "PART_Jump";
  private Button _buttonLeft;
  private Button _buttonRight;
  private RadioButton _buttonFirst;
  private FrameworkElement _moreLeft;
  private Panel _panelMain;
  private FrameworkElement _moreRight;
  private RadioButton _buttonLast;
  private NumericUpDown _jumpNumericUpDown;
  private bool _appliedTemplate;
  public static readonly RoutedEvent PageUpdatedEvent = EventManager.RegisterRoutedEvent("PageUpdated", RoutingStrategy.Bubble, typeof (EventHandler<FunctionEventArgs<int>>), typeof (Pagination));
  public static readonly DependencyProperty MaxPageCountProperty = DependencyProperty.Register(nameof (MaxPageCount), typeof (int), typeof (Pagination), new PropertyMetadata(ValueBoxes.Int1Box, new PropertyChangedCallback(Pagination.OnMaxPageCountChanged), new CoerceValueCallback(Pagination.CoerceMaxPageCount)), new ValidateValueCallback(ValidateHelper.IsInRangeOfPosIntIncludeZero));
  public static readonly DependencyProperty DataCountPerPageProperty = DependencyProperty.Register(nameof (DataCountPerPage), typeof (int), typeof (Pagination), new PropertyMetadata((object) 20, new PropertyChangedCallback(Pagination.OnDataCountPerPageChanged)), new ValidateValueCallback(ValidateHelper.IsInRangeOfPosInt));
  public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(nameof (PageIndex), typeof (int), typeof (Pagination), new PropertyMetadata(ValueBoxes.Int1Box, new PropertyChangedCallback(Pagination.OnPageIndexChanged), new CoerceValueCallback(Pagination.CoercePageIndex)), new ValidateValueCallback(ValidateHelper.IsInRangeOfPosIntIncludeZero));
  public static readonly DependencyProperty MaxPageIntervalProperty = DependencyProperty.Register(nameof (MaxPageInterval), typeof (int), typeof (Pagination), new PropertyMetadata((object) 3, new PropertyChangedCallback(Pagination.OnMaxPageIntervalChanged)), new ValidateValueCallback(ValidateHelper.IsInRangeOfPosIntIncludeZero));
  public static readonly DependencyProperty IsJumpEnabledProperty = DependencyProperty.Register(nameof (IsJumpEnabled), typeof (bool), typeof (Pagination), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty AutoHidingProperty = DependencyProperty.Register(nameof (AutoHiding), typeof (bool), typeof (Pagination), new PropertyMetadata(ValueBoxes.TrueBox, new PropertyChangedCallback(Pagination.OnAutoHidingChanged)));
  public static readonly DependencyProperty PaginationButtonStyleProperty = DependencyProperty.Register(nameof (PaginationButtonStyle), typeof (Style), typeof (Pagination), new PropertyMetadata((object) null));

  public event EventHandler<FunctionEventArgs<int>> PageUpdated
  {
    add => this.AddHandler(Pagination.PageUpdatedEvent, (Delegate) value);
    remove => this.RemoveHandler(Pagination.PageUpdatedEvent, (Delegate) value);
  }

  public Pagination()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Prev, new ExecutedRoutedEventHandler(this.ButtonPrev_OnClick)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Next, new ExecutedRoutedEventHandler(this.ButtonNext_OnClick)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Selected, new ExecutedRoutedEventHandler(this.ToggleButton_OnChecked)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Jump, (ExecutedRoutedEventHandler) ((s, e) => this.PageIndex = (int) this._jumpNumericUpDown.Value)));
    this.OnAutoHidingChanged(this.AutoHiding);
    this.Update();
  }

  private static object CoerceMaxPageCount(DependencyObject d, object basevalue)
  {
    int num = (int) basevalue;
    return (object) (num < 1 ? 1 : num);
  }

  private static void OnMaxPageCountChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is Pagination pagination))
      return;
    if (pagination.PageIndex > pagination.MaxPageCount)
      pagination.PageIndex = pagination.MaxPageCount;
    pagination.CoerceValue(Pagination.PageIndexProperty);
    pagination.OnAutoHidingChanged(pagination.AutoHiding);
    pagination.Update();
  }

  public int MaxPageCount
  {
    get => (int) this.GetValue(Pagination.MaxPageCountProperty);
    set => this.SetValue(Pagination.MaxPageCountProperty, (object) value);
  }

  private static void OnDataCountPerPageChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is Pagination pagination))
      return;
    pagination.Update();
  }

  public int DataCountPerPage
  {
    get => (int) this.GetValue(Pagination.DataCountPerPageProperty);
    set => this.SetValue(Pagination.DataCountPerPageProperty, (object) value);
  }

  private static object CoercePageIndex(DependencyObject d, object basevalue)
  {
    if (!(d is Pagination pagination))
      return (object) 1;
    int num = (int) basevalue;
    return (object) (num < 1 ? 1 : (num > pagination.MaxPageCount ? pagination.MaxPageCount : num));
  }

  private static void OnPageIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is Pagination source) || !(e.NewValue is int newValue))
      return;
    source.Update();
    source.RaiseEvent((RoutedEventArgs) new FunctionEventArgs<int>(Pagination.PageUpdatedEvent, (object) source)
    {
      Info = newValue
    });
  }

  public int PageIndex
  {
    get => (int) this.GetValue(Pagination.PageIndexProperty);
    set => this.SetValue(Pagination.PageIndexProperty, (object) value);
  }

  private static void OnMaxPageIntervalChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is Pagination pagination))
      return;
    pagination.Update();
  }

  public int MaxPageInterval
  {
    get => (int) this.GetValue(Pagination.MaxPageIntervalProperty);
    set => this.SetValue(Pagination.MaxPageIntervalProperty, (object) value);
  }

  public bool IsJumpEnabled
  {
    get => (bool) this.GetValue(Pagination.IsJumpEnabledProperty);
    set => this.SetValue(Pagination.IsJumpEnabledProperty, ValueBoxes.BooleanBox(value));
  }

  private static void OnAutoHidingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is Pagination pagination))
      return;
    pagination.OnAutoHidingChanged((bool) e.NewValue);
  }

  private void OnAutoHidingChanged(bool newValue) => this.Show(!newValue || this.MaxPageCount > 1);

  public bool AutoHiding
  {
    get => (bool) this.GetValue(Pagination.AutoHidingProperty);
    set => this.SetValue(Pagination.AutoHidingProperty, ValueBoxes.BooleanBox(value));
  }

  public Style PaginationButtonStyle
  {
    get => (Style) this.GetValue(Pagination.PaginationButtonStyleProperty);
    set => this.SetValue(Pagination.PaginationButtonStyleProperty, (object) value);
  }

  public override void OnApplyTemplate()
  {
    this._appliedTemplate = false;
    base.OnApplyTemplate();
    this._buttonLeft = this.GetTemplateChild("PART_ButtonLeft") as Button;
    this._buttonRight = this.GetTemplateChild("PART_ButtonRight") as Button;
    this._buttonFirst = this.GetTemplateChild("PART_ButtonFirst") as RadioButton;
    this._moreLeft = this.GetTemplateChild("PART_MoreLeft") as FrameworkElement;
    this._panelMain = this.GetTemplateChild("PART_PanelMain") as Panel;
    this._moreRight = this.GetTemplateChild("PART_MoreRight") as FrameworkElement;
    this._buttonLast = this.GetTemplateChild("PART_ButtonLast") as RadioButton;
    this._jumpNumericUpDown = this.GetTemplateChild("PART_Jump") as NumericUpDown;
    this.CheckNull();
    this._appliedTemplate = true;
    this.Update();
  }

  private void CheckNull()
  {
    if (this._buttonLeft == null || this._buttonRight == null || this._buttonFirst == null || this._moreLeft == null || this._panelMain == null || this._moreRight == null || this._buttonLast == null)
      throw new Exception();
  }

  private void Update()
  {
    if (!this._appliedTemplate)
      return;
    this._buttonLeft.IsEnabled = this.PageIndex > 1;
    this._buttonRight.IsEnabled = this.PageIndex < this.MaxPageCount;
    if (this.MaxPageInterval == 0)
    {
      this._buttonFirst.Collapse();
      this._buttonLast.Collapse();
      this._moreLeft.Collapse();
      this._moreRight.Collapse();
      this._panelMain.Children.Clear();
      RadioButton button = this.CreateButton(this.PageIndex);
      this._panelMain.Children.Add((UIElement) button);
      button.IsChecked = new bool?(true);
    }
    else
    {
      this._buttonFirst.Show();
      this._buttonLast.Show();
      this._moreLeft.Show();
      this._moreRight.Show();
      if (this.MaxPageCount == 1)
      {
        this._buttonLast.Collapse();
      }
      else
      {
        this._buttonLast.Show();
        this._buttonLast.Content = (object) this.MaxPageCount.ToString();
      }
      int num1 = this.MaxPageCount - this.PageIndex;
      int num2 = this.PageIndex - 1;
      this._moreRight.Show(num1 > this.MaxPageInterval);
      this._moreLeft.Show(num2 > this.MaxPageInterval);
      this._panelMain.Children.Clear();
      if (this.PageIndex > 1 && this.PageIndex < this.MaxPageCount)
      {
        RadioButton button = this.CreateButton(this.PageIndex);
        this._panelMain.Children.Add((UIElement) button);
        button.IsChecked = new bool?(true);
      }
      else if (this.PageIndex == 1)
        this._buttonFirst.IsChecked = new bool?(true);
      else
        this._buttonLast.IsChecked = new bool?(true);
      int pageIndex1 = this.PageIndex;
      for (int index = 0; index < this.MaxPageInterval - 1 && --pageIndex1 > 1; ++index)
        this._panelMain.Children.Insert(0, (UIElement) this.CreateButton(pageIndex1));
      int pageIndex2 = this.PageIndex;
      for (int index = 0; index < this.MaxPageInterval - 1 && ++pageIndex2 < this.MaxPageCount; ++index)
        this._panelMain.Children.Add((UIElement) this.CreateButton(pageIndex2));
    }
  }

  private void ButtonPrev_OnClick(object sender, RoutedEventArgs e) => --this.PageIndex;

  private void ButtonNext_OnClick(object sender, RoutedEventArgs e) => ++this.PageIndex;

  private RadioButton CreateButton(int page)
  {
    RadioButton button = new RadioButton();
    button.Style = this.PaginationButtonStyle;
    button.Content = (object) page.ToString();
    return button;
  }

  private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
  {
    if (!(e.OriginalSource is RadioButton originalSource))
      return;
    bool? isChecked = originalSource.IsChecked;
    bool flag = false;
    if (isChecked.GetValueOrDefault() == flag & isChecked.HasValue)
      return;
    this.PageIndex = int.Parse(originalSource.Content.ToString());
  }
}
