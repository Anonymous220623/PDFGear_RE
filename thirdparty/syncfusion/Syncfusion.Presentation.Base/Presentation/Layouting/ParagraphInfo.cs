// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Layouting.ParagraphInfo
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.RichText;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Layouting;

internal class ParagraphInfo
{
  private IParagraph _paragraph;
  private List<LineInfo> _lineInfo;

  internal ParagraphInfo(IParagraph paragraph)
  {
    this._paragraph = paragraph;
    this._lineInfo = new List<LineInfo>();
  }

  internal Paragraph Paragraph => (Paragraph) this._paragraph;

  internal List<LineInfo> LineInfoCollection => this._lineInfo;

  internal void Close()
  {
    if (this._lineInfo == null)
      return;
    foreach (LineInfo lineInfo in this._lineInfo)
      lineInfo.Close();
    this._lineInfo.Clear();
    this._lineInfo = (List<LineInfo>) null;
  }

  internal ParagraphInfo Clone()
  {
    ParagraphInfo paragraphInfo = (ParagraphInfo) this.MemberwiseClone();
    paragraphInfo._lineInfo = this.CloneLineInfoList();
    return paragraphInfo;
  }

  private List<LineInfo> CloneLineInfoList()
  {
    List<LineInfo> lineInfoList = new List<LineInfo>();
    foreach (LineInfo lineInfo1 in this._lineInfo)
    {
      LineInfo lineInfo2 = lineInfo1.Clone();
      lineInfoList.Add(lineInfo2);
    }
    return lineInfoList;
  }

  internal void SetParent(Paragraph paragraph) => this._paragraph = (IParagraph) paragraph;
}
