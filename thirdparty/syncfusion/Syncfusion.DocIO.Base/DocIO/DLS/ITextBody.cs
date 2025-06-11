// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ITextBody
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public interface ITextBody : ICompositeEntity, IEntity
{
  IWTableCollection Tables { get; }

  IWParagraphCollection Paragraphs { get; }

  FormFieldCollection FormFields { get; }

  IWParagraph LastParagraph { get; }

  new EntityCollection ChildEntities { get; }

  IWParagraph AddParagraph();

  IWTable AddTable();

  IBlockContentControl AddBlockContentControl(ContentControlType controlType);

  void InsertXHTML(string html);

  void InsertXHTML(string html, int paragraphIndex);

  void InsertXHTML(string html, int paragraphIndex, int paragraphItemIndex);

  bool IsValidXHTML(string html, XHTMLValidationType type);

  bool IsValidXHTML(string html, XHTMLValidationType type, out string exceptionMessage);

  void EnsureMinimum();
}
