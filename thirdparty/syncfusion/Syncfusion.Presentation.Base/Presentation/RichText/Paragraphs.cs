// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.RichText.Paragraphs
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.Presentation.RichText;

internal class Paragraphs : IParagraphs, IEnumerable<IParagraph>, IEnumerable
{
  private List<IParagraph> _list;
  private TextBody _textFrame;
  private Dictionary<int, LevelContainer> _levelHolder;

  internal Paragraphs(TextBody textFrame)
  {
    this._textFrame = textFrame;
    this._list = new List<IParagraph>();
    this._levelHolder = new Dictionary<int, LevelContainer>();
  }

  internal bool HasSameTextAlignment
  {
    get
    {
      int num = this.Count - 1;
      for (int index = 0; index < num; ++index)
      {
        IParagraph paragraph1 = this[index];
        IParagraph paragraph2 = this[index + 1];
        if (((Paragraph) paragraph1).GetDefaultAlignmentType() != ((Paragraph) paragraph2).GetDefaultAlignmentType())
          return false;
      }
      return true;
    }
  }

  internal Dictionary<int, LevelContainer> LevelHolder => this._levelHolder;

  internal BaseSlide BaseSlide => this._textFrame.BaseSlide;

  internal TextBody TextBody => this._textFrame;

  internal string Text
  {
    get
    {
      if (this.Count == 0)
        return "";
      StringBuilder stringBuilder = new StringBuilder(this[0].Text);
      for (int index = 1; index < this.Count; ++index)
      {
        stringBuilder.Append('\r');
        stringBuilder.Append(this[index].Text);
      }
      return stringBuilder.ToString();
    }
    set
    {
      if (value == null)
        return;
      Paragraph paragraph = this.Count <= 0 ? new Paragraph(this) : (Paragraph) this[this.Count - 1];
      paragraph.Text = value;
      this.Clear();
      this.Add((IParagraph) paragraph);
    }
  }

  public int Count => this._list.Count;

  public IParagraph this[int index] => this._list[index];

  public IEnumerator<IParagraph> GetEnumerator()
  {
    return (IEnumerator<IParagraph>) this._list.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();

  public IParagraph Add()
  {
    IParagraph paragraph = (IParagraph) new Paragraph(this);
    this._list.Add(paragraph);
    return paragraph;
  }

  public int Add(IParagraph value)
  {
    this._list.Add(value);
    ((Paragraph) value).Index = this._list.IndexOf(value);
    return this._list.IndexOf(value);
  }

  public void Insert(int index, IParagraph value)
  {
    if (this._textFrame.FitTextOption == FitTextOption.ShrinkTextOnOverFlow)
      this._textFrame.SetFitTextOptionChanged(true);
    if ((value as Paragraph).GetParagraphCollection() == null)
      (value as Paragraph).SetParagraphCollection(this);
    this._list.Insert(index, value);
  }

  public void RemoveAt(int index)
  {
    if (this._textFrame.FitTextOption == FitTextOption.ShrinkTextOnOverFlow)
      this._textFrame.SetFitTextOptionChanged(true);
    this._list.RemoveAt(index);
  }

  public void Remove(IParagraph item)
  {
    if (this._textFrame.FitTextOption == FitTextOption.ShrinkTextOnOverFlow)
      this._textFrame.SetFitTextOptionChanged(true);
    this._list.Remove(item);
  }

  public int IndexOf(IParagraph item) => this._list.IndexOf(item);

  public void Clear()
  {
    if (this._textFrame.FitTextOption == FitTextOption.ShrinkTextOnOverFlow)
      this._textFrame.SetFitTextOptionChanged(true);
    this._list.Clear();
  }

  public IParagraph Add(string text)
  {
    if (this._textFrame.FitTextOption == FitTextOption.ShrinkTextOnOverFlow)
      this._textFrame.SetFitTextOptionChanged(true);
    Paragraph paragraph = new Paragraph(this);
    this._list.Add((IParagraph) paragraph);
    TextPart textPart = new TextPart(paragraph, text);
    ((TextParts) paragraph.TextParts).Add((ITextPart) textPart);
    return (IParagraph) paragraph;
  }

  public IParagraph Create() => (IParagraph) new Paragraph(this);

  internal void Close()
  {
    if (this._list != null)
    {
      foreach (Paragraph paragraph in this._list)
        paragraph.Close();
      this._list = (List<IParagraph>) null;
    }
    if (this._levelHolder != null)
    {
      this._levelHolder.Clear();
      this._levelHolder = (Dictionary<int, LevelContainer>) null;
    }
    this._textFrame = (TextBody) null;
  }

  internal Paragraph GetPreviousParagraph()
  {
    return this.Count != 0 ? this[this.Count - 1] as Paragraph : (Paragraph) null;
  }

  internal void ClearAllBullets()
  {
    foreach (Paragraph paragraph in this)
    {
      if (paragraph.IndentLevelNumber != 0 && this._levelHolder.ContainsKey(paragraph.IndentLevelNumber))
        this._levelHolder[paragraph.IndentLevelNumber] = this._levelHolder[paragraph.IndentLevelNumber].Clear();
    }
  }

  internal Dictionary<int, LevelContainer> GetLevelHolder(int currentParagraphIndentLevel)
  {
    if (this.Count != 0)
    {
      ListType listType = ((ListFormat) this.GetPreviousParagraph().ListFormat).GetListType();
      for (int count = this._levelHolder.Count; count >= 0; --count)
      {
        if (listType != ListType.Numbered && this.GetPreviousParagraph().IndentLevelNumber == currentParagraphIndentLevel)
          this._levelHolder.Remove(count);
      }
    }
    return this._levelHolder;
  }

  public Paragraphs Clone()
  {
    Paragraphs newParent = (Paragraphs) this.MemberwiseClone();
    newParent._levelHolder = this.CloneLevelHolder();
    newParent._list = this.CloneParagraphList(newParent);
    return newParent;
  }

  private List<IParagraph> CloneParagraphList(Paragraphs newParent)
  {
    List<IParagraph> paragraphList = new List<IParagraph>();
    foreach (Paragraph paragraph1 in this._list)
    {
      Paragraph paragraph2 = paragraph1.InternalClone();
      paragraph2.SetParent(newParent);
      paragraphList.Add((IParagraph) paragraph2);
    }
    return paragraphList;
  }

  private Dictionary<int, LevelContainer> CloneLevelHolder()
  {
    Dictionary<int, LevelContainer> dictionary = new Dictionary<int, LevelContainer>();
    foreach (KeyValuePair<int, LevelContainer> keyValuePair in this._levelHolder)
    {
      LevelContainer levelContainer = keyValuePair.Value.Clone();
      dictionary.Add(keyValuePair.Key, levelContainer);
    }
    return dictionary;
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    foreach (Paragraph paragraph in this._list)
      paragraph.SetParent(presentation);
  }

  internal void SetParent(TextBody textBodyClone) => this._textFrame = textBodyClone;

  internal void SetParent(Shape shape)
  {
    foreach (Paragraph paragraph in this._list)
      paragraph.SetParent(shape);
  }
}
