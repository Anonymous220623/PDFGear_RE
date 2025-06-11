// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.INames
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface INames : IEnumerable
{
  IApplication Application { get; }

  int Count { get; }

  object Parent { get; }

  IName this[int index] { get; }

  IName this[string name] { get; }

  IWorksheet ParentWorksheet { get; }

  IName Add(string name);

  IName Add(string name, IRange namedObject);

  IName Add(IName name);

  void Remove(string name);

  void RemoveAt(int index);

  bool Contains(string name);
}
