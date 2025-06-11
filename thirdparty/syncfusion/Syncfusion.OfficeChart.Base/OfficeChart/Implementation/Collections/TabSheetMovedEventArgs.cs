// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.TabSheetMovedEventArgs
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal sealed class TabSheetMovedEventArgs : EventArgs
{
  private int m_iOldIndex;
  private int m_iNewIndex;

  private TabSheetMovedEventArgs()
  {
  }

  public TabSheetMovedEventArgs(int oldIndex, int newIndex)
  {
    this.m_iOldIndex = oldIndex;
    this.m_iNewIndex = newIndex;
  }

  public int OldIndex => this.m_iOldIndex;

  public int NewIndex => this.m_iNewIndex;
}
