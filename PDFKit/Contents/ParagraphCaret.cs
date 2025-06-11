// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.ParagraphCaret
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Net;
using PDFKit.Contents.Utils;

#nullable disable
namespace PDFKit.Contents;

public class ParagraphCaret
{
  private PdfTextObject textObject;
  private TextLine line;
  private int charIndex;
  private bool rotate;
  private FS_POINTF topPoint;
  private FS_POINTF bottomPoint;

  public bool IsTextObjTail
  {
    get => this.textObject != null && this.charIndex >= this.textObject.CharsCount;
  }

  public bool IsCharPunctuation
  {
    get
    {
      if (this.textObject == null || this.charIndex < 0)
        return false;
      int charCode;
      this.textObject.GetCharInfo(this.charIndex, out charCode, out float _, out float _);
      if (this.textObject.Font == null)
        return false;
      return this.textObject.Font.IsUnicodeCompatible ? CharHelper.IsPunctuation((int) this.textObject.Font.ToUnicode(charCode)) : CharHelper.IsPunctuation(charCode);
    }
  }

  public PdfTextObject TextObject
  {
    get => this.textObject;
    internal set => this.textObject = value;
  }

  public TextLine Line
  {
    get => this.line;
    internal set => this.line = value;
  }

  public int CharIndex
  {
    get => this.charIndex;
    internal set => this.charIndex = value;
  }

  public bool Rotate
  {
    get => this.rotate;
    internal set => this.rotate = value;
  }

  public FS_POINTF TopPoint
  {
    get => this.topPoint;
    internal set => this.topPoint = value;
  }

  public FS_POINTF BottomPoint
  {
    get => this.bottomPoint;
    internal set => this.bottomPoint = value;
  }

  public ParagraphCaret Copy()
  {
    return new ParagraphCaret()
    {
      textObject = this.textObject,
      line = this.line,
      charIndex = this.charIndex,
      rotate = this.rotate,
      topPoint = this.topPoint,
      bottomPoint = this.bottomPoint
    };
  }
}
