// Decompiled with JetBrains decompiler
// Type: NLog.NLogConfigurationException
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace NLog;

[Serializable]
public class NLogConfigurationException : Exception
{
  public NLogConfigurationException()
  {
  }

  public NLogConfigurationException(string message)
    : base(message)
  {
  }

  [StringFormatMethod("message")]
  public NLogConfigurationException(string message, params object[] messageParameters)
    : base(string.Format(message, messageParameters))
  {
  }

  [StringFormatMethod("message")]
  public NLogConfigurationException(
    Exception innerException,
    string message,
    params object[] messageParameters)
    : base(string.Format(message, messageParameters), innerException)
  {
  }

  public NLogConfigurationException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  protected NLogConfigurationException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
