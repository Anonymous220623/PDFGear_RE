// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.ComEnumFORMATETC
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

[ComVisible(true)]
[CLSCompliant(false)]
public class ComEnumFORMATETC : ArrayList, IComEnumFORMATETC
{
  private int m_position = -1;

  public uint Clone(ref IComEnumFORMATETC ppenum) => 2147483649 /*0x80000001*/;

  public uint RemoteNext(uint celt, ref FORMATETC rgelt, ref uint pceltFetched)
  {
    int num = celt > 1U ? 1 : (int) celt;
    this.m_position += num;
    if (this.m_position >= this.Count)
      return 2147500037 /*0x80004005*/;
    rgelt = ((DataObjectEntry) this[this.m_position]).Format;
    pceltFetched = (uint) num;
    return 0;
  }

  public uint Reset()
  {
    this.m_position = -1;
    return 0;
  }

  public uint Skip(uint celt)
  {
    if ((long) this.m_position + (long) celt > (long) this.Count)
      return 2147500037 /*0x80004005*/;
    this.m_position += (int) celt;
    return 0;
  }
}
