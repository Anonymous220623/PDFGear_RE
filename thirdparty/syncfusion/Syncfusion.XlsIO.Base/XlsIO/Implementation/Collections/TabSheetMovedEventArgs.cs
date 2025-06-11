// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.TabSheetMovedEventArgs
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public sealed class TabSheetMovedEventArgs : EventArgs
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
