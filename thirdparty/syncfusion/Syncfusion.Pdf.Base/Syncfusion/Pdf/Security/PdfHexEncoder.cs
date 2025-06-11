// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PdfHexEncoder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal sealed class PdfHexEncoder
{
  private static readonly IHexEncoder encoder = (IHexEncoder) new HexStringEncoder();

  public static byte[] Decode(string data)
  {
    MemoryStream outputStream = new MemoryStream((data.Length + 1) / 2);
    PdfHexEncoder.encoder.DecodeString(data, (Stream) outputStream);
    return outputStream.ToArray();
  }

  public static string DecodeString(string data)
  {
    return Encoding.UTF8.GetString(Convert.FromBase64String(data));
  }
}
