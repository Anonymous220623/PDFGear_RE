// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.HPageBreakImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class HPageBreakImpl : CommonObject, IHPageBreak
{
  private ExcelPageBreakExtent m_extent = ExcelPageBreakExtent.PageBreakPartial;
  private ExcelPageBreak m_type = ExcelPageBreak.PageBreakManual;
  private HorizontalPageBreaksRecord.THPageBreak m_HPageBreak;
  private WorksheetImpl m_sheet;

  public ExcelPageBreakExtent Extent => this.m_extent;

  public IRange Location
  {
    get
    {
      return this.m_sheet.Range[(int) this.m_HPageBreak.Row + 1, (int) this.m_HPageBreak.StartColumn + 1, (int) this.m_HPageBreak.Row + 1, (int) this.m_HPageBreak.EndColumn + 1];
    }
    set
    {
      this.m_HPageBreak.StartColumn = (ushort) (value.Column - 1);
      this.m_HPageBreak.EndColumn = (ushort) (value.LastColumn - 1);
      this.m_HPageBreak.Row = (ushort) (value.Row - 1);
    }
  }

  public ExcelPageBreak Type
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  private HPageBreakImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
  }

  private HPageBreakImpl(IApplication application, object parent, BiffReader reader)
    : this(application, parent)
  {
  }

  [CLSCompliant(false)]
  public HPageBreakImpl(
    IApplication application,
    object parent,
    HorizontalPageBreaksRecord.THPageBreak pagebreak)
    : this(application, parent)
  {
    this.m_HPageBreak = pagebreak;
    this.m_type = ExcelPageBreak.PageBreakManual;
  }

  public HPageBreakImpl(IApplication application, object parent, IRange location)
    : this(application, parent)
  {
    this.m_HPageBreak = new HorizontalPageBreaksRecord.THPageBreak();
    this.m_HPageBreak.Row = (ushort) (location.Row - 1);
    this.m_HPageBreak.StartColumn = (ushort) (location.Column - 1);
    this.m_HPageBreak.EndColumn = (ushort) (location.LastColumn - 1);
  }

  [CLSCompliant(false)]
  public HorizontalPageBreaksRecord.THPageBreak HPageBreak
  {
    get
    {
      return this.m_HPageBreak != null ? this.m_HPageBreak : throw new ArgumentNullException(nameof (HPageBreak));
    }
    set => this.m_HPageBreak = value;
  }

  public int Row
  {
    get => (int) this.m_HPageBreak.Row;
    internal set => this.m_HPageBreak.Row = (ushort) value;
  }

  private void FindParents()
  {
    this.m_sheet = (WorksheetImpl) (this.FindParent(typeof (WorksheetImpl)) ?? throw new ArgumentNullException("Can't find parent worksheet"));
  }

  public HPageBreakImpl Clone(object parent)
  {
    HPageBreakImpl hpageBreakImpl = (HPageBreakImpl) this.MemberwiseClone();
    hpageBreakImpl.SetParent(parent);
    hpageBreakImpl.FindParents();
    this.m_HPageBreak = (HorizontalPageBreaksRecord.THPageBreak) this.m_HPageBreak.Clone();
    return hpageBreakImpl;
  }
}
