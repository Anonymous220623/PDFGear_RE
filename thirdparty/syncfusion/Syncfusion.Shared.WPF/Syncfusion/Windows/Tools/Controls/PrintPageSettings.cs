// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.PrintPageSettings
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

internal class PrintPageSettings
{
  public PrintPageSettings()
  {
    this.PageOrientation = PrintPageSettings.Orientation.Portrait;
    this.PageHeight = 11.0;
    this.PageWidth = 8.5;
    this.PageMargin = new Thickness(0.21);
    this.PageType = "Letter";
  }

  public PrintPageSettings.Orientation PageOrientation { get; set; }

  internal double PageWidth { get; set; }

  internal double PageHeight { get; set; }

  internal Thickness PageMargin { get; set; }

  internal string PageType { get; set; }

  internal enum Orientation
  {
    Portrait,
    Landscape,
  }
}
