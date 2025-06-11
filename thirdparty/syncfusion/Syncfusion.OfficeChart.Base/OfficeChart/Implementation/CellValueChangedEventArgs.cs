// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.CellValueChangedEventArgs
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class CellValueChangedEventArgs : EventArgs
{
  private object m_oldValue;
  private object m_newValue;
  private IRange m_range;

  public object OldValue
  {
    get => this.m_oldValue;
    set => this.m_oldValue = value;
  }

  public object NewValue
  {
    get => this.m_newValue;
    set => this.m_newValue = value;
  }

  public IRange Range
  {
    get => this.m_range;
    set => this.m_range = value;
  }
}
