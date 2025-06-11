// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Utils.TextObjectExtraDataExtensions
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net;
using System;
using System.Collections.Generic;

#nullable disable
namespace PDFKit.Contents.Utils;

internal static class TextObjectExtraDataExtensions
{
  private static LinkedList<TextObjectExtraDataExtensions.Data> list = new LinkedList<TextObjectExtraDataExtensions.Data>();

  private static LinkedListNode<TextObjectExtraDataExtensions.Data> Find(PdfTextObject textObject)
  {
    LinkedListNode<TextObjectExtraDataExtensions.Data> linkedListNode = TextObjectExtraDataExtensions.list.First;
    while (linkedListNode != null)
    {
      LinkedListNode<TextObjectExtraDataExtensions.Data> node = linkedListNode;
      linkedListNode = node.Next;
      if (!node.Value.IsValid)
        TextObjectExtraDataExtensions.list.Remove(node);
      else if (node.Value.TextObject.Handle == textObject.Handle)
        return node;
    }
    return (LinkedListNode<TextObjectExtraDataExtensions.Data>) null;
  }

  private static LinkedListNode<TextObjectExtraDataExtensions.Data> GetOrCreateData(
    PdfTextObject textObject,
    Func<bool> createPredicate = null)
  {
    LinkedListNode<TextObjectExtraDataExtensions.Data> node = TextObjectExtraDataExtensions.Find(textObject);
    if (node == null && createPredicate != null && createPredicate())
    {
      node = new LinkedListNode<TextObjectExtraDataExtensions.Data>(new TextObjectExtraDataExtensions.Data(textObject));
      TextObjectExtraDataExtensions.list.AddFirst(node);
    }
    return node;
  }

  internal static ScriptEnum GetScript(this PdfTextObject textObject)
  {
    LinkedListNode<TextObjectExtraDataExtensions.Data> data = TextObjectExtraDataExtensions.GetOrCreateData(textObject);
    return data == null ? ScriptEnum.Normal : data.Value.Script;
  }

  internal static void SetScript(this PdfTextObject textObject, ScriptEnum script)
  {
    LinkedListNode<TextObjectExtraDataExtensions.Data> data = TextObjectExtraDataExtensions.GetOrCreateData(textObject, (Func<bool>) (() => script != 0));
    if (data == null)
      return;
    data.Value.Script = script;
  }

  internal static (PdfFont font, BoldItalicFlags flag) GetStoredFont(this PdfTextObject textObject)
  {
    LinkedListNode<TextObjectExtraDataExtensions.Data> data = TextObjectExtraDataExtensions.GetOrCreateData(textObject);
    return data == null ? () : (data.Value.StoredFont, data.Value.StoredFontFlag);
  }

  internal static void SetStoredFont(
    this PdfTextObject textObject,
    PdfFont font,
    BoldItalicFlags flag)
  {
    LinkedListNode<TextObjectExtraDataExtensions.Data> data = TextObjectExtraDataExtensions.GetOrCreateData(textObject, (Func<bool>) (() => font != null));
    if (data == null)
      return;
    data.Value.StoredFont = font;
    data.Value.StoredFontFlag = flag;
  }

  private class Data
  {
    private WeakReference<PdfTextObject> weakObj;
    private WeakReference<PdfFont> weakStoredFont;
    private BoldItalicFlags storedFontFlag;

    internal Data(PdfTextObject obj) => this.weakObj = new WeakReference<PdfTextObject>(obj);

    public PdfTextObject TextObject
    {
      get
      {
        PdfTextObject target;
        return this.weakObj != null && this.weakObj.TryGetTarget(out target) ? target : (PdfTextObject) null;
      }
    }

    public bool IsValid => this.TextObject != null;

    public ScriptEnum Script { get; set; }

    public PdfFont StoredFont
    {
      get
      {
        PdfFont target;
        return this.weakStoredFont != null && this.weakStoredFont.TryGetTarget(out target) ? target : (PdfFont) null;
      }
      set
      {
        if (value == null)
          this.weakStoredFont = (WeakReference<PdfFont>) null;
        else
          this.weakStoredFont = new WeakReference<PdfFont>(value);
      }
    }

    public BoldItalicFlags StoredFontFlag
    {
      get => this.StoredFont == null ? BoldItalicFlags.None : this.storedFontFlag;
      set => this.storedFontFlag = value;
    }
  }
}
