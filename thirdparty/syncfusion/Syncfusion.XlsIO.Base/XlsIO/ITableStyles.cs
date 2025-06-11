// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ITableStyles
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO;

public interface ITableStyles : IEnumerable
{
  ITableStyle this[string tableStyleName] { get; }

  ITableStyle this[int index] { get; }

  ITableStyle Add(string tableStyleName);

  ITableStyle Add(ITableStyle tableStyle);

  int Count { get; }

  bool Contains(ITableStyle tableStyle);

  bool Contains(string tableStyleName);

  void Remove(ITableStyle tableStyle);

  void RemoveAt(int index);
}
