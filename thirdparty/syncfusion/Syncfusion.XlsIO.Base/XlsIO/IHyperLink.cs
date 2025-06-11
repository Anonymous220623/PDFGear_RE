// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IHyperLink
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public interface IHyperLink : IParentApplication
{
  string Address { get; set; }

  string Name { get; }

  IRange Range { get; }

  string ScreenTip { get; set; }

  string SubAddress { get; set; }

  string TextToDisplay { get; set; }

  ExcelHyperLinkType Type { get; set; }

  IShape Shape { get; }

  ExcelHyperlinkAttachedType AttachedType { get; }
}
