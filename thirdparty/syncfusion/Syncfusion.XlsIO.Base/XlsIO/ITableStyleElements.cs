// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ITableStyleElements
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO;

public interface ITableStyleElements : IEnumerable
{
  ITableStyleElement this[ExcelTableStyleElementType tableStyleElementType] { get; }

  ITableStyleElement this[int index] { get; }

  ITableStyleElement Add(ExcelTableStyleElementType tableStyleElementType);

  ITableStyleElement Add(ITableStyleElement tableStyleElement);

  int Count { get; }

  bool Contains(ITableStyleElement tableStyleElement);

  bool Contains(ExcelTableStyleElementType tableStyleElementType);

  void Remove(ITableStyleElement tableStyleElement);

  void RemoveAt(int index);
}
