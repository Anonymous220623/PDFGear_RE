// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DateTimeHandler
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal class DateTimeHandler
{
  internal static char[] allStandardFormats = new char[19]
  {
    'd',
    'D',
    'f',
    'F',
    'g',
    'G',
    'm',
    'M',
    'o',
    'O',
    'r',
    'R',
    's',
    't',
    'T',
    'u',
    'U',
    'y',
    'Y'
  };
  internal static DateTimeHandler m_dateTimeHandler = new DateTimeHandler();
  public static bool selectedflag = false;

  public static DateTimeHandler dateTimeHandler
  {
    get
    {
      if (DateTimeHandler.m_dateTimeHandler == null)
        DateTimeHandler.m_dateTimeHandler = new DateTimeHandler();
      return DateTimeHandler.m_dateTimeHandler;
    }
    set => DateTimeHandler.m_dateTimeHandler = value;
  }

  public ObservableCollection<DateTimeProperties> CreateDateTimePatteren(
    DateTimeEdit MaskedTextBoxAdv)
  {
    ObservableCollection<DateTimeProperties> dateTimePatteren = new ObservableCollection<DateTimeProperties>();
    DateTimeFormatInfo dateTimeFormatInfo = MaskedTextBoxAdv.GetCulture().DateTimeFormat.Clone() as DateTimeFormatInfo;
    int num1;
    for (string mask = DateTimeHandler.GetSpecificFormat(MaskedTextBoxAdv.GetStringPattern(dateTimeFormatInfo, MaskedTextBoxAdv.Pattern, MaskedTextBoxAdv.CustomPattern), dateTimeFormatInfo); mask.Length > 0; mask = mask.Substring(num1))
    {
      num1 = DateTimeHandler.GetGroupLengthByMask(mask);
      switch (mask[0])
      {
        case '"':
        case '\'':
          int num2 = mask.IndexOf(mask[0], 1);
          dateTimePatteren.Add(new DateTimeProperties()
          {
            IsReadOnly = new bool?(true),
            Type = DateTimeType.others,
            Lenghth = 1,
            Content = mask.Substring(1, Math.Max(1, num2 - 1)).ToString()
          });
          num1 = Math.Max(1, num2 + 1);
          break;
        case 'D':
        case 'd':
          string str1 = mask.Substring(0, num1);
          if (num1 == 1)
            str1 = "%" + str1;
          if (num1 > 2)
          {
            dateTimePatteren.Add(new DateTimeProperties()
            {
              IsReadOnly = new bool?(true),
              Type = DateTimeType.Dayname,
              Pattern = str1
            });
            break;
          }
          dateTimePatteren.Add(new DateTimeProperties()
          {
            IsReadOnly = new bool?(false),
            Type = DateTimeType.Day,
            Pattern = str1
          });
          break;
        case 'F':
        case 'f':
          string str2 = mask.Substring(0, num1);
          if (num1 == 1)
            str2 = "%" + str2;
          dateTimePatteren.Add(new DateTimeProperties()
          {
            IsReadOnly = new bool?(false),
            Type = DateTimeType.Fraction,
            Pattern = str2
          });
          break;
        case 'H':
          string str3 = mask.Substring(0, num1);
          if (num1 == 1)
            str3 = "%" + str3;
          dateTimePatteren.Add(new DateTimeProperties()
          {
            IsReadOnly = new bool?(false),
            Type = DateTimeType.Hour24,
            Pattern = str3
          });
          break;
        case 'M':
          string str4 = mask.Substring(0, num1);
          if (num1 == 1)
            str4 = "%" + str4;
          if (num1 >= 3)
          {
            dateTimePatteren.Add(new DateTimeProperties()
            {
              IsReadOnly = new bool?(false),
              Type = DateTimeType.monthname,
              Pattern = str4
            });
            break;
          }
          dateTimePatteren.Add(new DateTimeProperties()
          {
            IsReadOnly = new bool?(false),
            Type = DateTimeType.Month,
            Pattern = str4
          });
          break;
        case 'S':
        case 's':
          string str5 = mask.Substring(0, num1);
          if (num1 == 1)
            str5 = "%" + str5;
          dateTimePatteren.Add(new DateTimeProperties()
          {
            IsReadOnly = new bool?(false),
            Type = DateTimeType.Second,
            Pattern = str5
          });
          break;
        case 'T':
        case 't':
          string str6 = mask.Substring(0, num1);
          if (num1 == 1)
            str6 = "%" + str6;
          dateTimePatteren.Add(new DateTimeProperties()
          {
            IsReadOnly = new bool?(false),
            Type = DateTimeType.designator,
            Pattern = str6
          });
          break;
        case 'Y':
        case 'y':
          string str7 = mask.Substring(0, num1);
          if (num1 == 1)
            str7 = "%" + str7;
          dateTimePatteren.Add(new DateTimeProperties()
          {
            IsReadOnly = new bool?(false),
            Type = DateTimeType.year,
            Pattern = str7
          });
          break;
        case '\\':
          if (mask.Length >= 2)
          {
            dateTimePatteren.Add(new DateTimeProperties()
            {
              IsReadOnly = new bool?(true),
              Content = mask.Substring(1, 1),
              Lenghth = 1,
              Type = DateTimeType.others
            });
            num1 = 2;
            break;
          }
          break;
        case 'g':
          string str8 = mask.Substring(0, num1);
          if (num1 == 1)
          {
            string str9 = "%" + str8;
          }
          dateTimePatteren.Add(new DateTimeProperties()
          {
            IsReadOnly = new bool?(true),
            Type = DateTimeType.period,
            Pattern = mask.Substring(0, num1)
          });
          break;
        case 'h':
          string str10 = mask.Substring(0, num1);
          if (num1 == 1)
            str10 = "%" + str10;
          dateTimePatteren.Add(new DateTimeProperties()
          {
            IsReadOnly = new bool?(false),
            Type = DateTimeType.Hour12,
            Pattern = str10
          });
          break;
        case 'm':
          string str11 = mask.Substring(0, num1);
          if (num1 == 1)
            str11 = "%" + str11;
          dateTimePatteren.Add(new DateTimeProperties()
          {
            IsReadOnly = new bool?(false),
            Type = DateTimeType.Minutes,
            Pattern = str11
          });
          break;
        case 'z':
          string str12 = mask.Substring(0, num1);
          if (num1 == 1)
            str12 = "%" + str12;
          dateTimePatteren.Add(new DateTimeProperties()
          {
            IsReadOnly = new bool?(true),
            Type = DateTimeType.TimeZone,
            Pattern = str12
          });
          break;
        default:
          num1 = 1;
          dateTimePatteren.Add(new DateTimeProperties()
          {
            IsReadOnly = new bool?(true),
            Lenghth = 1,
            Content = mask[0].ToString(),
            Type = DateTimeType.others
          });
          break;
      }
    }
    return dateTimePatteren;
  }

  private static int GetGroupLengthByMask(string mask)
  {
    for (int index = 1; index < mask.Length; ++index)
    {
      if ((int) mask[index] != (int) mask[0])
        return index;
    }
    return mask.Length;
  }

  private static string GetSpecificFormat(string format, DateTimeFormatInfo info)
  {
    if (format == null || format.Length == 0)
      format = "G";
    if (format.Length == 1)
    {
      switch (format[0])
      {
        case 'D':
          return info.LongDatePattern;
        case 'F':
          return info.FullDateTimePattern;
        case 'G':
          return info.ShortDatePattern + (object) ' ' + info.LongTimePattern;
        case 'M':
        case 'm':
          return info.MonthDayPattern;
        case 'R':
        case 'r':
          return info.RFC1123Pattern;
        case 'T':
          return info.LongTimePattern;
        case 'Y':
        case 'y':
          return info.YearMonthPattern;
        case 'd':
          return info.ShortDatePattern;
        case 'f':
          return info.LongDatePattern + (object) ' ' + info.ShortTimePattern;
        case 'g':
          return info.ShortDatePattern + (object) ' ' + info.ShortTimePattern;
        case 's':
          return info.SortableDateTimePattern;
        case 't':
          return info.ShortTimePattern;
        case 'u':
          return info.UniversalSortableDateTimePattern;
      }
    }
    if (format.Length == 2 && format[0] == '%')
      format = format.Substring(1);
    return format;
  }

  public string CreateDisplayText(DateTimeEdit datetimeeditobj)
  {
    string empty = string.Empty;
    DateTimeFormatInfo dateTimeFormat = datetimeeditobj.GetCulture().DateTimeFormat;
    for (int index1 = 0; index1 < datetimeeditobj.DateTimeProperties.Count; ++index1)
    {
      DateTimeProperties dateTimeProperty = datetimeeditobj.DateTimeProperties[index1];
      if (dateTimeProperty.Pattern == null)
      {
        datetimeeditobj.DateTimeProperties[index1].StartPosition = empty.Length;
        datetimeeditobj.DateTimeProperties[index1].Lenghth = dateTimeProperty.Content.Length;
        empty += dateTimeProperty.Content;
      }
      else
      {
        DateTime result;
        if (!datetimeeditobj.mValue.HasValue)
          DateTime.TryParse(datetimeeditobj.DateTime.ToString(), out result);
        else
          DateTime.TryParse(datetimeeditobj.mValue.ToString(), out result);
        if (datetimeeditobj.mValue.HasValue)
          result = datetimeeditobj.mValue.Value;
        datetimeeditobj.DateTimeProperties[index1].StartPosition = empty.Length;
        if (result > datetimeeditobj.CultureInfo.Calendar.MaxSupportedDateTime)
          result = datetimeeditobj.CultureInfo.Calendar.MaxSupportedDateTime;
        if (result < datetimeeditobj.CultureInfo.Calendar.MinSupportedDateTime)
          result = datetimeeditobj.CultureInfo.Calendar.MinSupportedDateTime;
        string str = result.ToString(dateTimeProperty.Pattern, (IFormatProvider) datetimeeditobj.CultureInfo);
        datetimeeditobj.DateTimeProperties[index1].Lenghth = str.Length;
        if (!datetimeeditobj.CanEdit && datetimeeditobj.IsNull && string.IsNullOrEmpty(datetimeeditobj.DateTime.ToString()) && datetimeeditobj.WatermarkVisibility != Visibility.Visible && datetimeeditobj.ShowMaskOnNullValue)
        {
          for (int index2 = 0; index2 < str.Length; ++index2)
            empty += " ";
        }
        else
        {
          dateTimeProperty.Content = str;
          empty += dateTimeProperty.Content;
        }
      }
    }
    return empty;
  }

  public void HandleSelection(DateTimeEdit datetimeeditobj)
  {
    if (!datetimeeditobj.IsEditable || datetimeeditobj.CanEdit)
      return;
    if (datetimeeditobj.SelectWholeContent && datetimeeditobj.SelectionLength == datetimeeditobj.Text.Length)
    {
      if (ModifierKeys.Shift != Keyboard.Modifiers && DateTimeHandler.selectedflag)
        return;
      datetimeeditobj.selectionChanged = false;
      datetimeeditobj.SelectAll();
      DateTimeHandler.selectedflag = true;
      datetimeeditobj.selectionChanged = true;
    }
    else
    {
      for (int index1 = 0; index1 < datetimeeditobj.DateTimeProperties.Count; ++index1)
      {
        DateTimeProperties dateTimeProperty = datetimeeditobj.DateTimeProperties[index1];
        if (dateTimeProperty.StartPosition <= datetimeeditobj.SelectionStart && datetimeeditobj.SelectionStart < dateTimeProperty.StartPosition + dateTimeProperty.Lenghth)
        {
          bool? isReadOnly = dateTimeProperty.IsReadOnly;
          if ((isReadOnly.GetValueOrDefault() ? 0 : (isReadOnly.HasValue ? 1 : 0)) != 0)
          {
            datetimeeditobj.selectionChanged = false;
            for (int index2 = 0; index2 < datetimeeditobj.DateTimeProperties.Count; ++index2)
              datetimeeditobj.DateTimeProperties[index2].KeyPressCount = 0;
            datetimeeditobj.Select(dateTimeProperty.StartPosition, dateTimeProperty.Lenghth);
            if (index1 + 1 < datetimeeditobj.DateTimeProperties.Count && datetimeeditobj.SelectedText.Contains(datetimeeditobj.DateTimeProperties[index1 + 1].Content))
            {
              datetimeeditobj.selectionChanged = false;
              datetimeeditobj.Select(dateTimeProperty.StartPosition, dateTimeProperty.Lenghth - datetimeeditobj.DateTimeProperties[index1 + 1].Content.Length);
            }
            datetimeeditobj.selectionChanged = true;
            if (index1 < 0 || !datetimeeditobj.mTextInputpartended)
              return;
            datetimeeditobj.mSelectedCollection = index1;
            return;
          }
          datetimeeditobj.selectionChanged = false;
          for (int index3 = 0; index3 < datetimeeditobj.DateTimeProperties.Count; ++index3)
            datetimeeditobj.DateTimeProperties[index3].KeyPressCount = 0;
          if (dateTimeProperty.Content == datetimeeditobj.GetCulture().DateTimeFormat.DateSeparator)
          {
            datetimeeditobj.SelectionLength = 0;
            if (datetimeeditobj.DateTime.HasValue && datetimeeditobj.SelectionStart != datetimeeditobj.DateTimeProperties[index1 - 1].StartPosition && !datetimeeditobj.CanEdit && datetimeeditobj.IsEmptyDateEnabled && datetimeeditobj.WatermarkVisibility != Visibility.Visible && datetimeeditobj.ShowMaskOnNullValue)
              datetimeeditobj.Select(datetimeeditobj.DateTimeProperties[index1 - 1].StartPosition, dateTimeProperty.Lenghth);
          }
          else
            datetimeeditobj.Select(dateTimeProperty.StartPosition, dateTimeProperty.Lenghth);
          datetimeeditobj.selectionChanged = true;
          if (index1 < 0 || !datetimeeditobj.mTextInputpartended)
            return;
          datetimeeditobj.mSelectedCollection = index1;
          return;
        }
        if (datetimeeditobj.SelectionStart == dateTimeProperty.StartPosition + dateTimeProperty.Lenghth && index1 == datetimeeditobj.DateTimeProperties.Count - 1 && !datetimeeditobj.CanEdit && datetimeeditobj.IsEmptyDateEnabled && datetimeeditobj.WatermarkVisibility != Visibility.Visible && datetimeeditobj.ShowMaskOnNullValue && datetimeeditobj.DateTime.HasValue)
        {
          datetimeeditobj.Select(dateTimeProperty.StartPosition, dateTimeProperty.Lenghth);
          return;
        }
      }
      datetimeeditobj.selectionChanged = false;
      datetimeeditobj.Select(datetimeeditobj.SelectionStart, 0);
      datetimeeditobj.selectionChanged = true;
      datetimeeditobj.mSelectedCollection = 0;
    }
  }

  private DateTime ValidateYearField(
    DateTime date,
    DateTimeEdit dateTimeEdit,
    DateTimeProperties characterProperty,
    int month,
    int day)
  {
    if (characterProperty.Type == DateTimeType.year)
    {
      int year = dateTimeEdit.MaxDateTime.Year;
      while (!dateTimeEdit.CheckLeapYear(year))
        --year;
      while (day > DateTime.DaysInMonth(date.Year, month))
        date = date.Year > year ? date.AddYears(-1) : date.AddYears(1);
    }
    else if (characterProperty.Type == DateTimeType.Day)
      date = date.AddDays(-1.0);
    return date;
  }

  private void ValidateDateTimeInput(
    DateTimeEdit dateTimeEdit,
    DateTimeProperties characterProperty,
    DateTime date)
  {
    if (characterProperty.KeyPressCount == 0)
    {
      if (string.IsNullOrEmpty(dateTimeEdit.DateTime.ToString()) && !string.IsNullOrEmpty(date.ToString()))
        dateTimeEdit.UpdateDateTimeValue(date, dateTimeEdit, characterProperty);
      dateTimeEdit.selectionChanged = false;
      if (characterProperty.Pattern != "%h" && characterProperty.Pattern != "%d" && characterProperty.Pattern != "%M" && characterProperty.Type != DateTimeType.monthname)
        dateTimeEdit.SelectedText = "00";
      else
        dateTimeEdit.SelectedText = "0";
      dateTimeEdit.selectionChanged = true;
    }
    else
    {
      if (characterProperty.Type == DateTimeType.year)
        return;
      dateTimeEdit.ValidateDateTimeField(Key.None);
    }
  }

  public bool MatchWithMask(DateTimeEdit datetimeeditobj, string text)
  {
    string s1 = text;
    if (!Regex.IsMatch(text, "[0-9]{1,}") || datetimeeditobj.IsReadOnly)
      return true;
    int selectionStart = datetimeeditobj.SelectionStart;
    int result1;
    int.TryParse(datetimeeditobj.SelectedText.ToString(), out result1);
    for (int index1 = 0; index1 < datetimeeditobj.DateTimeProperties.Count; ++index1)
    {
      datetimeeditobj.GetCulture().DateTimeFormat.Clone();
      DateTimeProperties dateTimeProperty1 = datetimeeditobj.DateTimeProperties[index1];
      if (dateTimeProperty1.StartPosition <= datetimeeditobj.SelectionStart && datetimeeditobj.SelectionStart <= dateTimeProperty1.StartPosition + dateTimeProperty1.Lenghth - 1)
      {
        if (datetimeeditobj.SelectWholeContent && datetimeeditobj.Text != null && datetimeeditobj.SelectionLength == datetimeeditobj.Text.Length && !datetimeeditobj.IsReadOnly && datetimeeditobj.mSelectedCollection > 0)
          datetimeeditobj.mSelectedCollection = datetimeeditobj.SelectionStart;
        bool? isReadOnly = dateTimeProperty1.IsReadOnly;
        if ((isReadOnly.GetValueOrDefault() ? 0 : (isReadOnly.HasValue ? 1 : 0)) != 0 && dateTimeProperty1.Type == DateTimeType.year)
        {
          int keyPressCount = dateTimeProperty1.KeyPressCount;
          DateTime date = datetimeeditobj.DateTime.HasValue || !datetimeeditobj.IsEmptyDateEnabled || datetimeeditobj.WatermarkVisibility == Visibility.Visible ? datetimeeditobj.mValue.Value : new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
          datetimeeditobj.mTextInputpartended = false;
          this.ValidateDateTimeInput(datetimeeditobj, datetimeeditobj.DateTimeProperties[index1], date);
          if (dateTimeProperty1.KeyPressCount == 0)
          {
            datetimeeditobj.selectionChanged = false;
            datetimeeditobj.SelectedText = string.Empty;
            datetimeeditobj.selectionChanged = true;
          }
          string text1 = datetimeeditobj.Text;
          string str = datetimeeditobj.SelectedText.Length >= dateTimeProperty1.Lenghth ? text1.Remove(dateTimeProperty1.StartPosition, text.Length).Insert(dateTimeProperty1.StartPosition + (datetimeeditobj.SelectionLength - 1), text) : text1.Insert(dateTimeProperty1.StartPosition + dateTimeProperty1.KeyPressCount, text);
          datetimeeditobj.Text = str;
          datetimeeditobj.selectionChanged = false;
          if (keyPressCount < dateTimeProperty1.Lenghth)
            datetimeeditobj.Select(dateTimeProperty1.StartPosition, keyPressCount + 1);
          else
            datetimeeditobj.Select(dateTimeProperty1.StartPosition, keyPressCount);
          datetimeeditobj.selectionChanged = true;
          if (datetimeeditobj.SelectedText.Length == dateTimeProperty1.Lenghth)
          {
            int result2;
            int.TryParse(datetimeeditobj.SelectedText, out result2);
            if (result2 > 0)
            {
              int num = result2 - date.Year;
              date = date.AddYears(num);
              datetimeeditobj.mSelectedCollection = index1;
              if (date < datetimeeditobj.MinDateTime || date > datetimeeditobj.MaxDateTime)
              {
                for (int index2 = 1; index2 < datetimeeditobj.DateTimeProperties.Count && date.Year.ToString().Length > 3; ++index2)
                {
                  if (datetimeeditobj.DateTimeProperties[index2].Type == DateTimeType.year)
                  {
                    DateTime dateTime;
                    if (date < datetimeeditobj.MinDateTime)
                    {
                      if (date.Year == datetimeeditobj.MinDateTime.Year)
                        date = new DateTime(datetimeeditobj.MinDateTime.Year, date.Month, date.Day);
                      if (date.Year == datetimeeditobj.MinDateTime.Year && date.Month == datetimeeditobj.MinDateTime.Month)
                        date = new DateTime(date.Year, datetimeeditobj.MinDateTime.Month, date.Day);
                      if (date.Year == datetimeeditobj.MinDateTime.Year && date.Month == datetimeeditobj.MinDateTime.Month)
                      {
                        int day1 = date.Day;
                        dateTime = datetimeeditobj.MinDateTime;
                        int day2 = dateTime.Day;
                        if (day1 < day2)
                        {
                          ref DateTime local = ref date;
                          int year = date.Year;
                          int month = date.Month;
                          dateTime = datetimeeditobj.MinDateTime;
                          int day3 = dateTime.Day;
                          local = new DateTime(year, month, day3);
                        }
                      }
                    }
                    if (date > datetimeeditobj.MinDateTime)
                    {
                      int year1 = date.Year;
                      dateTime = datetimeeditobj.MaxDateTime;
                      int year2 = dateTime.Year;
                      if (year1 >= year2)
                      {
                        ref DateTime local = ref date;
                        dateTime = datetimeeditobj.MaxDateTime;
                        int year3 = dateTime.Year;
                        int month = date.Month;
                        int day = date.Day;
                        local = new DateTime(year3, month, day);
                      }
                      int year4 = date.Year;
                      dateTime = datetimeeditobj.MaxDateTime;
                      int year5 = dateTime.Year;
                      if (year4 == year5)
                      {
                        int month1 = date.Month;
                        dateTime = datetimeeditobj.MaxDateTime;
                        int month2 = dateTime.Month;
                        if (month1 == month2)
                        {
                          ref DateTime local = ref date;
                          int year6 = date.Year;
                          dateTime = datetimeeditobj.MinDateTime;
                          int month3 = dateTime.Month;
                          int day = date.Day;
                          local = new DateTime(year6, month3, day);
                        }
                      }
                      int year7 = date.Year;
                      dateTime = datetimeeditobj.MaxDateTime;
                      int year8 = dateTime.Year;
                      if (year7 == year8)
                      {
                        int month4 = date.Month;
                        dateTime = datetimeeditobj.MaxDateTime;
                        int month5 = dateTime.Month;
                        if (month4 == month5)
                        {
                          int day4 = date.Day;
                          dateTime = datetimeeditobj.MaxDateTime;
                          int day5 = dateTime.Day;
                          if (day4 >= day5)
                          {
                            ref DateTime local = ref date;
                            int year9 = date.Year;
                            int month6 = date.Month;
                            dateTime = datetimeeditobj.MaxDateTime;
                            int day6 = dateTime.Day;
                            local = new DateTime(year9, month6, day6);
                          }
                        }
                      }
                    }
                  }
                }
                datetimeeditobj.UpdateDateTimeValue(date, datetimeeditobj, dateTimeProperty1);
              }
              datetimeeditobj.ValidateDateTimeField(Key.None);
            }
          }
          dateTimeProperty1.KeyPressCount = keyPressCount + 1;
          if (dateTimeProperty1.Pattern.ToLower() == "yy")
          {
            if (datetimeeditobj.SelectedText.Length == 2)
            {
              datetimeeditobj.mTextInputpartended = true;
              dateTimeProperty1.KeyPressCount = 2;
              if (datetimeeditobj.AutoForwarding)
                KeyHandler.keyHandler.HandleRightKey(datetimeeditobj);
            }
          }
          else if (datetimeeditobj.SelectedText.Length == 4)
          {
            datetimeeditobj.mTextInputpartended = true;
            dateTimeProperty1.KeyPressCount = 4;
            if (datetimeeditobj.AutoForwarding)
              KeyHandler.keyHandler.HandleRightKey(datetimeeditobj);
          }
          return true;
        }
        if (dateTimeProperty1.Type == DateTimeType.Minutes)
        {
          DateTime? nullable = new DateTime?();
          if (datetimeeditobj.mValue.HasValue)
          {
            nullable = new DateTime?(datetimeeditobj.mValue.Value);
            datetimeeditobj.mTextInputpartended = false;
            if ((text + nullable.Value.Minute.ToString()).Length > 2)
            {
              text = nullable.Value.Minute.ToString() + text;
              text = text.Substring(text.Length - 2, 2);
            }
            else
              text = nullable.Value.Minute.ToString() + text;
          }
          else
            nullable = new DateTime?(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0));
          int result3;
          int.TryParse(text, out result3);
          if (result3 > 59 || datetimeeditobj.DateTimeProperties[index1].KeyPressCount == 0 && text.Length > 1)
            text = text.Substring(text.Length - 1, 1);
          int.TryParse(text, out result3);
          int num = result3;
          DateTime dateTime1 = nullable.Value;
          int minute = dateTime1.Minute;
          result3 = num - minute;
          ref DateTime? local = ref nullable;
          dateTime1 = nullable.Value;
          DateTime dateTime2 = dateTime1.AddMinutes((double) result3);
          local = new DateTime?(dateTime2);
          int keyPressCount = datetimeeditobj.DateTimeProperties[index1].KeyPressCount;
          datetimeeditobj.UpdateDateTimeValue(nullable.Value, datetimeeditobj, dateTimeProperty1);
          datetimeeditobj.DateTimeProperties[index1].KeyPressCount = keyPressCount + 1;
          if (datetimeeditobj.DateTimeProperties[index1].KeyPressCount == 2 || int.Parse(text) > 5)
          {
            datetimeeditobj.mTextInputpartended = true;
            datetimeeditobj.DateTimeProperties[index1].KeyPressCount = 0;
            if (datetimeeditobj.AutoForwarding)
              KeyHandler.keyHandler.HandleRightKey(datetimeeditobj);
          }
          return true;
        }
        if (dateTimeProperty1.Type == DateTimeType.Second)
        {
          DateTime? nullable = new DateTime?();
          int length = dateTimeProperty1.Lenghth;
          if (datetimeeditobj.mValue.HasValue)
          {
            nullable = new DateTime?(datetimeeditobj.mValue.Value);
            datetimeeditobj.mTextInputpartended = false;
            if (length < 2)
              length = 2;
            if ((text + nullable.Value.Second.ToString()).Length > length)
            {
              text = nullable.Value.Second.ToString() + text;
              text = text.Substring(text.Length - length, length);
            }
            else
              text = nullable.Value.Second.ToString() + text;
          }
          else
            nullable = new DateTime?(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0));
          int result4;
          int.TryParse(text, out result4);
          if (result4 > 59 || datetimeeditobj.DateTimeProperties[index1].KeyPressCount == 0 && text.Length > 1)
            text = text.Substring(text.Length - 1, 1);
          int.TryParse(text, out result4);
          int num = result4;
          DateTime dateTime3 = nullable.Value;
          int second = dateTime3.Second;
          result4 = num - second;
          ref DateTime? local = ref nullable;
          dateTime3 = nullable.Value;
          DateTime dateTime4 = dateTime3.AddSeconds((double) result4);
          local = new DateTime?(dateTime4);
          int keyPressCount = datetimeeditobj.DateTimeProperties[index1].KeyPressCount;
          datetimeeditobj.UpdateDateTimeValue(nullable.Value, datetimeeditobj, dateTimeProperty1);
          datetimeeditobj.DateTimeProperties[index1].KeyPressCount = keyPressCount + 1;
          if (datetimeeditobj.DateTimeProperties[index1].KeyPressCount == length || int.Parse(text) > 5)
          {
            datetimeeditobj.mTextInputpartended = true;
            datetimeeditobj.DateTimeProperties[index1].KeyPressCount = 0;
            if (datetimeeditobj.AutoForwarding)
              KeyHandler.keyHandler.HandleRightKey(datetimeeditobj);
          }
          return true;
        }
        if (dateTimeProperty1.Type == DateTimeType.Hour24 || dateTimeProperty1.Type == DateTimeType.Hour12)
        {
          DateTime? nullable = new DateTime?();
          int length = dateTimeProperty1.Lenghth;
          if (datetimeeditobj.mValue.HasValue)
          {
            nullable = new DateTime?(datetimeeditobj.mValue.Value);
            datetimeeditobj.mTextInputpartended = false;
            if (length < 2)
              length = 2;
            if (dateTimeProperty1.KeyPressCount > 0 && result1 > 0)
            {
              if (int.Parse(text) > 0 && (text + nullable.Value.Hour.ToString()).Length > length)
              {
                text = nullable.Value.Hour.ToString() + text;
                text = text.Substring(text.Length - length, length);
              }
              else
                text = nullable.Value.Hour.ToString() + text;
            }
          }
          else
            nullable = new DateTime?(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0));
          int result5;
          int.TryParse(text, out result5);
          if (result5 > (dateTimeProperty1.Type == DateTimeType.Hour12 ? 12 : 23) || int.Parse(s1) > 0 && datetimeeditobj.DateTimeProperties[index1].KeyPressCount == 0 && text.Length > 1)
            text = text.Substring(text.Length - 1, 1);
          int.TryParse(text, out result5);
          int keyPressCount = datetimeeditobj.DateTimeProperties[index1].KeyPressCount;
          if (result5 > 0 || dateTimeProperty1.Type == DateTimeType.Hour24)
          {
            result5 -= nullable.Value.Hour;
            nullable = new DateTime?(nullable.Value.AddHours((double) result5));
            datetimeeditobj.UpdateDateTimeValue(nullable.Value, datetimeeditobj, dateTimeProperty1);
          }
          else if (dateTimeProperty1.Type == DateTimeType.Hour12)
            this.ValidateDateTimeInput(datetimeeditobj, dateTimeProperty1, nullable.Value);
          datetimeeditobj.DateTimeProperties[index1].KeyPressCount = keyPressCount + 1;
          if (datetimeeditobj.DateTimeProperties[index1].KeyPressCount == length || dateTimeProperty1.Type == DateTimeType.Hour12 && int.Parse(text) > 1 || dateTimeProperty1.Type == DateTimeType.Hour24 && int.Parse(text) > 2)
          {
            datetimeeditobj.mTextInputpartended = true;
            datetimeeditobj.DateTimeProperties[index1].KeyPressCount = 0;
            if (datetimeeditobj.AutoForwarding)
              KeyHandler.keyHandler.HandleRightKey(datetimeeditobj);
          }
          return true;
        }
        if (dateTimeProperty1.Type == DateTimeType.Fraction)
        {
          DateTime date = !datetimeeditobj.mValue.HasValue ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) : datetimeeditobj.mValue.Value;
          if ((text + date.Millisecond.ToString()).Length > 3)
          {
            text = date.Millisecond.ToString() + text;
            text = text.Substring(text.Length - 3, 3);
          }
          else
            text = date.Millisecond.ToString() + text;
          int result6;
          int.TryParse(text, out result6);
          result6 -= date.Millisecond;
          date = date.AddMilliseconds((double) result6);
          datetimeeditobj.UpdateDateTimeValue(date, datetimeeditobj, dateTimeProperty1);
          return true;
        }
        if (dateTimeProperty1.Type == DateTimeType.Month || dateTimeProperty1.Type == DateTimeType.monthname)
        {
          for (int index3 = 0; index3 < datetimeeditobj.DateTimeProperties.Count; ++index3)
          {
            if (index3 != index1)
              datetimeeditobj.DateTimeProperties[index3].KeyPressCount = 0;
          }
          DateTime date = !datetimeeditobj.mValue.HasValue ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) : datetimeeditobj.mValue.Value;
          datetimeeditobj.mTextInputpartended = false;
          if (dateTimeProperty1.KeyPressCount > 0 && (!Regex.IsMatch(datetimeeditobj.SelectedText, "[0-9]{1,}") || result1 > 0))
          {
            if (date.Month.ToString() != "1" && date.Month.ToString() != null)
            {
              if ((text + date.Month.ToString()).Length > dateTimeProperty1.Lenghth)
              {
                text = date.Month.ToString() + text;
                text = text.Substring(text.Length - dateTimeProperty1.Lenghth, dateTimeProperty1.Lenghth);
              }
              else
                text = date.Month.ToString() + text;
              if ((text + date.Month.ToString()).Length > 2)
              {
                text = date.Month.ToString() + text;
                text = text.Substring(text.Length - 2, 2);
              }
              else
                text = date.Month.ToString() + text;
            }
            string str = date.Month.ToString();
            if (str.Length > 1)
            {
              if (datetimeeditobj.checkmonth && datetimeeditobj.DateTimeProperties[index1].KeyPressCount == 1)
              {
                if (str == "11" && (text == "1" || text == "2"))
                {
                  text = str.Remove(datetimeeditobj.DateTimeProperties[index1].KeyPressCount - 1, 2).Insert(datetimeeditobj.DateTimeProperties[index1].KeyPressCount - 1, text);
                  datetimeeditobj.checkmonth = false;
                }
                else
                {
                  text = str.Replace(str[datetimeeditobj.DateTimeProperties[index1].KeyPressCount - 1], str[datetimeeditobj.DateTimeProperties[index1].KeyPressCount]).Remove(datetimeeditobj.DateTimeProperties[index1].KeyPressCount, 1).Insert(datetimeeditobj.DateTimeProperties[index1].KeyPressCount, text);
                  datetimeeditobj.checkmonth = false;
                }
              }
              else if ((str == "12" && text == "0" || str == "11" && text == "0" || str == "10" && text == "0") && datetimeeditobj.DateTimeProperties[index1].KeyPressCount == 0)
                datetimeeditobj.checkmonth = true;
              else if (datetimeeditobj.DateTimeProperties[index1].KeyPressCount == 0)
              {
                text = str.Remove(datetimeeditobj.DateTimeProperties[index1].KeyPressCount, 2).Insert(datetimeeditobj.DateTimeProperties[index1].KeyPressCount, text);
              }
              else
              {
                string s2 = str.Remove(datetimeeditobj.DateTimeProperties[index1].KeyPressCount, 1);
                int num = datetimeeditobj.checkmonth ? 1 : 0;
                if (int.Parse(s2) == 0 && int.Parse(text) == 0)
                  datetimeeditobj.checkmonth = true;
                text = s2.Insert(datetimeeditobj.DateTimeProperties[index1].KeyPressCount, text);
              }
            }
            else
            {
              if (datetimeeditobj.DateTimeProperties[index1].KeyPressCount == 0)
                str.Replace(str[datetimeeditobj.DateTimeProperties[index1].KeyPressCount], text[datetimeeditobj.DateTimeProperties[index1].KeyPressCount]);
              int keyPressCount = datetimeeditobj.DateTimeProperties[index1].KeyPressCount;
            }
          }
          int result7;
          int.TryParse(text, out result7);
          if (dateTimeProperty1.KeyPressCount > 0 && (!Regex.IsMatch(datetimeeditobj.SelectedText, "[0-9]{1,}") || result1 > 0) && text.Length == 1)
          {
            datetimeeditobj.checktext += text;
            int result8;
            int.TryParse(datetimeeditobj.checktext, out result8);
            if (result8 > result7)
              int.TryParse(datetimeeditobj.checktext, out result7);
            text = datetimeeditobj.checktext;
          }
          if (result7 > 12)
          {
            if (text.Length >= 3)
              text = text.Substring(text.Length - (text.Length - 1), 2);
            if (int.Parse(text) >= 11)
              text = text.Substring(text.Length - 1, 1);
          }
          if (datetimeeditobj.DateTimeProperties[index1].KeyPressCount == 0 && (result7.ToString().Length == datetimeeditobj.DateTimeProperties[index1].Lenghth || result7.ToString().Length > 1 && datetimeeditobj.DateTimeProperties[index1].Type == DateTimeType.monthname))
            text = text.Substring(text.Length - 1, 1);
          if (datetimeeditobj.DateTimeProperties[index1].KeyPressCount == 1 && (!Regex.IsMatch(datetimeeditobj.SelectedText, "[0-9]{1,}") || result1 > 0) && (text == "0" || text == "1" || text == "2") && date.Month.ToString() == "1" && date.Month.ToString() != null)
          {
            switch (text)
            {
              case "0":
                text = "10";
                break;
              case "1":
                text = "11";
                break;
              default:
                text = "12";
                break;
            }
          }
          for (int index4 = index1 + 1; int.Parse(text) > 0 && date.Day > DateTime.DaysInMonth(date.Year, int.Parse(text)) && index4 < datetimeeditobj.DateTimeProperties.Count; ++index4)
          {
            DateTimeProperties dateTimeProperty2 = datetimeeditobj.DateTimeProperties[index4];
            string str = date.Day.ToString();
            if (int.Parse(text) == 2 && int.Parse(str[0].ToString()) < 3)
              date = this.ValidateYearField(date, datetimeeditobj, dateTimeProperty2, int.Parse(text), date.Day);
          }
          int.TryParse(text, out result7);
          int keyPressCount1 = datetimeeditobj.DateTimeProperties[index1].KeyPressCount;
          if (result7 > 0)
          {
            result7 -= date.Month;
            date = date.AddMonths(result7);
            if (date < datetimeeditobj.MinDateTime || date > datetimeeditobj.MaxDateTime)
            {
              for (int index5 = index1 + 1; (datetimeeditobj.DateTimeProperties[index1].KeyPressCount > 0 || int.Parse(text) > 1) && index5 < datetimeeditobj.DateTimeProperties.Count; ++index5)
              {
                if (datetimeeditobj.DateTimeProperties[index5].Type == DateTimeType.year)
                {
                  DateTime dateTime;
                  if (date < datetimeeditobj.MinDateTime)
                  {
                    int year10 = date.Year;
                    dateTime = datetimeeditobj.MaxDateTime;
                    int year11 = dateTime.Year;
                    if (year10 < year11)
                      date = new DateTime(date.Year + 1, date.Month, date.Day);
                    int year12 = date.Year;
                    dateTime = datetimeeditobj.MinDateTime;
                    int year13 = dateTime.Year;
                    if (year12 == year13)
                    {
                      int month7 = date.Month;
                      dateTime = datetimeeditobj.MinDateTime;
                      int month8 = dateTime.Month;
                      if (month7 < month8)
                      {
                        ref DateTime local = ref date;
                        int year14 = date.Year;
                        dateTime = datetimeeditobj.MinDateTime;
                        int month9 = dateTime.Month;
                        int day = date.Day;
                        local = new DateTime(year14, month9, day);
                      }
                    }
                    int year15 = date.Year;
                    dateTime = datetimeeditobj.MinDateTime;
                    int year16 = dateTime.Year;
                    if (year15 == year16)
                    {
                      int month10 = date.Month;
                      dateTime = datetimeeditobj.MinDateTime;
                      int month11 = dateTime.Month;
                      if (month10 == month11)
                      {
                        int day7 = date.Day;
                        dateTime = datetimeeditobj.MinDateTime;
                        int day8 = dateTime.Day;
                        if (day7 < day8)
                        {
                          ref DateTime local = ref date;
                          int year17 = date.Year;
                          int month12 = date.Month;
                          dateTime = datetimeeditobj.MinDateTime;
                          int day9 = dateTime.Day;
                          local = new DateTime(year17, month12, day9);
                        }
                      }
                    }
                  }
                  if (date > datetimeeditobj.MaxDateTime)
                  {
                    int year18 = date.Year;
                    dateTime = datetimeeditobj.MaxDateTime;
                    int year19 = dateTime.Year;
                    if (year18 >= year19)
                    {
                      ref DateTime local = ref date;
                      dateTime = datetimeeditobj.MaxDateTime;
                      int year20 = dateTime.Year;
                      int month = date.Month;
                      int day = date.Day;
                      local = new DateTime(year20, month, day);
                    }
                    int year21 = date.Year;
                    dateTime = datetimeeditobj.MaxDateTime;
                    int year22 = dateTime.Year;
                    if (year21 == year22)
                    {
                      int month13 = date.Month;
                      dateTime = datetimeeditobj.MaxDateTime;
                      int month14 = dateTime.Month;
                      if (month13 > month14)
                      {
                        ref DateTime local = ref date;
                        int year23 = date.Year;
                        dateTime = datetimeeditobj.MaxDateTime;
                        int month15 = dateTime.Month;
                        int day = date.Day;
                        local = new DateTime(year23, month15, day);
                      }
                    }
                    int year24 = date.Year;
                    dateTime = datetimeeditobj.MaxDateTime;
                    int year25 = dateTime.Year;
                    if (year24 == year25)
                    {
                      int month16 = date.Month;
                      dateTime = datetimeeditobj.MaxDateTime;
                      int month17 = dateTime.Month;
                      if (month16 == month17)
                      {
                        int day10 = date.Day;
                        dateTime = datetimeeditobj.MaxDateTime;
                        int day11 = dateTime.Day;
                        if (day10 > day11)
                        {
                          ref DateTime local = ref date;
                          int year26 = date.Year;
                          int month18 = date.Month;
                          dateTime = datetimeeditobj.MaxDateTime;
                          int day12 = dateTime.Day;
                          local = new DateTime(year26, month18, day12);
                        }
                      }
                    }
                  }
                }
              }
            }
            datetimeeditobj.UpdateDateTimeValue(date, datetimeeditobj, dateTimeProperty1);
          }
          else
            this.ValidateDateTimeInput(datetimeeditobj, dateTimeProperty1, date);
          datetimeeditobj.DateTimeProperties[index1].KeyPressCount = keyPressCount1 + 1;
          if (datetimeeditobj.DateTimeProperties[index1].KeyPressCount == 2 || s1.Length == 2 || int.Parse(text) > 1)
          {
            datetimeeditobj.mTextInputpartended = true;
            datetimeeditobj.DateTimeProperties[index1].KeyPressCount = 0;
            datetimeeditobj.checktext = "";
            if (datetimeeditobj.AutoForwarding)
              KeyHandler.keyHandler.HandleRightKey(datetimeeditobj);
          }
          return true;
        }
        if (dateTimeProperty1.Type == DateTimeType.Day)
        {
          DateTime date = !datetimeeditobj.mValue.HasValue ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) : datetimeeditobj.mValue.Value;
          datetimeeditobj.mTextInputpartended = false;
          if (dateTimeProperty1.KeyPressCount > 0 && result1 > 0)
          {
            if ((text + date.Day.ToString()).Length > dateTimeProperty1.Lenghth)
            {
              text = date.Day.ToString() + text;
              text = dateTimeProperty1.Content != null || !datetimeeditobj.ShowMaskOnNullValue ? text.Substring(text.Length - dateTimeProperty1.Lenghth, dateTimeProperty1.Lenghth) : text.Substring(1, 1);
            }
            else
              text = date.Day.ToString() + text;
            if ((text + date.Day.ToString()).Length > 2)
            {
              text = date.Day.ToString() + text;
              text = text.Substring(text.Length - 2, 2);
            }
            else if (dateTimeProperty1.Content != null && !datetimeeditobj.CanEdit && datetimeeditobj.WatermarkVisibility != Visibility.Visible && datetimeeditobj.ShowMaskOnNullValue || !datetimeeditobj.ShowMaskOnNullValue)
              text = date.Day.ToString() + text;
            string s3 = date.Day.ToString();
            if (s3.Length > 1)
            {
              if (int.Parse(s3) >= 12 && int.Parse(s3) <= 29 && int.Parse(text) >= 3 && datetimeeditobj.DateTimeProperties[index1].KeyPressCount == 0)
                text = s3.Remove(datetimeeditobj.DateTimeProperties[index1].KeyPressCount, 2).Insert(datetimeeditobj.DateTimeProperties[index1].KeyPressCount, text);
              else if (int.Parse(s3) >= 30 && datetimeeditobj.DateTimeProperties[index1].KeyPressCount == 0 && int.Parse(text) == 3 && int.Parse(text) != 3)
              {
                text = s3;
                datetimeeditobj.checkday1 = true;
              }
              else if (datetimeeditobj.checkday && datetimeeditobj.DateTimeProperties[index1].KeyPressCount != 0)
              {
                text = s3.Replace(s3[datetimeeditobj.DateTimeProperties[index1].KeyPressCount - 1], s3[datetimeeditobj.DateTimeProperties[index1].KeyPressCount]).Remove(datetimeeditobj.DateTimeProperties[index1].KeyPressCount, 1).Insert(datetimeeditobj.DateTimeProperties[index1].KeyPressCount, text);
                datetimeeditobj.checkday = false;
              }
              else if (datetimeeditobj.checkday1 && datetimeeditobj.DateTimeProperties[index1].KeyPressCount != 0)
              {
                text = s3.Replace(s3[datetimeeditobj.DateTimeProperties[index1].KeyPressCount], text[datetimeeditobj.DateTimeProperties[index1].KeyPressCount - 1]);
                datetimeeditobj.checkday1 = false;
              }
              else if (datetimeeditobj.checkday2 && datetimeeditobj.DateTimeProperties[index1].KeyPressCount == 1)
              {
                text = s3.Replace(s3[datetimeeditobj.DateTimeProperties[index1].KeyPressCount - 1], s3[datetimeeditobj.DateTimeProperties[index1].KeyPressCount]).Remove(datetimeeditobj.DateTimeProperties[index1].KeyPressCount, 1).Insert(datetimeeditobj.DateTimeProperties[index1].KeyPressCount, text);
                datetimeeditobj.checkday2 = false;
              }
              else
                text = datetimeeditobj.DateTimeProperties[index1].KeyPressCount != 0 ? s3.Remove(datetimeeditobj.DateTimeProperties[index1].KeyPressCount - 1, 2).Insert(datetimeeditobj.DateTimeProperties[index1].KeyPressCount - 1, text) : s3.Remove(datetimeeditobj.DateTimeProperties[index1].KeyPressCount, 2).Insert(datetimeeditobj.DateTimeProperties[index1].KeyPressCount, text);
            }
            else
            {
              if (datetimeeditobj.DateTimeProperties[index1].KeyPressCount == 0)
                s3.Replace(s3[datetimeeditobj.DateTimeProperties[index1].KeyPressCount], text[datetimeeditobj.DateTimeProperties[index1].KeyPressCount]);
              int keyPressCount = datetimeeditobj.DateTimeProperties[index1].KeyPressCount;
            }
          }
          int result9;
          int.TryParse(text, out result9);
          if (dateTimeProperty1.KeyPressCount > 0 && result1 > 0 && text.Length == 1)
          {
            datetimeeditobj.checktext += text;
            int result10;
            int.TryParse(datetimeeditobj.checktext, out result10);
            if (result10 > result9)
              int.TryParse(datetimeeditobj.checktext, out result9);
            text = datetimeeditobj.checktext;
          }
          if (int.Parse(s1) > 0 && datetimeeditobj.DateTimeProperties[index1].KeyPressCount == 0 && text.Length > 1)
            text = text.Substring(text.Length - 1, 1);
          for (int index6 = index1 + 1; datetimeeditobj.DateTimeProperties[index1].KeyPressCount > 0 && int.Parse(text) > DateTime.DaysInMonth(date.Year, date.Month) && index6 < datetimeeditobj.DateTimeProperties.Count; ++index6)
          {
            DateTimeProperties dateTimeProperty3 = datetimeeditobj.DateTimeProperties[index6];
            string str = text.ToString();
            if (date.Month == 2 && int.Parse(str[0].ToString()) < 3 && (dateTimeProperty3.Type == DateTimeType.year || dateTimeProperty3.Type == DateTimeType.Day))
              date = this.ValidateYearField(date, datetimeeditobj, dateTimeProperty3, date.Month, int.Parse(text));
            else if ((dateTimeProperty3.Type == DateTimeType.Month || dateTimeProperty3.Type == DateTimeType.monthname) && int.Parse(text) <= DateTime.DaysInMonth(date.Year, 1))
              date = new DateTime(date.Year, 1, date.Day);
          }
          if (int.Parse(text) > DateTime.DaysInMonth(date.Year, date.Month))
            text = text.Substring(text.Length - 1, 1);
          int result11;
          int.TryParse(text, out result11);
          int keyPressCount2 = datetimeeditobj.DateTimeProperties[index1].KeyPressCount;
          DateTime dateTime5;
          if (result11 > 0)
          {
            result11 -= date.Day;
            date = date.AddDays((double) result11);
            if (date < datetimeeditobj.MinDateTime || date > datetimeeditobj.MaxDateTime)
            {
              for (int index7 = 0 + 1; (datetimeeditobj.DateTimeProperties[index1].KeyPressCount > 0 || int.Parse(text) > 3) && index7 < datetimeeditobj.DateTimeProperties.Count; ++index7)
              {
                DateTimeProperties dateTimeProperty4 = datetimeeditobj.DateTimeProperties[index7 - 1];
                if (dateTimeProperty4.Type == DateTimeType.Month || dateTimeProperty4.Type == DateTimeType.monthname)
                {
                  if (date < datetimeeditobj.MinDateTime)
                  {
                    int month19 = date.Month;
                    dateTime5 = datetimeeditobj.MaxDateTime;
                    int month20 = dateTime5.Month;
                    if (month19 < month20 && date.Month != 12)
                    {
                      if (date.Day < DateTime.DaysInMonth(date.Year, date.Month) && date.Month != 1)
                        date = new DateTime(date.Year, date.Month + 1, date.Day);
                    }
                    else
                    {
                      int year27 = date.Year;
                      dateTime5 = datetimeeditobj.MaxDateTime;
                      int year28 = dateTime5.Year;
                      if (year27 < year28)
                        date = new DateTime(date.Year + 1, date.Month, date.Day);
                    }
                  }
                  else if (date > datetimeeditobj.MaxDateTime)
                  {
                    if (date > datetimeeditobj.MaxDateTime)
                    {
                      int month21 = date.Month;
                      dateTime5 = datetimeeditobj.MaxDateTime;
                      int month22 = dateTime5.Month;
                      if (month21 >= month22)
                      {
                        if (date.Day > DateTime.DaysInMonth(date.Year, date.Month - 1))
                        {
                          int day13 = date.Day;
                          dateTime5 = datetimeeditobj.MaxDateTime;
                          int day14 = dateTime5.Day;
                          if (day13 > day14)
                          {
                            ref DateTime local = ref date;
                            int year = date.Year;
                            int month23 = date.Month;
                            dateTime5 = datetimeeditobj.MaxDateTime;
                            int day15 = dateTime5.Day;
                            local = new DateTime(year, month23, day15);
                            continue;
                          }
                          date = new DateTime(date.Year, date.Month - 1, date.Day - 1);
                          continue;
                        }
                        date = new DateTime(date.Year, date.Month - 1, date.Day);
                        continue;
                      }
                    }
                    int month24 = date.Month;
                    dateTime5 = datetimeeditobj.MaxDateTime;
                    int month25 = dateTime5.Month;
                    if (month24 <= month25)
                    {
                      int year29 = date.Year;
                      dateTime5 = datetimeeditobj.MinDateTime;
                      int year30 = dateTime5.Year;
                      if (year29 > year30)
                        date = new DateTime(date.Year - 1, date.Month, date.Day);
                    }
                  }
                }
              }
            }
            datetimeeditobj.UpdateDateTimeValue(date, datetimeeditobj, dateTimeProperty1);
          }
          else
            this.ValidateDateTimeInput(datetimeeditobj, dateTimeProperty1, date);
          datetimeeditobj.DateTimeProperties[index1].KeyPressCount = keyPressCount2 + 1;
          bool flag = false;
          for (int index8 = 0; index8 < datetimeeditobj.DateTimeProperties.Count; ++index8)
          {
            DateTimeProperties dateTimeProperty5 = datetimeeditobj.DateTimeProperties[index8];
            if (index8 > index1 && (dateTimeProperty5.Type == DateTimeType.Month || dateTimeProperty5.Type == DateTimeType.monthname))
              flag = true;
          }
          if (datetimeeditobj.DateTimeProperties[index1].KeyPressCount != 2 && int.Parse(text) <= 3)
          {
            if (!flag && datetimeeditobj.DateTime.HasValue)
            {
              DateTime dateTime6 = datetimeeditobj.DateTime.Value;
              dateTime5 = datetimeeditobj.DateTime.Value;
              if (dateTime5.Month != 2 || int.Parse(text) <= 2)
                goto label_253;
            }
            else
              goto label_253;
          }
          datetimeeditobj.mTextInputpartended = true;
          datetimeeditobj.DateTimeProperties[index1].KeyPressCount = 0;
          datetimeeditobj.checktext = "";
          if (datetimeeditobj.AutoForwarding)
            KeyHandler.keyHandler.HandleRightKey(datetimeeditobj);
label_253:
          return true;
        }
      }
    }
    datetimeeditobj.Select(datetimeeditobj.SelectionStart, 0);
    datetimeeditobj.selectionChanged = true;
    return true;
  }
}
