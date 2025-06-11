// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.RibbonGroup
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

public class RibbonGroup : HeaderedItemsControl
{
  public static readonly DependencyProperty ShowLauncherButtonProperty = DependencyProperty.Register(nameof (ShowLauncherButton), typeof (bool), typeof (RibbonGroup), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty ShowSplitterProperty = DependencyProperty.Register(nameof (ShowSplitter), typeof (bool), typeof (RibbonGroup), new PropertyMetadata(ValueBoxes.TrueBox));
  public static readonly DependencyProperty LauncherPoptipProperty = DependencyProperty.Register(nameof (LauncherPoptip), typeof (Poptip), typeof (RibbonGroup), new PropertyMetadata((object) null));
  public static readonly RoutedEvent LauncherClickEvent = EventManager.RegisterRoutedEvent("LauncherClick", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (RibbonGroup));

  public RibbonGroup()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.More, new ExecutedRoutedEventHandler(this.LauncherButton_OnClick)));
  }

  private void LauncherButton_OnClick(object sender, ExecutedRoutedEventArgs e)
  {
    this.OnLauncherClick(new RoutedEventArgs(RibbonGroup.LauncherClickEvent, (object) this));
  }

  public bool ShowLauncherButton
  {
    get => (bool) this.GetValue(RibbonGroup.ShowLauncherButtonProperty);
    set => this.SetValue(RibbonGroup.ShowLauncherButtonProperty, (object) value);
  }

  public bool ShowSplitter
  {
    get => (bool) this.GetValue(RibbonGroup.ShowSplitterProperty);
    set => this.SetValue(RibbonGroup.ShowSplitterProperty, (object) value);
  }

  public Poptip LauncherPoptip
  {
    get => (Poptip) this.GetValue(RibbonGroup.LauncherPoptipProperty);
    set => this.SetValue(RibbonGroup.LauncherPoptipProperty, (object) value);
  }

  public event RoutedEventHandler LauncherClick
  {
    add => this.AddHandler(RibbonGroup.LauncherClickEvent, (Delegate) value);
    remove => this.RemoveHandler(RibbonGroup.LauncherClickEvent, (Delegate) value);
  }

  protected virtual void OnLauncherClick(RoutedEventArgs e) => this.RaiseEvent(e);
}
