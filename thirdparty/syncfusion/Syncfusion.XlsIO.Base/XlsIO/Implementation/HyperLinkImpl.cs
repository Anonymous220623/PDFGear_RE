// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.HyperLinkImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class HyperLinkImpl : CommonObject, IHyperLink, IParentApplication, ICloneParent
{
  public const string DEF_STYLE_NAME = "Hyperlink";
  private HLinkRecord m_link = (HLinkRecord) BiffRecordFactory.GetRecord(TBIFFRecord.HLink);
  private string m_strToolTip;
  private WorksheetImpl m_sheet;
  private IRange m_range;
  private IShape m_shape;
  private ExcelHyperlinkAttachedType m_attachedType;

  public HyperLinkImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
  }

  public HyperLinkImpl(IApplication application, object parent, IList data, ref int iPos)
    : this(application, parent)
  {
    iPos = this.Parse(data, iPos);
  }

  public HyperLinkImpl(IApplication application, object parent, IRange range)
    : this(application, parent)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    this.m_link.FirstRow = (uint) (range.Row - 1);
    this.m_link.FirstColumn = (uint) (range.Column - 1);
    this.m_link.LastRow = (uint) (range.LastRow - 1);
    this.m_link.LastColumn = (uint) (range.LastColumn - 1);
    this.AttachedType = ExcelHyperlinkAttachedType.Range;
  }

  public HyperLinkImpl(IApplication application, object parent, IShape shape)
    : this(application, parent)
  {
    ShapeImpl shapeImpl = shape != null ? shape as ShapeImpl : throw new ArgumentNullException(nameof (shape));
    shapeImpl.Hyperlink = (IHyperLink) this;
    this.Shape = shape;
    this.AttachedType = ExcelHyperlinkAttachedType.Shape;
    shapeImpl.IsHyperlink = true;
  }

  private void SetParents()
  {
    this.m_sheet = this.FindParent(typeof (WorksheetImpl)) as WorksheetImpl;
    if (this.m_sheet == null)
      throw new ArgumentNullException("Can't find parent worksheet");
  }

  public string Address
  {
    get
    {
      switch (this.m_link.LinkType)
      {
        case ExcelHyperLinkType.None:
          return (string) null;
        case ExcelHyperLinkType.Url:
          return this.m_link.Url;
        case ExcelHyperLinkType.File:
          return this.m_link.FileName;
        case ExcelHyperLinkType.Unc:
          return this.m_link.UncPath;
        case ExcelHyperLinkType.Workbook:
          return this.m_link.TextMark;
        default:
          throw new ArgumentOutOfRangeException("LinkType");
      }
    }
    set => this.SetAddress(value, true);
  }

  public string Name => this.m_link.Description;

  public IRange Range
  {
    get
    {
      if (this.m_range == null && this.m_attachedType != ExcelHyperlinkAttachedType.Shape)
        this.m_range = this.m_sheet.Range[(int) this.m_link.FirstRow + 1, (int) this.m_link.FirstColumn + 1, (int) this.m_link.LastRow + 1, (int) this.m_link.LastColumn + 1];
      return this.m_range;
    }
    set
    {
      this.m_link.FirstRow = (uint) (ushort) (value.Row - 1);
      this.m_link.FirstColumn = (uint) (ushort) (value.Column - 1);
      this.m_link.LastRow = (uint) (ushort) (value.LastRow - 1);
      this.m_link.LastColumn = (uint) (ushort) (value.LastColumn - 1);
      this.m_range = this.m_sheet.Range[(int) this.m_link.FirstRow + 1, (int) this.m_link.FirstColumn + 1, (int) this.m_link.LastRow + 1, (int) this.m_link.LastColumn + 1];
    }
  }

  public string ScreenTip
  {
    get => this.m_strToolTip;
    set => this.m_strToolTip = value;
  }

  public string SubAddress
  {
    get => this.m_link.TextMark;
    set
    {
      if (this.Range.CellStyleName != "Hyperlink")
        this.Range.CellStyleName = "Hyperlink";
      this.m_link.TextMark = value;
    }
  }

  public string TextToDisplay
  {
    get
    {
      if (this.Range == null)
        return (string) null;
      return this.m_link.Description != string.Empty || this.Range.Value == string.Empty || this.Range.Value == null ? this.m_link.Description.Replace("\0", "") : this.Range.Value;
    }
    set
    {
      if (!(value != this.TextToDisplay) && !(this.m_link.Description == string.Empty))
        return;
      this.m_link.Description = value;
      if (this.m_sheet.ParentWorkbook.Loading)
        return;
      this.TopLeftCell.Value2 = (object) this.m_link.Description;
    }
  }

  public ExcelHyperLinkType Type
  {
    get => this.m_link.LinkType;
    set => this.m_link.LinkType = value;
  }

  public int FirstRow => (int) this.m_link.FirstRow;

  public int FirstColumn => (int) this.m_link.FirstColumn;

  public int LastRow => (int) this.m_link.LastRow;

  public int LastColumn => (int) this.m_link.LastColumn;

  public IShape Shape
  {
    get => this.m_shape;
    internal set => this.m_shape = value;
  }

  public ExcelHyperlinkAttachedType AttachedType
  {
    get => this.m_attachedType;
    internal set
    {
      if (value == ExcelHyperlinkAttachedType.Shape)
        this.m_range = (IRange) null;
      else
        this.m_shape = (IShape) null;
      this.m_attachedType = value;
    }
  }

  private int Parse(IList data, int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (iPos < 0 || iPos > data.Count - 1)
      throw new ArgumentOutOfRangeException(nameof (iPos), "Value cannot be less than 0 and greater than data.Length - 1");
    BiffRecordRaw biffRecordRaw1 = (BiffRecordRaw) data[iPos];
    biffRecordRaw1.CheckTypeCode(TBIFFRecord.HLink);
    this.m_link = (HLinkRecord) biffRecordRaw1;
    ++iPos;
    BiffRecordRaw biffRecordRaw2 = (BiffRecordRaw) data[iPos];
    if (biffRecordRaw2.TypeCode == TBIFFRecord.QuickTip)
    {
      this.m_strToolTip = ((QuickTipRecord) biffRecordRaw2).ToolTip;
      ++iPos;
    }
    return iPos;
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    records.Add((IBiffStorage) this.m_link);
    if (this.m_strToolTip == null || this.m_strToolTip.Length <= 0)
      return;
    QuickTipRecord record = (QuickTipRecord) BiffRecordFactory.GetRecord(TBIFFRecord.QuickTip);
    record.ToolTip = this.m_strToolTip;
    record.CellRange = new TAddr((int) this.m_link.FirstRow, (int) this.m_link.FirstColumn, (int) this.m_link.LastRow, (int) this.m_link.LastColumn);
    records.Add((IBiffStorage) record);
  }

  public void SetSubAddress(string strSubAddress) => this.m_link.TextMark = strSubAddress;

  public void SetAddress(string strAddress, bool bSetText)
  {
    if (ExcelHyperlinkAttachedType.Shape != this.AttachedType && bSetText)
    {
      this.Range.CellStyle = this.m_sheet.Workbook.Styles["Hyperlink"];
      if (this.m_link.Description == null || this.m_link.Description.Length == 0 && this.Range.Value == string.Empty)
        this.TopLeftCell.Text = strAddress;
    }
    if (strAddress.IndexOf('#') == 0)
      strAddress = strAddress.Remove(0, 1);
    switch (this.m_link.LinkType)
    {
      case ExcelHyperLinkType.Url:
        this.m_link.Url = strAddress;
        break;
      case ExcelHyperLinkType.File:
        this.m_link.FileName = strAddress;
        this.m_link.IsAbsolutePathOrUrl = Path.IsPathRooted(strAddress);
        break;
      case ExcelHyperLinkType.Unc:
        this.m_link.UncPath = strAddress;
        break;
      case ExcelHyperLinkType.Workbook:
        this.m_link.TextMark = strAddress;
        break;
      default:
        throw new ArgumentOutOfRangeException("LinkType");
    }
  }

  public void Clear()
  {
    this.m_link = (HLinkRecord) null;
    this.m_range = (IRange) null;
    this.m_sheet = (WorksheetImpl) null;
    this.m_shape = (IShape) null;
    this.Dispose();
  }

  protected IRange TopLeftCell
  {
    get => this.m_sheet.Range[(int) this.m_link.FirstRow + 1, (int) this.m_link.FirstColumn + 1];
  }

  public object Clone(object parent)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    HyperLinkImpl hyperLinkImpl = (HyperLinkImpl) this.MemberwiseClone();
    hyperLinkImpl.SetParent(parent);
    hyperLinkImpl.SetParents();
    if (parent is ShapeImpl)
      hyperLinkImpl.m_shape = (IShape) (parent as ShapeImpl);
    hyperLinkImpl.m_link = (HLinkRecord) CloneUtils.CloneCloneable((ICloneable) this.m_link);
    hyperLinkImpl.m_range = (IRange) null;
    return (object) hyperLinkImpl;
  }
}
