// Decompiled with JetBrains decompiler
// Type: PDFLauncher.CustomControl.PathTextBoxPathChangedEventArgs
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using System;

#nullable disable
namespace PDFLauncher.CustomControl;

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
