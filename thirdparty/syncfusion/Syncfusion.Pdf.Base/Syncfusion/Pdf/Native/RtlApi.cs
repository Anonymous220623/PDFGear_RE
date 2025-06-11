// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.RtlApi
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Pdf.Native;

internal class RtlApi
{
  public const int S_OK = 0;
  public const uint E_OUTOFMEMORY = 2147942414 /*0x8007000E*/;
  public const int DefaultBuffSize = 16 /*0x10*/;
  public const uint USP_E_SCRIPT_NOT_IN_FONT = 2147746304 /*0x80040200*/;
  public const uint ScriptUndefined = 0;
  public const ushort ScriptUndefinedMask = 64512;
  public const ushort RtlLayout = 1;

  private RtlApi()
  {
  }

  [DllImport("Usp10.dll", CharSet = CharSet.Unicode)]
  internal static extern uint ScriptItemize(
    string pwcInChars,
    int cInChars,
    int cMaxItems,
    ref RtlApi.SCRIPT_CONTROL psControl,
    ref RtlApi.SCRIPT_STATE psState,
    ref RtlApi.SCRIPT_ITEM pItems,
    ref int pcItems);

  [DllImport("Usp10.dll", CharSet = CharSet.Unicode)]
  internal static extern uint ScriptItemize(
    string pwcInChars,
    int cInChars,
    int cMaxItems,
    ref RtlApi.SCRIPT_CONTROL psControl,
    ref RtlApi.SCRIPT_STATE psState,
    RtlApi.SCRIPT_ITEM pItems,
    ref int pcItems);

  [DllImport("Usp10.dll", CharSet = CharSet.Unicode)]
  internal static extern uint ScriptShape(
    IntPtr hdc,
    ref IntPtr psc,
    string pwcChars,
    int cChars,
    int cMaxGlyphs,
    ref RtlApi.SCRIPT_ANALYSIS psa,
    ref ushort pwOutGlyphs,
    ref ushort pwLogClust,
    ref RtlApi.SCRIPT_VISATTR psva,
    ref int pcGlyphs);

  [DllImport("Usp10.dll")]
  internal static extern uint ScriptLayout(
    int cRuns,
    ref byte pbLevel,
    ref int piVisualToLogical,
    ref int piLogicalToVisual);

  [DllImport("Usp10.dll")]
  internal static extern uint ScriptPlace(
    IntPtr hdc,
    ref IntPtr psc,
    ref ushort pwGlyphs,
    int cGlyphs,
    ref RtlApi.SCRIPT_VISATTR psva,
    ref RtlApi.SCRIPT_ANALYSIS psa,
    ref int piAdvance,
    ref RtlApi.GOFFSET pGoffset,
    ref ABC pABC);

  public static int Decrypt(int val, int pos, int len)
  {
    int num1 = 0;
    int y = pos;
    for (int index = pos + len; y < index; ++y)
    {
      int num2 = (int) Math.Pow(2.0, (double) y);
      int num3 = val & num2;
      num1 |= num3;
    }
    return num1;
  }

  internal struct SCRIPT_STATE
  {
    public ushort val;
  }

  internal struct SCRIPT_ITEM
  {
    public int iCharPos;
    public RtlApi.SCRIPT_ANALYSIS a;
  }

  internal struct SCRIPT_ANALYSIS
  {
    public ushort val;
    public RtlApi.SCRIPT_STATE s;
  }

  internal struct SCRIPT_CONTROL
  {
    public int val;
  }

  internal struct SCRIPT_VISATTR
  {
    public ushort val;
  }

  internal struct GOFFSET
  {
    public int du;
    public int dv;
  }
}
