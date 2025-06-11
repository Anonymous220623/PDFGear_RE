// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.LOGFONT
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class LOGFONT
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
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32 /*0x20*/)]
  public byte[] lfFaceName = new byte[32 /*0x20*/];
}
