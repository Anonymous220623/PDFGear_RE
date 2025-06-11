// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IRichTextString
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IRichTextString : IParentApplication, IOptimizedUpdate
{
  IFont GetFont(int iPosition);

  void SetFont(int iStartPos, int iEndPos, IFont font);

  void ClearFormatting();

  void Clear();

  void Append(string text, IFont font);

  string Text { get; set; }

  string RtfText { get; set; }

  bool IsFormatted { get; }
}
