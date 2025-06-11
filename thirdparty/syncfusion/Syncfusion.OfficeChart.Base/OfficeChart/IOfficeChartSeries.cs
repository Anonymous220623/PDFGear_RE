// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IOfficeChartSeries
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IOfficeChartSeries : 
  IParentApplication,
  ICollection<IOfficeChartSerie>,
  IEnumerable<IOfficeChartSerie>,
  IEnumerable
{
  new int Count { get; }

  IOfficeChartSerie this[int index] { get; }

  IOfficeChartSerie this[string name] { get; }

  IOfficeChartSerie Add();

  IOfficeChartSerie Add(OfficeChartType serieType);

  IOfficeChartSerie Add(string name);

  IOfficeChartSerie Add(string name, OfficeChartType type);

  void RemoveAt(int index);

  void Remove(string serieName);
}
