// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.MigrantRangeImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class MigrantRangeImpl : 
  RangeImpl,
  IMigrantRange,
  IRange,
  IParentApplication,
  IEnumerable<IRange>,
  IEnumerable
{
  private IWorksheet sheet;

  public MigrantRangeImpl(IApplication application, IWorksheet parent)
    : base(application, (object) parent)
  {
    this.sheet = parent;
  }

  public void ResetRowColumn(int iRow, int iColumn)
  {
    this.m_dataValidation = (DataValidationWrapper) null;
    this.m_rtfString = (IRTFWrapper) null;
    this.m_iTopRow = this.m_iBottomRow = iRow;
    this.m_iLeftColumn = this.m_iRightColumn = iColumn;
    this.ResetCells();
    if (this.m_style == null)
      return;
    this.m_style.SetFormatIndex((int) this.ExtendedFormatIndex);
    (this.m_style.Font as FontWrapper).ColorObject.m_color = Color.Black.ToArgb();
  }

  public void SetValue(int value)
  {
    this.sheet.SetNumber(this.m_iTopRow, this.m_iLeftColumn, (double) value);
  }

  public void SetValue(double value)
  {
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
