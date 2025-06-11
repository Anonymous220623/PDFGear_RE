// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.IWordWriterBase
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser;
using Syncfusion.Layouting;
using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal interface IWordWriterBase
{
  WordStyleSheet StyleSheet { get; }

  int CurrentStyleIndex { get; set; }

  CharacterPropertyException CHPX { get; set; }

  CharacterPropertyException BreakCHPX { get; set; }

  bool BreakCHPXStickProperties { get; set; }

  bool CHPXStickProperties { get; set; }

  ParagraphPropertyException PAPX { get; set; }

  bool PAPXStickProperties { get; set; }

  ListProperties ListProperties { get; }

  void WriteChunk(string textChunk);

  void WriteMarker(WordChunkType chunkType);

  void WriteCellMark(int nestingLevel);

  void WriteRowMark(int nestingLevel, int cellCount);

  void InsertStartField(string fieldcode, bool hasSeparator);

  void InsertStartField(string fieldcode, WField field, bool hasSeparator);

  void InsertEndField();

  void InsertFieldSeparator();

  void InsertImage(WPicture pict);

  void InsertImage(WPicture pict, int height, int width);

  void InsertShape(WPicture pict, PictureShapeProps pictProps);

  int InsertTextBox(bool visible, WTextBoxFormat txbxFormat);

  void InsertFormField(string fieldcode, FormField formField, WFormField wFormField);

  void InsertBookmarkStart(string name, BookmarkStart start);

  void InsertBookmarkEnd(string name);

  void InsertWatermark(Watermark watermark, UnitsConvertor initsConvertor, float maxWidth);

  void WriteSafeChunk(string textChunk);

  void InsertFieldIndexEntry(string fieldCode);
}
