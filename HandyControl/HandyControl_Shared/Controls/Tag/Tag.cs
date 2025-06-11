// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Tag
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Controls;

public class Tag : ContentControl
{
  public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register(nameof (ShowCloseButton), typeof (bool), typeof (HandyControl.Controls.Tag), new PropertyMetadata(ValueBoxes.TrueBox));
  public static readonly DependencyProperty SelectableProperty = DependencyProperty.Register(nameof (Selectable), typeof (bool), typeof (HandyControl.Controls.Tag), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof (IsSelected), typeof (bool), typeof (HandyControl.Controls.Tag), new PropertyMetadata(ValueBoxes.FalseBox, (PropertyChangedCallback) ((o, args) =>
  {
    HandyControl.Controls.Tag source = (HandyControl.Controls.Tag) o;
    source.RaiseEvent(new RoutedEventArgs(HandyControl.Controls.Tag.SelectedEvent, (object) source));
  })));
  public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (HandyControl.Controls.Tag), new PropertyMetadata((object) null, new PropertyChangedCallback(HandyControl.Controls.Tag.OnHeaderChanged)));
  internal static readonly DependencyPropertyKey HasHeaderPropertyKey = DependencyProperty.RegisterReadOnly(nameof (HasHeader), typeof (bool), typeof (HandyControl.Controls.Tag), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty HasHeaderProperty = HandyControl.Controls.Tag.HasHeaderPropertyKey.DependencyProperty;
  public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register(nameof (HeaderStringFormat), typeof (string), typeof (HandyControl.Controls.Tag), new PropertyMetadata((object) null));
  public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register(nameof (HeaderTemplateSelector), typeof (DataTemplateSelector), typeof (HandyControl.Controls.Tag), new PropertyMetadata((object) null));
  public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (HandyControl.Controls.Tag), new PropertyMetadata((object) null));
  public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof (EventHandler), typeof (HandyControl.Controls.Tag));
  public static readonly RoutedEvent ClosingEvent = EventManager.RegisterRoutedEvent("Closing", RoutingStrategy.Bubble, typeof (EventHandler), typeof (HandyControl.Controls.Tag));
  public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof (EventHandler), typeof (HandyControl.Controls.Tag));

  public Tag()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Close, (ExecutedRoutedEventHandler) ((s, e) =>
    {
      CancelRoutedEventArgs e1 = new CancelRoutedEventArgs(HandyControl.Controls.Tag.ClosingEvent, (object) this);
      this.RaiseEvent((RoutedEventArgs) e1);
      if (e1.Cancel)
        return;
      this.RaiseEvent(new RoutedEventArgs(HandyControl.Controls.Tag.ClosedEvent, (object) this));
    })));
  }

  public bool ShowCloseButton
  {
    get => (bool) this.GetValue(HandyControl.Controls.Tag.ShowCloseButtonProperty);
    set => this.SetValue(HandyControl.Controls.Tag.ShowCloseButtonProperty, ValueBoxes.BooleanBox(value));
  }

  public bool Selectable
  {
    get => (bool) this.GetValue(HandyControl.Controls.Tag.SelectableProperty);
    set => this.SetValue(HandyControl.Controls.Tag.SelectableProperty, ValueBoxes.BooleanBox(value));
  }

  public bool IsSelected
  {
    get => (bool) this.GetValue(HandyControl.Controls.Tag.IsSelectedProperty);
    set => this.SetValue(HandyControl.Controls.Tag.IsSelectedProperty, ValueBoxes.BooleanBox(value));
  }

  private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    HandyControl.Controls.Tag tag = (HandyControl.Controls.Tag) d;
    tag.SetValue(HandyControl.Controls.Tag.HasHeaderPropertyKey, e.NewValue != null ? ValueBoxes.TrueBox : ValueBoxes.FalseBox);
    tag.OnHeaderChanged(e.OldValue, e.NewValue);
  }

  protected virtual void OnHeaderChanged(object oldHeader, object newHeader)
  {
    this.RemoveLogicalChild(oldHeader);
    this.AddLogicalChild(newHeader);
  }

  public object Header
  {
    get => this.GetValue(HandyControl.Controls.Tag.HeaderProperty);
    set => this.SetValue(HandyControl.Controls.Tag.HeaderProperty, value);
  }

  [Bindable(false)]
  [Browsable(false)]
  public bool HasHeader => (bool) this.GetValue(HandyControl.Controls.Tag.HasHeaderProperty);

  public string HeaderStringFormat
  {
    get => (string) this.GetValue(HandyControl.Controls.Tag.HeaderStringFormatProperty);
    set => this.SetValue(HandyControl.Controls.Tag.HeaderStringFormatProperty, (object) value);
  }

  public DataTemplateSelector HeaderTemplateSelector
  {
    get => (DataTemplateSelector) this.GetValue(HandyControl.Controls.Tag.HeaderTemplateSelectorProperty);
    set => this.SetValue(HandyControl.Controls.Tag.HeaderTemplateSelectorProperty, (object) value);
  }

  public DataTemplate HeaderTemplate
  {
    get => (DataTemplate) this.GetValue(HandyControl.Controls.Tag.HeaderTemplateProperty);
    set => this.SetValue(HandyControl.Controls.Tag.HeaderTemplateProperty, (object) value);
  }

  public event EventHandler Selected
  {
    add => this.AddHandler(HandyControl.Controls.Tag.SelectedEvent, (Delegate) value);
    remove => this.RemoveHandler(HandyControl.Controls.Tag.SelectedEvent, (Delegate) value);
  }

  public event EventHandler Closing
  {
    add => this.AddHandler(HandyControl.Controls.Tag.ClosingEvent, (Delegate) value);
    remove => this.RemoveHandler(HandyControl.Controls.Tag.ClosingEvent, (Delegate) value);
  }

  public event EventHandler Closed
  {
    add => this.AddHandler(HandyControl.Controls.Tag.ClosedEvent, (Delegate) value);
    remove => this.RemoveHandler(HandyControl.Controls.Tag.ClosedEvent, (Delegate) value);
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    base.OnMouseLeftButtonDown(e);
    if (!this.Selectable)
      return;
    this.IsSelected = true;
  }
}
