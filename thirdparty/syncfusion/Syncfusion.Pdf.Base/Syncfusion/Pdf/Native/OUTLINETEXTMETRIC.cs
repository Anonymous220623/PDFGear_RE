// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.OUTLINETEXTMETRIC
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Native;

internal struct OUTLINETEXTMETRIC
{
  public uint otmSize;
  public TEXTMETRIC otmTextMetrics;
  public byte otmFiller;
  public PANOSE otmPanoseNumber;
  public uint otmfsSelection;
  public uint otmfsType;
  public int otmsCharSlopeRise;
  public int otmsCharSlopeRun;
  public int otmItalicAngle;
  public uint otmEMSquare;
  public int otmAscent;
  public int otmDescent;
  public uint otmLineGap;
  public uint otmsCapEmHeight;
  public uint otmsXHeight;
  public RECT otmrcFontBox;
  public int otmMacAscent;
  public int otmMacDescent;
  public uint otmMacLineGap;
  public uint otmusMinimumPPEM;
  public POINT otmptSubscriptSize;
  public POINT otmptSubscriptOffset;
  public POINT otmptSuperscriptSize;
  public POINT otmptSuperscriptOffset;
  public uint otmsStrikeoutSize;
  public int otmsStrikeoutPosition;
  public int otmsUnderscoreSize;
  public int otmsUnderscorePosition;
  public IntPtr otmpFamilyName;
  public IntPtr otmpFaceName;
  public IntPtr otmpStyleName;
  public IntPtr otmpFullName;
}
