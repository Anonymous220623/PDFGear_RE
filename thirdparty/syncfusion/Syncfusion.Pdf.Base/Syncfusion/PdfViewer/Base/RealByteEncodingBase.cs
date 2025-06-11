// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.RealByteEncodingBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class RealByteEncodingBase : ByteEncodingBase
{
  private static Dictionary<Nibble, string> nibbleMapping = new Dictionary<Nibble, string>();
  private static Nibble endOfNumber;

  static RealByteEncodingBase()
  {
    RealByteEncodingBase.nibbleMapping[new Nibble((byte) 0)] = "0";
    RealByteEncodingBase.nibbleMapping[new Nibble((byte) 1)] = "1";
    RealByteEncodingBase.nibbleMapping[new Nibble((byte) 2)] = "2";
    RealByteEncodingBase.nibbleMapping[new Nibble((byte) 3)] = "3";
    RealByteEncodingBase.nibbleMapping[new Nibble((byte) 4)] = "4";
    RealByteEncodingBase.nibbleMapping[new Nibble((byte) 5)] = "5";
    RealByteEncodingBase.nibbleMapping[new Nibble((byte) 6)] = "6";
    RealByteEncodingBase.nibbleMapping[new Nibble((byte) 7)] = "7";
    RealByteEncodingBase.nibbleMapping[new Nibble((byte) 8)] = "8";
    RealByteEncodingBase.nibbleMapping[new Nibble((byte) 9)] = "9";
    RealByteEncodingBase.nibbleMapping[new Nibble((byte) 10)] = ".";
    RealByteEncodingBase.nibbleMapping[new Nibble((byte) 11)] = "E";
    RealByteEncodingBase.nibbleMapping[new Nibble((byte) 12)] = "E-";
    RealByteEncodingBase.nibbleMapping[new Nibble((byte) 13)] = "";
    RealByteEncodingBase.nibbleMapping[new Nibble((byte) 14)] = "-";
    RealByteEncodingBase.endOfNumber = new Nibble((byte) 15);
  }

  public RealByteEncodingBase()
    : base((byte) 30, (byte) 30)
  {
  }

  public override object Read(EncodedDataParser reader)
  {
    bool flag = false;
    int num = (int) reader.Read();
    StringBuilder stringBuilder = new StringBuilder();
    do
    {
      foreach (Nibble nibble in Nibble.GetNibbles(reader.Read()))
      {
        if (nibble == RealByteEncodingBase.endOfNumber)
        {
          flag = true;
          break;
        }
        stringBuilder.Append(RealByteEncodingBase.nibbleMapping[nibble]);
      }
    }
    while (!flag);
    return (object) double.Parse(stringBuilder.ToString(), NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat);
  }
}
