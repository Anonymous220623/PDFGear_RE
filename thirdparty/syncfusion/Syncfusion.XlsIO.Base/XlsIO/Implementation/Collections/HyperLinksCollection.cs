// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.HyperLinksCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Exceptions;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class HyperLinksCollection : 
  CollectionBaseEx<HyperLinkImpl>,
  IHyperLinks,
  IParentApplication,
  ICloneParent
{
  private WorkbookImpl m_book;
  private bool m_bReadOnly;
  internal Dictionary<long, System.Collections.Generic.List<HyperLinkImpl>> m_dicCellToList = new Dictionary<long, System.Collections.Generic.List<HyperLinkImpl>>();
  private System.Collections.Generic.List<IHyperLink> m_listHyperlinks = new System.Collections.Generic.List<IHyperLink>();

  public HyperLinksCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
    this.CreateHyperlinkStyles();
    this.Removed += new CollectionBaseEx<HyperLinkImpl>.CollectionChange(this.HyperLinksCollection_Removed);
  }

  public HyperLinksCollection(IApplication application, object parent, bool isReadOnly)
    : this(application, parent)
  {
    this.m_bReadOnly = isReadOnly;
    if (!this.m_bReadOnly)
      this.m_listHyperlinks = new System.Collections.Generic.List<IHyperLink>();
    this.SetParents();
    this.CreateHyperlinkStyles();
  }

  private void SetParents()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("Can't find parent workbook.");
  }

  public IHyperLink this[int index]
  {
    get
    {
      return index >= 0 && index <= this.Count - 1 ? (IHyperLink) this.List[index] : throw new ArgumentOutOfRangeException(nameof (index), "Value cannot be less than 0 and greater than Count - 1");
    }
  }

  public new bool IsReadOnly => this.m_bReadOnly;

  public IHyperLink Add(IRange range)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    if (this.m_bReadOnly)
      throw new ReadOnlyException();
    HyperLinkImpl hyperLink = this.AppImplementation.CreateHyperLink((object) this, range);
    int count = range.Hyperlinks.Count;
    if (count > 0 && ((range.Hyperlinks[count - 1] as HyperLinkImpl).Range as RangeImpl).IsSingleCell)
      this.Remove(range.Hyperlinks[count - 1] as HyperLinkImpl);
    this.Add((IHyperLink) hyperLink);
    this.AddToHash(hyperLink);
    return (IHyperLink) hyperLink;
  }

  public IHyperLink Add(
    IRange range,
    ExcelHyperLinkType hyperlinkType,
    string address,
    string screenTip)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    if (this.m_bReadOnly)
      throw new ReadOnlyException();
    HyperLinkImpl hyperLink = this.AppImplementation.CreateHyperLink((object) this, range);
    int count = range.Hyperlinks.Count;
    if (count > 0 && ((range.Hyperlinks[count - 1] as HyperLinkImpl).Range as RangeImpl).IsSingleCell)
      this.Remove(range.Hyperlinks[count - 1] as HyperLinkImpl);
    hyperLink.Type = hyperlinkType;
    hyperLink.SetAddress(address, false);
    hyperLink.ScreenTip = screenTip;
    this.Add((IHyperLink) hyperLink);
    this.AddToHash(hyperLink);
    return (IHyperLink) hyperLink;
  }

  public IHyperLink Add(IShape shape)
  {
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    if (this.m_bReadOnly)
      throw new ReadOnlyException();
    HyperLinkImpl hyperLink = this.AppImplementation.CreateHyperLink((object) this, shape);
    this.Add((IHyperLink) hyperLink);
    return (IHyperLink) hyperLink;
  }

  public IHyperLink Add(
    IShape shape,
    ExcelHyperLinkType hyperlinkType,
    string address,
    string screenTip)
  {
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    if (this.m_bReadOnly)
      throw new ReadOnlyException();
    HyperLinkImpl hyperLink = this.AppImplementation.CreateHyperLink((object) this, shape);
    hyperLink.Type = hyperlinkType;
    hyperLink.SetAddress(address, false);
    hyperLink.ScreenTip = screenTip;
    this.Add((IHyperLink) hyperLink);
    return (IHyperLink) hyperLink;
  }

  public new void RemoveAt(int index)
  {
    HyperLinkImpl hyperLinkImpl = index >= 0 && index <= this.Count - 1 ? base[index] : throw new ArgumentOutOfRangeException(nameof (index), "Value cannot be less than 0 and greater than Count - 1");
    if (this.m_dicCellToList.Count == 0 && hyperLinkImpl.Range != null)
    {
      WorksheetImpl worksheet = hyperLinkImpl.Range.Worksheet as WorksheetImpl;
      int index1 = (worksheet.HyperLinks as HyperLinksCollection).IndexOf(hyperLinkImpl);
      (worksheet.HyperLinks as HyperLinksCollection).RemoveAt(index1);
    }
    if (hyperLinkImpl.AttachedType == ExcelHyperlinkAttachedType.Range)
    {
      hyperLinkImpl.Range.Clear(ExcelClearOptions.ClearFormat);
    }
    else
    {
      ShapeImpl shape = hyperLinkImpl.Shape as ShapeImpl;
      shape.Hyperlink = (IHyperLink) null;
      shape.IsHyperlink = false;
    }
    base.RemoveAt(index);
  }

  internal void RemoveHyperlinksWithoutClearingFormat(int index)
  {
    base.RemoveAt(this.IndexOf(base[index]));
  }

  public int Add(IHyperLink link)
  {
    if (link == null)
      throw new ArgumentNullException(nameof (link));
    if (this.m_bReadOnly)
    {
      if (this.m_listHyperlinks.Contains(link))
        return -1;
      this.m_listHyperlinks.Add(link);
    }
    this.Add(link as HyperLinkImpl);
    return this.Count - 1;
  }

  public int Parse(IList data, int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (iPos < 0 || iPos > data.Count - 1)
      throw new ArgumentOutOfRangeException(nameof (iPos), "Value cannot be less than 0 and greater than data.Count - 1");
    for (BiffRecordRaw biffRecordRaw = (BiffRecordRaw) data[iPos]; biffRecordRaw.TypeCode == TBIFFRecord.HLink; biffRecordRaw = (BiffRecordRaw) data[iPos])
    {
      HyperLinkImpl link = new HyperLinkImpl(this.Application, (object) this, data, ref iPos);
      this.Add((IHyperLink) link);
      this.AddToHash(link);
    }
    return iPos;
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.List[index].Serialize(records);
  }

  public void CreateHyperlinkStyles()
  {
    if (this.m_book.InnerStyles.ContainsName("Hyperlink"))
      return;
    StyleImpl builtInStyle = this.m_book.InnerStyles.CreateBuiltInStyle("Hyperlink");
    builtInStyle.IncludeAlignment = false;
    builtInStyle.IncludeBorder = false;
    builtInStyle.IncludePatterns = false;
    builtInStyle.IncludeProtection = false;
    builtInStyle.IncludeNumberFormat = false;
    IFont font = builtInStyle.Font;
    font.Underline = ExcelUnderline.Single;
    font.RGBColor = ColorExtension.FromArgb(353217);
  }

  public HyperLinksCollection GetRangeHyperlinks(IRange range)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    if (this.m_bReadOnly)
      throw new NotSupportedException("This operation is not supported for read-only Hyperlinks collections");
    HyperLinksCollection rangeHyperlinks = new HyperLinksCollection(this.Application, (object) range, true);
    int row = range.Row;
    int column = range.Column;
    int lastRow = range.LastRow;
    int lastColumn = range.LastColumn;
    for (int firstRow = row; firstRow <= lastRow; ++firstRow)
    {
      for (int firstColumn = column; firstColumn <= lastColumn; ++firstColumn)
      {
        System.Collections.Generic.List<HyperLinkImpl> collection;
        if (this.m_dicCellToList.TryGetValue(RangeImpl.GetCellIndex(firstColumn, firstRow), out collection) && collection.Count > 0)
          rangeHyperlinks.AddRange((IList<HyperLinkImpl>) collection);
      }
    }
    return rangeHyperlinks;
  }

  public void AddToHash(HyperLinkImpl link)
  {
    if (link == null)
      throw new ArgumentNullException(nameof (link));
    int num1 = link.FirstRow + 1;
    int num2 = link.FirstColumn + 1;
    int num3 = link.LastRow + 1;
    int num4 = link.LastColumn + 1;
    for (int firstRow = num1; firstRow <= num3; ++firstRow)
    {
      for (int firstColumn = num2; firstColumn <= num4; ++firstColumn)
      {
        long cellIndex = RangeImpl.GetCellIndex(firstColumn, firstRow);
        System.Collections.Generic.List<HyperLinkImpl> hyperLinkImplList;
        if (!this.m_dicCellToList.TryGetValue(cellIndex, out hyperLinkImplList))
        {
          hyperLinkImplList = new System.Collections.Generic.List<HyperLinkImpl>();
          this.m_dicCellToList[cellIndex] = hyperLinkImplList;
        }
        hyperLinkImplList.Add(link);
      }
    }
  }

  public void AddRange(IList<HyperLinkImpl> collection)
  {
    if (collection == null)
      throw new ArgumentNullException(nameof (collection));
    foreach (IHyperLink link in (IEnumerable<HyperLinkImpl>) collection)
      this.Add(link);
  }

  public IHyperLink GetHyperlinkByCellIndex(long lCellIndex)
  {
    System.Collections.Generic.List<HyperLinkImpl> hyperLinkImplList;
    return this.m_dicCellToList.TryGetValue(lCellIndex, out hyperLinkImplList) && hyperLinkImplList.Count > 0 ? (IHyperLink) hyperLinkImplList[hyperLinkImplList.Count - 1] : (IHyperLink) null;
  }

  public override object Clone(object parent)
  {
    HyperLinksCollection parent1 = parent != null ? (HyperLinksCollection) base.Clone(parent) : throw new ArgumentNullException(nameof (parent));
    parent1.m_bReadOnly = this.m_bReadOnly;
    foreach (KeyValuePair<long, System.Collections.Generic.List<HyperLinkImpl>> dicCellTo in this.m_dicCellToList)
    {
      System.Collections.Generic.List<HyperLinkImpl> hyperLinkImplList = CloneUtils.CloneCloneable<HyperLinkImpl>((IList<HyperLinkImpl>) dicCellTo.Value, (object) parent1);
      parent1.m_dicCellToList.Add(dicCellTo.Key, hyperLinkImplList);
    }
    return (object) parent1;
  }

  internal void ClearAll()
  {
    if (this.m_listHyperlinks != null)
    {
      foreach (HyperLinkImpl listHyperlink in this.m_listHyperlinks)
        listHyperlink.Clear();
      this.m_listHyperlinks.Clear();
      this.m_listHyperlinks = (System.Collections.Generic.List<IHyperLink>) null;
    }
    if (this.m_dicCellToList != null)
    {
      foreach (long key in this.m_dicCellToList.Keys)
      {
        System.Collections.Generic.List<HyperLinkImpl> dicCellTo = this.m_dicCellToList[key];
        foreach (HyperLinkImpl hyperLinkImpl in dicCellTo)
          hyperLinkImpl.Clear();
        dicCellTo.Clear();
      }
      this.m_dicCellToList.Clear();
      this.m_dicCellToList = (Dictionary<long, System.Collections.Generic.List<HyperLinkImpl>>) null;
    }
    this.m_book = (WorkbookImpl) null;
  }

  internal void Remove(IHyperLink link)
  {
    if (link == null)
      throw new ArgumentNullException(nameof (link));
    if (this.m_bReadOnly)
    {
      if (!this.m_listHyperlinks.Contains(link))
        return;
      this.m_listHyperlinks.Remove(link);
    }
    this.Remove(link as HyperLinkImpl);
  }

  private void HyperLinksCollection_Removed(
    object sender,
    CollectionChangeEventArgs<HyperLinkImpl> args)
  {
    HyperLinkImpl hyperLinkImpl = args.Value;
    if (hyperLinkImpl == null)
      throw new ArgumentNullException("link");
    if (hyperLinkImpl.AttachedType != ExcelHyperlinkAttachedType.Range)
      return;
    IRange range = hyperLinkImpl.Range;
    int row = range.Row;
    int column = range.Column;
    int lastRow = range.LastRow;
    int lastColumn = range.LastColumn;
    for (int firstRow = row; firstRow <= lastRow; ++firstRow)
    {
      for (int firstColumn = column; firstColumn <= lastColumn; ++firstColumn)
      {
        System.Collections.Generic.List<HyperLinkImpl> hyperLinkImplList;
        if (this.m_dicCellToList.TryGetValue(RangeImpl.GetCellIndex(firstColumn, firstRow), out hyperLinkImplList))
          hyperLinkImplList.Remove(hyperLinkImpl);
      }
    }
  }
}
