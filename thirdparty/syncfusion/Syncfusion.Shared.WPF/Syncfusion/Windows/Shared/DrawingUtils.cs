// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DrawingUtils
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

public static class DrawingUtils
{
  private static readonly System.Windows.Point EMPTY_POINT = new System.Windows.Point(0.0, 0.0);
  private static bool wfcHostFlag = false;
  private static FrameworkElement m_winhost;

  public static DrawingBrush PrepareFake(UIElement mainElement)
  {
    System.Windows.Point safePointToScreen = PermissionHelper.GetSafePointToScreen((Visual) mainElement, DrawingUtils.EMPTY_POINT);
    return new DrawingBrush((System.Windows.Media.Drawing) DrawingUtils.GetDrawingGroup(mainElement, safePointToScreen));
  }

  public static void PrepareFake(FrameworkElement mainElement, Border fake)
  {
    System.Windows.Point safePointToScreen = PermissionHelper.GetSafePointToScreen((Visual) mainElement, DrawingUtils.EMPTY_POINT);
    DrawingUtils.m_winhost = mainElement;
    DrawingGroup drawingGroup = DrawingUtils.GetDrawingGroup((UIElement) mainElement, safePointToScreen);
    if (DrawingUtils.m_winhost != null)
      fake.FlowDirection = DrawingUtils.m_winhost.FlowDirection;
    fake.Height = mainElement.ActualHeight;
    fake.Width = mainElement.ActualWidth;
    fake.Margin = mainElement.Margin;
    fake.Background = (System.Windows.Media.Brush) new DrawingBrush((System.Windows.Media.Drawing) drawingGroup);
  }

  public static Bitmap GetScreenShot(IntPtr winHandle, System.Windows.Size bounds)
  {
    if (winHandle == IntPtr.Zero)
      throw new ArgumentException("Window handle is invalid!");
    Bitmap screenShot = (Bitmap) null;
    IntPtr dc = NativeMethods.GetDC(winHandle);
    IntPtr compatibleDc = NativeMethods.CreateCompatibleDC(dc);
    int width = (int) bounds.Width;
    int height = (int) bounds.Height;
    IntPtr compatibleBitmap = NativeMethods.CreateCompatibleBitmap(dc, width, height);
    if (compatibleBitmap != IntPtr.Zero)
    {
      IntPtr bmp = NativeMethods.SelectObject(compatibleDc, compatibleBitmap);
      NativeMethods.BitBlt(compatibleDc, 0, 0, width, height, dc, 0, 0, 13369376);
      NativeMethods.SelectObject(compatibleDc, bmp);
      NativeMethods.DeleteDC(compatibleDc);
      NativeMethods.ReleaseDC(winHandle, dc);
      screenShot = System.Drawing.Image.FromHbitmap(compatibleBitmap);
      NativeMethods.DeleteObject(compatibleBitmap);
    }
    return screenShot;
  }

  private static DrawingGroup GetDrawingGroup(UIElement element, System.Windows.Point mainPoint)
  {
    List<System.Windows.Media.Drawing> drawingList = new List<System.Windows.Media.Drawing>();
    System.Windows.Media.Drawing drawing = DrawingUtils.GetDrawing(element, mainPoint);
    if (!element.GetType().Name.Equals("DockedElementTabbedHost") && drawing != null)
      drawingList.Add(drawing);
    DrawingUtils.wfcHostFlag = false;
    DrawingUtils.GetDrawingTree((DependencyObject) element, drawingList, mainPoint, true);
    DrawingGroup drawingGroup = new DrawingGroup();
    int index = 0;
    for (int count = drawingList.Count; index < count; ++index)
    {
      if (drawingList[index] != null)
        drawingGroup.Children.Add(drawingList[index]);
    }
    return drawingGroup;
  }

