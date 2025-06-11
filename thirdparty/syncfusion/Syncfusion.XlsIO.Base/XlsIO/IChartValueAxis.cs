// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChartValueAxis
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChartValueAxis : IChartAxis
{
  double MinimumValue { get; set; }

  double MaximumValue { get; set; }

  double MajorUnit { get; set; }

  double MinorUnit { get; set; }

  [Obsolete("This property is obsolete. Please use CrossesAt instead of it")]
  double CrossValue { get; set; }

  double CrossesAt { get; set; }

  bool IsAutoMin { get; set; }

  bool IsAutoMax { get; set; }

  bool IsAutoMajor { get; set; }

  bool IsAutoMinor { get; set; }

  bool IsAutoCross { get; set; }

  bool IsLogScale { get; set; }

  bool IsMaxCross { get; set; }

  double DisplayUnitCustom { get; set; }

  ExcelChartDisplayUnit DisplayUnit { get; set; }

  bool HasDisplayUnitLabel { get; set; }

  IChartTextArea DisplayUnitLabel { get; }
}
