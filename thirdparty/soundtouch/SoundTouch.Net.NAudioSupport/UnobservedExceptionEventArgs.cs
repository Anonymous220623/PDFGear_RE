// Decompiled with JetBrains decompiler
// Type: SoundTouch.Net.NAudioSupport.UnobservedExceptionEventArgs
// Assembly: SoundTouch.Net.NAudioSupport, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 99206FE3-DB71-4C89-91A8-76F439C9BC37
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.NAudioSupport.dll

using System;

#nullable enable
namespace SoundTouch.Net.NAudioSupport;

public class UnobservedExceptionEventArgs : EventArgs
{
  public UnobservedExceptionEventArgs(Exception exception) => this.Exception = exception;

  public Exception Exception { get; }

  public bool IsObserved { get; private set; }

  public void SetObserved() => this.IsObserved = true;
}
