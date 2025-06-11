// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.WindowLifetimeListenerExtensions
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System.Windows;

#nullable disable
namespace CommomLib.Commom;

public static class WindowLifetimeListenerExtensions
{
  private static object locker = new object();
  private static IWindowLifetimeListener listener;

  public static IWindowLifetimeListener GetWindowLifetimeListener(this Application app)
  {
    if (app == null)
      return (IWindowLifetimeListener) null;
    if (WindowLifetimeListenerExtensions.listener == null)
    {
      lock (WindowLifetimeListenerExtensions.locker)
      {
        if (WindowLifetimeListenerExtensions.listener == null)
          WindowLifetimeListenerExtensions.listener = (IWindowLifetimeListener) new WindowLifetimeListener();
      }
    }
    return WindowLifetimeListenerExtensions.listener;
  }
}
