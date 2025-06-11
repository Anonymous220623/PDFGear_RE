// Decompiled with JetBrains decompiler
// Type: NAudio.Utils.NativeMethods
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Utils;

public static class NativeMethods
{
  [DllImport("kernel32.dll")]
  public static extern IntPtr LoadLibrary(string dllToLoad);

  [DllImport("kernel32.dll")]
  public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

  [DllImport("kernel32.dll")]
  public static extern bool FreeLibrary(IntPtr hModule);
}
