// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.KeyHandler
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal class KeyHandler
{
  public static KeyHandler keyHandler = new KeyHandler();
  private Hashtable monthTable = new Hashtable();

  private KeyHandler()
  {
    this.monthTable.Add((object) 1, (object) "january");
    this.monthTable.Add((object) 2, (object) "february");
    this.monthTable.Add((object) 3, (object) "march");
    this.monthTable.Add((object) 4, (object) "april");
    this.monthTable.Add((object) 5, (object) "may");
    this.monthTable.Add((object) 6, (object) "june");
    this.monthTable.Add((object) 7, (object) "july");
    this.monthTable.Add((object) 8, (object) "august");
    this.monthTable.Add((object) 9, (object) "september");
    this.monthTable.Add((object) 10, (object) "october");
    this.monthTable.Add((object) 11, (object) "november");
    this.monthTable.Add((object) 12, (object) "december");
  }

  public bool HandleKeyDown(DateTimeEdit dateTimeEdit, KeyEventArgs eventArgs)
  {
    if (dateTimeEdit.IsDropDownOpen)
    {
      if (eventArgs.Key == Key.Escape)
        dateTimeEdit.IsDropDownOpen = false;
      return true;
    }
    if (dateTimeEdit.DateTimeProperties != null && !dateTimeEdit.IsReadOnly && !dateTimeEdit.CanEdit && dateTimeEdit.DateTimeProperties.Count > 0)
    {
      for (int index = 0; index < dateTimeEdit.DateTimeProperties.Count; ++index)
      {
        DateTimeProperties dateTimeProperty = dateTimeEdit.DateTimeProperties[index];
        if (dateTimeProperty != null && dateTimeProperty.Pattern != null && dateTimeProperty.StartPosition == dateTimeEdit.SelectionStart)
        {
          if (dateTimeProperty.Type == DateTimeType.designator && !string.IsNullOrEmpty(dateTimeEdit.mValue.ToString()))
          {
            DateTimeFormatInfo dateTimeFormat = dateTimeEdit.GetCulture().DateTimeFormat;
            string str = KeyCode.KeycodeToChar(eventArgs.Key, false);
            DateTime date = dateTimeEdit.mValue.Value;
            if (dateTimeFormat != null && !string.IsNullOrEmpty(date.ToString()) && !string.IsNullOrEmpty(str) && str != dateTimeProperty.Content[0].ToString())
            {
              if (str == dateTimeFormat.AMDesignator[0].ToString())
                date = date.AddHours(-12.0);
              else if (str == dateTimeFormat.PMDesignator[0].ToString())
                date = date.AddHours(12.0);
              DateTime dateTime = dateTimeEdit.IsBlackoutDate(Key.None, date);
              dateTimeEdit.DateTime = new DateTime?(dateTime);
            }
          }
          else if (eventArgs.Key == Key.Escape && dateTimeProperty.KeyPressCount > 0)
            dateTimeProperty.KeyPressCount = 0;
        }
      }
    }
    if (eventArgs.Key == Key.Space)
      return dateTimeEdit.CanEdit || this.HandleRightKey(dateTimeEdit);
    switch (eventArgs.Key)
    {
      case Key.Back:
        if (!dateTimeEdit.EnableBackspaceKey)
          return true;
        for (int index = 0; index < dateTimeEdit.DateTimeProperties.Count; ++index)
        {
          DateTimeProperties dateTimeProperty = dateTimeEdit.DateTimeProperties[index];
          if (dateTimeProperty.StartPosition <= dateTimeEdit.SelectionStart && dateTimeEdit.SelectionStart <= dateTimeProperty.StartPosition + dateTimeProperty.Lenghth - 1)
          {
            if (dateTimeProperty.Type == DateTimeType.Hour24 || dateTimeProperty.Type == DateTimeType.Hour12)
            {
              DateTime dateTime1 = dateTimeEdit.mValue.Value;
              dateTimeEdit.mTextInputpartended = false;
              int result;
              int.TryParse("00", out result);
              int num = result - dateTime1.Hour;
              DateTime dateTime2 = dateTime1.AddHours((double) num);
              dateTimeEdit.mValueChanged = false;
              dateTimeEdit.DateTime = new DateTime?(dateTime2);
              dateTimeEdit.mValueChanged = true;
              return false;
            }
            if (dateTimeProperty.Type == DateTimeType.Minutes)
            {
              DateTime dateTime3 = dateTimeEdit.mValue.Value;
              dateTimeEdit.mTextInputpartended = false;
              int result;
              int.TryParse("00", out result);
              int num = result - dateTime3.Minute;
              DateTime dateTime4 = dateTime3.AddMinutes((double) num);
              dateTimeEdit.mValueChanged = false;
              dateTimeEdit.DateTime = new DateTime?(dateTime4);
              dateTimeEdit.mValueChanged = true;
              return false;
            }
            bool? isReadOnly = dateTimeProperty.IsReadOnly;
            if ((isReadOnly.GetValueOrDefault() ? 0 : (isReadOnly.HasValue ? 1 : 0)) != 0 && dateTimeProperty.Type == DateTimeType.year || dateTimeProperty.Type == DateTimeType.Month || dateTimeProperty.Type == DateTimeType.monthname || dateTimeProperty.Type == DateTimeType.Day)
              return false;
          }
        }
        if (dateTimeEdit.SelectWholeContent && dateTimeEdit.SelectionLength == dateTimeEdit.Text.Length && !dateTimeEdit.IsReadOnly)
        {
          dateTimeEdit.Text = string.Empty;
          dateTimeEdit.DateTime = new DateTime?();
          break;
        }
        break;
      case Key.Escape:
        dateTimeEdit.IsDropDownOpen = false;
        break;
      case Key.Left:
        return dateTimeEdit.FlowDirection == FlowDirection.RightToLeft ? this.HandleRightKey(dateTimeEdit) : this.HandleLeftKey(dateTimeEdit);
      case Key.Up:
        return this.HandleUpKey(dateTimeEdit);
      case Key.Right:
        return dateTimeEdit.FlowDirection == FlowDirection.RightToLeft ? this.HandleLeftKey(dateTimeEdit) : this.HandleRightKey(dateTimeEdit);
      case Key.Down:
        return this.HandleDownKey(dateTimeEdit);
      case Key.Delete:
        if (!dateTimeEdit.EnableDeleteKey)
          return true;
        for (int index = 0; index < dateTimeEdit.DateTimeProperties.Count; ++index)
        {
          DateTimeProperties dateTimeProperty = dateTimeEdit.DateTimeProperties[index];
          if (dateTimeProperty.StartPosition <= dateTimeEdit.SelectionStart && dateTimeEdit.SelectionStart <= dateTimeProperty.StartPosition + dateTimeProperty.Lenghth - 1)
          {
            if (dateTimeProperty.Type == DateTimeType.Hour24 || dateTimeProperty.Type == DateTimeType.Hour12)
            {
              DateTime dateTime5 = dateTimeEdit.mValue.Value;
              dateTimeEdit.mTextInputpartended = false;
              int result;
              int.TryParse("00", out result);
              int num = result - dateTime5.Hour;
              DateTime dateTime6 = dateTime5.AddHours((double) num);
              dateTimeEdit.mValueChanged = false;
              dateTimeEdit.DateTime = new DateTime?(dateTime6);
              dateTimeEdit.mValueChanged = true;
              return false;
            }
            if (dateTimeProperty.Type == DateTimeType.Minutes)
            {
              DateTime dateTime7 = dateTimeEdit.mValue.Value;
              dateTimeEdit.mTextInputpartended = false;
              int result;
              int.TryParse("00", out result);
              int num = result - dateTime7.Minute;
              DateTime dateTime8 = dateTime7.AddMinutes((double) num);
              dateTimeEdit.mValueChanged = false;
              dateTimeEdit.DateTime = new DateTime?(dateTime8);
              dateTimeEdit.mValueChanged = true;
              return false;
            }
            bool? isReadOnly = dateTimeProperty.IsReadOnly;
            if ((isReadOnly.GetValueOrDefault() ? 0 : (isReadOnly.HasValue ? 1 : 0)) != 0 && dateTimeProperty.Type == DateTimeType.year || dateTimeProperty.Type == DateTimeType.Month || dateTimeProperty.Type == DateTimeType.monthname || dateTimeProperty.Type == DateTimeType.Day)
              return false;
          }
        }
        if (dateTimeEdit.SelectWholeContent && dateTimeEdit.SelectionLength == dateTimeEdit.Text.Length && !dateTimeEdit.IsReadOnly)
        {
          dateTimeEdit.Text = string.Empty;
          dateTimeEdit.DateTime = new DateTime?();
          break;
        }
        break;
      case Key.A:
        return this.HandleAlphaKeys(dateTimeEdit, Key.A);
      case Key.B:
        return this.HandleAlphaKeys(dateTimeEdit, Key.B);
      case Key.C:
        return this.HandleAlphaKeys(dateTimeEdit, Key.C);
      case Key.D:
        return this.HandleAlphaKeys(dateTimeEdit, Key.D);
      case Key.E:
        return this.HandleAlphaKeys(dateTimeEdit, Key.E);
      case Key.F:
        return this.HandleAlphaKeys(dateTimeEdit, Key.F);
      case Key.G:
        return this.HandleAlphaKeys(dateTimeEdit, Key.G);
      case Key.H:
        return this.HandleAlphaKeys(dateTimeEdit, Key.H);
      case Key.I:
        return this.HandleAlphaKeys(dateTimeEdit, Key.I);
      case Key.J:
        return this.HandleAlphaKeys(dateTimeEdit, Key.J);
      case Key.K:
        return this.HandleAlphaKeys(dateTimeEdit, Key.K);
      case Key.L:
        return this.HandleAlphaKeys(dateTimeEdit, Key.L);
      case Key.M:
        return this.HandleAlphaKeys(dateTimeEdit, Key.M);
      case Key.N:
        return this.HandleAlphaKeys(dateTimeEdit, Key.N);
      case Key.O:
        return this.HandleAlphaKeys(dateTimeEdit, Key.O);
      case Key.P:
        return this.HandleAlphaKeys(dateTimeEdit, Key.P);
      case Key.Q:
        return this.HandleAlphaKeys(dateTimeEdit, Key.Q);
      case Key.R:
        return this.HandleAlphaKeys(dateTimeEdit, Key.R);
      case Key.S:
        return this.HandleAlphaKeys(dateTimeEdit, Key.S);
      case Key.T:
        return this.HandleAlphaKeys(dateTimeEdit, Key.T);
      case Key.U:
        return this.HandleAlphaKeys(dateTimeEdit, Key.U);
      case Key.V:
        return this.HandleAlphaKeys(dateTimeEdit, Key.V);
      case Key.W:
        return this.HandleAlphaKeys(dateTimeEdit, Key.W);
      case Key.X:
        return this.HandleAlphaKeys(dateTimeEdit, Key.X);
      case Key.Y:
        return this.HandleAlphaKeys(dateTimeEdit, Key.Y);
      case Key.Z:
        return this.HandleAlphaKeys(dateTimeEdit, Key.Z);
    }
    return false;
  }

  public bool HandleAlphaKeys(DateTimeEdit dateTimeEdit, Key k)
  {
    if (!dateTimeEdit.EnableAlphaKeyNavigation || dateTimeEdit.IsReadOnly || dateTimeEdit.IsNull)
      return true;
    dateTimeEdit.GetCulture().DateTimeFormat.Clone();
    DateTimeProperties dateTimeProperty = dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection];
    if (dateTimeProperty.Type == DateTimeType.Month || dateTimeProperty.Type == DateTimeType.monthname)
    {
      dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection].MonthName += k.ToString();
      string str1 = dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection].MonthName;
      if (str1.Length > 1 && str1.Substring(str1.Length - 2, 1).Equals(str1.Substring(str1.Length - 1, 1), StringComparison.CurrentCultureIgnoreCase))
      {
        string str2 = str1.Substring(str1.Length - 1, 1);
        str1 = dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection].MonthName = str2;
      }
      DateTime dateTime = dateTimeEdit.mValue.Value;
      int num1 = dateTime.Month;
      string lower = str1.ToLower();
      int key = 1;
      int num2 = num1;
      for (; key <= 12; ++key)
      {
        string str3 = this.monthTable[(object) key].ToString();
        if (num2 == 6 && lower.Equals("j", StringComparison.CurrentCultureIgnoreCase))
        {
          num1 = 7;
          break;
        }
        if (str3.StartsWith(lower))
        {
          num1 = key;
          if (num1 == num2 && lower.Length <= 1)
          {
            switch (num2)
            {
              case 1:
                num1 = 6;
                goto label_20;
              case 3:
                num1 = 5;
                goto label_20;
              case 4:
                num1 = 8;
                goto label_20;
              case 5:
                num1 = 3;
                goto label_20;
              case 6:
                num1 = 7;
                goto label_20;
              case 7:
                num1 = 1;
                goto label_20;
              case 8:
                num1 = 4;
                goto label_20;
              default:
                goto label_20;
            }
          }
          else
            break;
        }
      }
