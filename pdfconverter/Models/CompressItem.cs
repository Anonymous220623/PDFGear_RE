// Decompiled with JetBrains decompiler
// Type: pdfconverter.Models.CompressItem
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using pdfconverter.ViewModels;
using System;
using System.IO;

#nullable disable
namespace pdfconverter.Models;

public class CompressItem : ObservableObject
{
  private static readonly string CompressTemp = Path.Combine(AppDataHelper.TemporaryFolder, "Compress");
  private static readonly string CompressHigh = Path.Combine(CompressItem.CompressTemp, "Hight");
  private static readonly string CompressMeidum = Path.Combine(CompressItem.CompressTemp, "Meidum");
  private static readonly string CompressLow = Path.Combine(CompressItem.CompressTemp, "Low");
  private CompressPDFUCViewModel Parent;
  private string filePath;
  private string fileName;
  private string outputPath;
  private string sourceSize;
  private string compressedHighSize = "";
  private string compressedMediumSize = "";
  private string compressedLowSize = "";
  private string compressedSize = "";
  private string _password;
  private bool? _isFileSelected = new bool?(true);
  private bool? _isEnable;
  private int compressMode;
  private ToPDFItemStatus _status;
  private object locker = new object();

  public CompressItem(string path, CompressMode mode, CompressPDFUCViewModel model)
  {
    this.FileName = Path.GetFileName(path);
    this.FilePath = path;
    this.sourceSize = this.GetFileSize(path);
    this._status = ToPDFItemStatus.Loaded;
    this._isEnable = new bool?(true);
    this.compressMode = (int) mode;
    this.Parent = model;
  }

  public string CompressHighFilePath => Path.Combine(CompressItem.CompressHigh, this.fileName);

  public string CompressMediumFilePath => Path.Combine(CompressItem.CompressMeidum, this.fileName);

  public string CompressLowFilePath => Path.Combine(CompressItem.CompressLow, this.fileName);

  public string CompressHighFolderPath => CompressItem.CompressHigh;

  public string CompressMediumFolderPath => CompressItem.CompressMeidum;

  public string CompressLowFolderPath => CompressItem.CompressLow;

  public string Password
  {
    get => this._password;
    set => this.SetProperty<string>(ref this._password, value, nameof (Password));
  }

  private void UpdateSize()
  {
    if (this.compressMode == 0)
      this.CompressedSize = this.CompressedHighSize;
    else if (this.compressMode == 1)
      this.CompressedSize = this.CompressedMediumSize;
    else
      this.CompressedSize = this.CompressedLowSize;
  }

  public string OutputPath
  {
    get => this.outputPath;
    set => this.SetProperty<string>(ref this.outputPath, value, nameof (OutputPath));
  }

  public int Compress_Mode
  {
    get => this.compressMode;
    set
    {
      this.SetProperty<int>(ref this.compressMode, value, nameof (Compress_Mode));
      this.UpdateSize();
      this.Parent.SelectModeChanged();
    }
  }

  public string FilePath
  {
    get => this.filePath;
    set => this.SetProperty<string>(ref this.filePath, value, nameof (FilePath));
  }

  public string FileName
  {
    get => this.fileName;
    set => this.SetProperty<string>(ref this.fileName, value, nameof (FileName));
  }

  public string SourceSize => this.sourceSize;

  public string CompressedSize
  {
    get
    {
      if (this.compressMode == 0)
      {
        this.compressedSize = this.CompressedHighSize;
        return this.compressedSize;
      }
      if (this.compressMode == 1)
      {
        this.compressedSize = this.CompressedMediumSize;
        return this.compressedSize;
      }
      this.compressedSize = this.CompressedLowSize;
      return this.compressedSize;
    }
    set
    {
      if (this.compressMode == 0)
        this.CompressedHighSize = value;
      else if (this.compressMode == 1)
        this.CompressedMediumSize = value;
      else
        this.CompressedLowSize = value;
      this.SetProperty<string>(ref this.compressedSize, value, nameof (CompressedSize));
    }
  }

  private string CompressedHighSize
  {
    get => this.compressedHighSize;
    set
    {
      this.SetProperty<string>(ref this.compressedHighSize, value, nameof (CompressedHighSize));
    }
  }

  private string CompressedMediumSize
  {
    get => this.compressedMediumSize;
    set
    {
      this.SetProperty<string>(ref this.compressedMediumSize, value, nameof (CompressedMediumSize));
    }
  }

  private string CompressedLowSize
  {
    get => this.compressedLowSize;
    set => this.SetProperty<string>(ref this.compressedLowSize, value, nameof (CompressedLowSize));
  }

  public bool? IsFileSelected
  {
    get => this._isFileSelected;
    set => this.SetProperty<bool?>(ref this._isFileSelected, value, nameof (IsFileSelected));
  }

  public bool? IsEnable
  {
    get => this._isEnable;
    set => this.SetProperty<bool?>(ref this._isEnable, value, nameof (IsEnable));
  }

  public ToPDFItemStatus Status
  {
    get => this._status;
    set => this.SetProperty<ToPDFItemStatus>(ref this._status, value, nameof (Status));
  }

  public string GetFileSize(string FileName)
  {
    if (!File.Exists(FileName))
      return "";
    long length = new FileInfo(FileName).Length;
    double x = 1024.0;
    if ((double) length < x)
      return length.ToString() + "B";
    if ((double) length < Math.Pow(x, 2.0))
      return ((double) length / x).ToString("f2") + "K";
    if ((double) length < Math.Pow(x, 3.0))
      return ((double) length / Math.Pow(x, 2.0)).ToString("f2") + "M";
    return (double) length < Math.Pow(x, 4.0) ? ((double) length / Math.Pow(x, 3.0)).ToString("f2") + "G" : ((double) length / Math.Pow(x, 4.0)).ToString("f2") + "T";
  }
}
