// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.STGM
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

[Flags]
public enum STGM
{
  STGM_READ = 0,
  STGM_WRITE = 1,
  STGM_READWRITE = 2,
  STGM_SHARE_DENY_NONE = 64, // 0x00000040
  STGM_SHARE_DENY_READ = 48, // 0x00000030
  STGM_SHARE_DENY_WRITE = 32, // 0x00000020
  STGM_SHARE_EXCLUSIVE = 16, // 0x00000010
  STGM_PRIORITY = 262144, // 0x00040000
  STGM_CREATE = 4096, // 0x00001000
  STGM_CONVERT = 131072, // 0x00020000
  STGM_FAILIFTHERE = 0,
  STGM_DIRECT = 0,
  STGM_TRANSACTED = 65536, // 0x00010000
  STGM_NOSCRATCH = 1048576, // 0x00100000
  STGM_NOSNAPSHOT = 2097152, // 0x00200000
  STGM_SIMPLE = 134217728, // 0x08000000
  STGM_DIRECT_SWMR = 4194304, // 0x00400000
  STGM_DELETEONRELEASE = 67108864, // 0x04000000
}
