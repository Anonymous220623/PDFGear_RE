// Decompiled with JetBrains decompiler
// Type: FileWatcher.WindowMessageListener
// Assembly: FileWatcher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 60845E26-CA8C-4ACF-A930-D757CD9B4993
// Assembly location: C:\Program Files\PDFgear\FileWatcher.exe

using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace FileWatcher;

public class WindowMessageListener : NativeWindow
{
  private const string WindowCaption = "FileWatcher_37CCD8B0-9B92-435E-88A1-79102B13E510";
  private const int WM_COPYDATA = 74;

  public WindowMessageListener()
  {
    this.CreateHandle(new CreateParams()
    {
      Caption = "FileWatcher_37CCD8B0-9B92-435E-88A1-79102B13E510"
    });
  }

  protected override void WndProc(ref Message m)
  {
    if (m.Msg == 74)
    {
      string str = WindowMessageListener.ProcessCopyDataMessage(m.LParam);
      if (string.IsNullOrEmpty(str))
        return;
      try
      {
        MessageData messageData = JsonConvert.DeserializeObject<MessageData>(str);
        if (messageData != null)
        {
          MessageReceivedEventHandler messageReceived = this.MessageReceived;
          if (messageReceived != null)
            messageReceived((object) this, new MessageReceivedEventArgs(messageData));
        }
      }
      catch
      {
      }
    }
    base.WndProc(ref m);
  }

  private static string ProcessCopyDataMessage(IntPtr lParam)
  {
    if (lParam == IntPtr.Zero)
      return string.Empty;
    try
    {
      return Marshal.PtrToStructure<WindowMessageListener.COPYDATASTRUCT>(lParam).lpData ?? string.Empty;
    }
    catch
    {
    }
    return string.Empty;
  }

  internal event MessageReceivedEventHandler MessageReceived;

  private struct COPYDATASTRUCT
  {
    public IntPtr dwData;
    public int cbData;
    [MarshalAs(UnmanagedType.LPStr)]
    public string lpData;
  }
}
