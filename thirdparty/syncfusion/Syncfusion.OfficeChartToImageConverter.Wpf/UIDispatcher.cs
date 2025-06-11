// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChartToImageConverter.UIDispatcher
// Assembly: Syncfusion.OfficeChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 82053128-0A33-4E43-8DD1-E8016B1463BC
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChartToImageConverter.Wpf.dll

using System;
using System.Windows;

#nullable disable
namespace Syncfusion.OfficeChartToImageConverter;

internal class UIDispatcher
{
  internal static Exception Execute(Action action)
  {
    Exception exceptionFromDispatcher = (Exception) null;
    if (Application.Current != null)
    {
      if (Application.Current.Dispatcher.CheckAccess())
      {
        action();
      }
      else
      {
        int num = (int) Application.Current.Dispatcher.BeginInvoke((Delegate) (() =>
        {
          try
          {
            action();
          }
          catch (Exception ex)
          {
            exceptionFromDispatcher = ex;
          }
        }), (object[]) null).Wait();
      }
    }
    return exceptionFromDispatcher;
  }
}
