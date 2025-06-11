// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.NameResolvedEventArgs
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;

#nullable disable
namespace HandyControl.Interactivity;

internal sealed class NameResolvedEventArgs : EventArgs
{
  public NameResolvedEventArgs(object oldObject, object newObject)
  {
    this.OldObject = oldObject;
    this.NewObject = newObject;
  }

  public object NewObject { get; }

  public object OldObject { get; }
}
