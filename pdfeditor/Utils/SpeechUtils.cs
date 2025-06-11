// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.SpeechUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using NAudio.Wave;
using Patagames.Pdf;
using Patagames.Pdf.Net;
using pdfeditor.Controls.Speech;
using pdfeditor.Models.Menus;
using pdfeditor.ViewModels;
using SoundTouch.Net.NAudioSupport;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Utils;

public class SpeechUtils : IDisposable
{
  private PdfDocument pdfDocument;
  public SpeechSynthesizer speechSynthesizer;
  private MemoryStream memoryStream;
  public bool IsReading;
  private bool Readed;
  private bool Readcurent = true;
  private string read;
  private double _pitch;
  private double _rate;
  private int _tempo;
  private float Volume;
  private int Pageend = 1;
  private int Pagesindex;
  public bool Pausing = true;
  public int CultureIndex;
  public bool toneChange;
  private MainViewModel viewModel = Ioc.Default.GetRequiredService<MainViewModel>();
  private IWavePlayer _waveOut;
  private bool disposedValue;

  public SpeechUtils(PdfDocument PdfDocument)
  {
    this.pdfDocument = PdfDocument;
    this.Rate = 0.0;
    this.Volume = 60f;
    if (this.viewModel.ReadCulIndex < 0)
    {
      this.CultureIndex = this.GetcultureIndex();
      this.viewModel.ReadCulIndex = this.CultureIndex;
    }
    else
      this.CultureIndex = this.viewModel.ReadCulIndex;
  }

  public void SpeakCurrentPage(int PageIndex)
  {
    this.Pagesindex = PageIndex;
    this.Pageend = PageIndex;
    this.Reading(PageIndex);
    this.Readcurent = true;
  }

  public bool Setculture(int index, Window window)
  {
    bool flag = false;
    if (this._waveOut.PlaybackState == PlaybackState.Playing)
      flag = true;
    SpeechMessage speechMessage = new SpeechMessage(this.speechSynthesizer.GetInstalledVoices()[index].VoiceInfo.Culture.DisplayName, this.Pagesindex);
    speechMessage.Owner = window;
    speechMessage.WindowStartupLocation = WindowStartupLocation.Manual;
    speechMessage.Top = window.Top + 130.0;
    speechMessage.Left = window.Left - 50.0;
    if (speechMessage.ShowDialog().Value)
    {
      this.CultureIndex = index;
      this.viewModel.ReadCulIndex = index;
      if (this._waveOut != null)
        this._waveOut.PlaybackStopped -= new EventHandler<StoppedEventArgs>(this.OnPlaybackStopped);
      this.Close();
      this.Reading(this.Pagesindex);
      return true;
    }
    if (flag)
      this.Play();
    return false;
  }

  public void Activated()
  {
    if (this._waveOut != null)
      this._waveOut.PlaybackStopped -= new EventHandler<StoppedEventArgs>(this.OnPlaybackStopped);
    this.speechSynthesizer?.Pause();
    this.speechSynthesizer?.Dispose();
    this.Pageend = -1;
    this.memoryStream?.Dispose();
    this.Close();
  }

  public void PauseSpeak()
  {
    try
    {
      MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
      if (this._waveOut.PlaybackState == PlaybackState.Playing)
      {
        this.Pause();
        this.Pausing = true;
        requiredService.speechControl.imgaeFilePath.ImagePath = "pack://application:,,,/Style/Resources/Speech/Play.png";
      }
      else
      {
        this.Play();
        this.Pausing = false;
        requiredService.speechControl.imgaeFilePath.ImagePath = "pack://application:,,,/Style/Resources/Speech/Pause.png";
      }
    }
    catch
    {
    }
  }

  public bool isSpeak()
  {
    return this._waveOut != null && this._waveOut.PlaybackState == PlaybackState.Playing;
  }

  public void SpeakPages(int PageStart, int PageEnd)
  {
    this.Pageend = PageEnd;
    this.Pagesindex = PageStart;
    this.Readcurent = false;
    if (PageEnd > 1)
      this.Reading(this.Pagesindex);
    else
      this.Reading(this.Pagesindex);
  }

