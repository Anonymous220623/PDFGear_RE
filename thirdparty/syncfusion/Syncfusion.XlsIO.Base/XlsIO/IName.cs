// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IName
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public interface IName : IParentApplication
{
  int Index { get; }

  string Name { get; set; }

  string NameLocal { get; set; }

  IRange RefersToRange { get; set; }

  string Value { get; set; }

  bool Visible { get; set; }

  bool IsLocal { get; }

  string ValueR1C1 { get; }

  string RefersTo { get; }

  string RefersToR1C1 { get; }

  IWorksheet Worksheet { get; }

  string Scope { get; }

  string Description { get; set; }

  void Delete();
}
