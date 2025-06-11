// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ExcelUtils
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO;

public class ExcelUtils
{
  private static ExcelEngine excelEngine = new ExcelEngine();
  private static IApplication application = ExcelUtils.excelEngine.Excel;

  ~ExcelUtils() => ExcelUtils.excelEngine.Dispose();

  public static bool ThrowNotSavedOnDestroy
  {
    get => ExcelUtils.excelEngine.ThrowNotSavedOnDestroy;
    set => ExcelUtils.excelEngine.ThrowNotSavedOnDestroy = value;
  }

  public static IWorkbook CreateWorkbook(int numberOfSheets)
  {
    ExcelUtils.application.SheetsInNewWorkbook = numberOfSheets;
    return ExcelUtils.application.Workbooks.Add((string) null);
  }

  public static IWorkbook CreateWorkbook(string[] names)
  {
    IWorkbook workbook = ExcelUtils.CreateWorkbook(names.GetLength(0));
    int index = 0;
    foreach (ITabSheet worksheet in (IEnumerable<IWorksheet>) workbook.Worksheets)
    {
      worksheet.Name = names[index];
      ++index;
    }
    return workbook;
  }

  public static void CloseWorkBook() => ExcelUtils.excelEngine.Excel.Workbooks.Close();

  public static IWorkbook CreateWorkBookUsingTemplate(string templateLocation)
  {
    return ExcelUtils.excelEngine.Excel.Workbooks.Open(templateLocation);
  }

  public static IWorkbook CreateWorkBookUsingTemplate(Stream stream)
  {
    return ExcelUtils.excelEngine.Excel.Workbooks.Open(stream);
  }

  public static IWorkbook Open(string fileLocation)
  {
    return ExcelUtils.excelEngine.Excel.Workbooks.Open(fileLocation);
  }

  public static IWorkbook Open(Stream stream)
  {
    return ExcelUtils.excelEngine.Excel.Workbooks.Open(stream);
  }

  public static void Close()
  {
  }

  public static IWorkbook PasteWorkbook() => ExcelUtils.excelEngine.Excel.Workbooks.PasteWorkbook();
}
