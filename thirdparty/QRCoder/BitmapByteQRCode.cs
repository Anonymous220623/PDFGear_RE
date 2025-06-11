// Decompiled with JetBrains decompiler
// Type: QRCoder.BitmapByteQRCode
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace QRCoder;

public class BitmapByteQRCode : AbstractQRCode, IDisposable
{
  public BitmapByteQRCode()
  {
  }

  public BitmapByteQRCode(QRCodeData data)
    : base(data)
  {
  }

  public byte[] GetGraphic(int pixelsPerModule)
  {
    return this.GetGraphic(pixelsPerModule, new byte[3], new byte[3]
    {
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue
    });
  }

  public byte[] GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex)
  {
    return this.GetGraphic(pixelsPerModule, this.HexColorToByteArray(darkColorHtmlHex), this.HexColorToByteArray(lightColorHtmlHex));
  }

  public byte[] GetGraphic(int pixelsPerModule, byte[] darkColorRgb, byte[] lightColorRgb)
  {
    int inp = this.QrCodeData.ModuleMatrix.Count * pixelsPerModule;
    IEnumerable<byte> bytes1 = ((IEnumerable<byte>) darkColorRgb).Reverse<byte>();
    IEnumerable<byte> bytes2 = ((IEnumerable<byte>) lightColorRgb).Reverse<byte>();
    List<byte> byteList = new List<byte>();
    byteList.AddRange((IEnumerable<byte>) new byte[18]
    {
      (byte) 66,
      (byte) 77,
      (byte) 76,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 26,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 12,
      (byte) 0,
      (byte) 0,
      (byte) 0
    });
    byteList.AddRange((IEnumerable<byte>) this.IntTo4Byte(inp));
    byteList.AddRange((IEnumerable<byte>) this.IntTo4Byte(inp));
    byteList.AddRange((IEnumerable<byte>) new byte[4]
    {
      (byte) 1,
      (byte) 0,
      (byte) 24,
      (byte) 0
    });
    for (int index1 = inp - 1; index1 >= 0; index1 -= pixelsPerModule)
    {
      for (int index2 = 0; index2 < pixelsPerModule; ++index2)
      {
        for (int index3 = 0; index3 < inp; index3 += pixelsPerModule)
        {
          bool flag = this.QrCodeData.ModuleMatrix[(index1 + pixelsPerModule) / pixelsPerModule - 1][(index3 + pixelsPerModule) / pixelsPerModule - 1];
          for (int index4 = 0; index4 < pixelsPerModule; ++index4)
            byteList.AddRange(flag ? bytes1 : bytes2);
        }
        if (inp % 4 != 0)
        {
          for (int index5 = 0; index5 < inp % 4; ++index5)
            byteList.Add((byte) 0);
        }
      }
    }
    byteList.AddRange((IEnumerable<byte>) new byte[2]);
    return byteList.ToArray();
  }

  private byte[] HexColorToByteArray(string colorString)
  {
    if (colorString.StartsWith("#"))
      colorString = colorString.Substring(1);
    byte[] byteArray = new byte[colorString.Length / 2];
    for (int index = 0; index < byteArray.Length; ++index)
      byteArray[index] = byte.Parse(colorString.Substring(index * 2, 2), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture);
    return byteArray;
  }

  private byte[] IntTo4Byte(int inp)
  {
    byte[] numArray = new byte[2]
    {
      (byte) 0,
      (byte) (inp >> 8)
    };
    numArray[0] = (byte) inp;
    return numArray;
  }
}
