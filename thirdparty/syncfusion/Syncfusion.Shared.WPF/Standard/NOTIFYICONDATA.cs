// Decompiled with JetBrains decompiler
// Type: Standard.NOTIFYICONDATA
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[StructLayout(LayoutKind.Sequential)]
internal class NOTIFYICONDATA
{
  public int cbSize;
  public IntPtr hWnd;
  public int uID;
  public NIF uFlags;
  public int uCallbackMessage;
  public IntPtr hIcon;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128 /*0x80*/)]
  public char[] szTip = new char[128 /*0x80*/];
  public uint dwState;
  public uint dwStateMask;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256 /*0x0100*/)]
  public char[] szInfo = new char[256 /*0x0100*/];
  public uint uVersion;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64 /*0x40*/)]
  public char[] szInfoTitle = new char[64 /*0x40*/];
  public uint dwInfoFlags;
  public Guid guidItem;
  private IntPtr hBalloonIcon;
}
