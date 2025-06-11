// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionParseException
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NLog.Conditions;

[Serializable]
public class ConditionParseException : Exception
{
  public ConditionParseException()
  {
  }

  public ConditionParseException(string message)
    : base(message)
  {
  }

  public ConditionParseException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  protected ConditionParseException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
