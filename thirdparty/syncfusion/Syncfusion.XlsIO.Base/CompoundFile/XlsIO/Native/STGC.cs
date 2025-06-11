// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Native.STGC
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Native;

[Flags]
public enum STGC
{
  STGC_DEFAULT = 0,
  STGC_OVERWRITE = 1,
  STGC_ONLYIFCURRENT = 2,
  STGC_DANGEROUSLYCOMMITMERELYTODISKCACHE = 4,
  STGC_CONSOLIDATE = 8,
}
