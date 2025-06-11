// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.Exceptions.LayoutException
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Syncfusion.Layouting.Exceptions;

[Serializable]
internal class LayoutException : ApplicationException
{
  private const string DEF_MESSAGE = "Incorrect layouting process";

  public LayoutException()
    : base("Incorrect layouting process")
  {
  }

  public LayoutException(Exception innerExc)
    : this("Incorrect layouting process", innerExc)
  {
  }

  public LayoutException(string message)
    : base(message)
  {
  }

  public LayoutException(string message, Exception innerExc)
    : base(message, innerExc)
  {
  }

  public LayoutException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
