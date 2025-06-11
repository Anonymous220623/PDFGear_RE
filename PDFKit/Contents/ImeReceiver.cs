// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.ImeReceiver
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using PDFKit.Contents.Utils;
using System;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace PDFKit.Contents;

public class ImeReceiver : IDisposable
{
  private bool disposedValue;
  private bool notFirst;
  private readonly IntPtr hwnd;
  private readonly bool refreshOnFirstCall;
  private IntPtr context;
  private IntPtr previousContext;
  private IntPtr defaultImeWindow;
  private StringBuilder charBuffer;
  private static ImeNative.LOGFONT? defaultCaptionLogFont;

  public ImeReceiver(IntPtr hwnd, bool refreshOnFirstCall = false)
  {
    this.hwnd = !(hwnd == IntPtr.Zero) ? hwnd : throw new ArgumentException(nameof (hwnd));
    this.refreshOnFirstCall = refreshOnFirstCall;
    this.charBuffer = new StringBuilder();
  }

  public void ProcessGotKeyboardFocus()
  {
    if (this.disposedValue)
      return;
    if (!this.notFirst && this.refreshOnFirstCall)
    {
      this.notFirst = true;
      this.RefreshInputMethod();
    }
    this.CreateContext();
    this.UpdateCompositionWindow();
  }

  public void ProcessLostKeyboardFocus(bool sendCancel)
  {
    if (this.disposedValue)
      return;
    if (sendCancel && this.context != IntPtr.Zero)
      ImeNative.ImmNotifyIME(this.context, 21, 4);
    this.ClearContext();
  }

  public void ProcessInputLangChangedMessage()
  {
    if (this.disposedValue)
      return;
    this.CreateContext();
  }

  public void ProcessImeCompositionMessage()
  {
    if (this.disposedValue)
      return;
    this.UpdateCompositionWindow();
  }

  public void ProcessImeCharMessage(IntPtr wParam, IntPtr lParam)
  {
    this.charBuffer.Append((char) wParam.ToInt32());
  }

  public void CompleteImeCharMessage()
  {
    string text = this.charBuffer.ToString();
    this.charBuffer.Clear();
    ImeReceiverTextReceivedEventHandler textReveived = this.TextReveived;
    if (textReveived == null)
      return;
    textReveived(this, new ImeReceiverTextReceivedEventArgs(text));
  }

  private void CreateContext()
  {
    this.ClearContext();
    this.defaultImeWindow = ImeNative.ImmGetDefaultIMEWnd(IntPtr.Zero);
    if (this.defaultImeWindow == IntPtr.Zero)
    {
      this.RefreshInputMethod();
      this.defaultImeWindow = ImeNative.ImmGetDefaultIMEWnd(IntPtr.Zero);
      if (this.defaultImeWindow == IntPtr.Zero)
        this.defaultImeWindow = ImeNative.ImmGetDefaultIMEWnd(ImeReceiver.GetForegroundWindow());
    }
    this.context = ImeNative.ImmGetContext(this.defaultImeWindow);
    if (this.context == IntPtr.Zero)
      this.context = ImeNative.ImmGetContext(this.hwnd);
    this.previousContext = ImeNative.ImmAssociateContext(this.hwnd, this.context);
    ImeNative.GetTextFrameworkThreadManager()?.SetFocus(IntPtr.Zero);
  }

  private void ClearContext()
  {
    this.charBuffer.Clear();
    ImeNative.ImmAssociateContext(this.hwnd, this.previousContext);
    ImeNative.ImmReleaseContext(this.defaultImeWindow, this.context);
    this.context = IntPtr.Zero;
    this.defaultImeWindow = IntPtr.Zero;
  }

  private void UpdateCompositionWindow()
  {
    if (this.context == IntPtr.Zero)
      return;
    ImeReceiverImeRequestedEventArgs requestedEventArgs = this.OnImeRequested();
    if (requestedEventArgs == null || string.IsNullOrEmpty(requestedEventArgs.FontFamily))
    {
      ImeNative.LOGFONT? defaultCaptionFont = ImeReceiver.GetDefaultCaptionFont();
      if (requestedEventArgs == null)
        requestedEventArgs = new ImeReceiverImeRequestedEventArgs()
        {
          FontFamily = defaultCaptionFont?.lfFaceName,
          FontSize = defaultCaptionFont.HasValue ? (double) defaultCaptionFont.GetValueOrDefault().lfHeight : 0.0
        };
      else
        requestedEventArgs.FontFamily = defaultCaptionFont?.lfFaceName;
    }
    this.SetCompositionFont(requestedEventArgs.FontFamily, -Math.Abs(requestedEventArgs.FontSize));
    this.SetCompositionWindow(requestedEventArgs.CaretPointLeftInPixel, requestedEventArgs.CaretPointTopInPixel);
  }

