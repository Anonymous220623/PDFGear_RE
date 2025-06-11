// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChartSeries
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChartSeries : 
  IParentApplication,
  ICollection<IChartSerie>,
  IEnumerable<IChartSerie>,
  IEnumerable
{
  new int Count { get; }

  IChartSerie this[int index] { get; }

  IChartSerie this[string name] { get; }

  IChartSerie Add();

  IChartSerie Add(ExcelChartType serieType);

  IChartSerie Add(string name);

  IChartSerie Add(string name, ExcelChartType type);

  void RemoveAt(int index);

  void Remove(string serieName);
}
