// Decompiled with JetBrains decompiler
// Type: Standard.WNDCLASSEX
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct WNDCLASSEX
{
  public int cbSize;
  public CS style;
  public WndProc lpfnWndProc;
  public int cbClsExtra;
  public int cbWndExtra;
  public IntPtr hInstance;
  public IntPtr hIcon;
  public IntPtr hCursor;
  public IntPtr hbrBackground;
  [MarshalAs(UnmanagedType.LPWStr)]
  public string lpszMenuName;
  [MarshalAs(UnmanagedType.LPWStr)]
  public string lpszClassName;
  public IntPtr hIconSm;
}
