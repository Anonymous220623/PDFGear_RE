// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IHyperLinks
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public interface IHyperLinks : IParentApplication
{
  int Count { get; }

  IHyperLink this[int index] { get; }

  IHyperLink Add(IRange range);

  IHyperLink Add(IRange range, ExcelHyperLinkType hyperlinkType, string address, string screenTip);

  IHyperLink Add(IShape shape);

  IHyperLink Add(IShape shape, ExcelHyperLinkType hyperlinkType, string address, string screenTip);

  void RemoveAt(int index);
}
