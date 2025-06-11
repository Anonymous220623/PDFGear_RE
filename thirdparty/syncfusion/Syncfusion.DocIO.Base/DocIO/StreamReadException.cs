// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.StreamReadException
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Syncfusion.DocIO;

internal class StreamReadException : Exception
{
  private const string DEF_MESSAGE = "Was unable to read sufficient bytes from the stream";

  internal StreamReadException()
    : base("Was unable to read sufficient bytes from the stream")
  {
  }

  internal StreamReadException(Exception innerExc)
    : this("Was unable to read sufficient bytes from the stream", innerExc)
  {
  }

  internal StreamReadException(string message)
    : base(message)
  {
  }

  internal StreamReadException(string message, Exception innerExc)
    : base(message, innerExc)
  {
  }

  internal StreamReadException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
