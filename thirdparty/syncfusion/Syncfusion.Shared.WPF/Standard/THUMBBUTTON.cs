// Decompiled with JetBrains decompiler
// Type: Standard.THUMBBUTTON
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Unicode)]
internal struct THUMBBUTTON
{
  public const int THBN_CLICKED = 6144;
  public THB dwMask;
  public uint iId;
  public uint iBitmap;
  public IntPtr hIcon;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
  public string szTip;
  public THBF dwFlags;
}
