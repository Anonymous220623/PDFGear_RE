// Decompiled with JetBrains decompiler
// Type: Standard.CREATESTRUCT
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct CREATESTRUCT
{
  public IntPtr lpCreateParams;
  public IntPtr hInstance;
  public IntPtr hMenu;
  public IntPtr hwndParent;
  public int cy;
  public int cx;
  public int y;
  public int x;
  public WS style;
  [MarshalAs(UnmanagedType.LPWStr)]
  public string lpszName;
  [MarshalAs(UnmanagedType.LPWStr)]
  public string lpszClass;
  public WS_EX dwExStyle;
}
