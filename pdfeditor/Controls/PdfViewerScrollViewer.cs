// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PdfViewerScrollViewer
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace pdfeditor.Controls;

internal class PdfViewerScrollViewer : ScrollViewer
{
  private bool isScale;

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
