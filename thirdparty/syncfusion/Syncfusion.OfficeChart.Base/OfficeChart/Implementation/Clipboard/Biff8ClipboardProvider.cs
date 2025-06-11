// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Clipboard.Biff8ClipboardProvider
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Clipboard;

internal class Biff8ClipboardProvider : ClipboardProvider
{
  public const string DEF_BIFF8_FORMAT = "Biff8";

  public Biff8ClipboardProvider()
    : this((ClipboardProvider) null)
  {
  }

  public Biff8ClipboardProvider(ClipboardProvider next)
    : base((IWorkbook) null, next)
  {
    this.FormatName = "Biff8";
  }

  public Biff8ClipboardProvider(IWorksheet sheet, ClipboardProvider next)
    : base(sheet, next)
  {
    this.FormatName = "Biff8";
  }

  protected override IWorkbook ExtractWorkbook(IDataObject dataObject, IWorkbooks workbooks)
  {
    if (dataObject != null && dataObject.GetDataPresent("Biff8", true))
    {
      object data = dataObject.GetData("Biff8", true);
      if (data != null)
      {
        Stream stream = (Stream) data;
        return workbooks.Open(stream, OfficeParseOptions.Default);
      }
    }
    return (IWorkbook) null;
  }

  protected override void FillDataObject(IDataObject dataObject)
  {
    if (dataObject == null)
      throw new ArgumentNullException(nameof (dataObject));
  }

  protected override void FillDataObject(IDataObject dataObject, IRange range)
  {
  }

  private MemoryStream GetData()
  {
    WorkbookImpl workbook = (WorkbookImpl) this.Workbook;
    OffsetArrayList records = new OffsetArrayList();
    workbook.SerializeForClipboard(records, (WorksheetImpl) this.Worksheet);
    records.UpdateBiffRecordsOffsets();
    return new MemoryStream();
  }

  public override IDataObject GetForClipboard()
  {
    return (IDataObject) new DataObject(this.FormatName, (object) this.GetData());
  }

  public override IDataObject GetForClipboard(IRange range) => (IDataObject) new DataObject();
}
