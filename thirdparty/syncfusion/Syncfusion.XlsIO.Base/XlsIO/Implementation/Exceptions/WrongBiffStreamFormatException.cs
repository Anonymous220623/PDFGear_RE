// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Exceptions.WrongBiffStreamFormatException
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Exceptions;

[Serializable]
public class WrongBiffStreamFormatException : ApplicationException
{
  public WrongBiffStreamFormatException()
    : this("")
  {
  }

  public WrongBiffStreamFormatException(string message)
    : base("Wrong format of biff8 file structures." + message)
  {
  }

  public WrongBiffStreamFormatException(string message, Exception innerException)
    : base(message, innerException)
  {
  }
}
