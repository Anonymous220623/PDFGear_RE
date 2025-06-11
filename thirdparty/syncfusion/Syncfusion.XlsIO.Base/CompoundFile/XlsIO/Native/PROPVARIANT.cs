// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Native.PROPVARIANT
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Native;

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
