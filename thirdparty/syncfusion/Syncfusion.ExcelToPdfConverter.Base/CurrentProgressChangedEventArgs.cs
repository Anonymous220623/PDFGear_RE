// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelToPdfConverter.CurrentProgressChangedEventArgs
// Assembly: Syncfusion.ExcelToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 4304B189-CB46-46CF-B5C1-2287263DCC93
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelToPdfConverter.Base.dll

using System;

#nullable disable
namespace Syncfusion.ExcelToPdfConverter;

public class CurrentProgressChangedEventArgs : EventArgs
{
  private float m_eventsCurrentProgress;
  private int m_noOfSheets;
  private int m_activeSheetIndex;

  public float CurrentProgressChanged
  {
    get => this.m_eventsCurrentProgress;
    set => this.m_eventsCurrentProgress = value;
  }

  public CurrentProgressChangedEventArgs(int noOfSheets, int activeSheetIndex, object source)
  {
    this.m_noOfSheets = noOfSheets;
    this.m_activeSheetIndex = activeSheetIndex;
    this.m_eventsCurrentProgress = (float) (100 / noOfSheets * (activeSheetIndex + 1));
  }
}
