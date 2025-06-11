// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.ComDataObject
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

[Guid("0000010e-0000-0000-C000-000000000046")]
[CLSCompliant(false)]
[ClassInterface(ClassInterfaceType.None)]
[ComVisible(true)]
public class ComDataObject : IComDataObject
{
  private ComEnumFORMATETC m_storage = new ComEnumFORMATETC();

  public uint GetData(ref FORMATETC pformatetcIn, ref STGMEDIUM pRemoteMedium)
  {
    Trace.WriteLine(nameof (GetData));
    if (this.m_storage.Count <= 0)
      return 2147745892;
    DataObjectEntry dataObjectEntry = (DataObjectEntry) this.m_storage[0];
    pformatetcIn = dataObjectEntry.Format;
    pRemoteMedium = dataObjectEntry.Medium;
    return 0;
  }

  public uint GetDataHere(ref FORMATETC pformatetc, ref STGMEDIUM pRemoteMedium)
  {
    Trace.WriteLine(nameof (GetDataHere));
    return 2147483649 /*0x80000001*/;
  }

  public uint QueryGetData(ref FORMATETC pformatetc)
  {
    Trace.WriteLine(nameof (QueryGetData));
    return 2147483649 /*0x80000001*/;
  }

  public uint GetCanonicalFormatEtc(ref FORMATETC pformatectIn, ref FORMATETC pformatetcOut)
  {
    Trace.WriteLine(nameof (GetCanonicalFormatEtc));
    return 2147483649 /*0x80000001*/;
  }

  public uint SetData(ref FORMATETC pformatetc, ref STGMEDIUM pmedium, int fRelease)
  {
    Trace.WriteLine(nameof (SetData));
    FORMATETC format = pformatetc;
    STGMEDIUM medium = pmedium;
    if (IntPtr.Zero != medium.pStorage)
      Marshal.AddRef(medium.pStorage);
    medium.pUnkForRelease = IntPtr.Zero;
    if (fRelease > 0)
    {
      if (IntPtr.Zero != pmedium.pStorage)
        Marshal.Release(pmedium.pStorage);
      if (IntPtr.Zero != pmedium.pUnkForRelease)
        Marshal.Release(pmedium.pUnkForRelease);
    }
    this.m_storage.Add((object) new DataObjectEntry(DATADIR.DATADIR_SET, medium, format));
    return 0;
  }

  public uint EnumFormatEtc(uint dwDirection, ref IComEnumFORMATETC ppenumFormatEtc)
  {
    Trace.WriteLine(nameof (EnumFormatEtc));
    ppenumFormatEtc = (IComEnumFORMATETC) this.m_storage;
    return 0;
  }

  public uint DAdvise(
    ref FORMATETC pformatetc,
    uint advf,
    IntPtr pAdvSink,
    ref uint pdwConnection)
  {
    Trace.WriteLine(nameof (DAdvise));
    return 2147745795 /*0x80040003*/;
  }

  public uint DUnadvise(uint dwConnection)
  {
    Trace.WriteLine(nameof (DUnadvise));
    return 2147745795 /*0x80040003*/;
  }

  public uint EnumDAdvise(ref IntPtr ppenumAdvise)
  {
    Trace.WriteLine(nameof (EnumDAdvise));
    return 2147745795 /*0x80040003*/;
  }
}
