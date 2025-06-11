// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartFormatCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartFormatCollection : CollectionBaseEx<ChartFormatImpl>
{
  private const int DEF_ARRAY_VALUE = -1;
  public const int DEF_ARRAY_CAPACITY = 8;
  private static readonly TBIFFRecord[] DEF_NEED_SECONDARY_AXIS = new TBIFFRecord[4]
  {
    TBIFFRecord.ChartPie,
    TBIFFRecord.ChartRadar,
    TBIFFRecord.ChartRadarArea,
    TBIFFRecord.ChartBoppop
  };
  private int[] m_arrOrder;
  private ChartParentAxisImpl m_parentAxis;
  private bool m_isParetoFormat;

  public ChartFormatCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_arrOrder = new int[8];
    for (int index = 0; index < 8; ++index)
      this.m_arrOrder[index] = -1;
    this.SetParents();
  }

  public void SetParents()
  {
    this.m_parentAxis = (ChartParentAxisImpl) this.FindParent(typeof (ChartParentAxisImpl));
    if (this.m_parentAxis == null)
      throw new ApplicationException("Can't find parent axis.");
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    for (int index = 0; index < this.Count; ++index)
      this.List[index].Serialize(records);
  }

  public new ChartFormatImpl this[int index]
  {
    get
    {
      if (this.m_arrOrder[index] == -1)
        throw new ArgumentException("Index out of bounds.");
      return this.List[this.m_arrOrder[index]];
    }
  }

  public bool IsPrimary => this.m_parentAxis.IsPrimary;

  public bool NeedSecondaryAxis
  {
    get
    {
      if (!this.IsPrimary || this.Count < 1)
        return false;
      ChartFormatImpl chartFormatImpl = this.List[0];
      TBIFFRecord formatRecordType = chartFormatImpl.FormatRecordType;
      return !chartFormatImpl.Is3D && (Array.IndexOf<TBIFFRecord>(ChartFormatCollection.DEF_NEED_SECONDARY_AXIS, formatRecordType) != -1 || formatRecordType == TBIFFRecord.ChartBar && chartFormatImpl.IsHorizontalBar);
    }
  }

  internal bool IsParetoFormat
  {
    get => this.m_isParetoFormat;
    set => this.m_isParetoFormat = value;
  }

  internal bool IsBarChartAxes
  {
    get
    {
      if (this.Count < 1)
        return false;
      for (int index = 0; index < this.List.Count; ++index)
      {
        ChartFormatImpl chartFormatImpl = this.List[index];
        if (chartFormatImpl.FormatRecordType == TBIFFRecord.ChartBar && chartFormatImpl.IsHorizontalBar)
          return true;
      }
      return false;
    }
  }

  internal bool IsPercentStackedAxis
  {
    get
    {
      if (this.Count < 1)
        return false;
      for (int index = 0; index < this.List.Count; ++index)
      {
        ChartFormatImpl chartFormatImpl = this.List[index];
        if (chartFormatImpl.FormatRecordType == TBIFFRecord.ChartArea && chartFormatImpl.IsCategoryBrokenDown || chartFormatImpl.FormatRecordType == TBIFFRecord.ChartBar && chartFormatImpl.ShowAsPercentsBar || chartFormatImpl.FormatRecordType == TBIFFRecord.ChartLine && chartFormatImpl.ShowAsPercentsLine)
          return true;
      }
      return false;
    }
  }

  public ChartFormatImpl Add(ChartFormatImpl formatToAdd) => this.Add(formatToAdd, false);

  public ChartFormatImpl Add(ChartFormatImpl formatToAdd, bool bCanReplace)
  {
    int order = !(formatToAdd == (ChartFormatImpl) null) ? formatToAdd.DrawingZOrder : throw new ArgumentNullException(nameof (formatToAdd));
    int i = this.m_arrOrder[order];
    if (i < 0 || !bCanReplace)
    {
      base.Add(formatToAdd);
      int index = this.Count - 1;
      formatToAdd = this.m_parentAxis.Formats.AddFormat(formatToAdd, order, index, this.IsPrimary);
    }
    else
      this[i] = formatToAdd;
    return formatToAdd;
  }

  public ChartFormatImpl FindOrAdd(ChartFormatImpl formatToAdd)
  {
    ChartFormatImpl orAdd = (ChartFormatImpl) null;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      ChartFormatImpl inner = this.InnerList[index];
      if (formatToAdd == inner)
      {
        orAdd = inner;
        break;
      }
    }
    if (orAdd == (ChartFormatImpl) null)
      orAdd = this.Add(formatToAdd, false);
    return orAdd;
  }

  public bool ContainsIndex(int index) => index < 8 && index >= 0 && this.m_arrOrder[index] != -1;

  public void Remove(ChartFormatImpl toRemove)
  {
    int index = !(toRemove == (ChartFormatImpl) null) ? toRemove.DrawingZOrder : throw new ArgumentNullException(nameof (toRemove));
    if (((ChartSeriesCollection) this.m_parentAxis.m_parentChart.Series).GetCountOfSeriesWithSameDrawingOrder(index) != 0)
      throw new ArgumentException("Can't remove format.");
    int num = this.m_arrOrder[index];
    this.RemoveAt(num);
    this.m_arrOrder[index] = -1;
    this.m_parentAxis.Formats.RemoveFormat(num, index, this.IsPrimary);
  }

  public void UpdateIndexesAfterRemove(int removeIndex)
  {
    for (int index = 0; index < 8; ++index)
    {
      if (this.m_arrOrder[index] > removeIndex)
        this.m_arrOrder[index] = this.m_arrOrder[index] - 1;
    }
  }

  public void UpdateSeriesByChartGroup(int newIndex, int OldIndex)
  {
    ChartSeriesCollection series = (ChartSeriesCollection) this.m_parentAxis.m_parentChart.Series;
    int index = 0;
    for (int count = series.Count; index < count; ++index)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) series[index];
      if (chartSerieImpl.ChartGroup == OldIndex)
        chartSerieImpl.ChartGroup = newIndex;
    }
  }

  public new void Clear()
  {
    base.Clear();
    for (int index = 0; index < 8; ++index)
      this.m_arrOrder[index] = -1;
  }

  public override object Clone(object parent)
  {
    ChartFormatCollection formatCollection = (ChartFormatCollection) base.Clone(parent);
    formatCollection.m_arrOrder = CloneUtils.CloneIntArray(this.m_arrOrder);
    formatCollection.m_isParetoFormat = this.m_isParetoFormat;
    if (this.m_parentAxis != null)
      formatCollection.m_parentAxis = this.m_parentAxis;
    return (object) formatCollection;
  }

  public void SetIndex(int index, int Value)
  {
    if (index >= 8 || index < 0 || Value < 0 || Value >= this.List.Count)
      throw new ArgumentException("Index is out of bounds");
    this.m_arrOrder[index] = Value;
  }

  public void UpdateFormatsOnAdding(int index)
  {
    this[index].DrawingZOrder = index + 1;
    this.UpdateSeriesByChartGroup(index + 1, index);
    this.m_arrOrder[index + 1] = this.m_arrOrder[index];
    this.m_arrOrder[index] = -1;
  }

  public void UpdateFormatsOnRemoving(int index)
  {
    this[index].DrawingZOrder = index - 1;
    this.UpdateSeriesByChartGroup(index - 1, index);
    this.m_arrOrder[index - 1] = this.m_arrOrder[index];
    this.m_arrOrder[index] = -1;
  }

  public ChartFormatImpl GetFormat(int iOrder, bool bDelete)
  {
    int index1 = this.m_arrOrder[iOrder];
    ChartFormatImpl format = index1 != -1 ? this.List[index1] : throw new ArgumentException("Can't find format by current index.");
    if (bDelete)
    {
      this.m_arrOrder[iOrder] = -1;
      this.RemoveAt(index1);
      int index2 = 0;
      for (int length = this.m_arrOrder.Length; index2 < length; ++index2)
      {
        int num1 = this.m_arrOrder[index2];
        if (num1 > index1)
        {
          int num2;
          this.m_arrOrder[index2] = num2 = num1 - 1;
        }
      }
    }
    return format;
  }

  public void AddFormat(ChartFormatImpl format)
  {
    int index = !(format == (ChartFormatImpl) null) ? format.DrawingZOrder : throw new ArgumentNullException(nameof (format));
    base.Add(format);
    int num = this.Count - 1;
    this.m_arrOrder[index] = num;
  }
}
