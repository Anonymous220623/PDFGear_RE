// Decompiled with JetBrains decompiler
// Type: PDFKit.TaskUnhandledExceptionEventArgs
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;

#nullable disable
namespace PDFKit;

internal class TaskUnhandledExceptionEventArgs
{
  public TaskUnhandledExceptionEventArgs(Exception ex) => this.ExceptionObject = (object) ex;

  public object ExceptionObject { get; }
}
