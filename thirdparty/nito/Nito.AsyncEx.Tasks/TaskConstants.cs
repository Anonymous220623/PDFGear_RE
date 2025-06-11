// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.TaskConstants
// Assembly: Nito.AsyncEx.Tasks, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A9818263-2EB2-45AD-B24E-66ECAA8EEAB5
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Tasks.dll

using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

public static class TaskConstants
{
  private static readonly Task<bool> booleanTrue = Task.FromResult<bool>(true);
  private static readonly Task<int> intNegativeOne = Task.FromResult<int>(-1);

  public static Task<bool> BooleanTrue => TaskConstants.booleanTrue;

  public static Task<bool> BooleanFalse => TaskConstants<bool>.Default;

  public static Task<int> Int32Zero => TaskConstants<int>.Default;

  public static Task<int> Int32NegativeOne => TaskConstants.intNegativeOne;

  public static Task Completed => Task.CompletedTask;

  public static Task Canceled => (Task) TaskConstants<object>.Canceled;
}
