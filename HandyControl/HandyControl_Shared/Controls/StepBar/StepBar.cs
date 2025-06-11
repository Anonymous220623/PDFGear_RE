// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.StepBar
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Controls;

[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof (StepBarItem))]
[DefaultEvent("StepChanged")]
[TemplatePart(Name = "PART_ProgressBarBack", Type = typeof (ProgressBar))]
public class StepBar : ItemsControl
{
  private const string ElementProgressBarBack = "PART_ProgressBarBack";
  private ProgressBar _progressBarBack;
  private int _oriStepIndex = -1;
  public static readonly RoutedEvent StepChangedEvent = EventManager.RegisterRoutedEvent("StepChanged", RoutingStrategy.Bubble, typeof (EventHandler<FunctionEventArgs<int>>), typeof (StepBar));
  public static readonly DependencyProperty StepIndexProperty = DependencyProperty.Register(nameof (StepIndex), typeof (int), typeof (StepBar), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Int0Box, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(StepBar.OnStepIndexChanged), new CoerceValueCallback(StepBar.CoerceStepIndex)));
  public static readonly DependencyProperty DockProperty = DependencyProperty.Register(nameof (Dock), typeof (Dock), typeof (StepBar), new PropertyMetadata((object) Dock.Top));
  public static readonly DependencyProperty IsMouseSelectableProperty = DependencyProperty.Register(nameof (IsMouseSelectable), typeof (bool), typeof (StepBar), new PropertyMetadata(ValueBoxes.FalseBox));

  public StepBar()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Next, (ExecutedRoutedEventHandler) ((_1, _2) => this.Next())));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Prev, (ExecutedRoutedEventHandler) ((_3, _4) => this.Prev())));
    this.ItemContainerGenerator.StatusChanged += new EventHandler(this.ItemContainerGenerator_StatusChanged);
    this.AddHandler(SelectableItem.SelectedEvent, (Delegate) new RoutedEventHandler(this.OnStepBarItemSelected));
  }

  private void OnStepBarItemSelected(object sender, RoutedEventArgs e)
  {
    if (!this.IsMouseSelectable || !(e.OriginalSource is StepBarItem originalSource))
      return;
    this.SetCurrentValue(StepBar.StepIndexProperty, (object) (originalSource.Index - 1));
  }

  private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
  {
    if (this.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
      return;
    int count = this.Items.Count;
    this.InvalidateVisual();
    this._progressBarBack.Maximum = (double) (count - 1);
    this._progressBarBack.Value = (double) this.StepIndex;
    if (count <= 0)
      return;
    for (int index = 0; index < count; ++index)
    {
      if (this.ItemContainerGenerator.ContainerFromIndex(index) is StepBarItem stepBarItem)
        stepBarItem.Index = index + 1;
    }
    if (this._oriStepIndex > 0)
    {
      this.StepIndex = this._oriStepIndex;
      this._oriStepIndex = -1;
    }
    else
      this.OnStepIndexChanged(this.StepIndex);
  }

  protected override bool IsItemItsOwnContainerOverride(object item) => item is StepBarItem;

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new StepBarItem();
  }

  [Category("Behavior")]
  public event EventHandler<FunctionEventArgs<int>> StepChanged
  {
    add => this.AddHandler(StepBar.StepChangedEvent, (Delegate) value);
    remove => this.RemoveHandler(StepBar.StepChangedEvent, (Delegate) value);
  }

  private static object CoerceStepIndex(DependencyObject d, object baseValue)
  {
    StepBar stepBar = (StepBar) d;
    int num = (int) baseValue;
    if (stepBar.Items.Count == 0 && num > 0)
    {
      stepBar._oriStepIndex = num;
      return ValueBoxes.Int0Box;
    }
    if (num < 0)
      return ValueBoxes.Int0Box;
    return num < stepBar.Items.Count ? baseValue : (object) (stepBar.Items.Count == 0 ? 0 : stepBar.Items.Count - 1);
  }

  private static void OnStepIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((StepBar) d).OnStepIndexChanged((int) e.NewValue);
  }

  private void OnStepIndexChanged(int stepIndex)
  {
    for (int index = 0; index < stepIndex; ++index)
    {
      if (this.ItemContainerGenerator.ContainerFromIndex(index) is StepBarItem stepBarItem)
        stepBarItem.Status = StepStatus.Complete;
    }
    for (int index = stepIndex + 1; index < this.Items.Count; ++index)
    {
      if (this.ItemContainerGenerator.ContainerFromIndex(index) is StepBarItem stepBarItem)
        stepBarItem.Status = StepStatus.Waiting;
    }
    if (this.ItemContainerGenerator.ContainerFromIndex(stepIndex) is StepBarItem stepBarItem1)
      stepBarItem1.Status = StepStatus.UnderWay;
    this._progressBarBack?.BeginAnimation(RangeBase.ValueProperty, (AnimationTimeline) AnimationHelper.CreateAnimation((double) stepIndex));
    this.RaiseEvent((RoutedEventArgs) new FunctionEventArgs<int>(StepBar.StepChangedEvent, (object) this)
    {
      Info = stepIndex
    });
  }

  public int StepIndex
  {
    get => (int) this.GetValue(StepBar.StepIndexProperty);
    set => this.SetValue(StepBar.StepIndexProperty, (object) value);
  }

  public Dock Dock
  {
    get => (Dock) this.GetValue(StepBar.DockProperty);
    set => this.SetValue(StepBar.DockProperty, (object) value);
  }

  public bool IsMouseSelectable
  {
    get => (bool) this.GetValue(StepBar.IsMouseSelectableProperty);
    set => this.SetValue(StepBar.IsMouseSelectableProperty, ValueBoxes.BooleanBox(value));
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this._progressBarBack = this.GetTemplateChild("PART_ProgressBarBack") as ProgressBar;
  }

  protected override void OnRender(DrawingContext drawingContext)
  {
    base.OnRender(drawingContext);
    int count = this.Items.Count;
    if (this._progressBarBack == null || count <= 0)
      return;
    bool flag;
    switch (this.Dock)
    {
      case Dock.Top:
      case Dock.Bottom:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (flag)
      this._progressBarBack.Width = (double) (count - 1) * (this.ActualWidth / (double) count);
    else
      this._progressBarBack.Height = (double) (count - 1) * (this.ActualHeight / (double) count);
  }

  public void Next() => ++this.StepIndex;

  public void Prev() => --this.StepIndex;
}
