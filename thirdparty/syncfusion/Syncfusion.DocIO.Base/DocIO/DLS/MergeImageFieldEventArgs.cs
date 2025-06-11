// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.MergeImageFieldEventArgs
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class MergeImageFieldEventArgs : MergeFieldEventArgs
{
  private bool m_useText;
  private Image m_image;
  private string m_imageFileName = "";
  private Stream m_imageStream;
  private bool m_bSkip;
  private WPicture m_picture;

  public bool UseText => this.m_useText;

  public string ImageFileName
  {
    get => this.m_imageFileName;
    set
    {
      this.m_imageFileName = value;
      this.LoadImage(this.m_imageFileName);
    }
  }

  public Stream ImageStream
  {
    get => this.m_imageStream;
    set
    {
      this.m_imageStream = value;
      this.LoadImage(this.m_imageStream);
    }
  }

  public Image Image
  {
    get => this.m_image;
    set => this.m_image = value;
  }

  public bool Skip
  {
    get => this.m_bSkip;
    set => this.m_bSkip = value;
  }

  public WPicture Picture
  {
    get
    {
      if (this.Image != null)
        this.m_picture.LoadImage(this.Image);
      return this.m_picture;
    }
  }

  public MergeImageFieldEventArgs(
    IWordDocument doc,
    string tableName,
    int rowIndex,
    IWMergeField field,
    Image image)
    : base(doc, tableName, rowIndex, field, (object) null)
  {
    this.m_image = image;
    this.m_picture = new WPicture(doc);
  }

  public MergeImageFieldEventArgs(
    IWordDocument doc,
    string tableName,
    int rowIndex,
    IWMergeField field,
    object obj)
    : base(doc, tableName, rowIndex, field, obj)
  {
    this.m_image = obj as Image;
    this.m_picture = new WPicture(doc);
  }

  private void LoadImage(string name) => this.m_image = (Image) new Bitmap(name);

  private void LoadImage(Stream stream) => this.m_image = (Image) new Bitmap(stream);
}
