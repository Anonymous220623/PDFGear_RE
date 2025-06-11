// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.FormulaParsingEventArgs
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.Calculate;

public class FormulaParsingEventArgs : EventArgs
{
  private string text;

  public FormulaParsingEventArgs(string text) => this.text = text;

  public FormulaParsingEventArgs()
  {
  }

  public string Text
  {
    get => this.text;
    set => this.text = value;
  }
}
