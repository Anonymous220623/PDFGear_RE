// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Clipboard.Biff8ClipboardProvider
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using Syncfusion.XlsIO.Implementation.Security;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Clipboard;

public class Biff8ClipboardProvider : ClipboardProvider
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
        return workbooks.Open(stream, ExcelParseOptions.Default);
      }
    }
    return (IWorkbook) null;
  }

  protected override void FillDataObject(IDataObject dataObject)
  {
    if (dataObject == null)
      throw new ArgumentNullException(nameof (dataObject));
    dataObject.SetData(this.FormatName, (object) this.GetData((IRange) null));
  }

  protected override void FillDataObject(IDataObject dataObject, IRange range)
  {
  }

  private MemoryStream GetData(IRange range)
  {
    WorkbookImpl workbook = (WorkbookImpl) this.Workbook;
    OffsetArrayList records = new OffsetArrayList();
    workbook.SerializeForClipboard(records, (WorksheetImpl) this.Worksheet, range);
    records.UpdateBiffRecordsOffsets();
    MemoryStream data = new MemoryStream();
    using (ICompoundFile compoundFile = workbook.AppImplementation.CreateCompoundFile())
    {
      using (CompoundStream stream = compoundFile.RootStorage.CreateStream("Workbook"))
      {
        using (BiffWriter biffWriter = new BiffWriter((Stream) stream, false))
          biffWriter.WriteRecord(records, (IEncryptor) null);
        stream.Flush();
      }
      compoundFile.Save((Stream) data);
    }
    return data;
  }

  public override IDataObject GetForClipboard()
  {
    return (IDataObject) new DataObject(this.FormatName, (object) this.GetData((IRange) null));
  }

  public override IDataObject GetForClipboard(IRange range)
  {
    return (IDataObject) new DataObject(this.FormatName, (object) this.GetData(range));
  }
}
