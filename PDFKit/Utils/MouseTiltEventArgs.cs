// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.MouseTiltEventArgs
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System.Windows.Input;

#nullable disable
namespace PDFKit.Utils;

public class MouseTiltEventArgs : MouseWheelEventArgs
{
  private static int _delta;

  public new int Delta => MouseTiltEventArgs._delta;

  public MouseTiltEventArgs(MouseDevice mouse, int timestamp, int delta)
    : base(mouse, timestamp, delta)
  {
    MouseTiltEventArgs._delta = delta;
  }
}
