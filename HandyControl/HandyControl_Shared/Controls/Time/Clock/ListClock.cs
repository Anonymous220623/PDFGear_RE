// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ListClock
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_HourList", Type = typeof (ListBox))]
[TemplatePart(Name = "PART_MinuteList", Type = typeof (ListBox))]
[TemplatePart(Name = "PART_SecondList", Type = typeof (ListBox))]
public class ListClock : ClockBase
{
  private const string ElementHourList = "PART_HourList";
  private const string ElementMinuteList = "PART_MinuteList";
  private const string ElementSecondList = "PART_SecondList";
  private ListBox _hourList;
  private ListBox _minuteList;
  private ListBox _secondList;

  public override void OnClockOpened() => this.ScrollIntoView();

  public override void OnApplyTemplate()
  {
    this.AppliedTemplate = false;
    if (this.ButtonConfirm != null)
      this.ButtonConfirm.Click -= new RoutedEventHandler(((ClockBase) this).ButtonConfirm_OnClick);
    if (this._hourList != null)
      this._hourList.SelectionChanged -= new SelectionChangedEventHandler(this.HourList_SelectionChanged);
    if (this._minuteList != null)
      this._minuteList.SelectionChanged -= new SelectionChangedEventHandler(this.MinuteList_SelectionChanged);
    if (this._secondList != null)
      this._secondList.SelectionChanged -= new SelectionChangedEventHandler(this.SecondList_SelectionChanged);
    base.OnApplyTemplate();
    this._hourList = this.GetTemplateChild("PART_HourList") as ListBox;
    if (this._hourList != null)
    {
      this.CreateItemsSource((ItemsControl) this._hourList, 24);
      this._hourList.SelectionChanged += new SelectionChangedEventHandler(this.HourList_SelectionChanged);
    }
    this._minuteList = this.GetTemplateChild("PART_MinuteList") as ListBox;
    if (this._minuteList != null)
    {
      this.CreateItemsSource((ItemsControl) this._minuteList, 60);
      this._minuteList.SelectionChanged += new SelectionChangedEventHandler(this.MinuteList_SelectionChanged);
    }
    this._secondList = this.GetTemplateChild("PART_SecondList") as ListBox;
    if (this._secondList != null)
    {
      this.CreateItemsSource((ItemsControl) this._secondList, 60);
      this._secondList.SelectionChanged += new SelectionChangedEventHandler(this.SecondList_SelectionChanged);
    }
    this.ButtonConfirm = this.GetTemplateChild("PART_ButtonConfirm") as Button;
    if (this.ButtonConfirm != null)
      this.ButtonConfirm.Click += new RoutedEventHandler(((ClockBase) this).ButtonConfirm_OnClick);
    this.AppliedTemplate = true;
    DateTime? selectedTime = this.SelectedTime;
    if (selectedTime.HasValue)
    {
      selectedTime = this.SelectedTime;
      this.Update(selectedTime.Value);
    }
    else
    {
      this.DisplayTime = DateTime.Now;
      this.Update(this.DisplayTime);
    }
  }

  internal override void Update(DateTime time)
  {
    if (!this.AppliedTemplate)
      return;
    int hour = time.Hour;
    int minute = time.Minute;
    int second = time.Second;
    this._hourList.SelectedIndex = hour;
    this._minuteList.SelectedIndex = minute;
    this._secondList.SelectedIndex = second;
    this.ScrollIntoView();
    this.DisplayTime = time;
  }

  private void HourList_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    this.Update();
  }

  private void MinuteList_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    this.Update();
  }

  private void SecondList_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    this.Update();
  }

  private void CreateItemsSource(ItemsControl selector, int count)
  {
    List<string> stringList = new List<string>();
    for (int index = 0; index < count; ++index)
      stringList.Add(index.ToString("#00"));
    selector.ItemsSource = (IEnumerable) stringList;
  }

  private void Update()
  {
    if (this._hourList.SelectedIndex < 0 || this._hourList.SelectedIndex >= 24 || this._minuteList.SelectedIndex < 0 || this._minuteList.SelectedIndex >= 60 || this._secondList.SelectedIndex < 0 || this._secondList.SelectedIndex >= 60)
      return;
    DateTime now = DateTime.Now;
    this.DisplayTime = new DateTime(now.Year, now.Month, now.Day, this._hourList.SelectedIndex, this._minuteList.SelectedIndex, this._secondList.SelectedIndex);
  }

  private void ScrollIntoView()
  {
    this._hourList.ScrollIntoView(this._hourList.SelectedItem);
    this._minuteList.ScrollIntoView(this._minuteList.SelectedItem);
    this._secondList.ScrollIntoView(this._secondList.SelectedItem);
  }
}
