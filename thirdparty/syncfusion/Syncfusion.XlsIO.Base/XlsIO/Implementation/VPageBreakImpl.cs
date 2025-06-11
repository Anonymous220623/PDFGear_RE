// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.VPageBreakImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class VPageBreakImpl : CommonObject, IVPageBreak
{
  private VerticalPageBreaksRecord.TVPageBreak m_vPageBreak;
  private ExcelPageBreak m_type = ExcelPageBreak.PageBreakManual;
  private WorksheetImpl m_sheet;

  public IRange Location
  {
    get
    {
      return this.m_sheet.Range[(int) this.m_vPageBreak.StartRow + 1, (int) this.m_vPageBreak.Column + 1, (int) this.m_vPageBreak.EndRow + 1, (int) this.m_vPageBreak.Column + 1];
    }
    set
    {
      this.m_vPageBreak.Column = (ushort) (value.Column - 1);
      this.m_vPageBreak.StartRow = (uint) (ushort) (value.Row - 1);
      this.m_vPageBreak.EndRow = (uint) (ushort) (value.LastRow - 1);
    }
  }

  public ExcelPageBreak Type
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  public VPageBreakImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
  }

  private VPageBreakImpl(IApplication application, object parent, BiffReader reader)
    : this(application, parent)
  {
  }

  [CLSCompliant(false)]
  public VPageBreakImpl(
    IApplication application,
    object parent,
    VerticalPageBreaksRecord.TVPageBreak pagebreak)
    : this(application, parent)
  {
    this.m_vPageBreak = pagebreak;
    this.m_type = ExcelPageBreak.PageBreakManual;
  }

  public VPageBreakImpl(IApplication application, object parent, IRange location)
    : this(application, parent)
  {
    this.m_vPageBreak = new VerticalPageBreaksRecord.TVPageBreak();
    this.m_vPageBreak.Column = (ushort) (location.Column - 1);
    this.m_vPageBreak.StartRow = (uint) (ushort) (location.Row - 1);
    this.m_vPageBreak.EndRow = (uint) (ushort) (location.LastRow - 1);
  }

  [CLSCompliant(false)]
  public VerticalPageBreaksRecord.TVPageBreak VPageBreak
  {
    get
    {
      return this.m_vPageBreak != null ? this.m_vPageBreak : throw new ArgumentNullException(nameof (VPageBreak));
    }
    set => this.m_vPageBreak = value;
  }

  public int Column
  {
    get => (int) this.m_vPageBreak.Column;
    internal set => this.m_vPageBreak.Column = (ushort) value;
  }

  private void FindParents()
  {
    this.m_sheet = (WorksheetImpl) (this.FindParent(typeof (WorksheetImpl)) ?? throw new ArgumentNullException("Can't find parent worksheet"));
  }

  public VPageBreakImpl Clone(object parent)
  {
    VPageBreakImpl vpageBreakImpl = (VPageBreakImpl) this.MemberwiseClone();
    vpageBreakImpl.SetParent(parent);
    vpageBreakImpl.FindParents();
    this.m_vPageBreak = (VerticalPageBreaksRecord.TVPageBreak) this.m_vPageBreak.Clone();
    return vpageBreakImpl;
  }
}
