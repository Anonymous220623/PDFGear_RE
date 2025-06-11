// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.ListLevels
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

internal class ListLevels : List<object>
{
  internal ListLevel this[int index]
  {
    get => base[index] as ListLevel;
    set => this[index] = (object) value;
  }
}
