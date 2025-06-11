// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.STGC
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

[Flags]
public enum STGC
{
  STGC_DEFAULT = 0,
  STGC_OVERWRITE = 1,
  STGC_ONLYIFCURRENT = 2,
  STGC_DANGEROUSLYCOMMITMERELYTODISKCACHE = 4,
  STGC_CONSOLIDATE = 8,
}
