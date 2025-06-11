// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.Grouping.MigrantRangeGroup
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections.Grouping;

internal class MigrantRangeGroup : RangeGroup, IMigrantRange, IRange, IParentApplication, IEnumerable
{
  private IWorksheet sheet;

  public MigrantRangeGroup(IApplication application, object parent)
    : base(application, parent)
  {
    this.sheet = parent as IWorksheet;
  }

  public void ResetRowColumn(int iRow, int iColumn)
  {
    this.m_iFirstColumn = this.m_iLastColumn = iColumn;
    this.m_iFirstRow = this.m_iLastRow = iRow;
  }

  public void SetValue(int value)
  {
    this.sheet.SetNumber(this.m_iFirstRow, this.m_iFirstColumn, (double) value);
  }

  public void SetValue(double value)
  {
    this.sheet.SetNumber(this.m_iFirstRow, this.m_iFirstColumn, value);
  }

  public void SetValue(DateTime value) => this.DateTime = value;

  public void SetValue(bool value)
  {
    this.sheet.SetBoolean(this.m_iFirstRow, this.m_iFirstColumn, value);
  }

  public void SetValue(string value)
  {
    this.sheet.SetText(this.m_iFirstRow, this.m_iFirstColumn, value);
  }
}
