// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.IWParagraphCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public interface IWParagraphCollection : IEntityCollectionBase, ICollectionBase, IEnumerable
{
  WParagraph this[int index] { get; }

  int Add(IWParagraph paragraph);

  void Insert(int index, IWParagraph paragraph);

  int IndexOf(IWParagraph paragraph);

  void RemoveAt(int index);
}
