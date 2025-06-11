// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Exceptions.SmallBiffRecordDataException
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Exceptions;

[Serializable]
public class SmallBiffRecordDataException : ApplicationException
{
  public SmallBiffRecordDataException()
    : this("")
  {
  }

  public SmallBiffRecordDataException(string message)
    : base("BiffRecord data is too small. " + message)
  {
  }

  public SmallBiffRecordDataException(string message, Exception innerException)
    : base(message, innerException)
  {
  }
}
