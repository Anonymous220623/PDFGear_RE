// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Native.IEnumSTATPROPSETSTG
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Native;

[CLSCompliant(false)]
[Guid("0000013b-0000-0000-c000-000000000046")]
public interface IEnumSTATPROPSETSTG
{
  void Next(uint celt, out tagSTATPROPSETSTG rgelt, out uint pceltFetched);

  void Skip(uint celt);

  void Reset();

  void Clone(out IEnumSTATPROPSETSTG ppenum);
}
