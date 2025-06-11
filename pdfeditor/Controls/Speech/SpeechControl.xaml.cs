// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Speech.SpeechControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.AppTheme;
using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf.Net;
using pdfeditor.Models.Menus;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Speech;

public partial class SpeechControl : Window, IComponentConnector
{
  public ImgaeFilePath imgaeFilePath;
  internal Image _imgicon;
  internal Slider VolumeSlider;
  internal TextBlock VolumeBlock;
  internal Image Start;
  internal Button StopBtn;
  internal Button ToneBtn;
  internal Button SpeedBtn;
  internal Button GlobeBtn;
  internal Popup Speed;
  internal FormattedSlider SpeedSli;
  internal Popup Tone;
  internal FormattedSlider ToneSli;
  internal Popup Globe;
  internal ListBox CultureListBox;
  private bool _contentLoaded;

  public MainViewModel VM => Ioc.Default.GetRequiredService<MainViewModel>();

  public SpeechControl()
  {
    this.InitializeComponent();
    this.imgaeFilePath = new ImgaeFilePath();
    if (this.VM.speechUtils != null)
    {
      this.VolumeBlock.Text = $"{this.VM.speechUtils.SpeechVolume}";
      this.VolumeSlider.Value = (double) Convert.ToInt32(this.VM.speechUtils.SpeechVolume);
      this.ToneSli.Value = (double) Convert.ToInt32(this.VM.speechUtils.Pitch + 5.0);
      this.SpeedSli.Value = (double) Convert.ToInt32((this.VM.speechUtils.Rate + 10.0) / 2.0);
      if (this.VM.ViewToolbar.ReadButtonModel.IsChecked)
        this.StopBtn.IsEnabled = true;
      this.GlobeBtn.IsEnabled = true;
      if (this.VM.speechUtils.isSpeak())
        this.imgaeFilePath.ImagePath = "pack://application:,,,/Style/Resources/Speech/Pause.png";
    }
    else
    {
      this.VolumeBlock.Text = "60";
      this.VolumeSlider.Value = 60.0;
      this.ToneSli.Value = 5.0;
      this.SpeedSli.Value = 5.0;
      this.StopBtn.IsEnabled = false;
    }
    this.VolumeSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.VolumeSlider_ValueChanged);
    this.SpeedSli.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.SpeedSli_ValueChanged);
    this.ToneSli.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.ToneSli_ValueChanged);
    BindingOperations.SetBinding((DependencyObject) this.Start, Image.SourceProperty, (BindingBase) new Binding()
    {
      Source = (object) this.imgaeFilePath,
      Path = new PropertyPath("ImagePath", Array.Empty<object>())
    });
    this.Closed += new EventHandler(this.SpeechControl_Closed);
    (((Ioc.Default.GetRequiredService<MainViewModel>().ViewToolbar.ReadButtonModel.ChildButtonModel as ToolbarChildCheckableButtonModel).ContextMenu as ContextMenuModel)[3] as SpeechToolContextMenuItemModel).Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Speech/Checked.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Speech/Checked.png"));
  }

  private void SpeechControl_Closed(object sender, EventArgs e)
  {
    (((Ioc.Default.GetRequiredService<MainViewModel>().ViewToolbar.ReadButtonModel.ChildButtonModel as ToolbarChildCheckableButtonModel).ContextMenu as ContextMenuModel)[3] as SpeechToolContextMenuItemModel).Icon = (ImageSource) null;
    this.VolumeSlider.ValueChanged -= new RoutedPropertyChangedEventHandler<double>(this.VolumeSlider_ValueChanged);
    this.SpeedSli.ValueChanged -= new RoutedPropertyChangedEventHandler<double>(this.SpeedSli_ValueChanged);
    this.ToneSli.ValueChanged -= new RoutedPropertyChangedEventHandler<double>(this.ToneSli_ValueChanged);
    this.VM.speechControl = (SpeechControl) null;
    this.Owner.Activate();
  }

  private void Button_Click(object sender, RoutedEventArgs e) => this.Speed.IsOpen = true;

  private void TonedBtn_Click(object sender, RoutedEventArgs e) => this.Tone.IsOpen = true;

  private void GlobeBtn_Click(object sender, RoutedEventArgs e)
  {
    if (this.CultureListBox.Items.Count == 0)
    {
      this.CultureListBox.ItemsSource = (IEnumerable) this.Getculture();
      if (this.VM.ReadCulIndex < 0)
        this.CultureListBox.SelectedIndex = this.GetcultureIndex();
      else
        this.CultureListBox.SelectedIndex = this.VM.ReadCulIndex;
      this.CultureListBox.SelectionChanged += new SelectionChangedEventHandler(this.CultureListBox_SelectionChanged);
    }
    this.Globe.IsOpen = true;
  }

  private string[] Getculture()
  {
    SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
    ReadOnlyCollection<InstalledVoice> installedVoices = speechSynthesizer.GetInstalledVoices();
    string[] strArray = new string[installedVoices.Count];
    int index = 0;
    foreach (InstalledVoice installedVoice in installedVoices)
    {
      string str = pdfeditor.Properties.Resources.ReadWinFemaleVoice;
      if (installedVoice.VoiceInfo.Gender.ToString() == "Male")
        str = pdfeditor.Properties.Resources.ReadWinMaleVoice;
      strArray[index] = installedVoice.VoiceInfo.Culture.DisplayName.ToString().Replace(")", $", {str} )");
      ++index;
    }
    speechSynthesizer.Dispose();
    return strArray;
  }

  public int GetcultureIndex()
  {
    try
    {
      SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
      CultureInfo culture = CommomLib.Properties.Resources.Culture;
      ReadOnlyCollection<InstalledVoice> installedVoices = speechSynthesizer.GetInstalledVoices();
      for (int index = 0; index < installedVoices.Count; ++index)
      {
        if (culture.ToString() == installedVoices[index].VoiceInfo.Culture.ToString())
          return index;
      }
    }
    catch
    {
    }
    return 0;
  }

  private void CultureListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    ListBox listBox = (ListBox) sender;
    if (listBox != null)
    {
      if (this.VM.IsReading && listBox.SelectedIndex != this.VM.speechUtils.CultureIndex)
      {
        if (!this.VM.speechUtils.Setculture(listBox.SelectedIndex, (Window) this))
        {
          listBox.SelectedIndex = this.VM.speechUtils.CultureIndex;
          this.Globe.IsOpen = false;
          return;
        }
        this.VolumeBlock.Text = $"{this.VM.speechUtils.SpeechVolume}";
        this.VolumeSlider.Value = (double) Convert.ToInt32(this.VM.speechUtils.SpeechVolume);
        this.ToneSli.Value = (double) Convert.ToInt32(this.VM.speechUtils.Pitch + 5.0);
        this.SpeedSli.Value = (double) Convert.ToInt32((this.VM.speechUtils.Rate + 10.0) / 2.0);
      }
      else
        this.VM.ReadCulIndex = listBox.SelectedIndex;
    }
    this.Globe.IsOpen = false;
  }

  private void SpeedSli_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
  {
    try
    {
      double newValue = e.NewValue;
      if (this.VM.speechUtils == null)
        return;
      GAManager.SendEvent("PDFReader", "SpeedChange", "Count", 1L);
      this.VM.speechUtils.Rate = newValue * 2.0 - 10.0;
    }
    catch
    {
    }
  }

  private void ToneSli_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
  {
    try
    {
      double newValue = e.NewValue;
      if (this.VM.speechUtils == null)
        return;
      GAManager.SendEvent("PDFReader", "ToneChange", "Count", 1L);
      this.VM.speechUtils.Pitch = newValue - 5.0;
    }
    catch
    {
    }
  }

  private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
  {
    try
    {
      double newValue = e.NewValue;
      if (this.VM.speechUtils != null)
      {
        this.VM.speechUtils.SpeechVolume = (float) Convert.ToInt32(newValue);
        if (this.VolumeBlock == null)
          return;
        this.VolumeBlock.Text = $"{this.VM.speechUtils.SpeechVolume}";
      }
      else
        this.VolumeBlock.Text = newValue.ToString();
    }
    catch
    {
    }
  }

  private void StopButton_Click(object sender, RoutedEventArgs e)
  {
    this.VM.speechUtils.Stop();
    this.VM.speechUtils?.Dispose();
    this.VM.speechUtils = (SpeechUtils) null;
    ContextMenuModel contextMenu = (this.VM.ViewToolbar.ReadButtonModel.ChildButtonModel as ToolbarChildCheckableButtonModel).ContextMenu as ContextMenuModel;
    (contextMenu[2] as SpeedContextMenuItemModel).IsEnabled = true;
    (contextMenu[1] as SpeedContextMenuItemModel).IsEnabled = true;
    (contextMenu[0] as SpeedContextMenuItemModel).IsEnabled = true;
    (contextMenu[0] as SpeedContextMenuItemModel).Icon = (ImageSource) null;
    (contextMenu[1] as SpeedContextMenuItemModel).Icon = (ImageSource) null;
    (contextMenu[2] as SpeedContextMenuItemModel).Icon = (ImageSource) null;
    this.VM.ViewToolbar.ReadButtonModel.IsChecked = false;
    this.VM.IsReading = false;
    this.StopBtn.IsEnabled = false;
  }

  private void StartButton_Click(object sender, RoutedEventArgs e)
  {
    ContextMenuModel contextMenu = (this.VM.ViewToolbar.ReadButtonModel.ChildButtonModel as ToolbarChildCheckableButtonModel).ContextMenu as ContextMenuModel;
    (contextMenu[0] as SpeedContextMenuItemModel).IsEnabled = false;
    (contextMenu[1] as SpeedContextMenuItemModel).IsEnabled = false;
    (contextMenu[2] as SpeedContextMenuItemModel).IsEnabled = false;
    this.VM.ViewToolbar.ReadButtonModel.IsChecked = true;
    this.StopBtn.IsEnabled = true;
    this.GlobeBtn.IsEnabled = true;
    if (this.VM.speechUtils != null)
    {
      if (this.VM.speechUtils.ProcessorStream != null)
        this.VM.speechUtils.PauseSpeak();
      else if ((contextMenu[0] as SpeedContextMenuItemModel).IsChecked)
      {
        (contextMenu[0] as SpeedContextMenuItemModel).IsChecked = true;
        this.VM.speechUtils.SpeakCurrentPage(this.VM.CurrnetPageIndex - 1);
      }
      else if ((contextMenu[2] as SpeedContextMenuItemModel).IsChecked)
      {
        (contextMenu[2] as SpeedContextMenuItemModel).IsChecked = true;
        this.VM.speechUtils.SpeakPages(0, this.VM.Document.Pages.Count - 1);
      }
      else
      {
        (contextMenu[1] as SpeedContextMenuItemModel).IsChecked = true;
        this.VM.speechUtils.SpeakPages(this.VM.CurrnetPageIndex - 1, this.VM.Document.Pages.Count - 1);
      }
    }
    else
    {
      this.VM.ViewToolbar.ReadButtonModel.IsChecked = true;
      PdfDocument document = this.VM.Document == null ? (PdfDocument) null : this.VM.Document;
      this.VM.IsReading = true;
      this.VM.speechUtils?.Dispose();
      this.VM.speechUtils = new SpeechUtils(document);
      this.VM.speechUtils.Rate = this.SpeedSli.Value * 2.0 - 10.0;
      this.VM.speechUtils.SpeechVolume = (float) Convert.ToInt32(this.VolumeSlider.Value);
      this.VM.speechUtils.Pitch = (double) Convert.ToInt32(this.ToneSli.Value - 5.0);
      this.VM.speechUtils.CultureIndex = this.VM.speechControl.CultureListBox.SelectedIndex >= 0 ? this.VM.speechControl.CultureListBox.SelectedIndex : this.VM.speechUtils.GetcultureIndex();
      if ((contextMenu[0] as SpeedContextMenuItemModel).IsChecked)
      {
        (contextMenu[0] as SpeedContextMenuItemModel).IsChecked = true;
        this.VM.speechUtils.SpeakCurrentPage(this.VM.CurrnetPageIndex - 1);
      }
      else if ((contextMenu[2] as SpeedContextMenuItemModel).IsChecked)
      {
        (contextMenu[2] as SpeedContextMenuItemModel).IsChecked = true;
        this.VM.speechUtils.SpeakPages(0, this.VM.Document.Pages.Count - 1);
      }
      else
      {
        (contextMenu[1] as SpeedContextMenuItemModel).IsChecked = true;
        this.VM.speechUtils.SpeakPages(this.VM.CurrnetPageIndex - 1, this.VM.Document.Pages.Count - 1);
      }
    }
  }

  private void CluButton_Click(object sender, RoutedEventArgs e)
  {
    Process.Start("explorer.exe", "ms-settings:speech");
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/speech/speechcontrol.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this._imgicon = (Image) target;
        break;
      case 2:
        this.VolumeSlider = (Slider) target;
        break;
      case 3:
        this.VolumeBlock = (TextBlock) target;
        break;
      case 4:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.StartButton_Click);
        break;
      case 5:
        this.Start = (Image) target;
        break;
      case 6:
        this.StopBtn = (Button) target;
        this.StopBtn.Click += new RoutedEventHandler(this.StopButton_Click);
        break;
      case 7:
        this.ToneBtn = (Button) target;
        this.ToneBtn.Click += new RoutedEventHandler(this.TonedBtn_Click);
        break;
      case 8:
        this.SpeedBtn = (Button) target;
        this.SpeedBtn.Click += new RoutedEventHandler(this.Button_Click);
        break;
      case 9:
        this.GlobeBtn = (Button) target;
        this.GlobeBtn.Click += new RoutedEventHandler(this.GlobeBtn_Click);
        break;
      case 10:
        this.Speed = (Popup) target;
        break;
      case 11:
        this.SpeedSli = (FormattedSlider) target;
        break;
      case 12:
        this.Tone = (Popup) target;
        break;
      case 13:
        this.ToneSli = (FormattedSlider) target;
        break;
      case 14:
        this.Globe = (Popup) target;
        break;
      case 15:
        this.CultureListBox = (ListBox) target;
        break;
      case 16 /*0x10*/:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.CluButton_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
