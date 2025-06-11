// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.ListFormats
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

internal class ListFormats : List<ListData>
{
  internal ListData GetListFromId(int id)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index].ListID == id)
        return this[index];
    }
    return (ListData) null;
  }

  internal ListData FindListData(int listId)
  {
    return this.GetListFromId(listId) ?? throw new ArgumentException("List data with the specified id could not be found.");
  }

  internal new ListData this[int index]
  {
    get => base[index];
    set => base[index] = value;
  }
}
