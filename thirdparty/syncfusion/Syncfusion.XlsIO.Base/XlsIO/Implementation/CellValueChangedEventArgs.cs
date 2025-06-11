// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.CellValueChangedEventArgs
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class CellValueChangedEventArgs : EventArgs
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
