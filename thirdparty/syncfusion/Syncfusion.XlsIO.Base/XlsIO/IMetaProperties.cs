// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IMetaProperties
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IMetaProperties : IEnumerable
{
  IMetaProperty this[int iIndex] { get; }

  int Count { get; }

  string SchemaXml { get; }

  IApplication Application { get; }

  object Parent { get; }

  IMetaProperty GetItemByInternalName(string InternalName);
}
