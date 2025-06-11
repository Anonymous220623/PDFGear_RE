// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.Exceptions.InvalidLayoutStateException
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.Layouting.Exceptions;

internal class InvalidLayoutStateException : LayoutException
{
  private const string DEF_MESSAGE = "Fatal error";

  public InvalidLayoutStateException()
    : base("Fatal error")
  {
  }

  public InvalidLayoutStateException(Exception innerExc)
    : this("Fatal error", innerExc)
  {
  }

  public InvalidLayoutStateException(string message)
    : base(message)
  {
  }

  public InvalidLayoutStateException(string message, Exception innerExc)
    : base(message, innerExc)
  {
  }
}
