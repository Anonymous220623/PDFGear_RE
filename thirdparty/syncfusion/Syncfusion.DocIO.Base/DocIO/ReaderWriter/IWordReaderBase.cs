// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.IWordReaderBase
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal interface IWordReaderBase
{
  bool HasTableBody { get; }

  WordStyleSheet StyleSheet { get; }

  int CurrentStyleIndex { get; }

  WordChunkType ChunkType { get; }

  string TextChunk { get; set; }

  CharacterPropertyException CHPX { get; }

  ParagraphPropertyException PAPX { get; }

  int CurrentTextPosition { get; set; }

  BookmarkInfo[] Bookmarks { get; }

  Dictionary<int, string> SttbfRMarkAuthorNames { get; }

  Stack<Dictionary<WTableRow, short>> TableRowWidthStack { get; }

  List<short> MaximumTableRowWidth { get; }

  WordChunkType ReadChunk();

  IWordImageReader GetImageReader(WordDocument doc);

  Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ShapeBase GetDrawingObject();

  FormField GetFormField(FieldType fieldType);

  bool ReadWatermark(WordDocument doc, WTextBody m_textBody);
}
