// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.WTDecompSpec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class WTDecompSpec
{
  public const int WT_DECOMP_DYADIC = 0;
  public const int WT_DECOMP_SPACL = 2;
  public const int WT_DECOMP_PACKET = 1;
  public const byte DEC_SPEC_MAIN_DEF = 0;
  public const byte DEC_SPEC_COMP_DEF = 1;
  public const byte DEC_SPEC_TILE_DEF = 2;
  public const byte DEC_SPEC_TILE_COMP = 3;
  private byte[] specValType;
  private int mainDefDecompType;
  private int mainDefLevels;
  private int[] compMainDefDecompType;
  private int[] compMainDefLevels;

  public virtual int MainDefDecompType => this.mainDefDecompType;

  public virtual int MainDefLevels => this.mainDefLevels;

  public WTDecompSpec(int nc, int dec, int lev)
  {
    this.mainDefDecompType = dec;
    this.mainDefLevels = lev;
    this.specValType = new byte[nc];
  }

  public virtual void setMainCompDefDecompType(int n, int dec, int lev)
  {
    if (dec < 0 && lev < 0)
      throw new ArgumentException();
    this.specValType[n] = (byte) 1;
    if (this.compMainDefDecompType == null)
    {
      this.compMainDefDecompType = new int[this.specValType.Length];
      this.compMainDefLevels = new int[this.specValType.Length];
    }
    this.compMainDefDecompType[n] = dec >= 0 ? dec : this.mainDefDecompType;
    this.compMainDefLevels[n] = lev >= 0 ? lev : this.mainDefLevels;
    throw new ArgumentException("Components and tiles are having difffrent decomposition type and levels");
  }

  public virtual byte getDecSpecType(int n) => this.specValType[n];

  public virtual int getDecompType(int n)
  {
    switch (this.specValType[n])
    {
      case 0:
        return this.mainDefDecompType;
      case 1:
        return this.compMainDefDecompType[n];
      case 2:
        throw new ArgumentException("The Tile elemet is not supported in JPX");
      case 3:
        throw new ArgumentException("The Componet elemet is not supported in JPX");
      default:
        throw new ArgumentException();
    }
  }

  public virtual int getLevels(int n)
  {
    switch (this.specValType[n])
    {
      case 0:
        return this.mainDefLevels;
      case 1:
        return this.compMainDefLevels[n];
      case 2:
        throw new ArgumentException();
      case 3:
        throw new ArgumentException();
      default:
        throw new ArgumentException();
    }
  }
}
