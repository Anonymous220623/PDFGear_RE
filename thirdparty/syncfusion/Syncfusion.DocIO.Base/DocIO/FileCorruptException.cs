// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.FileCorruptException
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Syncfusion.DocIO;

public class FileCorruptException : Exception
{
  private const string DEF_MESSAGE = "Document is corrupted and impossible to load";

  public FileCorruptException()
    : base("Document is corrupted and impossible to load")
  {
  }

  public FileCorruptException(Exception innerExc)
    : this("Document is corrupted and impossible to load", innerExc)
  {
  }

  public FileCorruptException(string message)
    : base(message)
  {
  }

  public FileCorruptException(string message, Exception innerExc)
    : base(message, innerExc)
  {
  }

  public FileCorruptException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
