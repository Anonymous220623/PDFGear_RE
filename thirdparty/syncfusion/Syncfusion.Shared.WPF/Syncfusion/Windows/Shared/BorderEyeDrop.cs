// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.BorderEyeDrop
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class BorderEyeDrop : Border
{
  private bool m_bPressed;
  private int m_iLastX;
  private int m_iLastY;
  private Cursor m_cursor;
  public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof (Color), typeof (Color), typeof (BorderEyeDrop), (PropertyMetadata) new FrameworkPropertyMetadata((object) Colors.Transparent));
  public static readonly RoutedEvent BeginColorPickingEvent = EventManager.RegisterRoutedEvent("BeginColorPicking", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (BorderEyeDrop));
  public static readonly RoutedEvent EndColorPickingEvent = EventManager.RegisterRoutedEvent("EndColorPicking", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (BorderEyeDrop));
  public static readonly RoutedEvent CancelColorPickingEvent = EventManager.RegisterRoutedEvent("CancelColorPicking", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (BorderEyeDrop));

  public Color Color
  {
    get => (Color) this.GetValue(BorderEyeDrop.ColorProperty);
    set => this.SetValue(BorderEyeDrop.ColorProperty, (object) value);
  }

  protected override void OnInitialized(EventArgs e)
  {
    base.OnInitialized(e);
    Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Syncfusion.Windows.Shared.Controls.ColorPicker.Images.ColorPicker.cur");
    if (manifestResourceStream != null)
      this.m_cursor = new Cursor(manifestResourceStream);
    else
      this.m_cursor = Cursors.Pen;
  }

  protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (BrowserInteropHelper.IsBrowserHosted && !PermissionHelper.HasUnmanagedCodePermission)
      e.Handled = true;
    base.OnPreviewMouseLeftButtonDown(e);
  }

  protected override void OnMouseDown(MouseButtonEventArgs e)
  {
    this.m_bPressed = this.CaptureMouse();
    if (this.m_bPressed)
    {
      this.LostMouseCapture += new MouseEventHandler(this.BorderEyeDrop_LostMouseCapture);
      this.MouseMove += new MouseEventHandler(this.BorderEyeDrop_MouseMove);
      InputManager.Current.PostNotifyInput += new NotifyInputEventHandler(this.Current_PostNotifyInput);
      this.RaiseEvent(new RoutedEventArgs(BorderEyeDrop.BeginColorPickingEvent));
      this.Cursor = this.m_cursor;
    }
    e.Handled = true;
    base.OnMouseDown(e);
  }

  private void Current_PostNotifyInput(object sender, NotifyInputEventArgs e)
  {
    if (!(e.StagingItem.Input is KeyEventArgs input) || input.Key != Key.Escape || !this.m_bPressed)
      return;
    this.m_bPressed = false;
    this.ReleaseMouseCapture();
  }

  protected override void OnMouseUp(MouseButtonEventArgs e)
  {
    if (this.m_bPressed)
    {
      this.RaiseEvent(new RoutedEventArgs(BorderEyeDrop.EndColorPickingEvent));
      this.ReleaseMouseCapture();
      this.ClearValue(FrameworkElement.CursorProperty);
    }
    e.Handled = true;
    base.OnMouseUp(e);
  }

  private void BorderEyeDrop_MouseMove(object sender, MouseEventArgs e) => this.UpdatePixelColor();

  private void BorderEyeDrop_LostMouseCapture(object sender, MouseEventArgs e)
  {
    if (!this.m_bPressed)
      this.RaiseEvent(new RoutedEventArgs(BorderEyeDrop.CancelColorPickingEvent));
    this.m_bPressed = false;
    InputManager.Current.PostNotifyInput -= new NotifyInputEventHandler(this.Current_PostNotifyInput);
    this.LostMouseCapture -= new MouseEventHandler(this.BorderEyeDrop_LostMouseCapture);
    this.MouseMove -= new MouseEventHandler(this.BorderEyeDrop_MouseMove);
  }

  [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
  private void UpdatePixelColor()
  {
    BorderEyeDrop.Win32Point pt = new BorderEyeDrop.Win32Point(0, 0);
    BorderEyeDrop.GetCursorPos(ref pt);
    int x = pt.X;
    int y = pt.Y;
    if (x == this.m_iLastX && y == this.m_iLastY)
      return;
    this.m_iLastX = x;
    this.m_iLastY = y;
    IntPtr dc = BorderEyeDrop.CreateDC("Display", (string) null, (string) null, IntPtr.Zero);
    int pixel = BorderEyeDrop.GetPixel(dc, x, y);
    BorderEyeDrop.DeleteDC(dc);
    Color color = Color.FromArgb(byte.MaxValue, (byte) (pixel & (int) byte.MaxValue), (byte) ((pixel & 65280) >> 8), (byte) ((pixel & 16711680 /*0xFF0000*/) >> 16 /*0x10*/));
    if (!(this.Color != color))
      return;
    this.Color = color;
  }

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  internal static extern int GetCursorPos(ref BorderEyeDrop.Win32Point pt);

  [DllImport("gdi32.dll", EntryPoint = "CreateDCW", CharSet = CharSet.Unicode)]
  internal static extern IntPtr CreateDC(
    string strDriver,
    string strDevice,
    string strOutput,
    IntPtr pData);

  [DllImport("gdi32.dll")]
  internal static extern int GetPixel(IntPtr hdc, int x, int y);

  [DllImport("gdi32.dll")]
  internal static extern int DeleteDC(IntPtr hdc);

  internal struct Win32Point(int x, int y)
  {
    public int X = x;
    public int Y = y;
  }
}
