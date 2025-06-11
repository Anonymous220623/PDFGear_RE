// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.IEnumSTATPROPSETSTG
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

[Guid("0000013b-0000-0000-c000-000000000046")]
[CLSCompliant(false)]
public interface IEnumSTATPROPSETSTG
{
  void Next(uint celt, out tagSTATPROPSETSTG rgelt, out uint pceltFetched);

  void Skip(uint celt);

  void Reset();

  void Clone(out IEnumSTATPROPSETSTG ppenum);
}
