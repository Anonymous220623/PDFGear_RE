// Decompiled with JetBrains decompiler
// Type: Syncfusion.Lic.util.encoders.HexTranslator
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

#nullable disable
namespace Syncfusion.Lic.util.encoders;

public class HexTranslator
{
  private static readonly byte[] hexTable = new byte[16 /*0x10*/]
  {
    (byte) 48 /*0x30*/,
    (byte) 49,
    (byte) 50,
    (byte) 51,
    (byte) 52,
    (byte) 53,
    (byte) 54,
    (byte) 55,
    (byte) 56,
    (byte) 57,
    (byte) 97,
    (byte) 98,
    (byte) 99,
    (byte) 100,
    (byte) 101,
    (byte) 102
  };

  public int getEncodedBlockSize() => 2;
}
