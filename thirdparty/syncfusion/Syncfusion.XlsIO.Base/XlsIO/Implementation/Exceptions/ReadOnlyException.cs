// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Exceptions.ReadOnlyException
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Exceptions;

[Serializable]
public class ReadOnlyException : ApplicationException
{
  private const string DEF_MESSAGE = "Can't modify read-only data. ";

  public ReadOnlyException()
    : this("Can't modify read-only data. ")
  {
  }

  public ReadOnlyException(string message)
    : base("Can't modify read-only data. " + message)
  {
  }

  public ReadOnlyException(string message, Exception innerException)
    : base("Can't modify read-only data. " + message, innerException)
  {
  }
}
