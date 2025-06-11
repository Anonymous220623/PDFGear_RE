// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveHeader
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave;

[StructLayout(LayoutKind.Sequential)]
public sealed class WaveHeader
{
  public IntPtr dataBuffer;
  public int bufferLength;
  public int bytesRecorded;
  public IntPtr userData;
  public WaveHeaderFlags flags;
  public int loops;
  public IntPtr next;
  public IntPtr reserved;
}
