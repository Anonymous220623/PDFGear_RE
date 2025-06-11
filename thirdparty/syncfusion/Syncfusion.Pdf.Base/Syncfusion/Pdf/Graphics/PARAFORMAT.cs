// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PARAFORMAT
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal struct PARAFORMAT
{
  public int cbSize;
  public uint dwMask;
  public short wNumbering;
  public short wReserved;
  public int dxStartIndent;
  public int dxRightIndent;
  public int dxOffset;
  public short wAlignment;
  public short cTabCount;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32 /*0x20*/)]
  public int[] rgxTabs;
  public int dySpaceBefore;
  public int dySpaceAfter;
  public int dyLineSpacing;
  public short sStyle;
  public byte bLineSpacingRule;
  public byte bOutlineLevel;
  public short wShadingWeight;
  public short wShadingStyle;
  public short wNumberingStart;
  public short wNumberingStyle;
  public short wNumberingTab;
  public short wBorderSpace;
  public short wBorderWidth;
  public short wBorders;
}
