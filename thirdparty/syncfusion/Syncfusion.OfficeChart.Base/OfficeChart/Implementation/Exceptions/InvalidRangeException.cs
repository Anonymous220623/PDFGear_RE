// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Exceptions.InvalidRangeException
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Exceptions;

[Serializable]
internal class InvalidRangeException : ApplicationException
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
