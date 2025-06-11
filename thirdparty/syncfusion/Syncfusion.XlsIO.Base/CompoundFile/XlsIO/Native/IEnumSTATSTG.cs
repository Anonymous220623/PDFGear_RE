// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Native.IEnumSTATSTG
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Native;

[CLSCompliant(false)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("0000000d-0000-0000-C000-000000000046")]
public interface IEnumSTATSTG
{
  int Next(uint celt, ref STATSTG rgelt, ref uint pceltFetched);

  int Skip(uint celt);

  int Reset();

  int Clone(ref IEnumSTATSTG ppenum);
}
