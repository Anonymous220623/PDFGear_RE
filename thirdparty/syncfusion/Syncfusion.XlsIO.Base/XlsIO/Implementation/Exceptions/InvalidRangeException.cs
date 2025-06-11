// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Exceptions.InvalidRangeException
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Exceptions;

[Serializable]
public class InvalidRangeException : ApplicationException
{
  private const string DEF_MESSAGE = "Invalid range. ";

  public InvalidRangeException()
    : base("Invalid range. ")
  {
  }

  public InvalidRangeException(string message)
    : base("Invalid range. " + message)
  {
  }

  public InvalidRangeException(string message, Exception innerException)
    : base("Invalid range. " + message, innerException)
  {
  }
}
