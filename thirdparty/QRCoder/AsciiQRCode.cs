// Decompiled with JetBrains decompiler
// Type: QRCoder.AsciiQRCode
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace QRCoder;

public class AsciiQRCode : AbstractQRCode, IDisposable
{
  public AsciiQRCode()
  {
  }

  public AsciiQRCode(QRCodeData data)
    : base(data)
  {
  }

  public string GetGraphic(
    int repeatPerModule,
    string darkColorString = "██",
    string whiteSpaceString = "  ",
    bool drawQuietZones = true,
    string endOfLine = "\n")
  {
    return string.Join(endOfLine, this.GetLineByLineGraphic(repeatPerModule, darkColorString, whiteSpaceString, drawQuietZones));
  }

  public string[] GetLineByLineGraphic(
    int repeatPerModule,
    string darkColorString = "██",
    string whiteSpaceString = "  ",
    bool drawQuietZones = true)
  {
    List<string> stringList = new List<string>();
    int num1 = drawQuietZones ? 0 : 8;
    int num2 = (int) ((double) num1 * 0.5);
    int num3 = darkColorString.Length / 2 != 1 ? darkColorString.Length / 2 : 0;
    int num4 = repeatPerModule + num3;
    int num5 = (this.QrCodeData.ModuleMatrix.Count - num1) * num4;
    for (int index1 = 0; index1 < num5; ++index1)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index2 = 0; index2 < this.QrCodeData.ModuleMatrix.Count - num1; ++index2)
      {
        bool flag = this.QrCodeData.ModuleMatrix[index2 + num2][(index1 + num4) / num4 - 1 + num2];
        for (int index3 = 0; index3 < repeatPerModule; ++index3)
          stringBuilder.Append(flag ? darkColorString : whiteSpaceString);
      }
      stringList.Add(stringBuilder.ToString());
    }
    return stringList.ToArray();
  }
}
