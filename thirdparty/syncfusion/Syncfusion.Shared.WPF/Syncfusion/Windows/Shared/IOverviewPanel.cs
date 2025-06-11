// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.IOverviewPanel
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal interface IOverviewPanel
{
  double Scale { get; set; }

  double ZoomFactor { get; set; }

  double MinimumZoom { get; set; }

  double MaximumZoom { get; set; }

  bool IsZoomInEnabled { get; set; }

  ICommand ZoomIn { get; set; }

  bool IsZoomOutEnabled { get; set; }

  ICommand ZoomOut { get; set; }

  bool IsZoomToEnabled { get; set; }

  ICommand ZoomTo { get; set; }

  bool IsZoomResetEnabled { get; set; }

  ICommand ZoomReset { get; set; }

  ZoomMode ZoomMode { get; set; }

  bool AllowResize { get; set; }

  bool IsPanEnabled { get; set; }
}
