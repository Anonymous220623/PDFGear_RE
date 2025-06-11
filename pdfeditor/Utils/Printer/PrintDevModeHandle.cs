// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Printer.PrintDevModeHandle
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace pdfeditor.Utils.Printer;

public class PrintDevModeHandle : SafeHandleZeroOrMinusOneIsInvalid
{
  public PrintDevModeHandle(IntPtr handle)
    : base(true)
  {
    this.SetHandle(handle);
  }

  protected override bool ReleaseHandle()
  {
    return PrintDevModeHandle.GlobalFree(this.handle) == IntPtr.Zero;
  }

  [DllImport("kernel32.dll", SetLastError = true)]
  private static extern IntPtr GlobalFree(IntPtr handle);
}
