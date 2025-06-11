// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.VisibleDate
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

#nullable disable
namespace Syncfusion.Windows.Shared;

public struct VisibleDate(int year, int month, int day)
{
  public int VisibleYear = year;
  public int VisibleDay = day;
  public int VisibleMonth = month;
}
