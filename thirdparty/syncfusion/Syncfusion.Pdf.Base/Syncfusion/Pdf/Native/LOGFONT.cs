// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.LOGFONT
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Pdf.Native;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal class LOGFONT
{
  public int lfHeight;
  public int lfWidth;
  public int lfEscapement;
  public int lfOrientation;
  public FW_FONT_WEIGHT lfWeight = FW_FONT_WEIGHT.FW_NORMAL;
  [MarshalAs(UnmanagedType.U1)]
  public bool lfItalic;
  [MarshalAs(UnmanagedType.U1)]
  public bool lfUnderline;
  [MarshalAs(UnmanagedType.U1)]
  public bool lfStrikeOut;
  public byte lfCharSet;
  public byte lfOutPrecision;
  public byte lfClipPrecision;
  public byte lfQuality;
  public byte lfPitchAndFamily;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32 /*0x20*/)]
  public string lfFaceName;
}
