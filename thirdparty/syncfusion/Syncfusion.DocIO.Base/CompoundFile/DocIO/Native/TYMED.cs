// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.TYMED
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

[Flags]
public enum TYMED
{
  TYMED_NULL = 0,
  TYMED_HGLOBAL = 1,
  TYMED_FILE = 2,
  TYMED_ISTREAM = 4,
  TYMED_ISTORAGE = 8,
  TYMED_GDI = 16, // 0x00000010
  TYMED_MFPICT = 32, // 0x00000020
  TYMED_ENHMF = 64, // 0x00000040
}
