// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Exceptions.ParseException
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Exceptions;

[Serializable]
internal class ParseException : ArgumentException
{
  private const string DEF_MESSAGE_FORMAT = "{0}. Formula: {1}, Position: {2}";

  public ParseException()
    : base("Can't parse formula.")
  {
  }

  public ParseException(string message)
    : base(message)
  {
  }

  public ParseException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  public ParseException(string message, string formula, int position, Exception innerException)
    : this($"{message}. Formula: {formula}, Position: {position}", innerException)
  {
  }
}
