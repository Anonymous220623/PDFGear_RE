// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.GradientStartPoint
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.ComponentModel;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class GradientStartPoint : INotifyPropertyChanged
{
  internal double x;
  internal double y;

  public double X
  {
    get => this.x;
    set
    {
      this.x = value;
      this.NotifyPropertyChanged(nameof (X));
    }
  }

  public double Y
  {
    get => this.y;
    set
    {
      this.y = value;
      this.NotifyPropertyChanged(nameof (Y));
    }
  }

  private void NotifyPropertyChanged(string p)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(p));
  }

  public event PropertyChangedEventHandler PropertyChanged;

  internal void Dispose() => this.PropertyChanged = (PropertyChangedEventHandler) null;
}
