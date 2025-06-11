// Decompiled with JetBrains decompiler
// Type: Tesseract.TesseractException
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Tesseract;

[Serializable]
public class TesseractException : Exception, ISerializable
{
  public TesseractException()
  {
  }

  public TesseractException(string message)
    : base(message)
  {
  }

  public TesseractException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  protected TesseractException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
