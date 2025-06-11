// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ISharedFormula
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
public interface ISharedFormula
{
  int FirstRow { get; set; }

  int LastRow { get; set; }

  int FirstColumn { get; set; }

  int LastColumn { get; set; }

  Ptg[] Formula { get; }
}
