// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Operations.UndoTypes
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

#nullable disable
namespace PDFKit.Contents.Operations;

internal enum UndoTypes
{
  DeleteParagraph,
  InsertText,
  DeleteText,
  SetFont,
  SetFontSize,
  SetTextColor,
  SetBold,
  SetItalic,
  SetUnderline,
  SetStrikeout,
  SetSupperScript,
  SetSubScript,
  NoScript,
  SetCharSpace,
  SetWordSpace,
  SetLineSpace,
  SetAlign,
  InsertReturn,
  OffsetParagraph,
}
