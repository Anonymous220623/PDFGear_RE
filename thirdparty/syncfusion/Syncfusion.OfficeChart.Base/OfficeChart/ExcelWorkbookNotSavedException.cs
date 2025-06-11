// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.ExcelWorkbookNotSavedException
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart;

[Serializable]
internal class ExcelWorkbookNotSavedException : ApplicationException
{
  public ExcelWorkbookNotSavedException(string message)
    : base("Excel Binary workbook was not saved. " + message)
  {
  }

  public ExcelWorkbookNotSavedException(string message, Exception innerException)
    : base(message, innerException)
  {
  }
}
