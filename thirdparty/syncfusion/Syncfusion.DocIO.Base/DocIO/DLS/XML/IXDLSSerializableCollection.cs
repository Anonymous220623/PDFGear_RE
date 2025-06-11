// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.XML.IXDLSSerializableCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.DocIO.DLS.XML;

public interface IXDLSSerializableCollection : IEnumerable
{
  IXDLSSerializable AddNewItem(IXDLSContentReader reader);

  string TagItemName { get; }

  int Count { get; }
}
