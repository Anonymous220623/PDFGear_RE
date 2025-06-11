// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.CHARFORMAT
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal struct CHARFORMAT
{
  public int cbSize;
  public uint dwMask;
  public uint dwEffects;
  public int yHeight;
  public int yOffset;
  public int crTextColor;
  public byte bCharSet;
  public byte bPitchAndFamily;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32 /*0x20*/)]
  public char[] szFaceName;
  public short wWeight;
  public short sSpacing;
  public int crBackColor;
  public uint lcid;
  public uint dwReserved;
  public short sStyle;
  public short wKerning;
  public byte bUnderlineType;
  public byte bAnimation;
  public byte bRevAuthor;
  public byte bReserved1;
}
