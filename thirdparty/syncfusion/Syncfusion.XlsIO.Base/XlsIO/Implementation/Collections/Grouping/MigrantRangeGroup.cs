// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.Grouping.MigrantRangeGroup
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections.Grouping;

public class MigrantRangeGroup : 
  RangeGroup,
  IMigrantRange,
  IRange,
  IParentApplication,
  IEnumerable<IRange>,
  IEnumerable
{
  private IWorksheet sheet;

  public new IRange Offset(int row, int column) => throw new NotImplementedException();

  public new IRange Resize(int row, int column) => throw new NotImplementedException();

  public new IEnumerator<IRange> GetEnumerator() => throw new NotImplementedException();

  IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

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
