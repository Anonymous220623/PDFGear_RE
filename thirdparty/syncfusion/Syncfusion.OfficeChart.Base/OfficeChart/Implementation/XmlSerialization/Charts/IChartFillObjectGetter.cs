// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts.IChartFillObjectGetter
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Interfaces;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;

internal interface IChartFillObjectGetter
{
  ChartBorderImpl Border { get; }

  ChartInteriorImpl Interior { get; }

  IInternalFill Fill { get; }

  ShadowImpl Shadow { get; }

  ThreeDFormatImpl ThreeD { get; }
}
