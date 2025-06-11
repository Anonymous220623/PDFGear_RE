// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Extension.IDialogResultable`1
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;

#nullable disable
namespace HandyControl.Tools.Extension;

public interface IDialogResultable<T>
{
  T Result { get; set; }

  Action CloseAction { get; set; }
}
