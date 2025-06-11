// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.HidePopupEventArgs
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class HidePopupEventArgs : EventArgs
{
  private Date m_selectedDate;

  public HidePopupEventArgs(Date selectedDate) => this.SelectedDate = selectedDate;

  public Date SelectedDate
  {
    get => this.m_selectedDate;
    set => this.m_selectedDate = value;
  }
}
