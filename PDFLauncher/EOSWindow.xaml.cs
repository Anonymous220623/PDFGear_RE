// Decompiled with JetBrains decompiler
// Type: PDFLauncher.EOSWindow
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using CommomLib.Commom;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Cache;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace PDFLauncher;

public partial class EOSWindow : Window, IComponentConnector
{
  private PromoteAd ad;
  internal Image uImg;
  internal TextBlock uTitle;
  internal TextBlock uDesc;
  internal Button uBtn;
  private bool _contentLoaded;

  public EOSWindow(PromoteAd promoteAd)
  {
    this.InitializeComponent();
    this.ad = promoteAd;
    if (this.ad == null)
      return;
    GAManager.SendEvent("AdWin", "Show." + this.ad.adType.ToString(), this.ad.adID.ToString(), 1L);
    ConfigManager.setAdShowCount(ConfigManager.getAdShowCount() + 1U);
    this.uTitle.Text = this.ad.strTitle;
    this.uDesc.Text = this.ad.strDesc;
    this.uBtn.Content = (object) this.ad.strBtn;
    try
    {
      RequestCachePolicy uriCachePolicy = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable);
      if (this.ad.strImg.Length <= 0)
        return;
      this.uImg.Source = (ImageSource) new BitmapImage(new Uri(this.ad.strImg), uriCachePolicy);
    }
    catch (Exception ex)
    {
    }
  }

  private void Button_Click(object sender, RoutedEventArgs e)
  {
    if (this.ad == null)
      return;
    GAManager.SendEvent("AdWin", "ClickBtn." + this.ad.adType.ToString(), this.ad.adID.ToString(), 1L);
    Process.Start(this.ad.strUrl);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/PDFLauncher;component/eoswindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.uImg = (Image) target;
        break;
      case 2:
        this.uTitle = (TextBlock) target;
        break;
      case 3:
        this.uDesc = (TextBlock) target;
        break;
      case 4:
        this.uBtn = (Button) target;
        this.uBtn.Click += new RoutedEventHandler(this.Button_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
