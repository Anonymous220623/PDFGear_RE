// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.EventToCommand
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Interactivity;

public class EventToCommand : TriggerAction<DependencyObject>
{
  public const string EventArgsConverterParameterPropertyName = "EventArgsConverterParameter";
  public const string AlwaysInvokeCommandPropertyName = "AlwaysInvokeCommand";
  public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof (CommandParameter), typeof (object), typeof (EventToCommand), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, e) =>
  {
    if ((s is EventToCommand eventToCommand2 ? eventToCommand2.AssociatedObject : (DependencyObject) null) == null)
      return;
    eventToCommand2.EnableDisableElement();
  })));
  public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), typeof (ICommand), typeof (EventToCommand), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, e) => EventToCommand.OnCommandChanged(s as EventToCommand, e))));
  public static readonly DependencyProperty MustToggleIsEnabledProperty = DependencyProperty.Register(nameof (MustToggleIsEnabled), typeof (bool), typeof (EventToCommand), new PropertyMetadata(ValueBoxes.FalseBox, (PropertyChangedCallback) ((s, e) =>
  {
    if ((s is EventToCommand eventToCommand4 ? eventToCommand4.AssociatedObject : (DependencyObject) null) == null)
      return;
    eventToCommand4.EnableDisableElement();
  })));
  public static readonly DependencyProperty EventArgsConverterParameterProperty = DependencyProperty.Register(nameof (EventArgsConverterParameter), typeof (object), typeof (EventToCommand), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty AlwaysInvokeCommandProperty = DependencyProperty.Register(nameof (AlwaysInvokeCommand), typeof (bool), typeof (EventToCommand), new PropertyMetadata(ValueBoxes.FalseBox));
  private object _commandParameterValue;
  private bool? _mustToggleValue;

  public ICommand Command
  {
    get => (ICommand) this.GetValue(EventToCommand.CommandProperty);
    set => this.SetValue(EventToCommand.CommandProperty, (object) value);
  }

  public object CommandParameter
  {
    get => this.GetValue(EventToCommand.CommandParameterProperty);
    set => this.SetValue(EventToCommand.CommandParameterProperty, value);
  }

  public object CommandParameterValue
  {
    get => this._commandParameterValue ?? this.CommandParameter;
    set
    {
      this._commandParameterValue = value;
      this.EnableDisableElement();
    }
  }

  public bool MustToggleIsEnabled
  {
    get => (bool) this.GetValue(EventToCommand.MustToggleIsEnabledProperty);
    set => this.SetValue(EventToCommand.MustToggleIsEnabledProperty, ValueBoxes.BooleanBox(value));
  }

  public bool MustToggleIsEnabledValue
  {
    get => this._mustToggleValue.HasValue ? this._mustToggleValue.Value : this.MustToggleIsEnabled;
    set
    {
      this._mustToggleValue = new bool?(value);
      this.EnableDisableElement();
    }
  }

  public bool PassEventArgsToCommand { get; set; }

  public IEventArgsConverter EventArgsConverter { get; set; }

  public object EventArgsConverterParameter
  {
    get => this.GetValue(EventToCommand.EventArgsConverterParameterProperty);
    set => this.SetValue(EventToCommand.EventArgsConverterParameterProperty, value);
  }

  public bool AlwaysInvokeCommand
  {
    get => (bool) this.GetValue(EventToCommand.AlwaysInvokeCommandProperty);
    set => this.SetValue(EventToCommand.AlwaysInvokeCommandProperty, ValueBoxes.BooleanBox(value));
  }

  protected override void OnAttached()
  {
    base.OnAttached();
    this.EnableDisableElement();
  }

  private FrameworkElement GetAssociatedObject() => this.AssociatedObject as FrameworkElement;

  private ICommand GetCommand() => this.Command;

  public void Invoke() => this.Invoke((object) null);

  protected override void Invoke(object parameter)
  {
    if (this.AssociatedElementIsDisabled() && !this.AlwaysInvokeCommand)
      return;
    ICommand command = this.GetCommand();
    object parameter1 = this.CommandParameterValue;
    if (parameter1 == null && this.PassEventArgsToCommand)
      parameter1 = this.EventArgsConverter == null ? parameter : this.EventArgsConverter.Convert(parameter, this.EventArgsConverterParameter);
    if (command == null || !command.CanExecute(parameter1))
      return;
    command.Execute(parameter1);
  }

  private static void OnCommandChanged(EventToCommand element, DependencyPropertyChangedEventArgs e)
  {
    if (element == null)
      return;
    if (e.OldValue != null)
      ((ICommand) e.OldValue).CanExecuteChanged -= new EventHandler(element.OnCommandCanExecuteChanged);
    ICommand newValue = (ICommand) e.NewValue;
    if (newValue != null)
      newValue.CanExecuteChanged += new EventHandler(element.OnCommandCanExecuteChanged);
    element.EnableDisableElement();
  }

  private bool AssociatedElementIsDisabled()
  {
    FrameworkElement associatedObject = this.GetAssociatedObject();
    if (this.AssociatedObject == null)
      return true;
    return associatedObject != null && !associatedObject.IsEnabled;
  }

  private void EnableDisableElement()
  {
    FrameworkElement associatedObject = this.GetAssociatedObject();
    if (associatedObject == null)
      return;
    ICommand command = this.GetCommand();
    if (!this.MustToggleIsEnabledValue || command == null)
      return;
    associatedObject.IsEnabled = command.CanExecute(this.CommandParameterValue);
  }

  private void OnCommandCanExecuteChanged(object sender, EventArgs e)
  {
    this.EnableDisableElement();
  }
}
