// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.PromoteAd
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

#nullable disable
namespace CommomLib.Commom;

public class PromoteAd
{
  public int adID;
  public AdType adType;
  public int showMaxCount;
  public string strUrl = "";
  public string strTitle = "";
  public string strDesc = "";
  public string strBtn = "";
  public string strImg = "";

  public PromoteAd(int id, AdType type, string url, int count)
  {
    this.adID = id;
    this.adType = type;
    this.strUrl = url;
    this.showMaxCount = count;
    this.strImg = "";
    switch (type)
    {
      case AdType.EOS:
        this.strTitle = "Update needed";
        this.strDesc = "The version reaches end of life, please download the latest version.";
        this.strBtn = "Download";
        break;
      case AdType.App:
        this.strTitle = "Check out this app!";
        this.strDesc = "The app is limited free now. Download it right now.";
        this.strBtn = "Check it";
        break;
      case AdType.Website:
        this.strTitle = "Highly recommended";
        this.strDesc = "Check out the website to find more great apps.";
        this.strBtn = "Check it";
        break;
    }
  }
}
