// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PathTextBoxPathChangedEventArgs
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;

#nullable disable
namespace pdfeditor.Controls;

public class PathTextBoxPathChangedEventArgs : EventArgs
{
  public PathTextBoxPathChangedEventArgs(string newPath, string oldPath)
  {
    this.NewPath = newPath;
    this.OldPath = oldPath;
  }

  public string NewPath { get; }

  public string OldPath { get; }
}
