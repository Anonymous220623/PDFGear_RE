﻿// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.EntityType
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public enum EntityType
{
  WordDocument,
  Section,
  TextBody,
  HeaderFooter,
  Paragraph,
  AlternateChunk,
  BlockContentControl,
  InlineContentControl,
  RowContentControl,
  CellContentControl,
  [Obsolete("This enumeration option has been deprecated. On using this enumeration, BlockContentControl type will be set instead of StructureDocumentTag.")] StructureDocumentTag,
  [Obsolete("This enumeration option has been deprecated. On using this enumeration, InlineContentControl type will be set instead of StructureDocumentTagInline.")] StructureDocumentTagInline,
  [Obsolete("This enumeration option has been deprecated. On using this enumeration, RowContentControl type will be set instead of StructureDocumentTagRow.")] StructureDocumentTagRow,
  [Obsolete("This enumeration option has been deprecated. On using this enumeration, CellContentControl type will be set instead of StructureDocumentTagCell.")] StructureDocumentTagCell,
  [Obsolete("This enumeration option has been deprecated. On using this enumeration, BlockContentControl type will be set instead of SDTBlockContent.")] SDTBlockContent,
  [Obsolete("This enumeration option has been deprecated. On using this enumeration, InlineContentControl type will be set instead of SDTInlineContent.")] SDTInlineContent,
  [Obsolete("This enumeration option has been deprecated. On using this enumeration, RowContentControl type will be set instead of SDTRowContent.")] SDTRowContent,
  [Obsolete("This enumeration option has been deprecated. On using this enumeration, CellContentControl type will be set instead of SDTCellContent.")] SDTCellContent,
  Table,
  TableRow,
  TableCell,
  TextRange,
  Picture,
  Field,
  FieldMark,
  MergeField,
  SeqField,
  EmbededField,
  ControlField,
  TextFormField,
  DropDownFormField,
  CheckBox,
  BookmarkStart,
  BookmarkEnd,
  Shape,
  Comment,
  Footnote,
  TextBox,
  Break,
  Symbol,
  TOC,
  XmlParaItem,
  Undefined,
  Chart,
  CommentMark,
  CommentEnd,
  OleObject,
  AbsoluteTab,
  AutoShape,
  EditableRangeStart,
  EditableRangeEnd,
  GroupShape,
  ChildShape,
  ChildGroupShape,
  Math,
}