  private void Reading(int PageIndex)
  {
    try
    {
      this.speechSynthesizer?.Dispose();
      this.speechSynthesizer = new SpeechSynthesizer();
      this.viewModel.IsReading = true;
      this.speechSynthesizer.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(this.SpeechSynthesizer_SpeakCompleted);
      this.speechSynthesizer.Volume = 100;
      this.speechSynthesizer.SelectVoice(this.speechSynthesizer.GetInstalledVoices()[this.CultureIndex].VoiceInfo.Name);
      this.read = SpeechUtils.ExtractTextFromPage(this.pdfDocument, PageIndex).Replace("™", " ").Replace("\r\n\r\n", "   ").Replace("\r\n", "");
      this.memoryStream = new MemoryStream();
      this.speechSynthesizer.SetOutputToWaveStream((Stream) this.memoryStream);
      if ((string.IsNullOrEmpty(this.read) || string.IsNullOrEmpty(this.read.Replace(" ", ""))) && this.Readcurent)
      {
        int num = (int) MessageBox.Show(App.Current.MainWindow, pdfeditor.Properties.Resources.ReadPageEmpty.Replace("XXX", (PageIndex + 1).ToString()), UtilManager.GetProductName());
        ContextMenuModel contextMenu = (this.viewModel.ViewToolbar.ReadButtonModel.ChildButtonModel as ToolbarChildCheckableButtonModel).ContextMenu as ContextMenuModel;
        (contextMenu[2] as SpeedContextMenuItemModel).IsEnabled = true;
        (contextMenu[1] as SpeedContextMenuItemModel).IsEnabled = true;
        (contextMenu[0] as SpeedContextMenuItemModel).IsEnabled = true;
        (contextMenu[0] as SpeedContextMenuItemModel).Icon = (ImageSource) null;
        (contextMenu[1] as SpeedContextMenuItemModel).Icon = (ImageSource) null;
        (contextMenu[2] as SpeedContextMenuItemModel).Icon = (ImageSource) null;
        this.viewModel.ViewToolbar.ReadButtonModel.IsChecked = false;
        this.viewModel.IsReading = false;
        if (this.viewModel.speechControl == null)
          return;
        this.viewModel.speechControl.StopBtn.IsEnabled = false;
      }
      else
      {
        this.Readcurent = false;
        this.Readed = true;
        this.speechSynthesizer.SpeakAsync(this.read);
        MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
        if (requiredService.speechControl == null)
          return;
        requiredService.speechControl.StopBtn.IsEnabled = true;
        requiredService.speechControl.GlobeBtn.IsEnabled = true;
      }
    }
    catch
    {
      try
      {
        this.read = SpeechUtils.ExtractTextFromPage(this.pdfDocument, PageIndex).Replace("™", " ").Replace("\r\n\r\n", "   ").Replace("\r\n", "");
        this.memoryStream = new MemoryStream();
        this.speechSynthesizer.SetOutputToWaveStream((Stream) this.memoryStream);
        if ((string.IsNullOrEmpty(this.read) || string.IsNullOrEmpty(this.read.Replace(" ", ""))) && this.Readcurent)
        {
          int num = (int) MessageBox.Show(App.Current.MainWindow, pdfeditor.Properties.Resources.ReadPageEmpty.Replace("XXX", (PageIndex + 1).ToString()), UtilManager.GetProductName());
          ContextMenuModel contextMenu = (this.viewModel.ViewToolbar.ReadButtonModel.ChildButtonModel as ToolbarChildCheckableButtonModel).ContextMenu as ContextMenuModel;
          (contextMenu[2] as SpeedContextMenuItemModel).IsEnabled = true;
          (contextMenu[1] as SpeedContextMenuItemModel).IsEnabled = true;
          (contextMenu[0] as SpeedContextMenuItemModel).IsEnabled = true;
          (contextMenu[0] as SpeedContextMenuItemModel).Icon = (ImageSource) null;
          (contextMenu[1] as SpeedContextMenuItemModel).Icon = (ImageSource) null;
          (contextMenu[2] as SpeedContextMenuItemModel).Icon = (ImageSource) null;
          this.viewModel.ViewToolbar.ReadButtonModel.IsChecked = false;
          this.viewModel.IsReading = false;
          if (this.viewModel.speechControl == null)
            return;
          this.viewModel.speechControl.StopBtn.IsEnabled = false;
        }
        else
        {
          this.Readcurent = false;
          this.Readed = true;
          this.speechSynthesizer.SpeakAsync(this.read);
          MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
          if (requiredService.speechControl == null)
            return;
          requiredService.speechControl.StopBtn.IsEnabled = true;
          requiredService.speechControl.GlobeBtn.IsEnabled = true;
        }
      }
      catch
      {
        int num = (int) MessageBox.Show(App.Current.MainWindow, pdfeditor.Properties.Resources.ReadPageEmpty.Replace("XXX", (PageIndex + 1).ToString()), UtilManager.GetProductName());
      }
    }
  }

