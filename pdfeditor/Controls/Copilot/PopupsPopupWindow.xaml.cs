// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Copilot.Popups.PopupWindow
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Utils;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.Copilot.Popups;

public partial class PopupWindow : Window
{
  private static ImageSource defaultAppIcon;
  private HwndSource hwndSource;
  private Grid WindowIcon;
  private System.Windows.Controls.Image WindowDefaultIconImage;

  static PopupWindow()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (PopupWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (PopupWindow)));
    Window.IconProperty.OverrideMetadata(typeof (PopupWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(PopupWindow.OnIconPropertyChanged)));
  }

  private static void OnIconPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is PopupWindow popupWindow))
      return;
    popupWindow.UpdateIcon();
  }

  private static ImageSource EnsureDefaultAppIcon()
  {
    if (PopupWindow.defaultAppIcon == null)
    {
      try
      {
        using (System.Drawing.Icon associatedIcon = System.Drawing.Icon.ExtractAssociatedIcon(Process.GetCurrentProcess().MainModule.FileName))
        {
          using (Bitmap bitmap = Bitmap.FromHicon(associatedIcon.Handle))
          {
            Int32Rect sourceRect = new Int32Rect(0, 0, bitmap.Width, bitmap.Height);
            IntPtr num = IntPtr.Zero;
            try
            {
              num = bitmap.GetHbitmap();
              PopupWindow.defaultAppIcon = (ImageSource) System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(num, IntPtr.Zero, sourceRect, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
              try
              {
                if (num != IntPtr.Zero)
                  NativeMethods.DeleteObject(num);
              }
              catch
              {
              }
            }
          }
        }
      }
      catch
      {
      }
    }
    return PopupWindow.defaultAppIcon;
  }

  public PopupWindow()
  {
    WindowChrome.SetWindowChrome((Window) this, new WindowChrome()
    {
      NonClientFrameEdges = NonClientFrameEdges.Left | NonClientFrameEdges.Right | NonClientFrameEdges.Bottom,
      UseAeroCaptionButtons = false,
      CaptionHeight = 32.0,
      GlassFrameThickness = new Thickness(1.0),
      ResizeBorderThickness = new Thickness(8.0, 4.0, 8.0, 8.0)
    });
    this.CommandBindings.Add(new CommandBinding((ICommand) SystemCommands.CloseWindowCommand, new ExecutedRoutedEventHandler(this.CloseWindow)));
    this.Loaded += new RoutedEventHandler(this.PopupWindow_Loaded);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    if (this.WindowIcon != null)
      this.WindowIcon.MouseDown -= new MouseButtonEventHandler(this.WindowIcon_MouseDown);
    if (this.WindowDefaultIconImage != null)
      this.WindowDefaultIconImage.Source = (ImageSource) null;
    this.WindowIcon = this.GetTemplateChild("WindowIcon") as Grid;
    this.WindowDefaultIconImage = this.GetTemplateChild("WindowDefaultIconImage") as System.Windows.Controls.Image;
    if (this.WindowIcon != null)
      this.WindowIcon.MouseDown += new MouseButtonEventHandler(this.WindowIcon_MouseDown);
    if (this.WindowDefaultIconImage == null)
      return;
    this.WindowDefaultIconImage.Source = PopupWindow.EnsureDefaultAppIcon();
  }

  private void WindowIcon_MouseDown(object sender, MouseButtonEventArgs e)
  {
    if (e.ClickCount == 2)
    {
      SystemCommands.CloseWindow((Window) this);
    }
    else
    {
      NativeMethods.RECT lpRect;
      if (this.hwndSource == null || !NativeMethods.GetWindowRect(this.hwndSource.Handle, out lpRect))
        return;
      DpiScale dpi = VisualTreeHelper.GetDpi((Visual) this);
      SystemCommands.ShowSystemMenu((Window) this, new System.Windows.Point(this.FlowDirection != FlowDirection.LeftToRight ? (double) lpRect.right / dpi.PixelsPerDip - 3.0 : (double) lpRect.left / dpi.PixelsPerDip + 3.0, (double) lpRect.top / dpi.PixelsPerDip + 32.0));
    }
  }

  private void PopupWindow_Loaded(object sender, RoutedEventArgs e)
  {
    PopupWindow.UpdateWindowFrame(this);
  }

  protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
  {
    base.OnDpiChanged(oldDpi, newDpi);
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) (() => PopupWindow.UpdateWindowFrame(this)));
  }

  private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
  {
    SystemCommands.CloseWindow((Window) this);
  }

  protected override void OnSourceInitialized(EventArgs e)
  {
    base.OnSourceInitialized(e);
    this.hwndSource = (HwndSource) PresentationSource.FromVisual((Visual) this);
    this.hwndSource.AddHook(new HwndSourceHook(this.OnWindowProc));
    int int32 = NativeMethods.GetWindowLongPtr(this.hwndSource.Handle, -16).ToInt32();
    int exStyle = 0;
    PopupWindow.DisableTitleBar(ref int32, ref exStyle);
    NativeMethods.SetWindowLongPtr(this.hwndSource.Handle, -16, new IntPtr(int32));
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) (() => PopupWindow.UpdateWindowFrame(this)));
  }

  private static void UpdateWindowFrame(PopupWindow window)
  {
    if (VisualTreeHelper.GetChildrenCount((DependencyObject) window) > 0 && VisualTreeHelper.GetChild((DependencyObject) window, 0) is FrameworkElement child)
    {
      double width = child.Width;
      child.Width = 0.0;
      child.InvalidateMeasure();
      child.UpdateLayout();
      child.Width = width;
    }
    else
    {
      window.InvalidateMeasure();
      window.UpdateLayout();
    }
  }

  private void UpdateIcon()
  {
  }

  private unsafe IntPtr OnWindowProc(
    IntPtr hwnd,
    int msg,
    IntPtr wParam,
    IntPtr lParam,
    ref bool handled)
  {
    switch (msg)
    {
      case 125:
        if (wParam.ToInt32() == -16)
        {
          PopupWindow.StyleStruct* pointer = (PopupWindow.StyleStruct*) lParam.ToPointer();
          int styleNew = pointer->styleNew;
          int exStyle = 0;
          PopupWindow.DisableTitleBar(ref styleNew, ref exStyle);
          pointer->styleNew = styleNew;
          handled = true;
          return IntPtr.Zero;
        }
        break;
      case 132:
        try
        {
          if (lParam.ToInt64() > (long) int.MaxValue)
          {
            handled = true;
            break;
          }
          break;
        }
        catch (OverflowException ex)
        {
          handled = true;
          break;
        }
    }
    return IntPtr.Zero;
  }

  private static void DisableTitleBar(ref int style, ref int exStyle) => style &= -12779521;

  private struct StyleStruct
  {
    public int styleOld;
    public int styleNew;
  }
}
