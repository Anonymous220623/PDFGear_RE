// Decompiled with JetBrains decompiler
// Type: QRCoder.Framework4._0Methods.Stream4Methods
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

using System.IO;

#nullable disable
namespace QRCoder.Framework4._0Methods;

internal class Stream4Methods
{
  public static void CopyTo(Stream input, Stream output)
  {
    byte[] buffer = new byte[16384 /*0x4000*/];
    int count;
    while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
      output.Write(buffer, 0, count);
  }
}
