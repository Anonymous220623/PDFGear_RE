// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DateTimeProperties
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

#nullable disable
namespace Syncfusion.Windows.Shared;

internal class DateTimeProperties
{
  private int _KeyPressCount;

  public int StartPosition { get; set; }

  public int Lenghth { get; set; }

  public bool? IsReadOnly { get; set; }

  public DateTimeType Type { get; set; }

  public string Content { get; set; }

  public string Pattern { get; set; }

  public int KeyPressCount
  {
    get => this._KeyPressCount;
    set => this._KeyPressCount = value;
  }

  public string MonthName { get; set; }
}
