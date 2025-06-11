// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.IColumns
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation;

public interface IColumns : IEnumerable<IColumn>, IEnumerable
{
  int Count { get; }

  IColumn this[int columnIndex] { get; }

  IColumn Add();

  int Add(IColumn column);

  void Insert(int index, IColumn item);

  void RemoveAt(int index);

  void Remove(IColumn value);

  int IndexOf(IColumn value);

  void Clear();
}
