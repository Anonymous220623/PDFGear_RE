// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChartToImageConverter.DataLabelSetting
// Assembly: Syncfusion.OfficeChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 82053128-0A33-4E43-8DD1-E8016B1463BC
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChartToImageConverter.Wpf.dll

using Syncfusion.OfficeChart.Implementation.Charts;

#nullable disable
namespace Syncfusion.OfficeChartToImageConverter;

internal struct DataLabelSetting
{
  internal bool IsValuesFromCells;
  internal bool IsValue;
  internal bool IsCategoryName;
  internal bool IsSeriesName;
  internal bool IsPercentage;
  internal bool IsDelete;
  internal bool IsSourceLinked;
  internal string Seperator;
  internal string NumberFormat;
  internal string CustomText;

  internal DataLabelSetting(ChartDataLabelsImpl dataLabelsImpl, bool isCircularSeries)
  {
    this.IsValuesFromCells = dataLabelsImpl.IsValueFromCells;
    this.IsValue = dataLabelsImpl.IsValue;
    this.IsCategoryName = dataLabelsImpl.IsCategoryName;
    this.IsSeriesName = dataLabelsImpl.IsSeriesName;
    this.IsPercentage = isCircularSeries && dataLabelsImpl.IsPercentage;
    this.IsDelete = dataLabelsImpl.IsDelete;
    this.Seperator = dataLabelsImpl.Delimiter;
    this.NumberFormat = dataLabelsImpl.NumberFormat;
    this.IsSourceLinked = dataLabelsImpl.IsSourceLinked;
    this.CustomText = dataLabelsImpl.Text;
    if (this.NumberFormat == null)
      this.NumberFormat = "General";
    if (this.Seperator != null)
      return;
    this.Seperator = ", ";
  }
}
