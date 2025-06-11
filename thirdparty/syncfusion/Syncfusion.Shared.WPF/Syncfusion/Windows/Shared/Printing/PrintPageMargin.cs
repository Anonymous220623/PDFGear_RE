// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Printing.PrintPageMargin
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared.Printing;

public class PrintPageMargin : NotificationObject
{
  private Thickness thickness;
  private string marginName;
  private string imagePath;

  public string MarginName
  {
    get => this.marginName;
    set
    {
      this.marginName = value;
      this.RaisePropertyChanged(nameof (MarginName));
    }
  }

  public string ImagePath
  {
    get => this.imagePath;
    set
    {
      this.imagePath = value;
      this.RaisePropertyChanged(nameof (ImagePath));
    }
  }

  public Thickness Thickness
  {
    get => this.thickness;
    set
    {
      this.thickness = value;
      this.RaisePropertyChanged(nameof (Thickness));
    }
  }
}
