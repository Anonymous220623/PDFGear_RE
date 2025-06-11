// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.HtmlSaveOptions
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class HtmlSaveOptions
{
  public static HtmlSaveOptions Default = new HtmlSaveOptions();
  private string m_imagePath;
  private HtmlSaveOptions.GetText m_getText;

  public string ImagePath
  {
    get => this.m_imagePath;
    set => this.m_imagePath = value;
  }

  public HtmlSaveOptions.GetText TextMode
  {
    get => this.m_getText;
    set => this.m_getText = value;
  }

  public enum GetText
  {
    DisplayText,
    Value,
  }
}
