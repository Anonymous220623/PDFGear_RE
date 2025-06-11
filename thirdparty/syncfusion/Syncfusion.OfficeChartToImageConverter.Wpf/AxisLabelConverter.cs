// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChartToImageConverter.AxisLabelConverter
// Assembly: Syncfusion.OfficeChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 82053128-0A33-4E43-8DD1-E8016B1463BC
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChartToImageConverter.Wpf.dll

using Syncfusion.UI.Xaml.Charts;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.OfficeChartToImageConverter;

internal class AxisLabelConverter : IValueConverter
{
  internal string NumberFormat = "General";
  internal byte AxisTypeInByte;
  private Regex IntervalRegex = new Regex("(?<start>\\[|\\(){1}(?<first>[-0-9]+(\\.[0-9]+)*),(?<second>[-0-9]+(\\.[0-9]+)*)(?<end>\\]|\\)){1}");
  private Regex FlowBinRegex = new Regex("(?<first>[-0-9]+(\\.[0-9]+)*)");

  internal event Func<object, string, string> NumberFormatApplyEvent;

  internal double PreviousLabelValue { get; set; }

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
      return (object) null;
    if (((int) this.AxisTypeInByte & 8) != 0)
      return (object) string.Empty;
    string str;
    if (this.NumberFormatApplyEvent != null && this.NumberFormat != null && (this.NumberFormat.ToLower() == "general" ? (((int) this.AxisTypeInByte & 4) != 0 ? 1 : 0) : 1) != 0)
    {
      if (((int) this.AxisTypeInByte & 2) != 0)
      {
        value = (object) ((value as ChartAxisLabel).Position / 100.0);
        str = this.NumberFormatApplyEvent(value, this.NumberFormat);
      }
      else if (((int) this.AxisTypeInByte & 1) != 0)
      {
        string input = (value as ChartAxisLabel).LabelContent.ToString();
        Match match1 = this.IntervalRegex.Match(input);
        if (match1 != null && match1.Success)
        {
          double result1;
          double result2;
          if (double.TryParse(match1.Groups["first"].Value, out result1) && double.TryParse(match1.Groups["second"].Value, out result2))
            return (object) $"{match1.Groups["start"].Value}{this.NumberFormatApplyEvent((object) result1, this.NumberFormat)},{this.NumberFormatApplyEvent((object) result2, this.NumberFormat)}{match1.Groups["end"].Value}";
        }
        else
        {
          Match match2 = this.FlowBinRegex.Match(input);
          double result;
          if (match2 != null && match2.Success && double.TryParse(match2.Groups["first"].Value, out result))
            return (object) input.Replace(match2.Groups["first"].Value, this.NumberFormatApplyEvent((object) result, this.NumberFormat));
        }
        str = input;
      }
      else
        str = this.NumberFormatApplyEvent((object) (value as ChartAxisLabel).Position, this.NumberFormat);
    }
    else if (((int) this.AxisTypeInByte & 1) != 0 || ((int) this.AxisTypeInByte & 4) != 0)
    {
      str = (value as ChartAxisLabel).LabelContent.ToString();
    }
    else
    {
      ChartAxisLabel chartAxisLabel = value as ChartAxisLabel;
      str = chartAxisLabel.Position.ToString();
      if (chartAxisLabel.Position != 0.0 && str.Contains("E") && chartAxisLabel.LabelContent != null && chartAxisLabel.LabelContent.Equals((object) "0") && !(this.PreviousLabelValue - chartAxisLabel.Position).ToString().Contains("E"))
        str = "0";
      this.PreviousLabelValue = chartAxisLabel.Position;
    }
    return (object) str;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value;
  }
}
