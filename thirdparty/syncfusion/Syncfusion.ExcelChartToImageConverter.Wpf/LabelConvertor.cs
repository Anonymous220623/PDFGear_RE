// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelChartToImageConverter.LabelConvertor
// Assembly: Syncfusion.ExcelChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 8A92A829-7139-4C93-8632-144655877EB3
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelChartToImageConverter.Wpf.dll

using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.ExcelChartToImageConverter;

internal class LabelConvertor : IValueConverter
{
  private int m_index;

  internal event Func<object, string, string> NumberFormatApplyEvent;

  internal Dictionary<int, object> ValuesFromCells { get; set; }

  internal string SeriesName { get; set; }

  internal object[] CategoryNames { get; set; }

  internal double Percentage { get; set; }

  internal List<int> BlankIndexes { get; set; }

  internal Dictionary<int, DataLabelSetting> DataLabelSettings { get; set; }

  internal DataLabelSetting CommonDataLabelSetting { get; set; }

  internal int Index
  {
    get => this.m_index;
    set => this.m_index = value;
  }

  internal LabelConvertor()
  {
    this.DataLabelSettings = new Dictionary<int, DataLabelSetting>();
    this.CommonDataLabelSetting = new DataLabelSetting();
  }

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    string str = (string) null;
    ChartAdornment chartAdornment = value as ChartAdornment;
    if (this.BlankIndexes != null && this.BlankIndexes.Contains(this.m_index))
    {
      ++this.m_index;
      return (object) "";
    }
    if (this.DataLabelSettings.ContainsKey(this.m_index))
    {
      DataLabelSetting dataLabelSetting = this.DataLabelSettings[this.m_index];
      if (!dataLabelSetting.IsDelete)
      {
        object obj = chartAdornment != null ? (object) (chartAdornment.Item as ChartPoint).Value : value;
        if (chartAdornment == null || chartAdornment.Series.ShowEmptyPoints || !(obj is double d) || !double.IsNaN(d))
          str = this.UpdateDataLabelText(dataLabelSetting.CustomText != null ? dataLabelSetting.CustomText : this.LabelSettings(obj, dataLabelSetting), obj, dataLabelSetting);
      }
    }
    else if (chartAdornment != null && chartAdornment.Series is RangeColumnSeries && ((chartAdornment.Item as ChartPoint).Value <= 0.0 || double.IsNaN((chartAdornment.Item as ChartPoint).Value)))
    {
      str = string.Empty;
    }
    else
    {
      object obj = chartAdornment != null ? (object) (chartAdornment.Item as ChartPoint).Value : value;
      if (chartAdornment == null || chartAdornment.Series.ShowEmptyPoints || !(obj is double d) || !double.IsNaN(d))
        str = this.UpdateDataLabelText(this.LabelSettings(obj, this.CommonDataLabelSetting), obj, this.CommonDataLabelSetting);
    }
    this.m_index = this.m_index + 1 == this.CategoryNames.Length ? 0 : this.m_index + 1;
    return (object) str;
  }

  internal string UpdateDataLabelText(string text, object value, DataLabelSetting setting)
  {
    if (setting.IsValuesFromCells && text.Contains("[CELLRANGE]"))
    {
      string newValue = this.UpdateDataLabelValuesFromCells((string) null, setting);
      text = string.IsNullOrEmpty(newValue) ? (string) null : text.Replace("[CELLRANGE]", newValue);
    }
    if (setting.IsSeriesName && text.Contains("[SERIES NAME]"))
    {
      string newValue = this.UpdateDataLabelSeriesName((string) null, setting);
      if (!string.IsNullOrEmpty(newValue))
        text = text.Replace("[SERIES NAME]", newValue);
    }
    if (setting.IsCategoryName && text.Contains("[CATEGORY NAME]"))
    {
      string newValue = this.UpdateDataLabelCategoryName((string) null, setting);
      if (!string.IsNullOrEmpty(newValue))
        text = text.Replace("[CATEGORY NAME]", newValue);
    }
    if (setting.IsCategoryName && text.Contains("[X VALUE]"))
    {
      string newValue = this.UpdateDataLabelCategoryName((string) null, setting);
      if (!string.IsNullOrEmpty(newValue))
        text = text.Replace("[X VALUE]", newValue);
    }
    if (setting.IsValue && text.Contains("[VALUE]"))
    {
      string newValue = this.UpdateDataLabelValue((string) null, value, setting);
      if (!string.IsNullOrEmpty(newValue))
        text = text.Replace("[VALUE]", newValue);
    }
    if (setting.IsValue && text.Contains("[Y VALUE]"))
    {
      string newValue = this.UpdateDataLabelValue((string) null, value, setting);
      if (!string.IsNullOrEmpty(newValue))
        text = text.Replace("[Y VALUE]", newValue);
    }
    if (setting.IsPercentage && text.Contains("[PERCENTAGE]"))
    {
      string newValue = this.UpdateDataLabelPercentange((string) null, value, setting);
      if (!string.IsNullOrEmpty(newValue))
        text = text.Replace("[PERCENTAGE]", newValue);
    }
    return text;
  }

  private string LabelSettings(object value, DataLabelSetting dataLabelSetting)
  {
    string text = (string) null;
    if (dataLabelSetting.IsValuesFromCells && this.ValuesFromCells != null && this.ValuesFromCells.Count > 0)
      text = this.UpdateDataLabelValuesFromCells(text, dataLabelSetting);
    if (dataLabelSetting.IsSeriesName)
      text = this.UpdateDataLabelSeriesName(text, dataLabelSetting);
    if (dataLabelSetting.IsCategoryName)
      text = this.UpdateDataLabelCategoryName(text, dataLabelSetting);
    if (dataLabelSetting.IsValue)
      text = this.UpdateDataLabelValue(text, value, dataLabelSetting);
    if (dataLabelSetting.IsPercentage)
      text = this.UpdateDataLabelPercentange(text, value, dataLabelSetting);
    return text;
  }

  internal string UpdateDataLabelValuesFromCells(string text, DataLabelSetting dataLabelSetting)
  {
    object obj = (object) null;
    text = (string) null;
    if (this.ValuesFromCells != null && this.ValuesFromCells.Count > 0 && this.ValuesFromCells.TryGetValue(this.m_index, out obj))
      text = obj.ToString();
    return text;
  }

  internal string UpdateDataLabelSeriesName(string text, DataLabelSetting dataLabelSetting)
  {
    text += text != null ? dataLabelSetting.Seperator + this.SeriesName : this.SeriesName;
    return text;
  }

  internal string UpdateDataLabelCategoryName(string text, DataLabelSetting dataLabelSetting)
  {
    object obj = this.CategoryNames[this.m_index];
    if (!(this.CategoryNames[this.m_index] is string) && dataLabelSetting.NumberFormat != null && dataLabelSetting.NumberFormat.ToLower() != "general" && !dataLabelSetting.IsPercentage)
      obj = (object) this.NumberFormatApplyEvent(obj, dataLabelSetting.NumberFormat);
    text += (string) (text != null ? (object) (dataLabelSetting.Seperator + obj) : obj);
    return text;
  }

  internal string UpdateDataLabelValue(
    string text,
    object value,
    DataLabelSetting dataLabelSetting)
  {
    string str1 = (string) null;
    if (dataLabelSetting.NumberFormat != null)
    {
      if (dataLabelSetting.NumberFormat.ToLower() != "general")
      {
        if (!dataLabelSetting.IsPercentage || dataLabelSetting.IsSourceLinked)
          value = (object) this.NumberFormatApplyEvent(value, dataLabelSetting.NumberFormat);
      }
      else if (value is double)
      {
        str1 = value.ToString();
        if (str1.Length > 9)
          str1 = Math.Round((double) value, 9).ToString((IFormatProvider) CultureInfo.InvariantCulture).TrimEnd('0');
      }
    }
    string str2 = str1 ?? value.ToString();
    text += text != null ? dataLabelSetting.Seperator + str2 : str2;
    return text;
  }

  internal string UpdateDataLabelPercentange(
    string text,
    object value,
    DataLabelSetting dataLabelSetting)
  {
    double result = 0.0;
    switch (value)
    {
      case string _:
        double.TryParse(value.ToString(), out result);
        break;
      case IConvertible _:
        result = System.Convert.ToDouble(value);
        break;
      default:
        result = 0.0;
        break;
    }
    double num = result / this.Percentage;
    if (dataLabelSetting.NumberFormat != null && dataLabelSetting.NumberFormat.ToLower() != "general" && !dataLabelSetting.IsSourceLinked)
    {
      value = (object) this.NumberFormatApplyEvent((object) num, dataLabelSetting.NumberFormat);
      text += (string) (text != null ? (object) (dataLabelSetting.Seperator + value) : value);
    }
    else
    {
      value = (object) Math.Round(num * 100.0, MidpointRounding.AwayFromZero);
      text += text != null ? $"{dataLabelSetting.Seperator}{value}%" : value.ToString() + "%";
    }
    return text;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value;
  }
}
