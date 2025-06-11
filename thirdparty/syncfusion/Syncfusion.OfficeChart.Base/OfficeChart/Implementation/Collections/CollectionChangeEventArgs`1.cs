// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.CollectionChangeEventArgs`1
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class CollectionChangeEventArgs<T> : EventArgs
{
  private int m_iIndex;
  private T m_value;

  private CollectionChangeEventArgs()
  {
  }

  public CollectionChangeEventArgs(int index, T value)
  {
    this.m_iIndex = index;
    this.m_value = value;
  }

  public int Index => this.m_iIndex;

  public T Value => this.m_value;
}
