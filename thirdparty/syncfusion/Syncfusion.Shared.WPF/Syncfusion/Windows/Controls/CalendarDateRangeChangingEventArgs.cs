// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.CalendarDateRangeChangingEventArgs
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;

#nullable disable
namespace Syncfusion.Windows.Controls;

internal class CalendarDateRangeChangingEventArgs : EventArgs
{
  private DateTime _start;
  private DateTime _end;

  public CalendarDateRangeChangingEventArgs(DateTime start, DateTime end)
  {
    this._start = start;
    this._end = end;
  }

  public DateTime Start => this._start;

  public DateTime End => this._end;
}
