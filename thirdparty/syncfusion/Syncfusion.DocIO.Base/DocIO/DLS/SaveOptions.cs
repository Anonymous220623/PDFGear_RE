// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.SaveOptions
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class SaveOptions
{
  private byte m_bFlags = 1;
  private CssStyleSheetType m_htmlExportCssStyleSheetType = CssStyleSheetType.External;
  private string m_htmlExportCssStyleSheetFileName;
  private string m_htmlExportImagesFolder = string.Empty;
  private short m_EPubHeadingLevels = 3;
  private string[] m_fontFiles;

  internal string[] FontFiles
  {
    get => this.m_fontFiles;
    set => this.m_fontFiles = value;
  }

  public bool EPubExportFont
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal short EPubHeadingLevels
  {
    get => this.m_EPubHeadingLevels;
    set => this.m_EPubHeadingLevels = value;
  }

  public CssStyleSheetType HtmlExportCssStyleSheetType
  {
    get => this.m_htmlExportCssStyleSheetType;
    set => this.m_htmlExportCssStyleSheetType = value;
  }

  public string HtmlExportCssStyleSheetFileName
  {
    get => this.m_htmlExportCssStyleSheetFileName;
    set => this.m_htmlExportCssStyleSheetFileName = value;
  }

  public string HtmlExportImagesFolder
  {
    get => this.m_htmlExportImagesFolder;
    set => this.m_htmlExportImagesFolder = value;
  }

  public bool HtmlExportHeadersFooters
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  public bool OptimizeRtfFileSize
  {
    get => ((int) this.m_bFlags & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 191 | (value ? 1 : 0) << 6);
  }

  public bool UseContextualSpacing
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
  }

  public bool HtmlExportTextInputFormFieldAsText
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  public bool HTMLExportImageAsBase64
  {
    get => ((int) this.m_bFlags & 128 /*0x80*/) >> 7 != 0;
    set
    {
      this.m_bFlags = (byte) ((int) this.m_bFlags & (int) sbyte.MaxValue | (value ? 1 : 0) << 7);
    }
  }

  public bool HtmlExportOmitXmlDeclaration
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
  }

  public bool MaintainCompatibilityMode
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  internal bool IsEventSubscribed => this.ImageNodeVisited != null;

  public event ImageNodeVisitedEventHandler ImageNodeVisited;

  internal ImageNodeVisitedEventArgs ExecuteSaveImageEvent(Stream imageStream, string uri)
  {
    ImageNodeVisitedEventArgs args = new ImageNodeVisitedEventArgs(imageStream, uri);
    if (this.ImageNodeVisited != null)
      this.ImageNodeVisited((object) this, args);
    return args;
  }

  internal void Close() => this.m_fontFiles = (string[]) null;
}
