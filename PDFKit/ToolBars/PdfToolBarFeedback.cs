// Decompiled with JetBrains decompiler
// Type: PDFKit.ToolBars.PdfToolBarFeedback
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Diagnostics;
using System.Windows;

#nullable disable
namespace PDFKit.ToolBars;

public class PdfToolBarFeedback : PdfToolBar
{
  protected override void InitializeButtons()
  {
    this.Items.Add((object) this.CreateButton("btnFeedBack", "FeedBack", "FeedBack", this.CreateUriToResource("feedback.png"), new RoutedEventHandler(this.btn_FeedBackClick)));
  }

  private void btn_FeedBackClick(object sender, EventArgs e)
  {
    string str = $"mailto:{"pdfreadersustain@outlook.com"}?{$"subject=[{Common.GetProductName()}] [{Common.GetAppVersion()}] Support"}";
    Process.Start(new ProcessStartInfo()
    {
      FileName = str,
      UseShellExecute = true,
      CreateNoWindow = true
    });
  }
}
