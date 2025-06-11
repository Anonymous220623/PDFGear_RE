// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.RangeRichTextString
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class RangeRichTextString : 
  RichTextString,
  IRTFWrapper,
  IDisposable,
  IRichTextString,
  IParentApplication,
  IOptimizedUpdate
{
  private WorksheetImpl m_worksheet;

  public RangeRichTextString(IApplication application, object parent, int row, int column)
    : this(application, parent, RangeImpl.GetCellIndex(column, row))
  {
  }

  public RangeRichTextString(IApplication application, object parent, long cellIndex)
    : base(application, (object) ((WorksheetBaseImpl) parent).ParentWorkbook)
  {
    this.m_worksheet = (WorksheetImpl) parent;
    this.m_rtfParent = (object) this.m_worksheet;
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
    ExtendedFormatImpl extendedFormat = this.m_worksheet.GetExtendedFormat(this.m_lCellIndex);
    if (extendedFormat == null)
      return;
    this.m_text.DefaultFontIndex = extendedFormat.FontIndex;
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
      ExtendedFormatImpl extendedFormat = this.m_worksheet.GetExtendedFormat(this.m_lCellIndex);
      return this.m_book.InnerFonts[extendedFormat != null ? extendedFormat.FontIndex : this.m_iFontIndex] as FontImpl;
    }
    internal set
    {
      int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(this.m_lCellIndex);
      int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(this.m_lCellIndex);
      IInternalFont internalFont = this.m_book.AddFont((IFont) value) as IInternalFont;
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
          if (this.m_text.Text != " " && stringIndex != -1)
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

  internal WorksheetImpl Worksheet
  {
    get => this.m_worksheet;
    set => this.m_worksheet = value;
  }

  public void Dispose() => GC.SuppressFinalize((object) this);
}
