// Decompiled with JetBrains decompiler
// Type: QRCoder.Exceptions.DataTooLongException
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

using System;

#nullable disable
namespace QRCoder.Exceptions;

public class DataTooLongException : Exception
{
  public DataTooLongException(string eccLevel, string encodingMode, int maxSizeByte)
    : base($"The given payload exceeds the maximum size of the QR code standard. The maximum size allowed for the choosen paramters (ECC level={eccLevel}, EncodingMode={encodingMode}) is {maxSizeByte} byte.")
  {
  }

  public DataTooLongException(string eccLevel, string encodingMode, int version, int maxSizeByte)
    : base($"The given payload exceeds the maximum size of the QR code standard. The maximum size allowed for the choosen paramters (ECC level={eccLevel}, EncodingMode={encodingMode}, FixedVersion={version}) is {maxSizeByte} byte.")
  {
  }
}
