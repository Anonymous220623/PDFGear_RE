// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IWorksheetCustomProperties
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface IWorksheetCustomProperties
{
  ICustomProperty this[int index] { get; }

  ICustomProperty this[string strName] { get; }

  int Count { get; }

  ICustomProperty Add(string strName);

  bool Contains(string strName);
}
