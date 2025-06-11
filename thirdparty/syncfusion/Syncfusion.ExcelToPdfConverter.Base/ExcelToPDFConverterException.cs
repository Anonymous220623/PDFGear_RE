// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelToPdfConverter.ExcelToPDFConverterException
// Assembly: Syncfusion.ExcelToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 4304B189-CB46-46CF-B5C1-2287263DCC93
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelToPdfConverter.Base.dll

using System;

#nullable disable
namespace Syncfusion.ExcelToPdfConverter;

public class ExcelToPDFConverterException(string message) : ApplicationException("Can't convert the Excel file to an PDF document.\n" + message)
{
  private const string DefaultMessage = "Can't convert the Excel file to an PDF document.\n";

  public ExcelToPDFConverterException()
    : this("Can't convert the Excel file to an PDF document.\n")
  {
  }
}
