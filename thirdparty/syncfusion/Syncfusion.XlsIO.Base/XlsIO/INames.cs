// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.INames
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO;

public interface INames : IEnumerable
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
