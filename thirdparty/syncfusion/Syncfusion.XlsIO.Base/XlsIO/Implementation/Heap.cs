// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Heap
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public sealed class Heap
{
  [DllImport("kernel32")]
  public static extern IntPtr HeapAlloc(IntPtr hHeap, int dwFlags, int dwBytes);

  [DllImport("kernel32")]
  public static extern IntPtr HeapCreate(int flOptions, int dwInitialSize, int dwMaximumSize);

  [DllImport("kernel32")]
  public static extern int HeapDestroy(IntPtr hHeap);

  [DllImport("kernel32")]
  public static extern int HeapFree(IntPtr hHeap, int dwFlags, IntPtr lpMem);

  [DllImport("kernel32")]
  public static extern IntPtr HeapReAlloc(IntPtr hHeap, int dwFlags, IntPtr lpMem, int dwBytes);
}
