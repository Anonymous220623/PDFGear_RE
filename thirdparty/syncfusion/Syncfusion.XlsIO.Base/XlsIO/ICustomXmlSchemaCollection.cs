// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ICustomXmlSchemaCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO;

public interface ICustomXmlSchemaCollection : IEnumerable
{
  int Count { get; }

  string this[int index] { get; set; }

  void Add(string name);

  void Clear();

  ICustomXmlSchemaCollection Clone();

  int IndexOf(string value);

  void Remove(string name);

  void RemoveAt(int index);
}