  private void SetCompositionFont(string fontFamily, double fontSize)
  {
    if (string.IsNullOrEmpty(fontFamily))
      return;
    ImeNative.LOGFONT font = new ImeNative.LOGFONT();
    font.lfFaceName = fontFamily;
    font.lfHeight = (int) fontSize;
    IntPtr context = this.context;
    int dwIndex = 8;
    int compositionString = ImeNative.ImmGetCompositionString(context, dwIndex, (byte[]) null, 0);
    if (compositionString > 0)
    {
      byte[] numArray = new byte[compositionString];
      if (ImeNative.ImmGetCompositionString(context, dwIndex, numArray, compositionString) > 0 && string.IsNullOrWhiteSpace(Encoding.Default.GetString(numArray)))
        font.lfWidth = 1;
    }
    ImeNative.ImmSetCompositionFont(context, ref font);
  }

  private void SetCompositionWindow(int x, int y)
  {
    ImeNative.ImmSetCompositionWindow(this.context, ref new ImeNative.CompositionForm()
    {
      dwStyle = 2,
      ptCurrentPos = {
        x = x,
        y = y
      }
    });
  }

  private void RefreshInputMethod()
  {
    EventHandler inputMethodRequested = this.RefreshInputMethodRequested;
    if (inputMethodRequested == null)
      return;
    inputMethodRequested((object) this, EventArgs.Empty);
  }

  private ImeReceiverImeRequestedEventArgs OnImeRequested()
  {
    ImeReceiverImeRequestedEventHandler imeRequested = this.ImeRequested;
    if (imeRequested == null)
      return (ImeReceiverImeRequestedEventArgs) null;
    ImeReceiverImeRequestedEventArgs args = new ImeReceiverImeRequestedEventArgs();
    imeRequested(this, args);
    return args;
  }

  public event EventHandler RefreshInputMethodRequested;

  public event ImeReceiverImeRequestedEventHandler ImeRequested;

  public event ImeReceiverTextReceivedEventHandler TextReveived;

  protected virtual void Dispose(bool disposing)
  {
    if (this.disposedValue)
      return;
    if (!disposing)
      ;
    this.disposedValue = true;
  }

  ~ImeReceiver() => this.Dispose(false);

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  private static ImeNative.LOGFONT? GetDefaultCaptionFont()
  {
    if (!ImeReceiver.defaultCaptionLogFont.HasValue)
    {
      ImeReceiver.NONCLIENTMETRICS pvParam = new ImeReceiver.NONCLIENTMETRICS();
      if (ImeReceiver.SystemParametersInfo(41, pvParam.cbSize, pvParam, 0))
        ImeReceiver.defaultCaptionLogFont = new ImeNative.LOGFONT?(pvParam.lfCaptionFont);
    }
    return ImeReceiver.defaultCaptionLogFont;
  }

  [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern IntPtr GetForegroundWindow();

  [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern bool SystemParametersInfo(
    int uiAction,
    int uiParam,
    [In, Out] ImeReceiver.NONCLIENTMETRICS pvParam,
    int fWinIni);

  private enum ImeNotify
  {
    IMN_CLOSESTATUSWINDOW = 1,
    IMN_OPENSTATUSWINDOW = 2,
    IMN_CHANGECANDIDATE = 3,
    IMN_CLOSECANDIDATE = 4,
    IMN_OPENCANDIDATE = 5,
    IMN_SETCONVERSIONMODE = 6,
    IMN_SETSENTENCEMODE = 7,
    IMN_SETOPENSTATUS = 8,
    IMN_SETCANDIDATEPOS = 9,
    IMN_SETCOMPOSITIONFONT = 10, // 0x0000000A
    IMN_SETCOMPOSITIONWINDOW = 11, // 0x0000000B
    IMN_SETSTATUSWINDOWPOS = 12, // 0x0000000C
    IMN_GUIDELINE = 13, // 0x0000000D
    IMN_PRIVATE = 14, // 0x0000000E
  }

  [StructLayout(LayoutKind.Sequential)]
  private class NONCLIENTMETRICS
  {
    public int cbSize = Marshal.SizeOf(typeof (ImeReceiver.NONCLIENTMETRICS));
    public int iBorderWidth;
    public int iScrollWidth;
    public int iScrollHeight;
    public int iCaptionWidth;
    public int iCaptionHeight;
    [MarshalAs(UnmanagedType.Struct)]
    public ImeNative.LOGFONT lfCaptionFont;
    public int iSmCaptionWidth;
    public int iSmCaptionHeight;
    [MarshalAs(UnmanagedType.Struct)]
    public ImeNative.LOGFONT lfSmCaptionFont;
    public int iMenuWidth;
    public int iMenuHeight;
    [MarshalAs(UnmanagedType.Struct)]
    public ImeNative.LOGFONT lfMenuFont;
    [MarshalAs(UnmanagedType.Struct)]
    public ImeNative.LOGFONT lfStatusFont;
    [MarshalAs(UnmanagedType.Struct)]
    public ImeNative.LOGFONT lfMessageFont;
  }
}
