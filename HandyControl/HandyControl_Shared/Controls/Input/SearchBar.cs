// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.SearchBar
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Controls;

public class SearchBar : TextBox, ICommandSource
{
  public static readonly RoutedEvent SearchStartedEvent = EventManager.RegisterRoutedEvent("SearchStarted", RoutingStrategy.Bubble, typeof (EventHandler<FunctionEventArgs<string>>), typeof (SearchBar));
  public static readonly DependencyProperty IsRealTimeProperty = DependencyProperty.Register(nameof (IsRealTime), typeof (bool), typeof (SearchBar), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), typeof (ICommand), typeof (SearchBar), new PropertyMetadata((object) null, new PropertyChangedCallback(SearchBar.OnCommandChanged)));
  public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof (CommandParameter), typeof (object), typeof (SearchBar), new PropertyMetadata((object) null));
  public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(nameof (CommandTarget), typeof (IInputElement), typeof (SearchBar), new PropertyMetadata((object) null));

  public SearchBar()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Search, (ExecutedRoutedEventHandler) ((s, e) => this.OnSearchStarted())));
  }

  public event EventHandler<FunctionEventArgs<string>> SearchStarted
  {
    add => this.AddHandler(SearchBar.SearchStartedEvent, (Delegate) value);
    remove => this.RemoveHandler(SearchBar.SearchStartedEvent, (Delegate) value);
  }

  protected override void OnKeyDown(KeyEventArgs e)
  {
    base.OnKeyDown(e);
    if (e.Key != Key.Return)
      return;
    this.OnSearchStarted();
  }

  protected override void OnTextChanged(TextChangedEventArgs e)
  {
    base.OnTextChanged(e);
    if (!this.IsRealTime)
      return;
    this.OnSearchStarted();
  }

  private void OnSearchStarted()
  {
    this.RaiseEvent((RoutedEventArgs) new FunctionEventArgs<string>(SearchBar.SearchStartedEvent, (object) this)
    {
      Info = this.Text
    });
    ICommand command = this.Command;
    if (command == null)
      return;
    if (command is RoutedCommand routedCommand)
      routedCommand.Execute(this.CommandParameter, this.CommandTarget);
    else
      this.Command.Execute(this.CommandParameter);
  }

  public bool IsRealTime
  {
    get => (bool) this.GetValue(SearchBar.IsRealTimeProperty);
    set => this.SetValue(SearchBar.IsRealTimeProperty, ValueBoxes.BooleanBox(value));
  }

  private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    SearchBar searchBar = (SearchBar) d;
    if (e.OldValue is ICommand oldValue)
      oldValue.CanExecuteChanged -= new EventHandler(searchBar.CanExecuteChanged);
    if (!(e.NewValue is ICommand newValue))
      return;
    newValue.CanExecuteChanged += new EventHandler(searchBar.CanExecuteChanged);
  }

  public ICommand Command
  {
    get => (ICommand) this.GetValue(SearchBar.CommandProperty);
    set => this.SetValue(SearchBar.CommandProperty, (object) value);
  }

  public object CommandParameter
  {
    get => this.GetValue(SearchBar.CommandParameterProperty);
    set => this.SetValue(SearchBar.CommandParameterProperty, value);
  }

  public IInputElement CommandTarget
  {
    get => (IInputElement) this.GetValue(SearchBar.CommandTargetProperty);
    set => this.SetValue(SearchBar.CommandTargetProperty, (object) value);
  }

  private void CanExecuteChanged(object sender, EventArgs e)
  {
    if (this.Command == null)
      return;
    this.IsEnabled = this.Command is RoutedCommand command ? command.CanExecute(this.CommandParameter, this.CommandTarget) : this.Command.CanExecute(this.CommandParameter);
  }
}
