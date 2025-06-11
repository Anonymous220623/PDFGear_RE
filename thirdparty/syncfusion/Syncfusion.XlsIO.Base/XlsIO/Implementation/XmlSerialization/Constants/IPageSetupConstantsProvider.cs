// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Constants.IPageSetupConstantsProvider
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Constants;

public interface IPageSetupConstantsProvider
{
  string PageMarginsTag { get; }

  string LeftMargin { get; }

  string RightMargin { get; }

  string TopMargin { get; }

  string BottomMargin { get; }

  string HeaderMargin { get; }

  string FooterMargin { get; }

  string Namespace { get; }
}
