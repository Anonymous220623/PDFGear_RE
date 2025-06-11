// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Viewer.HoverOperationModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using PDFKit;
using System.Windows.Input;

#nullable disable
namespace pdfeditor.Models.Viewer;

internal class HoverOperationModel(PdfViewer viewer) : DataOperationModel(viewer)
{
  protected override void OnMouseDown(MouseButtonEventArgs e)
  {
    base.OnMouseDown(e);
    e.Handled = true;
    if (e.ChangedButton != MouseButton.Right)
      return;
    this.OnCompleted(false, false);
  }

  protected override void OnMouseUp(MouseButtonEventArgs e)
  {
    base.OnMouseUp(e);
    e.Handled = true;
    if (e.ChangedButton != MouseButton.Left || this.CurrentPage == -1)
      return;
    this.OnCompleted(true, true);
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    base.OnPreviewKeyDown(e);
    if (e.Key == Key.Escape)
    {
      e.Handled = true;
      this.OnCompleted(false, false);
    }
    else
    {
      if (e.Key != Key.Return || this.CurrentPage == -1)
        return;
      e.Handled = true;
      this.OnCompleted(true, true);
    }
  }
}
