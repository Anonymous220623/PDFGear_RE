// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfFileLinkAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfFileLinkAnnotation : PdfActionLinkAnnotation
{
  private PdfLaunchAction m_action;

  public string FileName
  {
    get => this.m_action.FileName;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (FileName));
        case "":
          throw new ArgumentException("FileName - string can not be empty");
        default:
          if (!(this.m_action.FileName != value))
            break;
          this.m_action.FileName = value;
          break;
      }
    }
  }

  public override PdfAction Action
  {
    get => base.Action;
    set
    {
      base.Action = value;
      this.m_action.Next = value;
    }
  }

  public PdfFileLinkAnnotation(RectangleF rectangle, string fileName)
    : base(rectangle)
  {
    switch (fileName)
    {
      case null:
        throw new ArgumentNullException(nameof (fileName));
      case "":
        throw new ArgumentException("fileName - string can not be empty");
      default:
        this.m_action = new PdfLaunchAction(fileName);
        break;
    }
  }

  protected override void Save()
  {
    base.Save();
    this.Dictionary.SetProperty("A", (IPdfWrapper) this.m_action);
  }
}
