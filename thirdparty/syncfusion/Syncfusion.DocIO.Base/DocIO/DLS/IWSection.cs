// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.IWSection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public interface IWSection : ICompositeEntity, IEntity
{
  IWParagraphCollection Paragraphs { get; }

  IWTableCollection Tables { get; }

  WTextBody Body { get; }

  WPageSetup PageSetup { get; }

  ColumnCollection Columns { get; }

  SectionBreakCode BreakCode { get; set; }

  bool ProtectForm { get; set; }

  Column AddColumn(float width, float spacing);

  IWParagraph AddParagraph();

  IWTable AddTable();

  WSection Clone();

  void MakeColumnsEqual();

  WHeadersFooters HeadersFooters { get; }
}
