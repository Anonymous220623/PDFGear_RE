// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IOfficeChartData
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IOfficeChartData
{
  void SetValue(int rowIndex, int columnIndex, int value);

  void SetValue(int rowIndex, int columnIndex, double value);

  void SetValue(int rowIndex, int columnIndex, string value);

  void SetValue(int rowIndex, int columnIndex, object value);

  void SetChartData(object[][] data);

  void SetDataRange(object[][] data, int rowIndex, int columnIndex);

  void SetDataRange(IEnumerable enumerable, int rowIndex, int columnIndex);

  object GetValue(int rowIndex, int columnIndex);

  IOfficeDataRange this[int firstRow, int firstCol, int lastRow, int lastCol] { get; }

  void Clear();
}
