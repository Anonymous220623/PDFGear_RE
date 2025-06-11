// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IStyles
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IStyles : IEnumerable
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
