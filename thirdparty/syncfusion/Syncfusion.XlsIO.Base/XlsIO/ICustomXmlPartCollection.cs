// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ICustomXmlPartCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO;

public interface ICustomXmlPartCollection : IEnumerable
{
  int Count { get; }

  ICustomXmlPart this[int index] { get; }

  ICustomXmlPart Add(ICustomXmlPart customXmlPart);

  ICustomXmlPart Add(string ID);

  ICustomXmlPart Add(string ID, byte[] XmlData);

  void Clear();

  void RemoveAt(int index);

  ICustomXmlPartCollection Clone();

  ICustomXmlPart GetById(string id);
}
