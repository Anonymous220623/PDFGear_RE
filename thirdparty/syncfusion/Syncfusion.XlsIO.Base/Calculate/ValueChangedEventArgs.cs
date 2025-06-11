// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.ValueChangedEventArgs
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.Calculate;

public class ValueChangedEventArgs : EventArgs
{
  private int col;
  private int row;
  private string val;

  public ValueChangedEventArgs(int row, int col, string value)
  {
    this.row = row;
    this.col = col;
    this.val = value;
  }

  public int ColIndex
  {
    get => this.col;
    set => this.col = value;
  }

  public int RowIndex
  {
    get => this.row;
    set => this.row = value;
  }

  public string Value
  {
    get => this.val;
    set => this.val = value;
  }
}