label_20:
      if (key > 12)
      {
        switch (k)
        {
          case Key.A:
            num1 = 4;
            dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection].MonthName = k.ToString();
            break;
          case Key.D:
            num1 = 12;
            dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection].MonthName = k.ToString();
            break;
          case Key.F:
            num1 = 2;
            dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection].MonthName = k.ToString();
            break;
          case Key.J:
            num1 = 1;
            dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection].MonthName = k.ToString();
            break;
          case Key.M:
            num1 = 3;
            dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection].MonthName = k.ToString();
            break;
          case Key.N:
            num1 = 11;
            dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection].MonthName = k.ToString();
            break;
          case Key.O:
            num1 = 10;
            dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection].MonthName = k.ToString();
            break;
          case Key.S:
            num1 = 9;
            dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection].MonthName = k.ToString();
            break;
          default:
            dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection].MonthName = "";
            break;
        }
      }
      if (num1 > 0)
      {
        int months = num1 - dateTime.Month;
        dateTime = dateTime.AddMonths(months);
      }
      dateTimeEdit.mValueChanged = false;
      dateTimeEdit.DateTime = new DateTime?(dateTime);
      dateTimeEdit.mValueChanged = true;
    }
    return true;
  }

  public bool HandleLeftKey(DateTimeEdit dateTimeEdit)
  {
    if (!dateTimeEdit.CanEdit)
      dateTimeEdit.checktext = "";
    for (int index = 0; index < dateTimeEdit.DateTimeProperties.Count; ++index)
    {
      DateTimeProperties dateTimeProperty = dateTimeEdit.DateTimeProperties[index];
      if (dateTimeProperty.StartPosition <= dateTimeEdit.SelectionStart && dateTimeProperty.StartPosition + dateTimeProperty.Lenghth >= dateTimeEdit.SelectionStart)
      {
        bool? isReadOnly = dateTimeProperty.IsReadOnly;
        if ((isReadOnly.GetValueOrDefault() ? 0 : (isReadOnly.HasValue ? 1 : 0)) != 0 || dateTimeProperty.Type == DateTimeType.Dayname)
          dateTimeEdit.mSelectedCollection = index;
        if (dateTimeEdit.SelectionLength == dateTimeEdit.Text.Length)
          this.UpdateSelection(true, dateTimeEdit);
      }
    }
    if (dateTimeEdit.IsEditable && !dateTimeEdit.Text.Equals(dateTimeEdit.NoneDateText))
    {
      int index;
      for (index = dateTimeEdit.mSelectedCollection != -1 ? (dateTimeEdit.mSelectedCollection <= dateTimeEdit.DateTimeProperties.Count - 1 ? dateTimeEdit.mSelectedCollection - 1 : dateTimeEdit.DateTimeProperties.Count - 1) : dateTimeEdit.DateTimeProperties.Count - 1; index - 1 >= 0; --index)
      {
        bool? isReadOnly = dateTimeEdit.DateTimeProperties[index].IsReadOnly;
        if ((isReadOnly.GetValueOrDefault() ? 0 : (isReadOnly.HasValue ? 1 : 0)) != 0)
        {
          dateTimeEdit.mSelectedCollection = index;
          dateTimeEdit.DateTimeProperties[index].KeyPressCount = 0;
          this.NavigateSelectionToNextField(index, dateTimeEdit);
          return true;
        }
      }
      if (index < dateTimeEdit.DateTimeProperties.Count && index >= 0)
      {
        dateTimeEdit.mSelectedCollection = index;
        dateTimeEdit.DateTimeProperties[index].KeyPressCount = 0;
        this.NavigateSelectionToNextField(index, dateTimeEdit);
      }
    }
    return true;
  }

  public bool HandleRightKey(DateTimeEdit dateTimeEdit)
  {
    if (!dateTimeEdit.CanEdit)
      dateTimeEdit.checktext = "";
    if (dateTimeEdit.IsEditable && !dateTimeEdit.Text.Equals(dateTimeEdit.NoneDateText))
    {
      for (int index = 0; index < dateTimeEdit.DateTimeProperties.Count; ++index)
      {
        DateTimeProperties dateTimeProperty = dateTimeEdit.DateTimeProperties[index];
        if (dateTimeProperty.StartPosition <= dateTimeEdit.SelectionStart && dateTimeProperty.StartPosition + dateTimeProperty.Lenghth >= dateTimeEdit.SelectionStart)
        {
          bool? isReadOnly = dateTimeProperty.IsReadOnly;
          if ((isReadOnly.GetValueOrDefault() ? 0 : (isReadOnly.HasValue ? 1 : 0)) != 0 || dateTimeProperty.Type == DateTimeType.Dayname)
            dateTimeEdit.mSelectedCollection = index;
          if (dateTimeEdit.SelectionLength == dateTimeEdit.Text.Length)
            this.UpdateSelection(false, dateTimeEdit);
        }
      }
      for (int index = dateTimeEdit.mSelectedCollection != -1 ? (dateTimeEdit.mSelectedCollection <= dateTimeEdit.DateTimeProperties.Count - 1 ? dateTimeEdit.mSelectedCollection + 1 : 0) : 0; index < dateTimeEdit.DateTimeProperties.Count; ++index)
      {
        bool? isReadOnly = dateTimeEdit.DateTimeProperties[index].IsReadOnly;
        if ((isReadOnly.GetValueOrDefault() ? 0 : (isReadOnly.HasValue ? 1 : 0)) != 0)
        {
          dateTimeEdit.mSelectedCollection = index;
          dateTimeEdit.DateTimeProperties[index].KeyPressCount = 0;
          this.NavigateSelectionToNextField(index, dateTimeEdit);
          return true;
        }
      }
      if (dateTimeEdit.mSelectedCollection + 1 < dateTimeEdit.DateTimeProperties.Count && dateTimeEdit.mSelectedCollection + 1 >= 0)
      {
        ++dateTimeEdit.mSelectedCollection;
        this.NavigateSelectionToNextField(dateTimeEdit.mSelectedCollection, dateTimeEdit);
      }
    }
    return true;
  }

  private void NavigateSelectionToNextField(int index, DateTimeEdit dateTimeEdit)
  {
    if (dateTimeEdit.DateTimeProperties[index].Type == DateTimeType.Dayname || dateTimeEdit.DateTimeProperties[index].Pattern == null)
      return;
    dateTimeEdit.Select(dateTimeEdit.DateTimeProperties[index].StartPosition, dateTimeEdit.DateTimeProperties[index].Lenghth);
  }

  private void UpdateSelection(bool isUpdateSelection, DateTimeEdit dateTimeEdit)
  {
    if (dateTimeEdit.CanEdit || string.IsNullOrEmpty(dateTimeEdit.Text) || dateTimeEdit.mSelectedCollection < 0)
      return;
    int selectedCollection = dateTimeEdit.mSelectedCollection;
    DateTimeProperties dateTimeProperty = dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection];
    while ((dateTimeProperty.Type == DateTimeType.Dayname || dateTimeProperty.Pattern == null) && selectedCollection < dateTimeEdit.DateTimeProperties.Count - 1)
    {
      dateTimeProperty = dateTimeEdit.DateTimeProperties[++selectedCollection];
      dateTimeEdit.mSelectedCollection = selectedCollection;
    }
    if (!isUpdateSelection)
      return;
    dateTimeEdit.Select(dateTimeProperty.StartPosition, dateTimeProperty.Lenghth);
  }

  private DateTimeProperties GetDateTimeEditProperty(DateTimeEdit dateTimeEdit)
  {
    for (int index = 0; index < dateTimeEdit.DateTimeProperties.Count; ++index)
    {
      DateTimeProperties dateTimeProperty = dateTimeEdit.DateTimeProperties[index];
      if (dateTimeProperty.StartPosition <= dateTimeEdit.SelectionStart && dateTimeEdit.SelectionStart <= dateTimeProperty.StartPosition + dateTimeProperty.Lenghth)
      {
        bool? isReadOnly = dateTimeProperty.IsReadOnly;
        if ((isReadOnly.GetValueOrDefault() ? 0 : (isReadOnly.HasValue ? 1 : 0)) != 0)
        {
          dateTimeEdit.selectionChanged = false;
          dateTimeEdit.Select(dateTimeProperty.StartPosition, dateTimeProperty.Lenghth);
          if (index >= 0)
            dateTimeEdit.mSelectedCollection = index;
        }
        return dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection];
      }
    }
    return (DateTimeProperties) null;
  }

  public bool HandleUpKey(DateTimeEdit dateTimeEdit)
  {
    if (dateTimeEdit.IsReadOnly || dateTimeEdit.Text.Equals(dateTimeEdit.NoneDateText) || dateTimeEdit.IsNull)
      return true;
    dateTimeEdit.GetCulture().DateTimeFormat.Clone();
    if (dateTimeEdit.mSelectedCollection < 0 || dateTimeEdit.mSelectedCollection > dateTimeEdit.DateTimeProperties.Count)
      return true;
    if (dateTimeEdit.CanEdit && dateTimeEdit.Text.ToString() != "")
      dateTimeEdit.DateTime = new DateTime?(dateTimeEdit.GetValidDateTime());
    if (!string.IsNullOrEmpty(dateTimeEdit.SelectedText))
    {
      for (int index = 0; index < dateTimeEdit.DateTimeProperties.Count; ++index)
      {
        DateTimeProperties dateTimeProperty = dateTimeEdit.DateTimeProperties[index];
        if (dateTimeProperty.StartPosition <= dateTimeEdit.SelectionStart && dateTimeEdit.SelectionStart <= dateTimeProperty.StartPosition + dateTimeProperty.Lenghth && dateTimeEdit.mSelectedCollection != index)
          dateTimeEdit.mSelectedCollection = index;
      }
    }
    DateTimeProperties dateTimeProperties = this.GetDateTimeEditProperty(dateTimeEdit);
    if (dateTimeEdit.DefaultDatePart != DateParts.None)
    {
      if (!dateTimeEdit.mtextboxclicked)
      {
        for (int index = 0; index < dateTimeEdit.DateTimeProperties.Count; ++index)
        {
          switch (dateTimeEdit.DateTimeProperties[index].Type)
          {
            case DateTimeType.Day:
              if (dateTimeEdit.DefaultDatePart == DateParts.Day)
              {
                dateTimeProperties = dateTimeEdit.DateTimeProperties[index];
                break;
              }
              break;
            case DateTimeType.Month:
              if (dateTimeEdit.DefaultDatePart == DateParts.Month)
              {
                dateTimeProperties = dateTimeEdit.DateTimeProperties[index];
                break;
              }
              break;
            case DateTimeType.year:
              if (dateTimeEdit.DefaultDatePart == DateParts.Year)
              {
                dateTimeProperties = dateTimeEdit.DateTimeProperties[index];
                break;
              }
              break;
          }
        }
      }
      else
      {
        switch (dateTimeProperties.Type)
        {
          case DateTimeType.Day:
            dateTimeEdit.DefaultDatePart = DateParts.Day;
            break;
          case DateTimeType.Month:
            dateTimeEdit.DefaultDatePart = DateParts.Month;
            break;
          case DateTimeType.year:
            dateTimeEdit.DefaultDatePart = DateParts.Year;
            break;
        }
      }
    }
    if (dateTimeProperties == null)
    {
      if (dateTimeEdit.mSelectedCollection >= 0 && dateTimeEdit.mSelectedCollection <= dateTimeEdit.DateTimeProperties.Count && dateTimeEdit.IsEditable)
      {
        dateTimeEdit.SelectionStart = dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection].StartPosition;
        dateTimeEdit.SelectionLength = dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection].Lenghth;
      }
    }
    else
    {
      bool? isReadOnly1 = dateTimeProperties.IsReadOnly;
      if ((!isReadOnly1.GetValueOrDefault() ? 0 : (isReadOnly1.HasValue ? 1 : 0)) == 0)
      {
        bool? isReadOnly2 = dateTimeProperties.IsReadOnly;
        if ((isReadOnly2.GetValueOrDefault() ? 0 : (isReadOnly2.HasValue ? 1 : 0)) == 0)
          goto label_84;
      }
      bool? isReadOnly3 = dateTimeProperties.IsReadOnly;
      if ((isReadOnly3.GetValueOrDefault() ? 0 : (isReadOnly3.HasValue ? 1 : 0)) != 0 && dateTimeProperties.Type == DateTimeType.year)
      {
        DateTime date = dateTimeEdit.mValue.Value;
        try
        {
          if (!dateTimeEdit.CanEdit && date.Year == dateTimeEdit.MaxDateTime.Year)
          {
            date = this.ValidateMinMaxDateTime(false, date, dateTimeEdit);
          }
          else
          {
            date = date.AddYears(1);
            if (!dateTimeEdit.CanEdit)
            {
              if (date.Year == dateTimeEdit.MaxDateTime.Year)
                date = this.ValidateMinMaxDateTime(true, date, dateTimeEdit);
            }
          }
        }
        catch
        {
        }
        dateTimeEdit.mTextInputpartended = false;
        dateTimeEdit.mValueChanged = false;
        DateTime dateTime = dateTimeEdit.IsBlackoutDate(Key.Up, date);
        dateTimeEdit.DateTime = new DateTime?(dateTime);
        dateTimeEdit.mValueChanged = true;
        return true;
      }
      if (dateTimeProperties.Type == DateTimeType.Minutes)
      {
        DateTime date = dateTimeEdit.mValue.Value;
        try
        {
          date = date.AddMinutes(1.0);
        }
        catch
        {
        }
        dateTimeEdit.mTextInputpartended = false;
        dateTimeEdit.mValueChanged = false;
        DateTime dateTime = dateTimeEdit.IsBlackoutDate(Key.Up, date);
        dateTimeEdit.DateTime = new DateTime?(dateTime);
        dateTimeEdit.mValueChanged = true;
        return true;
      }
      if (dateTimeProperties.Type == DateTimeType.Second)
      {
        DateTime date = dateTimeEdit.mValue.Value;
        try
        {
          date = date.AddSeconds(1.0);
        }
        catch
        {
        }
        dateTimeEdit.mTextInputpartended = false;
        dateTimeEdit.mValueChanged = false;
        DateTime dateTime = dateTimeEdit.IsBlackoutDate(Key.Up, date);
        dateTimeEdit.DateTime = new DateTime?(dateTime);
        dateTimeEdit.mValueChanged = true;
        return true;
      }
      if (dateTimeProperties.Type == DateTimeType.Hour24 || dateTimeProperties.Type == DateTimeType.Hour12)
      {
        DateTime date = dateTimeEdit.mValue.Value;
        try
        {
          date = date.AddHours(1.0);
        }
        catch
        {
        }
        dateTimeEdit.mTextInputpartended = false;
        dateTimeEdit.mValueChanged = false;
        DateTime dateTime = dateTimeEdit.IsBlackoutDate(Key.Up, date);
        dateTimeEdit.DateTime = new DateTime?(dateTime);
        dateTimeEdit.mValueChanged = true;
        return true;
      }
      if (dateTimeProperties.Type == DateTimeType.Fraction)
      {
        DateTime date = dateTimeEdit.mValue.Value;
        try
        {
          date = date.AddMilliseconds(1.0);
        }
        catch
        {
        }
        dateTimeEdit.mTextInputpartended = false;
        dateTimeEdit.mValueChanged = false;
        DateTime dateTime = dateTimeEdit.IsBlackoutDate(Key.Up, date);
        dateTimeEdit.DateTime = new DateTime?(dateTime);
        dateTimeEdit.mValueChanged = true;
        return true;
      }
      if (dateTimeProperties.Type == DateTimeType.Month || dateTimeProperties.Type == DateTimeType.monthname)
      {
        if (dateTimeEdit.mValue.HasValue)
        {
          DateTime date = dateTimeEdit.mValue.Value;
          try
          {
            date = date.AddMonths(1);
            if (!dateTimeEdit.CanEdit)
            {
              if (date > dateTimeEdit.MaxDateTime)
                date = dateTimeEdit.MinDateTime;
            }
          }
          catch
          {
          }
          dateTimeEdit.mTextInputpartended = false;
          dateTimeEdit.mValueChanged = false;
          DateTime dateTime = dateTimeEdit.IsBlackoutDate(Key.Up, date);
          dateTimeEdit.DateTime = new DateTime?(dateTime);
          dateTimeEdit.mValueChanged = true;
          return true;
        }
      }
      else
      {
        if (dateTimeProperties.Type == DateTimeType.Day || dateTimeProperties.Type == DateTimeType.Dayname)
        {
          DateTime date = dateTimeEdit.GetValidDateTime();
          try
          {
            if (dateTimeEdit.mValue.HasValue)
              date = date.AddDays(1.0);
            if (!dateTimeEdit.CanEdit)
            {
              if (date > dateTimeEdit.MaxDateTime)
                date = dateTimeEdit.MinDateTime;
            }
          }
          catch
          {
          }
          dateTimeEdit.mTextInputpartended = false;
          dateTimeEdit.mValueChanged = false;
          DateTime dateTime = dateTimeEdit.IsBlackoutDate(Key.Up, date);
          dateTimeEdit.DateTime = new DateTime?(dateTime);
          dateTimeEdit.mValueChanged = true;
          return true;
        }
        if (dateTimeProperties.Type == DateTimeType.designator)
        {
          DateTime date = dateTimeEdit.mValue.Value;
          try
          {
            date = date.AddHours(12.0);
          }
          catch
          {
          }
          dateTimeEdit.mTextInputpartended = false;
          dateTimeEdit.mValueChanged = false;
          DateTime dateTime = dateTimeEdit.IsBlackoutDate(Key.Up, date);
          dateTimeEdit.DateTime = new DateTime?(dateTime);
          dateTimeEdit.mValueChanged = true;
          return true;
        }
      }
    }
