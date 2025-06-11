// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontRealByteEncoding
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontRealByteEncoding : SystemFontByteEncoding
{
  private static Dictionary<SystemFontNibble, string> nibbleMapping = new Dictionary<SystemFontNibble, string>();
  private static SystemFontNibble endOfNumber;

  static SystemFontRealByteEncoding()
  {
    SystemFontRealByteEncoding.nibbleMapping[new SystemFontNibble((byte) 0)] = "0";
    SystemFontRealByteEncoding.nibbleMapping[new SystemFontNibble((byte) 1)] = "1";
    SystemFontRealByteEncoding.nibbleMapping[new SystemFontNibble((byte) 2)] = "2";
    SystemFontRealByteEncoding.nibbleMapping[new SystemFontNibble((byte) 3)] = "3";
    SystemFontRealByteEncoding.nibbleMapping[new SystemFontNibble((byte) 4)] = "4";
    SystemFontRealByteEncoding.nibbleMapping[new SystemFontNibble((byte) 5)] = "5";
    SystemFontRealByteEncoding.nibbleMapping[new SystemFontNibble((byte) 6)] = "6";
    SystemFontRealByteEncoding.nibbleMapping[new SystemFontNibble((byte) 7)] = "7";
    SystemFontRealByteEncoding.nibbleMapping[new SystemFontNibble((byte) 8)] = "8";
    SystemFontRealByteEncoding.nibbleMapping[new SystemFontNibble((byte) 9)] = "9";
    SystemFontRealByteEncoding.nibbleMapping[new SystemFontNibble((byte) 10)] = ".";
    SystemFontRealByteEncoding.nibbleMapping[new SystemFontNibble((byte) 11)] = "E";
    SystemFontRealByteEncoding.nibbleMapping[new SystemFontNibble((byte) 12)] = "E-";
    SystemFontRealByteEncoding.nibbleMapping[new SystemFontNibble((byte) 13)] = "";
    SystemFontRealByteEncoding.nibbleMapping[new SystemFontNibble((byte) 14)] = "-";
    SystemFontRealByteEncoding.endOfNumber = new SystemFontNibble((byte) 15);
  }

  public SystemFontRealByteEncoding()
    : base((byte) 30, (byte) 30)
  {
  }

  public override object Read(SystemFontEncodedDataReader reader)
  {
    bool flag = false;
    int num = (int) reader.Read();
    StringBuilder stringBuilder = new StringBuilder();
    do
    {
      foreach (SystemFontNibble nibble in SystemFontNibble.GetNibbles(reader.Read()))
      {
        if (nibble == SystemFontRealByteEncoding.endOfNumber)
        {
          flag = true;
          break;
        }
        stringBuilder.Append(SystemFontRealByteEncoding.nibbleMapping[nibble]);
      }
    }
    while (!flag);
    return (object) double.Parse(stringBuilder.ToString(), NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat);
  }
}
