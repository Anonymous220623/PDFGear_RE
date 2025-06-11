// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECConvertPoint
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal sealed class ECConvertPoint
{
  private ECConvertPoint()
  {
  }

  public static int GetByteLength(EllipticCurveElements field) => (field.ElementSize + 7) / 8;

  public static byte[] ConvetByte(Number numberS, int qLength)
  {
    byte[] byteArrayUnsigned = numberS.ToByteArrayUnsigned();
    if (qLength < byteArrayUnsigned.Length)
    {
      byte[] destinationArray = new byte[qLength];
      Array.Copy((Array) byteArrayUnsigned, byteArrayUnsigned.Length - destinationArray.Length, (Array) destinationArray, 0, destinationArray.Length);
      return destinationArray;
    }
    if (qLength <= byteArrayUnsigned.Length)
      return byteArrayUnsigned;
    byte[] destinationArray1 = new byte[qLength];
    Array.Copy((Array) byteArrayUnsigned, 0, (Array) destinationArray1, destinationArray1.Length - byteArrayUnsigned.Length, byteArrayUnsigned.Length);
    return destinationArray1;
  }
}
