// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelToPdfConverter.SheetAfterDrawnEventArgs
// Assembly: Syncfusion.ExcelToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 4304B189-CB46-46CF-B5C1-2287263DCC93
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelToPdfConverter.Base.dll

using System;

#nullable disable
namespace Syncfusion.ExcelToPdfConverter;

public class SheetAfterDrawnEventArgs : EventArgs
{
  private int m_afterSheet = -1;

  public int AfterSheet => this.m_afterSheet;

  public SheetAfterDrawnEventArgs(int afterSheet, object source)
  {
    this.m_afterSheet = afterSheet + 1;
  }
}
