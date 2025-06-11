// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Clipboard.ClipboardProvider
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Windows.Forms;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Clipboard;

internal abstract class ClipboardProvider
{
  private ClipboardProvider m_clpProviderNext;
  private IWorkbook m_book;
  private IWorksheet m_sheet;
  private string m_strFormatName = string.Empty;

  protected ClipboardProvider()
  {
  }

  protected ClipboardProvider(IWorksheet worksheet)
    : this(worksheet, (ClipboardProvider) null)
  {
  }

  protected ClipboardProvider(IWorkbook workbook)
    : this(workbook, (ClipboardProvider) null)
  {
  }

  protected ClipboardProvider(IWorkbook workbook, ClipboardProvider next)
  {
    this.m_clpProviderNext = next;
    this.Initialize(workbook);
  }

  protected ClipboardProvider(IWorksheet worksheet, ClipboardProvider next)
  {
    this.m_clpProviderNext = next;
    this.Initialize(worksheet);
  }

  public ClipboardProvider Next
  {
    get => this.m_clpProviderNext;
    set => this.m_clpProviderNext = value;
  }

  public virtual string FormatName
  {
    get => this.m_strFormatName;
    set => this.m_strFormatName = value;
  }

  public IWorkbook Workbook
  {
    get => this.m_book;
    set => this.Initialize(value);
  }

  public IWorksheet Worksheet
  {
    get => this.m_sheet;
    set => this.Initialize(value);
  }

  public virtual void Initialize(IWorkbook workbook) => this.m_book = workbook;

  public virtual void Initialize(IWorksheet worksheet)
  {
    this.m_sheet = worksheet;
    if (worksheet == null)
      return;
    this.m_book = worksheet.Workbook;
  }

  public virtual void SetClipboard()
  {
    IDataObject forClipboard = this.GetForClipboard();
    for (ClipboardProvider next = this.Next; next != null; next = next.Next)
      next.FillDataObject(forClipboard);
    System.Windows.Forms.Clipboard.SetDataObject((object) forClipboard, true);
  }

  public virtual void SetClipboard(IRange range)
  {
    IDataObject forClipboard = this.GetForClipboard(range);
    for (ClipboardProvider next = this.Next; next != null; next = next.Next)
      next.FillDataObject(forClipboard, range);
    System.Windows.Forms.Clipboard.SetDataObject((object) forClipboard, true);
  }

  public virtual IWorkbook GetBookFromClipboard(IWorkbooks workbooks)
  {
    if (workbooks == null)
      throw new ArgumentNullException(nameof (workbooks));
    IDataObject dataObject = System.Windows.Forms.Clipboard.GetDataObject();
    if (dataObject == null)
      return (IWorkbook) null;
    if (dataObject.GetDataPresent(this.FormatName, true))
      return this.ExtractWorkbook(dataObject, workbooks);
    return this.Next != null ? this.Next.GetBookFromClipboard(workbooks) : (IWorkbook) null;
  }

  public abstract IDataObject GetForClipboard();

  public abstract IDataObject GetForClipboard(IRange range);

  protected abstract IWorkbook ExtractWorkbook(IDataObject dataObject, IWorkbooks workbooks);

  protected abstract void FillDataObject(IDataObject dataObject);

  protected abstract void FillDataObject(IDataObject dataObject, IRange range);
}
