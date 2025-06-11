// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.WiaException
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System;
using System.Runtime.Serialization;

#nullable enable
namespace NAPS2.Wia;

[Serializable]
public class WiaException : Exception
{
  public static void Check(uint hresult)
  {
    if (hresult != 0U)
      throw new WiaException(hresult);
  }

  protected WiaException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }

  public WiaException(uint errorCode)
    : base($"WIA error code {errorCode:X}")
  {
    this.ErrorCode = errorCode;
  }

  public uint ErrorCode { get; set; }
}
