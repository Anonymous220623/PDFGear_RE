// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.FormulaInfoHashtable
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.Calculate;

public class FormulaInfoHashtable : Hashtable
{
  public FormulaInfo this[object obj]
  {
    get => base[obj] as FormulaInfo;
    set => this[obj] = (object) value;
  }
}
