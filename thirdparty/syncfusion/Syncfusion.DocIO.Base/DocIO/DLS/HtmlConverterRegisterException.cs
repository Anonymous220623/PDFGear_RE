// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.HtmlConverterRegisterException
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class HtmlConverterRegisterException : DLSException
{
  private const string DEF_MESSAGE = "Please call HTMLConverterFactory.Register method for using HTMLConverterFactory.GetInstance()";

  public HtmlConverterRegisterException()
    : base("Please call HTMLConverterFactory.Register method for using HTMLConverterFactory.GetInstance()")
  {
  }

  public HtmlConverterRegisterException(Exception innerExc)
    : this("Please call HTMLConverterFactory.Register method for using HTMLConverterFactory.GetInstance()", innerExc)
  {
  }

  public HtmlConverterRegisterException(string message)
    : base(message)
  {
  }

  public HtmlConverterRegisterException(string message, Exception innerExc)
    : base(message, innerExc)
  {
  }
}
