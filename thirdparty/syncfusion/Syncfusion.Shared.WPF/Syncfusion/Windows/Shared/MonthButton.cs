// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.MonthButton
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Controls.Tools.AutomationPeer;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class MonthButton : ContentControl
{
  static MonthButton()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (MonthButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (MonthButton)));
  }

  protected internal void Initialize(
    VisibleDate data,
    System.Globalization.Calendar calendar,
    CultureInfo culture,
    bool isAbbreviated,
    CalendarVisualMode mode)
  {
    int visibleMonth = data.VisibleMonth;
    int visibleYear = data.VisibleYear;
    DateTimeFormatInfo dateTimeFormat = culture.DateTimeFormat;
    Date date1 = new Date(calendar.MinSupportedDateTime, calendar);
    Date date2 = new Date(calendar.MaxSupportedDateTime, calendar);
    if (mode == CalendarVisualMode.WeekNumbers)
      this.Content = (object) (SharedLocalizationResourceAccessor.Instance.GetString("WeekNumbers") + visibleYear.ToString());
    if (mode == CalendarVisualMode.Days || mode == CalendarVisualMode.All)
    {
      if (isAbbreviated)
      {
        if (culture.Name == "ja-JP" || culture.Name == "zh-CN")
        {
          char[] charArray = new DateTime(visibleYear, visibleMonth, 1).ToString(dateTimeFormat.YearMonthPattern, (IFormatProvider) culture).ToCharArray();
          this.Content = (object) $"{visibleYear.ToString()}{(object) charArray[4]} {dateTimeFormat.MonthNames[visibleMonth - 1]}";
        }
        else
          this.Content = (object) $"{dateTimeFormat.AbbreviatedMonthNames[visibleMonth - 1]} {visibleYear.ToString()}";
      }
      else if (culture.Name == "ja-JP" || culture.Name == "zh-CN")
        this.Content = (object) new DateTime(visibleYear, visibleMonth, 1).ToString(dateTimeFormat.YearMonthPattern, (IFormatProvider) culture);
      else
        this.Content = (object) $"{dateTimeFormat.MonthNames[visibleMonth - 1]} {visibleYear.ToString()}";
    }
    if (mode == CalendarVisualMode.Months)
      this.Content = (object) visibleYear.ToString();
    if (mode == CalendarVisualMode.Years)
    {
      int num1 = data.VisibleYear;
      while (num1 % 10 != 0)
        --num1;
      int num2 = num1 + 9;
      if (num1 < date1.Year)
        num1 = date1.Year;
      if (num2 > date2.Year)
        num2 = date2.Year;
      this.Content = (object) $"{(object) num1}-{(object) num2}";
    }
    if (mode != CalendarVisualMode.YearsRange)
      return;
    int num3 = data.VisibleYear;
    while (num3 % 10 != 0)
      --num3;
    while (num3 % 100 != 0)
      num3 -= 10;
    int num4 = num3 + 99;
    if (num3 < date1.Year)
      num3 = date1.Year;
    if (num4 > date2.Year)
      num4 = date2.Year;
    this.Content = (object) $"{(object) num3}-{(object) num4}";
  }

  protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
  {
    return (System.Windows.Automation.Peers.AutomationPeer) new CalendarEditHeaderAutomationPeer(this);
  }

  internal void Dispose()
  {
    this.Content = (object) null;
    this.ContentTemplate = (DataTemplate) null;
    this.ContentTemplateSelector = (DataTemplateSelector) null;
    this.Style = (Style) null;
    this.Template = (ControlTemplate) null;
  }
}
