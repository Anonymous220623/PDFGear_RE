// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Printing.PrintQueueOption
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Printing;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.Windows.Shared.Printing;

public class PrintQueueOption : NotificationObject
{
  private PrintQueue printQueue;
  private Path imagePath;
  private bool isDefault;

  public PrintQueue PrintQueue
  {
    get => this.printQueue;
    set
    {
      if (this.printQueue == value)
        return;
      this.printQueue = value;
      this.RaisePropertyChanged(nameof (PrintQueue));
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

  public bool IsDefault
  {
    get => this.isDefault;
    set
    {
      if (this.isDefault == value)
        return;
      this.isDefault = value;
      this.RaisePropertyChanged(nameof (IsDefault));
    }
  }
}
