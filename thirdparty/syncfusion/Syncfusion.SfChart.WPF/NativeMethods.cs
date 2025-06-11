// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.NativeMethods
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Runtime;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal static class NativeMethods
{
  [TargetedPatchingOptOut("Internal method only, inlined across NGen boundaries for performance reasons")]
  internal static void SetUnmanagedMemory(IntPtr dst, int filler, int count)
  {
    NativeMethods.memset(dst, filler, count);
  }

  [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
  private static extern void memset(IntPtr dst, int filler, int count);
}
