// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IName
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface IName : IParentApplication
{
  int Index { get; }

  string Name { get; set; }

  string NameLocal { get; set; }

  IRange RefersToRange { get; set; }

  string Value { get; set; }

  bool Visible { get; set; }

  bool IsLocal { get; }

  string ValueR1C1 { get; }

  string RefersTo { get; }

  string RefersToR1C1 { get; }

  IWorksheet Worksheet { get; }

  string Scope { get; }

  void Delete();
}
