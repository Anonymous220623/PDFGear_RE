// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.InternalConstants
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

#nullable disable
namespace Ionic.Zlib;

internal static class InternalConstants
{
  internal static readonly int MAX_BITS = 15;
  internal static readonly int BL_CODES = 19;
  internal static readonly int D_CODES = 30;
  internal static readonly int LITERALS = 256 /*0x0100*/;
  internal static readonly int LENGTH_CODES = 29;
  internal static readonly int L_CODES = InternalConstants.LITERALS + 1 + InternalConstants.LENGTH_CODES;
  internal static readonly int MAX_BL_BITS = 7;
  internal static readonly int REP_3_6 = 16 /*0x10*/;
  internal static readonly int REPZ_3_10 = 17;
  internal static readonly int REPZ_11_138 = 18;
}
