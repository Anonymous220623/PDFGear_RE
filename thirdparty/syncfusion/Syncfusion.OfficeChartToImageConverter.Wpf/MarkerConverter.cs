// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChartToImageConverter.MarkerConverter
// Assembly: Syncfusion.OfficeChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 82053128-0A33-4E43-8DD1-E8016B1463BC
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChartToImageConverter.Wpf.dll

using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.OfficeChartToImageConverter;

internal class MarkerConverter : IValueConverter
{
  private int m_strokeIndex;
  private int m_fillIndex;
  private int m_thicknessIndex;
  private int m_pathIndex;

  internal Dictionary<int, MarkerSetting> MarkerSettings { get; set; }

  internal MarkerSetting CommonMarkerSetting { get; set; }

  internal List<int> AverageMarkerIndexes { get; set; }

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value is ChartAdornment)
    {
      if (targetType == typeof (Geometry))
      {
        if (this.AverageMarkerIndexes != null && this.AverageMarkerIndexes.Contains(this.m_pathIndex))
        {
          ++this.m_pathIndex;
          return (object) new RectangleGeometry(new Rect(0.0, 0.0, 0.0, 0.0));
        }
        if (this.MarkerSettings.ContainsKey(this.m_pathIndex))
        {
          Geometry markerSymbolGeometry = ChartCommon.GetMarkerSymbolGeometry(this.MarkerSettings[this.m_pathIndex].MarkerTypeInInt, this.MarkerSettings[this.m_pathIndex].MarkerSize);
          ++this.m_pathIndex;
          return (object) markerSymbolGeometry;
        }
        ++this.m_pathIndex;
        return (object) ChartCommon.GetMarkerSymbolGeometry(this.CommonMarkerSetting.MarkerTypeInInt, this.CommonMarkerSetting.MarkerSize);
      }
      if (targetType == typeof (Brush))
      {
        if ((bool) parameter)
        {
          if (this.AverageMarkerIndexes != null && this.AverageMarkerIndexes.Contains(this.m_fillIndex))
          {
            ++this.m_fillIndex;
            return (object) new SolidColorBrush(Color.FromArgb((byte) 0, (byte) 0, (byte) 0, (byte) 0));
          }
          if (this.MarkerSettings.ContainsKey(this.m_fillIndex))
          {
            Brush fillBrush = this.MarkerSettings[this.m_fillIndex].FillBrush;
            ++this.m_fillIndex;
            return (object) fillBrush;
          }
          ++this.m_fillIndex;
          return (object) this.CommonMarkerSetting.FillBrush;
        }
        if (this.AverageMarkerIndexes != null && this.AverageMarkerIndexes.Contains(this.m_strokeIndex))
        {
          ++this.m_strokeIndex;
          return (object) new SolidColorBrush(Color.FromArgb((byte) 0, (byte) 0, (byte) 0, (byte) 0));
        }
        if (this.MarkerSettings.ContainsKey(this.m_strokeIndex))
        {
          Brush borderBrush = this.MarkerSettings[this.m_strokeIndex].BorderBrush;
          ++this.m_strokeIndex;
          return (object) borderBrush;
        }
        ++this.m_strokeIndex;
        return (object) this.CommonMarkerSetting.BorderBrush;
      }
      if (targetType == typeof (double))
      {
        if (this.AverageMarkerIndexes != null && this.AverageMarkerIndexes.Contains(this.m_strokeIndex))
        {
          ++this.m_thicknessIndex;
          return (object) 0;
        }
        if (this.MarkerSettings.ContainsKey(this.m_thicknessIndex))
        {
          double num = this.MarkerSettings[this.m_thicknessIndex].BorderThickness * 1.25;
          ++this.m_thicknessIndex;
          return (object) num;
        }
        ++this.m_thicknessIndex;
        return (object) (this.CommonMarkerSetting.BorderThickness * 1.25);
      }
    }
    return (object) null;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value;
  }
}
