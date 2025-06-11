// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.PROPVARIANT
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

[CLSCompliant(false)]
[StructLayout(LayoutKind.Explicit, Size = 16 /*0x10*/, Pack = 1)]
public struct PROPVARIANT
{
  [FieldOffset(0)]
  public short vt;
  [FieldOffset(8)]
  public long intPtr;
  [FieldOffset(8)]
  public byte byteVal;
  [FieldOffset(8)]
  public int intVal;
  [FieldOffset(8)]
  public bool boolVal;
  [FieldOffset(8)]
  public long fileTime;
  [FieldOffset(8)]
  public double doubleVal;
  [FieldOffset(8)]
  public short shortVal;
  [FieldOffset(12)]
  public long intPtr2;
}
