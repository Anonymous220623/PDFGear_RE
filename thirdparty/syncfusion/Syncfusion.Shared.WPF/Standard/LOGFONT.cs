// Decompiled with JetBrains decompiler
// Type: Standard.LOGFONT
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct LOGFONT
{
  public int lfHeight;
  public int lfWidth;
  public int lfEscapement;
  public int lfOrientation;
  public int lfWeight;
  public byte lfItalic;
  public byte lfUnderline;
  public byte lfStrikeOut;
  public byte lfCharSet;
  public byte lfOutPrecision;
  public byte lfClipPrecision;
  public byte lfQuality;
  public byte lfPitchAndFamily;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32 /*0x20*/)]
  public string lfFaceName;
}
