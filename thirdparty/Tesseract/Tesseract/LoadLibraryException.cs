// Decompiled with JetBrains decompiler
// Type: Tesseract.LoadLibraryException
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Tesseract;

[Serializable]
public class LoadLibraryException : SystemException
{
  public LoadLibraryException()
  {
  }

  public LoadLibraryException(string message)
    : base(message)
  {
  }

  public LoadLibraryException(string message, Exception inner)
    : base(message, inner)
  {
  }

  protected LoadLibraryException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
