// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.MonthChangedEventArgs
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class MonthChangedEventArgs : EventArgs
{
  public MonthChangedEventArgs()
  {
  }

  public MonthChangedEventArgs(int oldMonth, int newMonth, object source)
  {
    this.PreviousMonth = oldMonth;
    this.NewMonth = newMonth;
    this.Source = source;
  }

  public int PreviousMonth { get; set; }

  public int NewMonth { get; set; }

  public object Source { get; set; }
}
