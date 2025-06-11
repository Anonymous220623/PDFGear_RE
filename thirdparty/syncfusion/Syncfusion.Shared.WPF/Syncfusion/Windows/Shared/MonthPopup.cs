// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.MonthPopup
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class MonthPopup
{
  private Popup m_popup;
  private ListBox m_selector;
  private bool m_canSelect;
  private Hashtable m_popupDates;
  private DispatcherTimer m_timer;
  private Date m_currentDate;
  private DateTimeFormatInfo m_format;
  private Date m_minDate;
  private Date m_maxDate;

  public MonthPopup(
    Popup popup,
    Date date,
    DateTimeFormatInfo format,
    Date minDate,
    Date maxDate)
  {
    if (popup == null || format == null || !(popup.Child is ListBox))
      throw new ArgumentNullException("argument can not be null");
    this.m_popup = popup;
    this.m_selector = this.m_popup.Child as ListBox;
    this.m_minDate = minDate;
    this.m_maxDate = maxDate;
    this.m_currentDate = date;
    this.m_canSelect = false;
    this.m_format = format;
    this.m_popupDates = new Hashtable();
    this.m_selector.KeyDown += new KeyEventHandler(this.Selector_KeyDown);
    this.m_selector.MouseLeftButtonUp += new MouseButtonEventHandler(this.Selector_MouseLeftButtonUp);
    this.m_selector.MouseMove += new MouseEventHandler(this.Selector_MouseMove);
    this.m_selector.SelectionChanged += new SelectionChangedEventHandler(this.Selector_SelectionChanged);
    this.m_timer = new DispatcherTimer(DispatcherPriority.Input);
    this.m_timer.Interval = TimeSpan.FromMilliseconds(300.0);
    this.m_timer.Tick += new EventHandler(this.Timer_Tick);
  }

  public void Show()
  {
    this.FillMonthSelector();
    this.m_popup.IsOpen = true;
    this.m_selector.CaptureMouse();
  }

  public void RefreshContent() => this.FillMonthSelector();

  internal void Dispose()
  {
    this.m_format = (DateTimeFormatInfo) null;
    if (this.m_popup != null)
      this.m_popup = (Popup) null;
    if (this.m_timer != null)
    {
      this.m_timer.Stop();
      this.m_timer.Tick -= new EventHandler(this.Timer_Tick);
      this.m_timer = (DispatcherTimer) null;
    }
    if (this.m_popupDates != null)
    {
      this.m_popupDates.Clear();
      this.m_popupDates = (Hashtable) null;
    }
    if (this.m_selector == null)
      return;
    if (this.m_selector.ItemsSource != null)
      this.m_selector.ItemsSource = (IEnumerable) null;
    else if (this.m_selector.Items.Count > 0)
      this.m_selector.Items.Clear();
    this.m_selector.KeyDown -= new KeyEventHandler(this.Selector_KeyDown);
    this.m_selector.MouseLeftButtonUp -= new MouseButtonEventHandler(this.Selector_MouseLeftButtonUp);
    this.m_selector.MouseMove -= new MouseEventHandler(this.Selector_MouseMove);
    this.m_selector.SelectionChanged -= new SelectionChangedEventHandler(this.Selector_SelectionChanged);
    this.m_selector = (ListBox) null;
  }

  private void RaiseHidePopupEvent(HidePopupEventArgs e)
  {
    if (this.HidePopup == null)
      return;
    this.HidePopup((object) this, e);
  }

  private void FillMonthSelector()
  {
    Date date1 = this.m_currentDate.AddMonthToDate(-3);
    Date date2 = this.m_currentDate.AddMonthToDate(4);
    if (date1 < this.m_minDate)
    {
      date1 = new Date(this.m_minDate.Year, this.m_minDate.Month, 1);
      date2 = date1.AddMonthToDate(7);
    }
    if (date2 > this.m_maxDate)
    {
      date2 = new Date(this.m_maxDate.Year, this.m_maxDate.Month, 1).AddMonthToDate(1);
      date1 = date2.AddMonthToDate(-7);
    }
    this.m_selector.Items.Clear();
    this.m_popupDates.Clear();
    for (; date1 != date2; date1 = date1.AddMonthToDate(1))
    {
      this.m_selector.Items.Add((object) $"{this.m_format.MonthNames[date1.Month - 1]} {(object) date1.Year}");
      this.m_popupDates.Add((object) (this.m_selector.Items.Count - 1), (object) date1);
    }
  }

  private void Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (!this.m_canSelect)
      this.m_selector.UnselectAll();
    this.m_canSelect = false;
  }

  private void Selector_MouseMove(object sender, MouseEventArgs e)
  {
    this.m_selector.UnselectAll();
    Point position = e.GetPosition((IInputElement) this.m_selector) with
    {
      X = this.m_selector.ActualWidth / 2.0
    };
    HitTestResult hitTestResult = VisualTreeHelper.HitTest((Visual) this.m_selector, position);
    if (hitTestResult != null)
    {
      if (!(hitTestResult.VisualHit is FrameworkElement visualHit) || !(VisualUtils.FindAncestor((Visual) visualHit, typeof (ListBoxItem)) is ListBoxItem ancestor) || position.Y <= 0.0 || position.Y >= this.m_selector.ActualHeight)
        return;
      this.m_timer.Stop();
      this.m_canSelect = true;
      this.m_selector.SelectedItem = ancestor.Content;
    }
    else
      this.m_timer.Start();
  }

  private void Timer_Tick(object sender, EventArgs e)
  {
    double num1 = 0.0;
    Point position = Mouse.GetPosition((IInputElement) this.m_selector);
    int num2 = this.m_selector.Items.Count - 1;
    if (position.Y > this.m_selector.ActualHeight)
    {
      num1 = position.Y - this.m_selector.ActualHeight;
      Date date = ((Date) this.m_popupDates[(object) num2]).AddMonthToDate(1);
      if (date <= this.m_maxDate)
      {
        string monthName = this.m_format.MonthNames[date.Month - 1];
        this.m_selector.Items.RemoveAt(0);
        this.m_selector.Items.Add((object) $"{monthName} {(object) date.Year}");
        this.m_popupDates.Remove((object) 0);
        Hashtable hashtable = (Hashtable) this.m_popupDates.Clone();
        this.m_popupDates.Clear();
        foreach (int key in (IEnumerable) hashtable.Keys)
          this.m_popupDates.Add((object) (key - 1), hashtable[(object) key]);
        num2 = this.m_selector.Items.Count - 1;
        this.m_popupDates.Add((object) num2, (object) date);
      }
    }
    if (position.Y < 0.0)
    {
      num1 = Math.Abs(position.Y);
      Date date = ((Date) this.m_popupDates[(object) 0]).AddMonthToDate(-1);
      if (date >= this.m_minDate)
      {
        string monthName = this.m_format.MonthNames[date.Month - 1];
        this.m_selector.Items.RemoveAt(num2);
        this.m_selector.Items.Insert(0, (object) $"{monthName} {(object) date.Year}");
        this.m_popupDates.Remove((object) num2);
        Hashtable hashtable = (Hashtable) this.m_popupDates.Clone();
        this.m_popupDates.Clear();
        foreach (int key in (IEnumerable) hashtable.Keys)
          this.m_popupDates.Add((object) (key + 1), hashtable[(object) key]);
        int num3 = this.m_selector.Items.Count - 1;
        this.m_popupDates.Add((object) 0, (object) date);
      }
    }
    if (num1 <= 10.0)
      this.m_timer.Interval = TimeSpan.FromMilliseconds(350.0);
    if (num1 > 10.0 && num1 <= 30.0)
      this.m_timer.Interval = TimeSpan.FromMilliseconds(300.0);
    if (num1 > 30.0 && num1 <= 50.0)
      this.m_timer.Interval = TimeSpan.FromMilliseconds(200.0);
    if (num1 > 50.0 && num1 <= 70.0)
      this.m_timer.Interval = TimeSpan.FromMilliseconds(150.0);
    if (num1 > 70.0 && num1 <= 100.0)
      this.m_timer.Interval = TimeSpan.FromMilliseconds(100.0);
    if (num1 <= 100.0)
      return;
    this.m_timer.Interval = TimeSpan.FromMilliseconds(30.0);
  }

  private void Selector_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    this.m_timer.Stop();
    this.m_popup.IsOpen = false;
    if (this.m_selector.SelectedItem == null)
      return;
    this.RaiseHidePopupEvent(new HidePopupEventArgs((Date) this.m_popupDates[(object) (sender as ListBox).SelectedIndex]));
  }

  private void Selector_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key != Key.Escape)
      return;
    this.m_popup.IsOpen = false;
  }

  public DateTimeFormatInfo Format
  {
    get => this.m_format;
    set => this.m_format = value;
  }

  public Date CurrentDate
  {
    get => this.m_currentDate;
    set => this.m_currentDate = value;
  }

  public Date MinDate
  {
    get => this.m_minDate;
    set => this.m_minDate = value;
  }

  public Date MaxDate
  {
    get => this.m_maxDate;
    set => this.m_maxDate = value;
  }

  public event EventHandler<HidePopupEventArgs> HidePopup;
}
