// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Charts.IChartFillObjectGetter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Interfaces;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;

internal interface IChartFillObjectGetter
{
  ChartBorderImpl Border { get; }

  ChartInteriorImpl Interior { get; }

  IInternalFill Fill { get; }

  ShadowImpl Shadow { get; }

  ThreeDFormatImpl ThreeD { get; }
}
