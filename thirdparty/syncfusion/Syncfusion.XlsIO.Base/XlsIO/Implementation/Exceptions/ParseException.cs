// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Exceptions.ParseException
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Exceptions;

[Serializable]
public class ParseException : ArgumentException
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
