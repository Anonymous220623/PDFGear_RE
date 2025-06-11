// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Interfaces.IPage
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Interfaces;

public interface IPage
{
  string LeftHeader { get; set; }

  string CenterHeader { get; set; }

  string RightHeader { get; set; }

  string LeftFooter { get; set; }

  string CenterFooter { get; set; }

  string RightFooter { get; set; }

  Image LeftHeaderImage { get; set; }

  Image CenterHeaderImage { get; set; }

  Image RightHeaderImage { get; set; }

  Image LeftFooterImage { get; set; }

  Image CenterFooterImage { get; set; }

  Image RightFooterImage { get; set; }
}
