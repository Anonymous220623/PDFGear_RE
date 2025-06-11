// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Compression.WaveFilter
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave.Compression;

[StructLayout(LayoutKind.Sequential)]
public class WaveFilter
{
  public int StructureSize = Marshal.SizeOf(typeof (WaveFilter));
  public int FilterTag;
  public int Filter;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
  public int[] Reserved;
}
