// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.ScrollAnchorPointUtils
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace PDFKit.Utils;

public static class ScrollAnchorPointUtils
{
  public static ScrollAnchorPointUtils.PdfViewerScrollSnapshot CreateScrollSnapshot(
    PdfControl pdfControl)
  {
    if (pdfControl == null || pdfControl.ActualWidth == 0.0 || pdfControl.ActualHeight == 0.0)
      return (ScrollAnchorPointUtils.PdfViewerScrollSnapshot) null;
    IScrollInfo scrollInfo = !pdfControl.ActualEditing ? (IScrollInfo) pdfControl.Viewer : (IScrollInfo) pdfControl.Editor;
    return new ScrollAnchorPointUtils.PdfViewerScrollSnapshot()
    {
      HorizontalOffset = scrollInfo.HorizontalOffset,
      VerticalOffset = scrollInfo.VerticalOffset,
      CanHorizontallyScroll = scrollInfo.CanHorizontallyScroll,
      CanVerticallyScroll = scrollInfo.CanVerticallyScroll
    };
  }

  public static void ApplyScrollSnapshot(
    PdfControl pdfControl,
    ScrollAnchorPointUtils.PdfViewerScrollSnapshot snapshot)
  {
    if (pdfControl == null || snapshot == null)
      return;
    double horizontalOffset = snapshot.HorizontalOffset;
    double verticalOffset = snapshot.VerticalOffset;
    IScrollInfo scrollInfo = !pdfControl.ActualEditing ? (IScrollInfo) pdfControl.Viewer : (IScrollInfo) pdfControl.Editor;
    if (scrollInfo.CanHorizontallyScroll)
      scrollInfo.SetHorizontalOffset(horizontalOffset);
    if (!scrollInfo.CanVerticallyScroll)
      return;
    scrollInfo.SetVerticalOffset(verticalOffset);
  }

  public static ScrollAnchorPointUtils.PdfViewerZoomPointSnapshot CreateZoomPointSnapshot(
    PdfControl pdfControl,
    Point? centerPoint = null)
  {
    if (pdfControl == null || pdfControl.ActualWidth == 0.0 || pdfControl.ActualHeight == 0.0)
      return (ScrollAnchorPointUtils.PdfViewerZoomPointSnapshot) null;
    ScrollAnchorPointUtils.PdfViewerScrollSnapshot scrollSnapshot = ScrollAnchorPointUtils.CreateScrollSnapshot(pdfControl);
    if (scrollSnapshot == null)
      return (ScrollAnchorPointUtils.PdfViewerZoomPointSnapshot) null;
    Point point1 = centerPoint ?? Mouse.GetPosition((IInputElement) pdfControl);
    int num = -1;
    Point pagePoint1 = new Point();
    if (point1.X >= 0.0 && point1.X <= pdfControl.ActualWidth && point1.Y >= 0.0 && point1.Y <= pdfControl.ActualHeight)
      num = pdfControl.DeviceToPage(point1.X, point1.Y, out pagePoint1);
    Point point2 = new Point(pdfControl.ActualWidth / 2.0, pdfControl.ActualHeight / 2.0);
    Point pagePoint2;
    int page = pdfControl.DeviceToPage(point2.X, point2.Y, out pagePoint2);
    return new ScrollAnchorPointUtils.PdfViewerZoomPointSnapshot()
    {
      ScrollSnapshot = scrollSnapshot,
      ClientCenterPoint = point2,
      PageCenterPoint = pagePoint2,
      CenterPointPageIndex = page,
      ClientMousePoint = point1,
      PageMousePoint = pagePoint1,
      MousePointPageIndex = num
    };
  }

  public static void ApplyZoomScrollOffset(
    PdfControl pdfControl,
    ScrollAnchorPointUtils.PdfViewerZoomPointSnapshot snapshot)
  {
    if (pdfControl == null || snapshot == null)
      return;
    int pageIndex = snapshot.MousePointPageIndex;
    Point pagePoint = snapshot.PageMousePoint;
    Point point = snapshot.ClientMousePoint;
    if (pageIndex == -1)
    {
      pageIndex = snapshot.CenterPointPageIndex;
      pagePoint = snapshot.PageCenterPoint;
      point = snapshot.ClientCenterPoint;
    }
    IScrollInfo scrollInfo = !pdfControl.ActualEditing ? (IScrollInfo) pdfControl.Viewer : (IScrollInfo) pdfControl.Editor;
    double offset1;
    double offset2;
    if (pageIndex != -1)
    {
      Point clientPoint;
      if (!pdfControl.TryGetClientPoint(pageIndex, pagePoint, out clientPoint))
      {
        pdfControl.ScrollToPage(snapshot.MousePointPageIndex);
        pdfControl.UpdateLayout();
        if (!pdfControl.TryGetClientPoint(pageIndex, pagePoint, out clientPoint))
          return;
      }
      offset1 = scrollInfo.HorizontalOffset - point.X + clientPoint.X;
      offset2 = scrollInfo.VerticalOffset - point.Y + clientPoint.Y;
    }
    else
    {
      offset1 = snapshot.ScrollSnapshot.HorizontalOffset;
      offset2 = snapshot.ScrollSnapshot.VerticalOffset;
    }
    if (scrollInfo.CanHorizontallyScroll)
      scrollInfo.SetHorizontalOffset(offset1);
    if (!scrollInfo.CanVerticallyScroll)
      return;
    scrollInfo.SetVerticalOffset(offset2);
  }

  public class PdfViewerScrollSnapshot
  {
    public int SelectedPageIndex { get; set; }

    public double HorizontalOffset { get; set; }

    public double VerticalOffset { get; set; }

    public bool CanHorizontallyScroll { get; set; }

    public bool CanVerticallyScroll { get; set; }
  }

  public class PdfViewerZoomPointSnapshot
  {
    public ScrollAnchorPointUtils.PdfViewerScrollSnapshot ScrollSnapshot { get; set; }

    public int CenterPointPageIndex { get; set; }

    public Point ClientCenterPoint { get; set; }

    public Point PageCenterPoint { get; set; }

    public Point ClientMousePoint { get; set; }

    public Point PageMousePoint { get; set; }

    public int MousePointPageIndex { get; set; }
  }
}
