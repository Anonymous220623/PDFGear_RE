// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.PdfControlViewStateUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Utils.Enums;
using pdfeditor.ViewModels;
using PDFKit;
using PDFKit.Utils;

#nullable disable
namespace pdfeditor.Utils;

internal static class PdfControlViewStateUtils
{
  public static PdfControlViewStateUtils.PdfViewerViewStateSnapshot CreateViewStateSnapshot(
    PdfControl pdfControl)
  {
    if (!(pdfControl?.DataContext is MainViewModel dataContext))
      return (PdfControlViewStateUtils.PdfViewerViewStateSnapshot) null;
    ScrollAnchorPointUtils.PdfViewerScrollSnapshot scrollSnapshot = ScrollAnchorPointUtils.CreateScrollSnapshot(pdfControl);
    if (scrollSnapshot == null)
      return (PdfControlViewStateUtils.PdfViewerViewStateSnapshot) null;
    return new PdfControlViewStateUtils.PdfViewerViewStateSnapshot()
    {
      ScrollSnapshot = scrollSnapshot,
      SizeModesWrap = dataContext.ViewToolbar.DocSizeModeWrap,
      SubViewModeContinuous = dataContext.ViewToolbar.SubViewModeContinuous,
      SubViewModePage = dataContext.ViewToolbar.SubViewModePage,
      Zoom = dataContext.ViewToolbar.DocZoom,
      SelectedPageIndex = dataContext.SelectedPageIndex
    };
  }

  public static void ApplyViewStateSnapshot(
    PdfControl pdfControl,
    PdfControlViewStateUtils.PdfViewerViewStateSnapshot snapshot)
  {
    if (pdfControl == null || snapshot == null || !(pdfControl.DataContext is MainViewModel dataContext))
      return;
    dataContext.ViewToolbar.DocSizeModeWrap = snapshot.SizeModesWrap;
    dataContext.ViewToolbar.SubViewModeContinuous = snapshot.SubViewModeContinuous;
    dataContext.ViewToolbar.SubViewModePage = snapshot.SubViewModePage;
    dataContext.ViewToolbar.DocZoom = snapshot.Zoom;
    pdfControl.UpdateLayout();
    ScrollAnchorPointUtils.ApplyScrollSnapshot(pdfControl, snapshot.ScrollSnapshot);
    dataContext.SelectedPageIndex = snapshot.SelectedPageIndex;
  }

  public class PdfViewerViewStateSnapshot
  {
    public ScrollAnchorPointUtils.PdfViewerScrollSnapshot ScrollSnapshot { get; set; }

    public int SelectedPageIndex { get; set; }

    public SizeModesWrap SizeModesWrap { get; set; } = SizeModesWrap.Zoom;

    public SubViewModePage SubViewModePage { get; set; }

    public SubViewModeContinuous SubViewModeContinuous { get; set; } = SubViewModeContinuous.Verticalcontinuous;

    public float Zoom { get; set; } = 1f;
  }
}
