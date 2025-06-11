// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IAutoFilterCondition
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface IAutoFilterCondition
{
  OfficeFilterDataType DataType { get; set; }

  OfficeFilterCondition ConditionOperator { get; set; }

  string String { get; set; }

  bool Boolean { get; }

  byte ErrorCode { get; }

  double Double { get; set; }
}
