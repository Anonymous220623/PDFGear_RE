// Decompiled with JetBrains decompiler
// Type: Syncfusion.Lic.util.encoders.Hex
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

#nullable disable
namespace Syncfusion.Lic.util.encoders;

public class Hex
{
  private static HexTranslator encoder = new HexTranslator();

  public static byte[] decode(string data)
  {
    byte[] numArray = new byte[data.Length / 2];
    string lower = data.ToLower();
    for (int index1 = 0; index1 < lower.Length; index1 += 2)
    {
      char ch1 = lower[index1];
      char ch2 = lower[index1 + 1];
      int index2 = index1 / 2;
      numArray[index2] = ch1 >= 'a' ? (byte) ((int) ch1 - 97 + 10 << 4) : (byte) ((int) ch1 - 48 /*0x30*/ << 4);
      if (ch2 < 'a')
        numArray[index2] += (byte) ((uint) ch2 - 48U /*0x30*/);
      else
        numArray[index2] += (byte) ((int) ch2 - 97 + 10);
    }
    return numArray;
  }
}
