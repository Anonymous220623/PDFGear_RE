// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ErrorIndicatorImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ErrorIndicatorImpl : RangesOperations, IErrorIndicator
{
  public const int MaximumIndicatorsInRecord = 1024 /*0x0400*/;
  private ExcelIgnoreError m_options;

  public ExcelIgnoreError IgnoreOptions
  {
    get => this.m_options;
    set => this.m_options = value;
  }

  public ErrorIndicatorImpl(Rectangle rect, ExcelIgnoreError options)
    : this(options)
  {
    if (rect.Top < 0 || rect.Left < 0)
      throw new ArgumentException("Incorrect range");
    this.AddRange(rect);
  }

  public ErrorIndicatorImpl(ExcelIgnoreError option)
    : base(new List<Rectangle>())
  {
    this.m_options = option;
  }

  public ErrorIndicatorImpl Clone() => (ErrorIndicatorImpl) base.Clone();

  public void AddCells(ErrorIndicatorImpl errorIndicator)
  {
    if (errorIndicator == null)
      return;
    this.AddCells((IList<Rectangle>) errorIndicator.CellList);
  }
}
