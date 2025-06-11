// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.DLSException
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class DLSException : Exception
{
  private const string DEF_MESSAGE = "Exception in DLS library";

  public DLSException()
    : base("Exception in DLS library")
  {
  }

  public DLSException(Exception innerExc)
    : this("Exception in DLS library", innerExc)
  {
  }

  public DLSException(string message)
    : base(message)
  {
  }

  public DLSException(string message, Exception innerExc)
    : base(message, innerExc)
  {
  }
}
