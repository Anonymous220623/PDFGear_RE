// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartGlobalFormatsCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartGlobalFormatsCollection
{
  public static ExcelChartType[] DEF_MABY_COMBINATION_TYPES = new ExcelChartType[16 /*0x10*/]
  {
    ExcelChartType.Scatter_Markers,
    ExcelChartType.Scatter_Line_Markers,
    ExcelChartType.Scatter_Line,
    ExcelChartType.Scatter_SmoothedLine_Markers,
    ExcelChartType.Scatter_SmoothedLine,
    ExcelChartType.Line,
    ExcelChartType.Line_3D,
    ExcelChartType.Line_Markers,
    ExcelChartType.Line_Markers_Stacked,
    ExcelChartType.Line_Markers_Stacked_100,
    ExcelChartType.Line_Stacked,
    ExcelChartType.Line_Stacked_100,
    ExcelChartType.Bubble,
    ExcelChartType.Bubble_3D,
    ExcelChartType.Radar_Markers,
    ExcelChartType.Radar
  };
  public static string[] DEF_MABY_COMBINATION_TYPES_START = new string[4]
  {
    "Scatter",
    "Line",
    "Bubble",
    "Radar"
  };
  private ChartFormatCollection m_primary;
  private ChartFormatCollection m_secondary;

  public ChartGlobalFormatsCollection()
  {
  }

  public ChartGlobalFormatsCollection(
    IApplication application,
    ChartParentAxisImpl primaryParent,
    ChartParentAxisImpl secondaryParent)
  {
    this.m_primary = new ChartFormatCollection(application, (object) primaryParent);
    this.m_secondary = new ChartFormatCollection(application, (object) secondaryParent);
  }

  public void Parse(IList data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
  }

  public ChartFormatCollection PrimaryFormats => this.m_primary;

  public ChartFormatCollection SecondaryFormats => this.m_secondary;

  public void Remove(ChartFormatImpl format)
  {
    int index = !(format == (ChartFormatImpl) null) ? format.DrawingZOrder : throw new ArgumentNullException(nameof (format));
    if (this.m_primary.ContainsIndex(index))
    {
      if (this.m_primary.Count == 1)
      {
        this.ChangeCollections();
        this.m_secondary.Remove(format);
      }
      else
        this.m_primary.Remove(format);
    }
    else
    {
      if (!this.m_secondary.ContainsIndex(index))
        return;
      this.m_secondary.Remove(format);
    }
  }

  public void CreateCollection(IApplication application, object parent, bool bIsPrimary)
  {
    if (bIsPrimary)
      this.m_primary = new ChartFormatCollection(application, parent);
    else
      this.m_secondary = new ChartFormatCollection(application, parent);
  }

  public void ChangeCollections()
  {
    ChartFormatCollection primary = this.m_primary;
    object parent1 = this.m_primary.Parent;
    object parent2 = this.m_secondary.Parent;
    this.m_primary = this.m_secondary;
    this.m_primary.SetParent(parent1);
    this.m_primary.SetParents();
    this.m_secondary = primary;
    this.m_secondary.SetParent(parent2);
    this.m_secondary.SetParents();
  }

  public ChartFormatImpl AddFormat(
    ChartFormatImpl formatToAdd,
    int order,
    int index,
    bool isPrimary)
  {
    ChartFormatCollection currentCollection = this.GetCurrentCollection(isPrimary);
    if (!this.m_primary.ContainsIndex(order) && !this.m_secondary.ContainsIndex(order))
    {
      currentCollection.SetIndex(order, index);
      return formatToAdd;
    }
    for (int index1 = 7; index1 >= order; --index1)
    {
      if (this.m_primary.ContainsIndex(index1))
        this.m_primary.UpdateFormatsOnAdding(index1);
      else if (this.m_secondary.ContainsIndex(index1))
        this.m_secondary.UpdateFormatsOnAdding(index1);
    }
    currentCollection.SetIndex(order, index);
    return formatToAdd;
  }

  public void RemoveFormat(int indexToRemove, int iOrder, bool isPrimary)
  {
    this.GetCurrentCollection(isPrimary);
    if (isPrimary)
      this.m_primary.UpdateIndexesAfterRemove(indexToRemove);
    else
      this.m_secondary.UpdateIndexesAfterRemove(indexToRemove);
    for (int index = iOrder + 1; index < 8; ++index)
    {
      if (this.m_primary.ContainsIndex(index))
        this.m_primary.UpdateFormatsOnRemoving(index);
      else if (this.m_secondary.ContainsIndex(index))
        this.m_secondary.UpdateFormatsOnRemoving(index);
    }
  }

  private ChartFormatCollection GetCurrentCollection(bool isPrimary)
  {
    return !isPrimary ? this.m_secondary : this.m_primary;
  }

  public ChartGlobalFormatsCollection CloneForPrimary(object parent)
  {
    ChartGlobalFormatsCollection formatsCollection = new ChartGlobalFormatsCollection();
    if (this.m_primary != null)
      formatsCollection.m_primary = (ChartFormatCollection) this.m_primary.Clone(parent);
    return formatsCollection;
  }

  public void CloneForSecondary(ChartGlobalFormatsCollection result, object parent)
  {
    if (result == null)
      throw new ArgumentNullException(nameof (result));
    if (this.m_secondary == null)
      return;
    result.m_secondary = (ChartFormatCollection) this.m_secondary.Clone(parent);
  }

  public ExcelChartType DetectChartType(ChartSeriesCollection series)
  {
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    int count1 = this.m_primary.Count;
    int count2 = series.Count;
    ExcelChartType excelChartType = ExcelChartType.Combination_Chart;
    if (count1 == 0)
      excelChartType = ExcelChartType.Column_Clustered;
    else if (count1 > 1)
      excelChartType = ExcelChartType.Combination_Chart;
    else if (this.m_secondary.Count == 0 || this.m_secondary.Count == 1 && this.m_secondary.IsParetoFormat)
      excelChartType = this.DetectTypeForPrimaryCollOnly(series);
    else if (count2 >= 4 && count2 <= 5 && this.SecondaryFormats.ContainsIndex(1))
    {
      ChartFormatImpl secondaryFormat = this.SecondaryFormats[1];
      if (secondaryFormat != (ChartFormatImpl) null && secondaryFormat.IsChartChartLine && secondaryFormat.FormatRecordType == TBIFFRecord.ChartLine && secondaryFormat.HasHighLowLines && (series[0] as ChartSerieImpl).ParentChart.IsStock)
        excelChartType = secondaryFormat.IsDropBar ? ExcelChartType.Stock_VolumeOpenHighLowClose : ExcelChartType.Stock_VolumeHighLowClose;
    }
    return excelChartType;
  }

  private ExcelChartType DetectTypeForPrimaryCollOnly(ChartSeriesCollection series)
  {
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    if (this.m_secondary.Count != 0 && (this.m_secondary.Count != 1 || !this.m_secondary.IsParetoFormat))
      throw new ApplicationException("Can't detect chart type");
    int count = series.Count;
    ChartFormatImpl chartFormatImpl = this.m_primary[0];
    if (count >= 3 && count <= 4 && chartFormatImpl.FormatRecordType == TBIFFRecord.ChartLine && (series[0] as ChartSerieImpl).ParentChart.IsStock && chartFormatImpl.IsChartChartLine && chartFormatImpl.HasHighLowLines)
      return !chartFormatImpl.IsDropBar ? ExcelChartType.Stock_HighLowClose : ExcelChartType.Stock_OpenHighLowClose;
    ChartSerieImpl chartSerieImpl1 = series[0] as ChartSerieImpl;
    string str1 = chartSerieImpl1.DetectSerieTypeStart();
    string str2 = chartSerieImpl1.DetectSerieTypeString();
    ExcelChartType excelChartType = ~ExcelChartType.Column_Clustered;
    if (Array.IndexOf<string>(ChartGlobalFormatsCollection.DEF_MABY_COMBINATION_TYPES_START, str1) != -1)
    {
      for (int index = 0; index < count; ++index)
      {
        ChartSerieImpl chartSerieImpl2 = series[index] as ChartSerieImpl;
        if (chartSerieImpl2.ChartGroup != chartSerieImpl1.ChartGroup || chartSerieImpl2.DetectSerieTypeString() != str2)
        {
          excelChartType = ExcelChartType.Combination_Chart;
          break;
        }
      }
    }
    if (excelChartType == ~ExcelChartType.Column_Clustered)
      excelChartType = chartSerieImpl1.SerieType;
    return excelChartType;
  }

  public void Clear()
  {
    this.m_primary.Clear();
    this.m_secondary.Clear();
  }

  public ChartFormatImpl ChangeNotIntimateSerieType(
    ExcelChartType typeToChange,
    ExcelChartType serieType,
    IApplication application,
    ChartImpl chart,
    ChartSerieImpl serieToChange)
  {
    if (application == null)
      throw new ArgumentNullException(nameof (application));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    bool flag = Array.IndexOf<ExcelChartType>(ChartImpl.DEF_NEED_SECONDARY_AXIS, typeToChange) != -1;
    ChartSeriesCollection series = (ChartSeriesCollection) chart.Series;
    ChartFormatImpl formatToAdd = flag ? new ChartFormatImpl(application, (object) this.m_secondary) : new ChartFormatImpl(application, (object) this.m_primary);
    formatToAdd.ChangeSerieType(typeToChange, false);
    formatToAdd.DrawingZOrder = series.FindOrderByType(typeToChange);
    List<ChartSerieImpl> withDrawingOrder = (chart.Series as ChartSeriesCollection).GetSeriesWithDrawingOrder(formatToAdd.DrawingZOrder);
    bool bCanReplace = withDrawingOrder.Count == 1 && withDrawingOrder[0] == serieToChange;
    if (flag)
    {
      chart.IsSecondaryAxes = true;
      this.m_secondary.Add(formatToAdd, bCanReplace);
    }
    else
    {
      if (Array.IndexOf<ExcelChartType>(ChartImpl.DEF_NEED_SECONDARY_AXIS, serieType) != -1)
        chart.ChangePrimaryAxis((chart.Workbook as WorkbookImpl).Loading);
      this.m_primary.Add(formatToAdd, bCanReplace);
    }
    return formatToAdd;
  }

  public void ChangeShallowAxis(bool bToPrimary, int iOrder, bool bAdd, int iNewOrder)
  {
    if (bToPrimary)
      this.ChangeInAxis(this.m_secondary, this.m_primary, iOrder, iNewOrder, bAdd);
    else
      this.ChangeInAxis(this.m_primary, this.m_secondary, iOrder, iNewOrder, bAdd);
  }

  private void ChangeInAxis(
    ChartFormatCollection from,
    ChartFormatCollection to,
    int iOrder,
    int iNewOrder,
    bool bAdd)
  {
    bool flag = false;
    ChartFormatImpl format1 = from.GetFormat(iOrder, !bAdd);
    ChartFormatImpl format2 = (ChartFormatImpl) null;
    if (!bAdd && format1.DataFormatOrNull != null)
      flag = true;
    if (!flag)
      format2 = (ChartFormatImpl) format1.Clone((object) to);
    else
      format1.CloneDeletedFormat((object) to, ref format2, false);
    if (bAdd)
    {
      format2.DrawingZOrder = iNewOrder;
      to.Add(format2, false);
    }
    else
      to.AddFormat(format2);
    if (!flag)
      return;
    format1.CloneDeletedFormat((object) to, ref format2, true);
  }
}