  public int GetcultureIndex()
  {
    try
    {
      this.speechSynthesizer?.Dispose();
      this.speechSynthesizer = new SpeechSynthesizer();
      CultureInfo culture = CommomLib.Properties.Resources.Culture;
      ReadOnlyCollection<InstalledVoice> installedVoices = this.speechSynthesizer.GetInstalledVoices();
      for (int index = 0; index < installedVoices.Count; ++index)
      {
        if (culture.ToString() == installedVoices[index].VoiceInfo.Culture.ToString())
        {
          this.CultureIndex = index;
          this.viewModel.ReadCulIndex = index;
          return index;
        }
      }
    }
    catch
    {
    }
    return 0;
  }

  private void SpeechSynthesizer_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
  {
    try
    {
      if (this.Readed)
      {
        this.memoryStream.Seek(0L, SeekOrigin.Begin);
        if (this.OpenFile((WaveStream) new WaveFileReader((Stream) this.memoryStream)))
          this.Play();
      }
      this.Readed = false;
      if (this.Pageend > this.Pagesindex || !string.IsNullOrEmpty(this.read) && !string.IsNullOrEmpty(this.read.Replace(" ", "")))
        return;
      int num = (int) MessageBox.Show(App.Current.MainWindow, pdfeditor.Properties.Resources.ReadPageEmpty.Replace("XXX", (this.Pagesindex + 1).ToString()), UtilManager.GetProductName());
    }
    catch
    {
    }
  }

  public SoundTouchWaveStream ProcessorStream { get; private set; }

  public event EventHandler<bool> PlaybackStopped = (_, __) => { };

  public double Pitch
  {
    get => this._pitch;
    set
    {
      this.Set<double>(ref this._pitch, value, nameof (Pitch));
      if (this.ProcessorStream == null)
        return;
      this.ProcessorStream.PitchSemiTones = value;
    }
  }

  public float SpeechVolume
  {
    get => this.Volume;
    set
    {
      this.Set<float>(ref this.Volume, value, nameof (SpeechVolume));
      if (this._waveOut == null)
        return;
      this._waveOut.Volume = value / 100f;
    }
  }

  public double Rate
  {
    get => this._rate;
    set
    {
      this.Set<double>(ref this._rate, value, nameof (Rate));
      if (this.ProcessorStream == null)
        return;
      this.ProcessorStream.RateChange = value;
    }
  }

  public int Tempo
  {
    get => this._tempo;
    set
    {
      this.Set<int>(ref this._tempo, value, nameof (Tempo));
      if (this.ProcessorStream == null)
        return;
      this.ProcessorStream.TempoChange = (double) value;
    }
  }

  private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
  {
    if (object.Equals((object) storage, (object) value))
      return;
    storage = value;
    this.OnPropertyChanged(propertyName);
  }

  public event PropertyChangedEventHandler PropertyChanged;

  private void OnPropertyChanged([CallerMemberName] string propertyName = null)
  {
    PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
    if (propertyChanged == null)
      return;
    propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }

  private bool OpenFile(WaveStream wave)
  {
    this.Close();
    try
    {
      this.ProcessorStream = new SoundTouchWaveStream((WaveStream) new WaveChannel32(wave)
      {
        PadWithZeroes = false
      });
      this._waveOut = (IWavePlayer) new WaveOutEvent()
      {
        DesiredLatency = 100
      };
      this._waveOut.Init((IWaveProvider) this.ProcessorStream);
      this._waveOut.PlaybackStopped += new EventHandler<StoppedEventArgs>(this.OnPlaybackStopped);
      return true;
    }
    catch (Exception ex)
    {
      this._waveOut = (IWavePlayer) null;
      return false;
    }
  }

  private void Close()
  {
    this.ProcessorStream?.Dispose();
    this.ProcessorStream = (SoundTouchWaveStream) null;
    this._waveOut?.Dispose();
    this._waveOut = (IWavePlayer) null;
  }

  private void speechQuit()
  {
    bool flag = false;
    foreach (Window window in Application.Current.Windows)
    {
      if (window.GetType() == typeof (SpeechControl))
        flag = true;
    }
    if (!flag)
      return;
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    requiredService.speechControl.StopBtn.IsEnabled = false;
    requiredService.speechControl.imgaeFilePath.ImagePath = "pack://application:,,,/Style/Resources/Speech/Play.png";
  }

