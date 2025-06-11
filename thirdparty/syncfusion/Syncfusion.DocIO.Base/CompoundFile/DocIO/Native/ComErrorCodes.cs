// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.ComErrorCodes
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

[CLSCompliant(false)]
public enum ComErrorCodes : uint
{
  S_OK = 0,
  S_FALSE = 1,
  E_NOTIMPL = 2147483649, // 0x80000001
  E_FAIL = 2147500037, // 0x80004005
  E_UNEXPECTED = 2147549183, // 0x8000FFFF
  OLE_E_ADVISENOTSUPPORTED = 2147745795, // 0x80040003
  DV_E_FORMATETC = 2147745892, // 0x80040064
  E_INVALIDARG = 2147942487, // 0x80070057
}
