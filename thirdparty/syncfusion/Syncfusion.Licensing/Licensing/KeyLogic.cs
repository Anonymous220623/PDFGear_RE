// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.KeyLogic
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Syncfusion.Licensing;

[EditorBrowsable(EditorBrowsableState.Never)]
public class KeyLogic
{
  private BitVector64 bv = new BitVector64(0);
  private static BitVector64.Section mySect1;
  private static BitVector64.Section mySect2;
  private static BitVector64.Section mySect3;
  private static BitVector64.Section mySect4;
  private static BitVector64.Section mySect5;
  private int emailChecksum;
  private ulong prods;

  public void InitializeBitVector(int version)
  {
    KeyLogic.mySect1 = BitVector64.CreateSection(short.MaxValue);
    KeyLogic.mySect2 = BitVector64.CreateSection((short) 2, KeyLogic.mySect1);
    if (version > 15)
    {
      KeyLogic.mySect3 = BitVector64.CreateSection((short) 21, KeyLogic.mySect2);
      KeyLogic.mySect4 = BitVector64.CreateSection((short) 21, KeyLogic.mySect3);
    }
    KeyLogic.mySect5 = BitVector64.CreateSection((short) 14, KeyLogic.mySect4);
  }

  public KeyLogic(int version, byte[] bytes)
  {
    this.InitializeBitVector(version);
    this.bv = new BitVector64(BitConverter.ToInt64(bytes, 0));
    this.emailChecksum = BitConverter.ToInt32(bytes, 4);
    if (version < 9)
    {
      this.prods = (ulong) BitConverter.ToInt32(bytes, 8);
    }
    else
    {
      try
      {
        this.prods = BitConverter.ToUInt64(bytes, 8);
      }
      catch
      {
        this.prods = (ulong) BitConverter.ToInt32(bytes, 8);
      }
    }
  }

  public int EmailChecksum
  {
    get => this.emailChecksum;
    set => this.emailChecksum = value;
  }

  public ulong Prods
  {
    get => this.prods;
    set => this.prods = value;
  }

  public ulong DaysExpire
  {
    get => Convert.ToUInt64(this.bv[KeyLogic.mySect1]);
    set => this.bv[KeyLogic.mySect1] = value;
  }

  public DateTime DateExpire
  {
    get => new DateTime(2005, 1, 1).AddDays((double) this.DaysExpire);
    set => this.DaysExpire = (ulong) (value - new DateTime(2005, 1, 1)).TotalDays;
  }

  public ulong Type
  {
    get => Convert.ToUInt64(this.bv[KeyLogic.mySect2]);
    set => this.bv[KeyLogic.mySect2] = value;
  }

  public ulong V1
  {
    get => this.bv[KeyLogic.mySect3];
    set => this.bv[KeyLogic.mySect3] = value;
  }

  public ulong V2
  {
    get => this.bv[KeyLogic.mySect4];
    set => this.bv[KeyLogic.mySect4] = value;
  }

  public ulong V3
  {
    get => this.bv[KeyLogic.mySect5];
    set => this.bv[KeyLogic.mySect5] = value;
  }
}