label_84:
    return true;
  }

  internal DateTime ValidateMinMaxDateTime(
    bool IsMaxDateValidation,
    DateTime date,
    DateTimeEdit dateTimeEdit)
  {
    date = !IsMaxDateValidation && new DateTime(dateTimeEdit.MinDateTime.Year, date.Month, date.Day) < dateTimeEdit.MinDateTime || IsMaxDateValidation && new DateTime(dateTimeEdit.MaxDateTime.Year, date.Month, date.Day) > dateTimeEdit.MaxDateTime ? (!IsMaxDateValidation && new DateTime(dateTimeEdit.MinDateTime.Year, dateTimeEdit.MinDateTime.Month, date.Day) < dateTimeEdit.MinDateTime || IsMaxDateValidation && new DateTime(dateTimeEdit.MaxDateTime.Year, dateTimeEdit.MaxDateTime.Month, date.Day) > dateTimeEdit.MaxDateTime ? (!IsMaxDateValidation ? dateTimeEdit.MinDateTime : dateTimeEdit.MaxDateTime) : (!IsMaxDateValidation ? new DateTime(dateTimeEdit.MinDateTime.Year, dateTimeEdit.MinDateTime.Month, date.Day) : new DateTime(dateTimeEdit.MaxDateTime.Year, dateTimeEdit.MaxDateTime.Month, date.Day))) : (!IsMaxDateValidation ? new DateTime(dateTimeEdit.MinDateTime.Year, date.Month, date.Day) : new DateTime(dateTimeEdit.MaxDateTime.Year, date.Month, date.Day));
    return date;
  }

  public bool HandleDownKey(DateTimeEdit dateTimeEdit)
  {
    if (dateTimeEdit.IsReadOnly || dateTimeEdit.Text.Equals(dateTimeEdit.NoneDateText) || dateTimeEdit.IsNull)
      return true;
    dateTimeEdit.GetCulture().DateTimeFormat.Clone();
    if (dateTimeEdit.mSelectedCollection < 0 || dateTimeEdit.mSelectedCollection > dateTimeEdit.DateTimeProperties.Count)
      return true;
    if (dateTimeEdit.CanEdit)
      dateTimeEdit.DateTime = new DateTime?(dateTimeEdit.GetValidDateTime());
    if (!string.IsNullOrEmpty(dateTimeEdit.SelectedText))
    {
      for (int index = 0; index < dateTimeEdit.DateTimeProperties.Count; ++index)
      {
        DateTimeProperties dateTimeProperty = dateTimeEdit.DateTimeProperties[index];
        if (dateTimeProperty.StartPosition <= dateTimeEdit.SelectionStart && dateTimeEdit.SelectionStart <= dateTimeProperty.StartPosition + dateTimeProperty.Lenghth && dateTimeEdit.mSelectedCollection != index)
          dateTimeEdit.mSelectedCollection = index;
      }
    }
    DateTimeProperties dateTimeProperties = this.GetDateTimeEditProperty(dateTimeEdit);
    if (dateTimeEdit.DefaultDatePart != DateParts.None)
    {
      if (!dateTimeEdit.mtextboxclicked)
      {
        for (int index = 0; index < dateTimeEdit.DateTimeProperties.Count; ++index)
        {
          switch (dateTimeEdit.DateTimeProperties[index].Type)
          {
            case DateTimeType.Day:
              if (dateTimeEdit.DefaultDatePart == DateParts.Day)
              {
                dateTimeProperties = dateTimeEdit.DateTimeProperties[index];
                break;
              }
              break;
            case DateTimeType.Month:
              if (dateTimeEdit.DefaultDatePart == DateParts.Month)
              {
                dateTimeProperties = dateTimeEdit.DateTimeProperties[index];
                break;
              }
              break;
            case DateTimeType.year:
              if (dateTimeEdit.DefaultDatePart == DateParts.Year)
              {
                dateTimeProperties = dateTimeEdit.DateTimeProperties[index];
                break;
              }
              break;
          }
        }
      }
      else
      {
        switch (dateTimeProperties.Type)
        {
          case DateTimeType.Day:
            dateTimeEdit.DefaultDatePart = DateParts.Day;
            break;
          case DateTimeType.Month:
            dateTimeEdit.DefaultDatePart = DateParts.Month;
            break;
          case DateTimeType.year:
            dateTimeEdit.DefaultDatePart = DateParts.Year;
            break;
        }
      }
    }
    if (dateTimeProperties == null)
    {
      if (dateTimeEdit.mSelectedCollection >= 0 && dateTimeEdit.mSelectedCollection <= dateTimeEdit.DateTimeProperties.Count && dateTimeEdit.IsEditable)
      {
        dateTimeEdit.SelectionStart = dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection].StartPosition;
        dateTimeEdit.SelectionLength = dateTimeEdit.DateTimeProperties[dateTimeEdit.mSelectedCollection].Lenghth;
      }
    }
    else
    {
      bool? isReadOnly1 = dateTimeProperties.IsReadOnly;
      if ((!isReadOnly1.GetValueOrDefault() ? 0 : (isReadOnly1.HasValue ? 1 : 0)) == 0)
      {
        bool? isReadOnly2 = dateTimeProperties.IsReadOnly;
        if ((isReadOnly2.GetValueOrDefault() ? 0 : (isReadOnly2.HasValue ? 1 : 0)) == 0)
          goto label_81;
      }
      bool? isReadOnly3 = dateTimeProperties.IsReadOnly;
      if ((isReadOnly3.GetValueOrDefault() ? 0 : (isReadOnly3.HasValue ? 1 : 0)) != 0 && dateTimeProperties.Type == DateTimeType.year)
      {
        DateTime date = dateTimeEdit.mValue.Value;
        try
        {
          if (!dateTimeEdit.CanEdit && date.Year == dateTimeEdit.MinDateTime.Year)
          {
            date = this.ValidateMinMaxDateTime(true, date, dateTimeEdit);
          }
          else
          {
            date = date.AddYears(-1);
            if (!dateTimeEdit.CanEdit)
            {
              if (date.Year == dateTimeEdit.MinDateTime.Year)
                date = this.ValidateMinMaxDateTime(false, date, dateTimeEdit);
            }
          }
        }
        catch
        {
        }
        dateTimeEdit.mTextInputpartended = false;
        dateTimeEdit.mValueChanged = false;
        DateTime dateTime = dateTimeEdit.IsBlackoutDate(Key.Down, date);
        dateTimeEdit.DateTime = new DateTime?(dateTime);
        dateTimeEdit.mValueChanged = true;
        return true;
      }
      if (dateTimeProperties.Type == DateTimeType.Minutes)
      {
        DateTime date = dateTimeEdit.mValue.Value;
        try
        {
          date = date.AddMinutes(-1.0);
        }
        catch
        {
        }
        dateTimeEdit.mTextInputpartended = false;
        dateTimeEdit.mValueChanged = false;
        DateTime dateTime = dateTimeEdit.IsBlackoutDate(Key.Down, date);
        dateTimeEdit.DateTime = new DateTime?(dateTime);
        dateTimeEdit.mValueChanged = true;
        return true;
      }
      if (dateTimeProperties.Type == DateTimeType.Second)
      {
        DateTime date = dateTimeEdit.mValue.Value;
        try
        {
          date = date.AddSeconds(-1.0);
        }
        catch
        {
        }
        dateTimeEdit.mTextInputpartended = false;
        dateTimeEdit.mValueChanged = false;
        DateTime dateTime = dateTimeEdit.IsBlackoutDate(Key.Down, date);
        dateTimeEdit.DateTime = new DateTime?(dateTime);
        dateTimeEdit.mValueChanged = true;
        return true;
      }
      if (dateTimeProperties.Type == DateTimeType.Hour24 || dateTimeProperties.Type == DateTimeType.Hour12)
      {
        DateTime date = dateTimeEdit.mValue.Value;
        try
        {
          date = date.AddHours(-1.0);
        }
        catch
        {
        }
        dateTimeEdit.mTextInputpartended = false;
        dateTimeEdit.mValueChanged = false;
        DateTime dateTime = dateTimeEdit.IsBlackoutDate(Key.Down, date);
        dateTimeEdit.DateTime = new DateTime?(dateTime);
        dateTimeEdit.mValueChanged = true;
        return true;
      }
      if (dateTimeProperties.Type == DateTimeType.Fraction)
      {
        DateTime date = dateTimeEdit.mValue.Value;
        try
        {
          date = date.AddMilliseconds(-1.0);
        }
        catch
        {
        }
        dateTimeEdit.mTextInputpartended = false;
        dateTimeEdit.mValueChanged = false;
        DateTime dateTime = dateTimeEdit.IsBlackoutDate(Key.Down, date);
        dateTimeEdit.DateTime = new DateTime?(dateTime);
        dateTimeEdit.mValueChanged = true;
        return true;
      }
      if (dateTimeProperties.Type == DateTimeType.Month || dateTimeProperties.Type == DateTimeType.monthname)
      {
        DateTime date = dateTimeEdit.GetValidDateTime();
        try
        {
          date = date.AddMonths(-1);
          if (!dateTimeEdit.CanEdit)
          {
            if (date < dateTimeEdit.MinDateTime)
              date = dateTimeEdit.MaxDateTime;
          }
        }
        catch
        {
        }
        dateTimeEdit.mTextInputpartended = false;
        dateTimeEdit.mValueChanged = false;
        DateTime dateTime = dateTimeEdit.IsBlackoutDate(Key.Down, date);
        dateTimeEdit.DateTime = new DateTime?(dateTime);
        dateTimeEdit.mValueChanged = true;
        return true;
      }
      if (dateTimeProperties.Type == DateTimeType.Day || dateTimeProperties.Type == DateTimeType.Dayname)
      {
        DateTime date = dateTimeEdit.GetValidDateTime();
        try
        {
          date = date.AddDays(-1.0);
          if (!dateTimeEdit.CanEdit)
          {
            if (date < dateTimeEdit.MinDateTime)
              date = dateTimeEdit.MaxDateTime;
          }
        }
        catch
        {
        }
        dateTimeEdit.mTextInputpartended = false;
        dateTimeEdit.mValueChanged = false;
        DateTime dateTime = dateTimeEdit.IsBlackoutDate(Key.Down, date);
        dateTimeEdit.DateTime = new DateTime?(dateTime);
        dateTimeEdit.mValueChanged = true;
        return true;
      }
      if (dateTimeProperties.Type == DateTimeType.designator)
      {
        DateTime date = dateTimeEdit.mValue.Value;
        try
        {
          date = date.AddHours(-12.0);
        }
        catch
        {
        }
        dateTimeEdit.mTextInputpartended = false;
        dateTimeEdit.mValueChanged = false;
        DateTime dateTime = dateTimeEdit.IsBlackoutDate(Key.Down, date);
        dateTimeEdit.DateTime = new DateTime?(dateTime);
        dateTimeEdit.mValueChanged = true;
        return true;
      }
    }
label_81:
    return true;
  }
}
