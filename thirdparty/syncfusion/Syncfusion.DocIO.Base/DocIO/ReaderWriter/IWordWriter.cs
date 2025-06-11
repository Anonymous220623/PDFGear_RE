// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.IWordWriter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal interface IWordWriter : IWordWriterBase
{
  DOPDescriptor DOP { get; }

  SectionProperties SectionProperties { get; }

  BuiltinDocumentProperties BuiltinDocumentProperties { get; }

  CustomDocumentProperties CustomDocumentProperties { get; }

  void WriteDocumentHeader();

  void WriteDocumentEnd(
    string password,
    string author,
    ushort fibVersion,
    Dictionary<string, Storage> oleObjectCollection);

  IWordSubdocumentWriter GetSubdocumentWriter(WordSubdocument subDocumentType);

  void InsertPageBreak();
}
