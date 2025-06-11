// Decompiled with JetBrains decompiler
// Type: pdfconverter.FlowDirectionContext
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System.Threading;
using System.Windows;

#nullable disable
namespace pdfconverter;

public class FlowDirectionContext
{
  private FlowDirection flowDirection;

  public FlowDirectionContext()
  {
    try
    {
      this.flowDirection = Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
    }
    catch
    {
    }
  }

  public FlowDirection FlowDirection => this.flowDirection;
}
