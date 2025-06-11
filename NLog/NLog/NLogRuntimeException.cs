// Decompiled with JetBrains decompiler
// Type: NLog.NLogRuntimeException
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace NLog;

[Serializable]
public class NLogRuntimeException : Exception
{
  public NLogRuntimeException()
  {
  }

  public NLogRuntimeException(string message)
    : base(message)
  {
  }

  [StringFormatMethod("message")]
  public NLogRuntimeException(string message, params object[] messageParameters)
    : base(string.Format(message, messageParameters))
  {
  }

  public NLogRuntimeException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  protected NLogRuntimeException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
