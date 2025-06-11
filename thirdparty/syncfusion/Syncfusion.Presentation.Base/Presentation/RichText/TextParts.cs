// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.RichText.TextParts
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.RichText;

internal class TextParts : ITextParts, IEnumerable<ITextPart>, IEnumerable
{
  private List<ITextPart> _list;
  private Paragraph _paragraph;

  internal TextParts(Paragraph paragraph)
  {
    this._paragraph = paragraph;
    this._list = new List<ITextPart>();
  }

  public int Count => this._list.Count;

  public ITextPart this[int index] => index != 0 ? this._list[index] : this._list[index];

  public IEnumerator<ITextPart> GetEnumerator()
  {
    return (IEnumerator<ITextPart>) this._list.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();

  public ITextPart Add()
  {
    ITextPart textPart = (ITextPart) new TextPart(this._paragraph);
    this._list.Add(textPart);
    return textPart;
  }

  public int Add(ITextPart value)
  {
    this._list.Add(value);
    return this._list.IndexOf(value);
  }

  public void Insert(int index, ITextPart value) => this._list.Insert(index, value);

  public void RemoveAt(int index) => this._list.RemoveAt(index);

  public void Remove(ITextPart item) => this._list.Remove(item);

  public int IndexOf(ITextPart item) => this._list.IndexOf(item);

  public void Clear() => this._list.Clear();

  public ITextPart Add(string text)
  {
    ITextPart textPart = (ITextPart) new TextPart(this._paragraph, text);
    this._list.Add(textPart);
    return textPart;
  }

  public ITextPart Create() => (ITextPart) new TextPart(this._paragraph);

  public bool Contains(string text)
  {
    foreach (ITextPart textPart in this._list)
    {
      if (textPart.Text.Contains(text))
        return true;
    }
    return false;
  }

  internal void Close()
  {
    if (this._list != null)
    {
      foreach (TextPart textPart in this._list)
        textPart.Close();
      this._list.Clear();
      this._list = (List<ITextPart>) null;
    }
    this._paragraph = (Paragraph) null;
  }

  public TextParts Clone()
  {
    TextParts textParts = (TextParts) this.MemberwiseClone();
    textParts._list = this.CloneTextPartList();
    return textParts;
  }

  private List<ITextPart> CloneTextPartList()
  {
    List<ITextPart> textPartList = new List<ITextPart>();
    foreach (TextPart textPart1 in this._list)
    {
      TextPart textPart2 = textPart1.Clone();
      textPartList.Add((ITextPart) textPart2);
    }
    return textPartList;
  }

  internal void SetParent(Paragraph paragraph)
  {
    this._paragraph = paragraph;
    foreach (TextPart textPart in this._list)
      textPart.SetParent(paragraph);
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    foreach (TextPart textPart in this._list)
      textPart.SetParent(presentation);
  }

  internal void SetParent(Shape shape)
  {
    foreach (TextPart textPart in this._list)
      textPart.SetParent(shape);
  }
}
