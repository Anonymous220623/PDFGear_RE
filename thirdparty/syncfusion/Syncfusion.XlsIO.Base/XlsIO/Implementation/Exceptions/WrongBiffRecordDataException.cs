// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Exceptions.WrongBiffRecordDataException
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Exceptions;

[Serializable]
public class WrongBiffRecordDataException : ApplicationException
{
  public WrongBiffRecordDataException()
    : this("")
  {
  }

  public WrongBiffRecordDataException(string message)
    : base("Wrong BiffRecord data format. " + message)
  {
  }

  public WrongBiffRecordDataException(string message, Exception innerException)
    : base(message, innerException)
  {
  }
}
