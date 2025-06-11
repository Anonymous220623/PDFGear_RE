// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChartSeriesAxis
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChartSeriesAxis : IChartAxis
{
  [Obsolete("Please, use TickLabelSpacing instead of it")]
  int LabelFrequency { get; set; }

  int TickLabelSpacing { get; set; }

  [Obsolete("Please, use TickMarkSpacing instead of it")]
  int TickMarksFrequency { get; set; }

  int TickMarkSpacing { get; set; }
}
