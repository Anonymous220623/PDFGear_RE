// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Exceptions.WrongBiffStreamPartException
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Exceptions;

[Serializable]
public class WrongBiffStreamPartException : ApplicationException
{
  public WrongBiffStreamPartException()
    : this("")
  {
  }

  public WrongBiffStreamPartException(string message)
    : base("Tried to parse a noncorresponding part of Biff8 stream." + message)
  {
  }

  public WrongBiffStreamPartException(string message, Exception innerException)
    : base(message, innerException)
  {
  }
}
