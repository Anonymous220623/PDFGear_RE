// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.IEnumSTATPROPSTG
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("00000139-0000-0000-C000-000000000046")]
[CLSCompliant(false)]
public interface IEnumSTATPROPSTG
{
  void Next(int celt, [MarshalAs(UnmanagedType.LPArray), In, Out] tagSTATPROPSTG[] rgelt, out int pceltFetched);

  void Skip(uint celt);

  void Reset();

  void Clone(out IEnumSTATPROPSTG ppenum);
}
