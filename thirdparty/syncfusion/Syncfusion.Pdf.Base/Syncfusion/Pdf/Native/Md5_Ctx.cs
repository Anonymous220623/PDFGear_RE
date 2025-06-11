// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.Md5_Ctx
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Pdf.Native;

internal struct Md5_Ctx
{
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
  public uint[] i;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
  public uint[] buf;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64 /*0x40*/)]
  public byte[] input;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16 /*0x10*/)]
  public byte[] digest;
}
