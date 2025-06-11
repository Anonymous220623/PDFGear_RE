// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.MigrantRangeImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class MigrantRangeImpl : RangeImpl, IMigrantRange, IRange, IParentApplication, IEnumerable
{
  private IWorksheet sheet;

  public MigrantRangeImpl(IApplication application, IWorksheet parent)
    : base(application, (object) parent)
  {
    this.sheet = parent;
  }

  public void ResetRowColumn(int iRow, int iColumn)
  {
    this.m_rtfString = (IRTFWrapper) null;
    this.m_iTopRow = this.m_iBottomRow = iRow;
    this.m_iLeftColumn = this.m_iRightColumn = iColumn;
    if (this.m_style == null)
      return;
    this.m_style.SetFormatIndex((int) this.ExtendedFormatIndex);
  }

  public void SetValue(int value)
  {
    if (this.NumberFormat.Contains("%"))
      value /= 100;
    this.sheet.SetNumber(this.m_iTopRow, this.m_iLeftColumn, (double) value);
  }

  public void SetValue(double value)
  {
    if (this.NumberFormat.Contains("%"))
      value /= 100.0;
    this.sheet.SetNumber(this.m_iTopRow, this.m_iLeftColumn, value);
  }

  public void SetValue(DateTime value) => this.DateTime = value;

  public void SetValue(bool value)
  {
    this.sheet.SetBoolean(this.m_iTopRow, this.m_iLeftColumn, value);
  }

  public void SetValue(string value)
  {
    this.sheet.SetText(this.m_iTopRow, this.m_iLeftColumn, value);
  }
}