  private void OnPlaybackStopped(object sender, StoppedEventArgs args)
  {
    bool e = this.ProcessorStream == null || this.ProcessorStream.Position >= this.ProcessorStream.Length;
    if (this.Pageend == -1)
      return;
    if (e)
    {
      if (this.Pageend > this.Pagesindex)
      {
        try
        {
          ++this.Pagesindex;
          this.read = SpeechUtils.ExtractTextFromPage(this.pdfDocument, this.Pagesindex).Replace("™", " ").Replace("\r\n\r\n", "   ").Replace("\r\n", "");
          this.memoryStream.Close();
          this.memoryStream = new MemoryStream();
          this.memoryStream.Seek(0L, SeekOrigin.Begin);
          this.speechSynthesizer.SetOutputToWaveStream((Stream) this.memoryStream);
          this.Readed = true;
          if (!string.IsNullOrEmpty(this.read))
            this.speechSynthesizer.SpeakAsync(this.read);
        }
        catch
        {
        }
      }
      else
      {
        ContextMenuModel contextMenu = (this.viewModel.ViewToolbar.ReadButtonModel.ChildButtonModel as ToolbarChildCheckableButtonModel).ContextMenu as ContextMenuModel;
        (contextMenu[2] as SpeedContextMenuItemModel).IsEnabled = true;
        (contextMenu[1] as SpeedContextMenuItemModel).IsEnabled = true;
        (contextMenu[0] as SpeedContextMenuItemModel).IsEnabled = true;
        (contextMenu[0] as SpeedContextMenuItemModel).Icon = (ImageSource) null;
        (contextMenu[1] as SpeedContextMenuItemModel).Icon = (ImageSource) null;
        (contextMenu[2] as SpeedContextMenuItemModel).Icon = (ImageSource) null;
        this.viewModel.ViewToolbar.ReadButtonModel.IsChecked = false;
        this.viewModel.IsReading = false;
        this.ProcessorStream?.Dispose();
        this.ProcessorStream = (SoundTouchWaveStream) null;
        if (this.viewModel.speechControl != null)
        {
          this.viewModel.speechControl.StopBtn.IsEnabled = false;
          this.viewModel.speechControl.imgaeFilePath.ImagePath = "pack://application:,,,/Style/Resources/Speech/Play.png";
        }
      }
    }
    this.PlaybackStopped(sender, e);
  }

  public bool Play()
  {
    if (this._waveOut == null)
      return false;
    if (this._waveOut.PlaybackState != PlaybackState.Playing)
    {
      this._waveOut.Volume = this.SpeechVolume / 100f;
      this.ProcessorStream.RateChange = this.Rate;
      this.ProcessorStream.TempoChange = this.Rate;
      this.ProcessorStream.PitchSemiTones = this.Pitch;
      this._waveOut.Play();
      this.Pausing = false;
      MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
      if (requiredService.speechControl != null)
        requiredService.speechControl.imgaeFilePath.ImagePath = "pack://application:,,,/Style/Resources/Speech/Pause.png";
    }
    return true;
  }

  public bool Pause()
  {
    if (this._waveOut == null || this._waveOut.PlaybackState != PlaybackState.Playing)
      return false;
    this._waveOut.Pause();
    this.Pausing = true;
    Ioc.Default.GetRequiredService<MainViewModel>().speechControl.imgaeFilePath.ImagePath = "pack://application:,,,/Style/Resources/Speech/Play.png";
    return true;
  }

  public bool Stop()
  {
    if (this._waveOut == null || this.ProcessorStream == null || this.ProcessorStream.Length == 0L)
      return false;
    this._waveOut.Stop();
    this.ProcessorStream.Position = 0L;
    this.ProcessorStream.Flush();
    this.Pausing = true;
    Ioc.Default.GetRequiredService<MainViewModel>().speechControl.imgaeFilePath.ImagePath = "pack://application:,,,/Style/Resources/Speech/Play.png";
    return true;
  }

  protected virtual void Dispose(bool disposing)
  {
    if (this.disposedValue)
      return;
    if (disposing)
    {
      this.speechSynthesizer?.Pause();
      this.speechSynthesizer?.Dispose();
      this.Pageend = -1;
      this.memoryStream?.Dispose();
      this.speechQuit();
      this.Close();
    }
    this.disposedValue = true;
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  public static string ExtractTextFromPage(PdfDocument document, int pageIndex)
  {
    if (document == null || pageIndex < 0 || pageIndex >= document.Pages.Count)
      return string.Empty;
    IntPtr page = IntPtr.Zero;
    IntPtr text_page = IntPtr.Zero;
    try
    {
      page = Pdfium.FPDF_LoadPage(document.Handle, pageIndex);
      text_page = Pdfium.FPDFText_LoadPage(page);
      int count = Pdfium.FPDFText_CountChars(text_page);
      return count > 0 ? Pdfium.FPDFText_GetText(text_page, 0, count) ?? string.Empty : string.Empty;
    }
    finally
    {
      if (text_page != IntPtr.Zero)
      {
        try
        {
          Pdfium.FPDFText_ClosePage(text_page);
        }
        catch
        {
        }
      }
      if (page != IntPtr.Zero)
      {
        try
        {
          Pdfium.FPDF_ClosePage(page);
        }
        catch
        {
        }
      }
    }
  }
}
