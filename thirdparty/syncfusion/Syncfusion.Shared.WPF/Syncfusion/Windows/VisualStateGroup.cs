// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.VisualStateGroup
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.Windows;

[ContentProperty("States")]
[RuntimeNameProperty("Name")]
public class VisualStateGroup : DependencyObject
{
  private Collection<Storyboard> _currentStoryboards;
  private FreezableCollection<VisualState> _states;
  private FreezableCollection<VisualTransition> _transitions;

  public string Name { get; set; }

  public IList States
  {
    get
    {
      if (this._states == null)
        this._states = new FreezableCollection<VisualState>();
      return (IList) this._states;
    }
  }

  public IList Transitions
  {
    get
    {
      if (this._transitions == null)
        this._transitions = new FreezableCollection<VisualTransition>();
      return (IList) this._transitions;
    }
  }

  internal VisualState CurrentState { get; set; }

  internal VisualState GetState(string stateName)
  {
    for (int index = 0; index < this.States.Count; ++index)
    {
      VisualState state = (VisualState) this.States[index];
      if (state.Name == stateName)
        return state;
    }
    return (VisualState) null;
  }

  internal Collection<Storyboard> CurrentStoryboards
  {
    get
    {
      if (this._currentStoryboards == null)
        this._currentStoryboards = new Collection<Storyboard>();
      return this._currentStoryboards;
    }
  }

  internal void StartNewThenStopOld(FrameworkElement element, params Storyboard[] newStoryboards)
  {
    for (int index = 0; index < newStoryboards.Length; ++index)
    {
      if (newStoryboards[index] != null)
        newStoryboards[index].Begin(element, HandoffBehavior.SnapshotAndReplace, true);
    }
    for (int index = 0; index < this.CurrentStoryboards.Count; ++index)
    {
      if (this.CurrentStoryboards[index] != null)
        this.CurrentStoryboards[index].Stop(element);
    }
    this.CurrentStoryboards.Clear();
    for (int index = 0; index < newStoryboards.Length; ++index)
      this.CurrentStoryboards.Add(newStoryboards[index]);
  }

  internal void RaiseCurrentStateChanging(
    FrameworkElement element,
    VisualState oldState,
    VisualState newState,
    Control control)
  {
    if (this.CurrentStateChanging == null)
      return;
    this.CurrentStateChanging((object) element, new VisualStateChangedEventArgs(oldState, newState, control));
  }

  internal void RaiseCurrentStateChanged(
    FrameworkElement element,
    VisualState oldState,
    VisualState newState,
    Control control)
  {
    if (this.CurrentStateChanged == null)
      return;
    this.CurrentStateChanged((object) element, new VisualStateChangedEventArgs(oldState, newState, control));
  }

  public event EventHandler<VisualStateChangedEventArgs> CurrentStateChanged;

  public event EventHandler<VisualStateChangedEventArgs> CurrentStateChanging;
}
