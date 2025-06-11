// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.ICalculationOptions
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface ICalculationOptions : IParentApplication
{
  int MaximumIteration { get; set; }

  bool RecalcOnSave { get; set; }

  double MaximumChange { get; set; }

  bool IsIterationEnabled { get; set; }

  bool R1C1ReferenceMode { get; set; }

  OfficeCalculationMode CalculationMode { get; set; }
}
