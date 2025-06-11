// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelToPdfConverter.SheetBeforeDrawnEventArgs
// Assembly: Syncfusion.ExcelToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 4304B189-CB46-46CF-B5C1-2287263DCC93
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelToPdfConverter.Base.dll

using System;

#nullable disable
namespace Syncfusion.ExcelToPdfConverter;

public class SheetBeforeDrawnEventArgs : EventArgs
{
  private int m_currentSheet = -1;
  private bool m_skip;

  public int CurrentSheet
  {
    get => this.m_currentSheet;
    set => this.m_currentSheet = value;
  }

  public bool Skip
  {
    get => this.m_skip;
    set => this.m_skip = value;
  }

  public SheetBeforeDrawnEventArgs(int currentSheet, object source)
  {
    if (this.m_skip)
      return;
    this.m_currentSheet = currentSheet + 1;
  }
}
