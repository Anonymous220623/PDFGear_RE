// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.TaskHelper
// Assembly: Nito.AsyncEx.Tasks, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A9818263-2EB2-45AD-B24E-66ECAA8EEAB5
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Tasks.dll

using System;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

public static class TaskHelper
{
  public static async Task ExecuteAsTask(Action func)
  {
    if (func == null)
      throw new ArgumentNullException(nameof (func));
    func();
  }

  public static async Task<T> ExecuteAsTask<T>(Func<T> func)
  {
    if (func == null)
      throw new ArgumentNullException(nameof (func));
    return func();
  }
}
