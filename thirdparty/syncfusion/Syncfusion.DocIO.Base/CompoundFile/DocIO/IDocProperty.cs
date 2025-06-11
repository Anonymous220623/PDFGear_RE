// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.IDocProperty
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO;

public interface IDocProperty
{
  bool IsBuiltIn { get; }

  BuiltInProperty PropertyId { get; }

  string Name { get; }

  object Value { get; set; }

  bool Boolean { get; set; }

  int Integer { get; set; }

  int Int32 { get; set; }

  double Double { get; set; }

  string Text { get; set; }

  DateTime DateTime { get; set; }

  TimeSpan TimeSpan { get; set; }

  string LinkSource { get; set; }

  bool LinkToContent { get; set; }
}
