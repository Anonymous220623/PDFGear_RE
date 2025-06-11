// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Exceptions.UnexpectedRecordException
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Exceptions;

[Serializable]
internal class UnexpectedRecordException : ApplicationException
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
