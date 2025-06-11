// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.QuickValueSetEventArgs
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.Calculate;

public class QuickValueSetEventArgs : EventArgs
{
  private FormulaInfoSetAction action;
  private string id;
  private string val;

  public QuickValueSetEventArgs(string key, string value, FormulaInfoSetAction action)
  {
    this.id = key;
    this.val = value;
    this.action = action;
  }

  public FormulaInfoSetAction Action
  {
    get => this.action;
    set => this.action = value;
  }

  public string Key
  {
    get => this.id;
    set => this.id = value;
  }

  public string Value
  {
    get => this.val;
    set => this.val = value;
  }
}
