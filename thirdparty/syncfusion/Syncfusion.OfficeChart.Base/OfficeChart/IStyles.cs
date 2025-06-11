// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IStyles
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface IStyles : IEnumerable
{
  IApplication Application { get; }

  int Count { get; }

  IStyle this[int Index] { get; }

  IStyle this[string name] { get; }

  object Parent { get; }

  IStyle Add(string Name, object BasedOn);

  IStyle Add(string Name);

  IStyles Merge(object Workbook, bool overwrite);

  IStyles Merge(object Workbook);

  bool Contains(string name);

  void Remove(string styleName);
}