  private static System.Windows.Media.Drawing GetDrawing(UIElement element, System.Windows.Point mainPoint)
  {
    System.Windows.Point safePointToScreen = PermissionHelper.GetSafePointToScreen((Visual) element, DrawingUtils.EMPTY_POINT);
    System.Windows.Point setPoint = new System.Windows.Point(safePointToScreen.X - mainPoint.X, safePointToScreen.Y - mainPoint.Y);
    return !(element is HwndHost) ? DrawingUtils.GetDrawingUseDrawingVisual(element, setPoint) : DrawingUtils.GetDrawingUseVisualTreeHelper(element, setPoint);
  }

  private static System.Windows.Media.Drawing GetDrawingUseDrawingVisual(
    UIElement element,
    System.Windows.Point setPoint)
  {
    DrawingVisual drawingVisual = new DrawingVisual();
    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
    {
      VisualBrush visualBrush = new VisualBrush((Visual) element);
      drawingContext.DrawRectangle((System.Windows.Media.Brush) visualBrush, (System.Windows.Media.Pen) null, new Rect(setPoint, element.RenderSize));
    }
    return (System.Windows.Media.Drawing) drawingVisual.Drawing;
  }

  private static System.Windows.Media.Drawing GetDrawingUseVisualTreeHelper(
    UIElement element,
    System.Windows.Point setPoint)
  {
    System.Windows.Media.Drawing visualTreeHelper = (System.Windows.Media.Drawing) null;
    DrawingGroup drawing = VisualTreeHelper.GetDrawing((Visual) element);
    if (drawing != null)
      visualTreeHelper = (System.Windows.Media.Drawing) new ImageDrawing(((ImageDrawing) drawing.Children[0]).ImageSource, new Rect(setPoint, element.RenderSize));
    return visualTreeHelper;
  }

  private static bool GetDrawingTree(
    DependencyObject element,
    List<System.Windows.Media.Drawing> drawingList,
    System.Windows.Point mainPoint,
    bool isFirstBranch)
  {
    bool flag1 = false;
    int childrenCount = VisualTreeHelper.GetChildrenCount(element);
    List<FrameworkElement> frameworkElementList = new List<FrameworkElement>();
    for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
    {
      if (VisualTreeHelper.GetChild(element, childIndex) is FrameworkElement child)
      {
        flag1 |= child is HwndHost;
        frameworkElementList.Add(child);
      }
    }
    bool flag2 = false;
    int count = frameworkElementList.Count;
    List<System.Windows.Media.Drawing> drawingList1 = new List<System.Windows.Media.Drawing>();
    for (int index = 0; index < count; ++index)
    {
      FrameworkElement element1 = frameworkElementList[index];
      System.Windows.Media.Drawing drawing1 = DrawingUtils.GetDrawing((UIElement) element1, mainPoint);
      if (element1 is HwndHost)
      {
        FrameworkElement frameworkElement = element as FrameworkElement;
        if (DrawingUtils.m_winhost != null && frameworkElement != null && DrawingUtils.m_winhost.FlowDirection != frameworkElement.FlowDirection)
        {
          DrawingGroup drawingGroup = new DrawingGroup();
          drawingGroup.Children.Add(drawing1);
          drawingGroup.Transform = (Transform) new ScaleTransform()
          {
            ScaleX = -1.0
          };
          System.Windows.Point safePointToScreen = PermissionHelper.GetSafePointToScreen((Visual) element1, DrawingUtils.EMPTY_POINT);
          Rect rect = new Rect(new System.Windows.Point(safePointToScreen.X - mainPoint.X, safePointToScreen.Y - mainPoint.Y), element1.RenderSize);
          System.Windows.Media.Drawing drawing2 = (System.Windows.Media.Drawing) new ImageDrawing((ImageSource) new DrawingImage((System.Windows.Media.Drawing) drawingGroup), rect);
          drawingList.Add(drawing2);
        }
        else
          drawingList.Add(drawing1);
        DrawingUtils.wfcHostFlag = true;
      }
      if (!flag1 && !DrawingUtils.wfcHostFlag)
        flag2 |= DrawingUtils.GetDrawingTree((DependencyObject) element1, drawingList1, mainPoint, false);
    }
    if (isFirstBranch || flag2)
      drawingList.AddRange((IEnumerable<System.Windows.Media.Drawing>) drawingList1);
    return flag1 || flag2;
  }
}
