// Decompiled with JetBrains decompiler
// Type: PDFKit.PdfViewerScrollViewer
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace PDFKit;

internal class PdfViewerScrollViewer : ScrollViewer
{
  private static Style scrollViewerStyle;
  private bool isScale;

  static PdfViewerScrollViewer()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (PdfViewerScrollViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (PdfViewerScrollViewer)));
    PdfViewerScrollViewer.scrollViewerStyle = PDFKit.Utils.StyleHelper.GetScrollViewerStyle();
  }

  public PdfViewerScrollViewer()
  {
    this.CanContentScroll = true;
    this.PanningMode = PanningMode.Both;
    this.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
    this.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
    if (PdfViewerScrollViewer.scrollViewerStyle == null)
      return;
    this.Style = PdfViewerScrollViewer.scrollViewerStyle;
  }

  protected override void OnManipulationStarting(ManipulationStartingEventArgs e)
  {
    base.OnManipulationStarting(e);
    this.isScale = false;
    e.Mode |= ManipulationModes.Scale;
  }

  protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
  {
    Vector scale = e.DeltaManipulation.Scale;
    if (scale.X != 1.0 || scale.Y != 1.0 || this.isScale)
      this.isScale = true;
    else
      base.OnManipulationDelta(e);
  }

  protected override void OnManipulationInertiaStarting(ManipulationInertiaStartingEventArgs e)
  {
    if (this.isScale)
      return;
    base.OnManipulationInertiaStarting(e);
  }

  protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
  {
    if (this.isScale)
      return;
    base.OnManipulationCompleted(e);
  }
}
