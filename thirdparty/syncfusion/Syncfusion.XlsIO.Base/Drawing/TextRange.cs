// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.TextRange
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation;

#nullable disable
namespace Syncfusion.Drawing;

public class TextRange : ITextRange
{
  private string m_text;
  private TextFrame textFrame;
  private RichTextString m_strText;
  private PreservationLogger preservationLogger;

  internal TextRange(TextFrame textFrame, PreservationLogger preservationLogger)
  {
    this.textFrame = textFrame;
    this.preservationLogger = preservationLogger;
  }

  public string Text
  {
    get => this.RichText.Text;
    set => this.RichText.Text = value;
  }

  public IRichTextString RichText
  {
    get
    {
      if (this.m_strText == null)
        this.InitializeVariables();
      return (IRichTextString) this.m_strText;
    }
  }

  protected virtual void InitializeVariables()
  {
    IWorkbook workbook = this.textFrame.GetWorkbook();
    this.m_strText = new RichTextString(workbook.Application, (object) workbook, false, true, this.preservationLogger);
  }

  internal TextRange Clone(object parent)
  {
    TextRange textRange = (TextRange) this.MemberwiseClone();
    this.textFrame = (TextFrame) parent;
    if (this.m_strText != null)
      textRange.m_strText = (RichTextString) this.m_strText.Clone(textRange.m_strText.Parent);
    return textRange;
  }
}
