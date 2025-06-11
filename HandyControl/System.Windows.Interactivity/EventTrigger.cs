// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.EventTrigger
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;

#nullable disable
namespace HandyControl.Interactivity;

public class EventTrigger : EventTriggerBase<object>
{
  public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register(nameof (EventName), typeof (string), typeof (EventTrigger), (PropertyMetadata) new FrameworkPropertyMetadata((object) "Loaded", new PropertyChangedCallback(EventTrigger.OnEventNameChanged)));

  public EventTrigger()
  {
  }

  public EventTrigger(string eventName) => this.EventName = eventName;

  protected override string GetEventName() => this.EventName;

  private static void OnEventNameChanged(object sender, DependencyPropertyChangedEventArgs args)
  {
    ((EventTriggerBase) sender).OnEventNameChanged((string) args.OldValue, (string) args.NewValue);
  }

  public string EventName
  {
    get => (string) this.GetValue(EventTrigger.EventNameProperty);
    set => this.SetValue(EventTrigger.EventNameProperty, (object) value);
  }
}
