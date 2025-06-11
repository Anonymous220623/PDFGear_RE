// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.StreamWriteException
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Syncfusion.DocIO;

internal class StreamWriteException : Exception
{
  private const string DEF_MESSAGE = "Incorrect writes process";

  internal StreamWriteException()
    : base("Incorrect writes process")
  {
  }

  internal StreamWriteException(Exception innerExc)
    : this("Incorrect writes process", innerExc)
  {
  }

  internal StreamWriteException(string message)
    : base(message)
  {
  }

  internal StreamWriteException(string message, Exception innerExc)
    : base(message, innerExc)
  {
  }

  internal StreamWriteException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
