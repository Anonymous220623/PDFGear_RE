// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.IDocumentProperty
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO;

public interface IDocumentProperty
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
