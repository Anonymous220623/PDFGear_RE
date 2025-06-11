// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ParagraphItemCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class ParagraphItemCollection : EntityCollection
{
  private static readonly Type[] DEF_ELEMENT_TYPES = new Type[1]
  {
    typeof (ParagraphItem)
  };

  public ParagraphItem this[int index] => this.InnerList[index] as ParagraphItem;

  protected WParagraph OwnerParagraph => this.Owner as WParagraph;

  protected override Type[] TypesOfElement => ParagraphItemCollection.DEF_ELEMENT_TYPES;

  public ParagraphItemCollection(WordDocument doc)
    : base(doc)
  {
  }

  internal ParagraphItemCollection(WParagraph owner)
    : base(owner.Document, (Entity) owner)
  {
  }

  internal ParagraphItemCollection(InlineContentControl owner)
    : base(owner.Document, (Entity) owner)
  {
  }

  internal void CloneItemsTo(ParagraphItemCollection items)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      ParagraphItem paragraphItem = (ParagraphItem) this[index].Clone();
      if (paragraphItem != null)
      {
        paragraphItem.SetOwner((OwnerHolder) items.Owner);
        items.UnsafeAdd(paragraphItem);
      }
    }
  }

  internal void UnsafeRemoveAt(int index) => this.RemoveFromInnerList(index);

  internal void UnsafeAdd(ParagraphItem item)
  {
    this.AddToInnerList((Entity) item);
    if (this.Document == null)
      return;
    switch (item)
    {
      case WField _ when (item as WField).FieldEnd != null:
        this.Document.ClonedFields.Push(item as WField);
        break;
      case TableOfContent _ when ((TableOfContent) item).TOCField.FieldEnd != null:
        this.Document.ClonedFields.Push(((TableOfContent) item).TOCField);
        break;
      case WOleObject _ when (item as WOleObject).Field.FieldEnd != null:
        this.Document.ClonedFields.Push((item as WOleObject).Field);
        break;
      case WFieldMark _ when this.Document.ClonedFields.Count > 0:
        WField wfield = this.Document.ClonedFields.Peek();
        if ((item as WFieldMark).Type == FieldMarkType.FieldSeparator)
        {
          wfield.FieldSeparator = item as WFieldMark;
          break;
        }
        this.Document.ClonedFields.Pop().FieldEnd = item as WFieldMark;
        break;
    }
  }

  protected override void OnInsertComplete(int index, Entity entity)
  {
    base.OnInsertComplete(index, entity);
    if (this.Joined && entity.Owner != null && this.OwnerParagraph != null)
    {
      ParagraphItem paragraphItem = (ParagraphItem) entity;
      int itemPos = 0;
      if (index > 0)
        itemPos = this[index - 1].EndPos;
      paragraphItem.AttachToParagraph(this.OwnerParagraph, itemPos);
    }
    else if (this.Joined && entity.Owner is WMergeField)
    {
      (entity as ParagraphItem).ParaItemCharFormat.ApplyBase((entity.Owner as WMergeField).CharacterFormat.BaseFormat);
    }
    else
    {
      if (!this.Joined || this.OwnerParagraph != null || !(entity.Owner is InlineContentControl))
        return;
      ParagraphItem paragraphItem = (ParagraphItem) entity;
      InlineContentControl owner = paragraphItem.Owner as InlineContentControl;
      WParagraph ownerParagraphValue = paragraphItem.GetOwnerParagraphValue();
      int itemPos = 0;
      if (paragraphItem.Index == 0 && owner != null)
        itemPos = owner.StartPos;
      else if (paragraphItem.Index > 0)
        itemPos = this[index - 1].EndPos;
      paragraphItem.AttachToParagraph(ownerParagraphValue, itemPos);
    }
  }

  protected override void OnRemove(int index)
  {
    Entity entity = (Entity) this[index];
    if (this.Joined)
      this[index].Detach();
    base.OnRemove(this.IndexOf((IEntity) entity));
  }

  protected override void OnClear()
  {
    if (this.Joined)
    {
      for (int index = 0; index < this.Count; ++index)
        this[index].Detach();
    }
    base.OnClear();
  }

  internal override void Close()
  {
    if (this.InnerList != null && this.InnerList.Count > 0)
    {
      while (this.InnerList.Count > 0)
      {
        if (this.InnerList[this.InnerList.Count - 1] is ParagraphItem inner)
        {
          inner.Close();
          if (inner.Owner == null)
            this.InnerList.Remove((object) inner);
          else
            this.Remove((IEntity) inner);
        }
      }
    }
    base.Close();
  }

  protected override OwnerHolder CreateItem(IXDLSContentReader reader)
  {
    Enum elementType;
    if (!reader.ParseElementType(typeof (ParagraphItemType), out elementType))
      elementType = (Enum) ParagraphItemType.TextRange;
    return (OwnerHolder) this.Document.CreateParagraphItem((ParagraphItemType) elementType);
  }

  protected override string GetTagItemName() => "item";

  internal IWidget GetCurrentWidget(int index) => this.InnerList[index] as IWidget;

  internal void GetMinimumAndMaximumWordWidthInPara(
    ref float maximumWordWidthInPara,
    ref float minumWordWidthInPara)
  {
    if (this.InnerList.Count <= 0)
      return;
    Entity key = this.InnerList[0] as Entity;
    StringBuilder stringBuilder = new StringBuilder();
    Dictionary<WTextRange, int> spans = new Dictionary<WTextRange, int>();
    List<float> floatList = new List<float>();
    while (true)
    {
      switch (key)
      {
        case WTextRange _ when !string.IsNullOrEmpty((key as WTextRange).Text) && !(key is WField):
          spans.Add(key as WTextRange, stringBuilder.Length);
          stringBuilder.Append((key as WTextRange).Text);
          break;
        case WField _:
        case WOleObject _:
          WField wfield = key is WField ? key as WField : ((key as WOleObject).Field != null ? (key as WOleObject).Field : (WField) null);
          if (wfield != null && wfield.FieldEnd != null)
          {
            key = wfield.FieldSeparator == null ? (Entity) wfield.FieldEnd : (Entity) wfield.FieldSeparator;
            break;
          }
          break;
        case WPicture _ when (key as WPicture).TextWrappingStyle != TextWrappingStyle.InFrontOfText && (key as WPicture).TextWrappingStyle != TextWrappingStyle.Behind:
          floatList.Add((key as WPicture).Width);
          break;
        case InlineContentControl _ when (key as InlineContentControl).ParagraphItems.Count > 0:
          (key as InlineContentControl).ParagraphItems.GetMinimumAndMaximumWordWidthInPara(ref maximumWordWidthInPara, ref minumWordWidthInPara);
          break;
      }
      if (key.NextSibling != null && (!(key is Break) || (key as Break).BreakType != BreakType.LineBreak))
        key = key.NextSibling as Entity;
      else
        break;
    }
    string str = stringBuilder.ToString();
    MatchCollection matches = new Regex("[\\s-\\t]").Matches(str);
    string empty = string.Empty;
    for (int i = 0; i <= matches.Count && matches.Count > 0; ++i)
    {
      int wordStartIndex = 0;
      int num = i != 0 ? matches[i - 1].Index + 1 : 0;
      int lengthTillDelimeter = this.GetLengthTillDelimeter(matches, i, str, num);
      int index = num;
      int wordCount = index;
      string matchedText = str.Substring(num, lengthTillDelimeter);
      float width = this.MeasureMinMaxWordWidth(spans, index, wordStartIndex, num, lengthTillDelimeter, wordCount, matchedText);
      this.UpdateMinMaxWordWidth(ref minumWordWidthInPara, ref maximumWordWidthInPara, width);
    }
    if (matches.Count == 0)
    {
      int wordStartIndex = 0;
      int textstartIndex = 0;
      int length = str.Length;
      int index = textstartIndex;
      int wordCount = index;
      float width = this.MeasureMinMaxWordWidth(spans, index, wordStartIndex, textstartIndex, length, wordCount, str);
      this.UpdateMinMaxWordWidth(ref minumWordWidthInPara, ref maximumWordWidthInPara, width);
    }
    float num1 = 0.0f;
    if (floatList.Count > 0)
    {
      float val1 = floatList[0];
      for (int index = 1; index < floatList.Count; ++index)
        val1 = Math.Max(val1, floatList[index]);
      num1 = val1;
    }
    if ((double) minumWordWidthInPara == 0.0 || (double) num1 > (double) minumWordWidthInPara)
      minumWordWidthInPara = num1;
    if ((double) maximumWordWidthInPara != 0.0 && (double) num1 <= (double) maximumWordWidthInPara)
      return;
    maximumWordWidthInPara = num1;
  }

  private float MeasureMinMaxWordWidth(
    Dictionary<WTextRange, int> spans,
    int index,
    int wordStartIndex,
    int textstartIndex,
    int lengthOfMatchedtext,
    int wordCount,
    string matchedText)
  {
    string text = string.Empty;
    float num = 0.0f;
    DrawingContext drawingContext = DocumentLayouter.DrawingContext;
    string empty = string.Empty;
    foreach (WTextRange key in spans.Keys)
    {
      int span = spans[key];
      int textLength = key.TextLength;
      if (index <= span + textLength)
      {
        wordStartIndex = index - span;
        if (textstartIndex + lengthOfMatchedtext <= span + textLength)
        {
          wordCount = textstartIndex + lengthOfMatchedtext - (span + wordStartIndex);
        }
        else
        {
          wordCount = textLength - wordStartIndex;
          index += wordCount;
        }
        try
        {
          text = key.Text.Substring(wordStartIndex, wordCount);
          empty += text;
        }
        catch
        {
        }
      }
      if (!string.IsNullOrEmpty(text))
        num += drawingContext.MeasureTextRange(key, text).Width;
      if (empty == matchedText)
        break;
    }
    return num;
  }

  private void UpdateMinMaxWordWidth(
    ref float minumWordWidthInPara,
    ref float maximumWordWidthInPara,
    float width)
  {
    if ((double) width != 0.0 && ((double) minumWordWidthInPara == 0.0 || (double) width < (double) minumWordWidthInPara))
      minumWordWidthInPara = width;
    if ((double) width == 0.0 || (double) maximumWordWidthInPara != 0.0 && (double) width <= (double) maximumWordWidthInPara)
      return;
    maximumWordWidthInPara = width;
  }

  private int GetLengthTillDelimeter(MatchCollection matches, int i, string text, int startIndex)
  {
    if (i == 0)
      return matches[0].Index;
    return i < matches.Count ? matches[i].Index - startIndex : text.Length - startIndex;
  }
}
