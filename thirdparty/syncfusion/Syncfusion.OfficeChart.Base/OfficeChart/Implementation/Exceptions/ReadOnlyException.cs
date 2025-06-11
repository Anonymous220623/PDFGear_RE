// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Exceptions.ReadOnlyException
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Exceptions;

[Serializable]
internal class ReadOnlyException : ApplicationException
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
