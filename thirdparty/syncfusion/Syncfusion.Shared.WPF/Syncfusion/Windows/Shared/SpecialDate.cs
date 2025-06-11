// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.SpecialDate
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class SpecialDate : IDisposable
{
  public DateTime Date { get; set; }

  public DataTemplate CellTemplate { get; set; }

  public void Dispose()
  {
    if (this.CellTemplate != null)
      this.CellTemplate = (DataTemplate) null;
    this.Date = new DateTime();
  }
}
