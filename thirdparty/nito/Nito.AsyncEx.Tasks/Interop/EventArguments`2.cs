// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.Interop.EventArguments`2
// Assembly: Nito.AsyncEx.Tasks, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A9818263-2EB2-45AD-B24E-66ECAA8EEAB5
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Tasks.dll

#nullable enable
namespace Nito.AsyncEx.Interop;

public struct EventArguments<TSender, TEventArgs>
{
  public TSender Sender { get; set; }

  public TEventArgs EventArgs { get; set; }
}
