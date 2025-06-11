// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.RangeRichTextString
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class RangeRichTextString : 
  RichTextString,
  IRTFWrapper,
  IDisposable,
  IRichTextString,
  IParentApplication,
  IOptimizedUpdate
{
  private WorksheetImpl m_worksheet;
  private long m_lCellIndex;

  public RangeRichTextString(IApplication application, object parent, int row, int column)
    : this(application, parent, RangeImpl.GetCellIndex(column, row))
  {
  }

  public RangeRichTextString(IApplication application, object parent, long cellIndex)
    : base(application, (object) ((WorksheetBaseImpl) parent).ParentWorkbook)
  {
    this.m_worksheet = (WorksheetImpl) parent;
    if (cellIndex != -1L)
    {
      this.m_lCellIndex = cellIndex;
      this.m_text = this.m_worksheet.GetTextWithFormat(this.m_lCellIndex);
    }
    else
      this.m_text = this.m_worksheet.GetTextWithFormat(-1L);
    if (this.m_text == null)
      return;
    this.m_text = this.m_text.TypedClone();
  }

  public RangeRichTextString(
    IApplication application,
    object parent,
    long cellIndex,
    TextWithFormat text)
    : base(application, (object) ((WorksheetBaseImpl) parent).ParentWorkbook, true)
  {
    this.m_worksheet = (WorksheetImpl) parent;
    this.m_lCellIndex = cellIndex;
    this.m_text = text;
  }

  public override FontImpl DefaultFont
  {
    get
    {
      return this.m_book.InnerFonts[this.m_worksheet.GetExtendedFormat(this.m_lCellIndex).FontIndex] as FontImpl;
    }
    internal set
    {
      int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(this.m_lCellIndex);
      int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(this.m_lCellIndex);
      IInternalFont internalFont = this.m_book.AddFont((IOfficeFont) value) as IInternalFont;
      if (rowFromCellIndex == 0 && columnFromCellIndex == 0)
        return;
      (this.m_worksheet[rowFromCellIndex, columnFromCellIndex].CellStyle as ExtendedFormatWrapper).FontIndex = internalFont.Index;
    }
  }

  public override void BeginUpdate()
  {
    if (this.BeginCallsCount == 0)
    {
      if (this.m_text != null)
      {
        SSTDictionary innerSst = this.m_book.InnerSST;
        innerSst.Parse();
        int stringIndex = this.m_worksheet.GetStringIndex(this.m_lCellIndex);
        if (innerSst.GetStringCount(stringIndex) != 1)
        {
          this.m_text = this.m_text.TypedClone();
          if (stringIndex != -1)
            innerSst.RemoveDecrease(stringIndex);
        }
        else
          innerSst.RemoveDecrease(stringIndex);
      }
      else
        this.m_text = new TextWithFormat();
    }
    base.BeginUpdate();
  }

  public override void EndUpdate()
  {
    base.EndUpdate();
    if (this.BeginCallsCount != 0)
      return;
    this.m_worksheet.SetLabelSSTIndex(this.m_lCellIndex, this.m_book.InnerSST.AddIncrease(this.m_text.FormattingRunsCount > 0 ? (object) this.m_text : (object) this.m_text.Text));
  }

  public int Index => this.m_worksheet.GetStringIndex(this.m_lCellIndex);

  public void Dispose() => GC.SuppressFinalize((object) this);
}
