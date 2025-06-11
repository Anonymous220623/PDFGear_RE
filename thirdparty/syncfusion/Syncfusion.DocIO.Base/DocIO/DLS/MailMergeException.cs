// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.MailMergeException
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class MailMergeException : Exception
{
  private const string DEF_MESSAGE = "Incorrect syntax of mail merge fields";

  public MailMergeException()
    : base("Incorrect syntax of mail merge fields")
  {
  }

  public MailMergeException(Exception innerExc)
    : this("Incorrect syntax of mail merge fields", innerExc)
  {
  }

  public MailMergeException(string message)
    : base(message)
  {
  }

  public MailMergeException(string message, Exception innerExc)
    : base(message, innerExc)
  {
  }
}
