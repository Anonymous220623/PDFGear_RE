// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Operations.IPdfUndoItem
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;

#nullable disable
namespace PDFKit.Contents.Operations;

public interface IPdfUndoItem : IDisposable
{
  int PageIndex { get; }

  void Redo(LogicalStructAnalyser analyser);

  void Undo(LogicalStructAnalyser analyser);
}
