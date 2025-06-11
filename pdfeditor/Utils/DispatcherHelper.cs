// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.DispatcherHelper
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Text;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Utils;

public static class DispatcherHelper
{
  public static Dispatcher UIDispatcher { get; private set; }

  public static void CheckBeginInvokeOnUI(Action action)
  {
    if (action == null)
      return;
    DispatcherHelper.CheckDispatcher();
    if (DispatcherHelper.UIDispatcher.CheckAccess())
      action();
    else
      DispatcherHelper.UIDispatcher.BeginInvoke((Delegate) action);
  }

  private static void CheckDispatcher()
  {
    if (DispatcherHelper.UIDispatcher == null)
    {
      StringBuilder stringBuilder = new StringBuilder("The DispatcherHelper is not initialized.");
      stringBuilder.AppendLine();
      stringBuilder.Append("Call DispatcherHelper.Initialize() in the static App constructor.");
      throw new InvalidOperationException(stringBuilder.ToString());
    }
  }

  public static DispatcherOperation RunAsync(Action action)
  {
    DispatcherHelper.CheckDispatcher();
    return DispatcherHelper.UIDispatcher.BeginInvoke((Delegate) action);
  }

  public static void Initialize()
  {
    if (DispatcherHelper.UIDispatcher != null && DispatcherHelper.UIDispatcher.Thread.IsAlive)
      return;
    DispatcherHelper.UIDispatcher = Dispatcher.CurrentDispatcher;
  }

  public static void Reset() => DispatcherHelper.UIDispatcher = (Dispatcher) null;
}
