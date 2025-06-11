// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Printing.PrintPageSize
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.Windows.Shared.Printing;

public class PrintPageSize : NotificationObject
{
  private string pageSizeName;
  private PageSizeUnit unit;
  private Path imagePath;
  private Size size;

  public PageSizeUnit PageSizeUnit
  {
    get => this.unit;
    set
    {
      this.unit = value;
      this.RaisePropertyChanged(nameof (PageSizeUnit));
    }
  }

  public string PageSizeName
  {
    get => this.pageSizeName;
    set
    {
      this.pageSizeName = value;
      this.RaisePropertyChanged(nameof (PageSizeName));
    }
  }

  public Path ImagePath
  {
    get => this.imagePath;
    set
    {
      this.imagePath = value;
      this.RaisePropertyChanged(nameof (ImagePath));
    }
  }

  public Size Size
  {
    get => this.size;
    set
    {
      this.size = value;
      this.RaisePropertyChanged(nameof (Size));
    }
  }

  internal Size ToPixels() => PrintExtensions.CmToPixel(this.Size);
}
