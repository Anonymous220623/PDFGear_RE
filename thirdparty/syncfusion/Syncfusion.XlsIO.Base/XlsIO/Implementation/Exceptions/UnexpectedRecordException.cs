// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Exceptions.UnexpectedRecordException
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Exceptions;

[Serializable]
public class UnexpectedRecordException : ApplicationException
{
  private const string DEF_MESSAGE = "Unexpected record.";
  private const string DEF_MESSAGE_CODE = "Unexpected record {0}.";

  public UnexpectedRecordException()
    : base("Unexpected record.")
  {
  }

  public UnexpectedRecordException(TBIFFRecord recordCode)
    : base($"Unexpected record {recordCode}.")
  {
  }

  public UnexpectedRecordException(string message)
    : base($"Unexpected record {message}.")
  {
  }

  public UnexpectedRecordException(string message, Exception innerException)
    : base(message, innerException)
  {
  }
}
