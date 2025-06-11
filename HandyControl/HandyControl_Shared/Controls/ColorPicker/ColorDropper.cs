// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ColorDropper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Interop;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Resources;

#nullable disable
namespace HandyControl.Controls;

internal class ColorDropper
{
  private bool _cursorIsSetted;
  private Cursor _dropperCursor;
  private readonly ColorPicker _colorPicker;

  public ColorDropper(ColorPicker colorPicker) => this._colorPicker = colorPicker;

  public void Update(bool isShow)
  {
    if (isShow)
    {
      if (this._dropperCursor == null)
      {
        StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri("pack://application:,,,/HandyControl;Component/Resources/dropper.cur"));
        if (resourceStream != null)
          this._dropperCursor = new Cursor(resourceStream.Stream);
      }
      if (this._dropperCursor == null)
        return;
      MouseHook.Start();
      MouseHook.StatusChanged += new EventHandler<MouseHookEventArgs>(this.MouseHook_StatusChanged);
    }
    else
    {
      Mouse.OverrideCursor = Cursors.Arrow;
      MouseHook.Stop();
      MouseHook.StatusChanged -= new EventHandler<MouseHookEventArgs>(this.MouseHook_StatusChanged);
    }
  }

  private void MouseHook_StatusChanged(object sender, MouseHookEventArgs e)
  {
    System.Windows.Window window = System.Windows.Window.GetWindow((DependencyObject) this._colorPicker);
    if (window == null)
      this.UpdateCursor(false);
    else if (!this._colorPicker.IsMouseOver && window.IsMouseOver)
    {
      this.UpdateCursor(true);
      if (e.MessageType != MouseHookMessageType.LeftButtonDown)
        return;
      this._colorPicker.SelectedBrush = new SolidColorBrush(ColorDropper.GetColorAt(e.Point.X, e.Point.Y));
    }
    else
      this.UpdateCursor(false);
  }

  private void UpdateCursor(bool isDropper)
  {
    if (isDropper)
    {
      Mouse.Captured?.ReleaseMouseCapture();
      if (this._cursorIsSetted)
        return;
      Mouse.OverrideCursor = this._dropperCursor;
      this._cursorIsSetted = true;
    }
    else
    {
      if (!this._cursorIsSetted)
        return;
      Mouse.OverrideCursor = Cursors.Arrow;
      this._cursorIsSetted = false;
    }
  }

  public static Color GetColorAt(int x, int y)
  {
    IntPtr desktopWindow = InteropMethods.GetDesktopWindow();
    IntPtr windowDc = InteropMethods.GetWindowDC(desktopWindow);
    int pixel = (int) InteropMethods.GetPixel(windowDc, x, y);
    InteropMethods.ReleaseDC(desktopWindow, windowDc);
    return Color.FromArgb(byte.MaxValue, (byte) (pixel & (int) byte.MaxValue), (byte) (pixel >> 8 & (int) byte.MaxValue), (byte) (pixel >> 16 /*0x10*/ & (int) byte.MaxValue));
  }
}
