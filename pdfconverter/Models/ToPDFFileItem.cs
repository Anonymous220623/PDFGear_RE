// Decompiled with JetBrains decompiler
// Type: pdfconverter.Models.ToPDFFileItem
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using pdfconverter.Properties;
using System;
using System.IO;
using System.Threading.Tasks;

#nullable disable
namespace pdfconverter.Models;

public class ToPDFFileItem : ObservableObject
{
  private int _pageCount;
  private int _pageFrom;
  private int _pageTo;
  private ToPDFItemStatus _status;
  private bool? _isFileSelected = new bool?(true);
  private bool _isEnable = true;
  private ConvToPDFType ConvType;
  private string _password;

  public ToPDFFileItem(string file, ConvToPDFType type)
  {
    this.FilePath = file;
    this.IsFileSelected = new bool?(true);
    this.ConvType = type;
  }

  public string FilePath { get; }

  public string OutputPath { get; set; }

  public string Extention => new FileInfo(this.FilePath).Extension;

  public string Password
  {
    get => this._password;
    set => this.SetProperty<string>(ref this._password, value, nameof (Password));
  }

  public string FileName
  {
    get => string.IsNullOrWhiteSpace(this.FilePath) ? "" : Path.GetFileName(this.FilePath);
  }

  public bool? IsFileSelected
  {
    get => this._isFileSelected;
    set => this.SetProperty<bool?>(ref this._isFileSelected, value, nameof (IsFileSelected));
  }

  public bool? IsEnable
  {
    get => this._isFileSelected;
    set => this.SetProperty<bool?>(ref this._isFileSelected, value, nameof (IsEnable));
  }

  public int PageCount
  {
    get => this._pageCount;
    set => this.SetProperty<int>(ref this._pageCount, value, nameof (PageCount));
  }

  public int PageFrom
  {
    get => this._pageFrom;
    set
    {
      int newValue = value;
      if (newValue <= 0 || newValue > this._pageCount || newValue > this._pageTo)
      {
        int num = (int) ModernMessageBox.Show(Resources.FileConvertMsgInvaildPageNum, UtilManager.GetProductName());
      }
      else
        this.SetProperty<int>(ref this._pageFrom, newValue, nameof (PageFrom));
    }
  }

  public int PageTo
  {
    get => this._pageTo;
    set
    {
      int newValue = value;
      if (newValue <= 0 || newValue > this._pageCount || newValue < this._pageFrom)
      {
        int num = (int) ModernMessageBox.Show(Resources.FileConvertMsgInvaildPageNum, UtilManager.GetProductName());
      }
      else
        this.SetProperty<int>(ref this._pageTo, newValue, nameof (PageTo));
    }
  }

  public ToPDFItemStatus Status
  {
    get => this._status;
    set => this.SetProperty<ToPDFItemStatus>(ref this._status, value, nameof (Status));
  }

  public void ParseFile()
  {
    Task.Run(TaskExceptionHelper.ExceptionBoundary((Func<Task>) (async () =>
    {
      this.Status = ToPDFItemStatus.Loading;
      this.GetPagesNum();
    })));
  }

  private async void GetPagesNum()
  {
    await Task.Run(TaskExceptionHelper.ExceptionBoundary((Action) (() =>
    {
      string extension = Path.GetExtension(this.FilePath);
      if (string.IsNullOrWhiteSpace(extension))
      {
        this.IsFileSelected = new bool?(false);
        this.Status = ToPDFItemStatus.Unsupport;
      }
      else if ((!CommomLib.ConvertUtils.ConvertUtils.ExtEquals(extension, UtilManager.WordExtention) ? (!CommomLib.ConvertUtils.ConvertUtils.ExtEquals(extension, UtilManager.ExcelExtention) ? (!CommomLib.ConvertUtils.ConvertUtils.ExtEquals(extension, UtilManager.RtfExtention) ? (!CommomLib.ConvertUtils.ConvertUtils.ExtEquals(extension, UtilManager.PPTExtention) ? (!CommomLib.ConvertUtils.ConvertUtils.ExtEquals(extension, UtilManager.TxtExtention) ? (!CommomLib.ConvertUtils.ConvertUtils.ExtEquals(extension, UtilManager.ImageExtention) ? -1 : (this.ConvType == ConvToPDFType.ImageToPDF ? 1 : -1)) : (this.ConvType == ConvToPDFType.TxtToPDF ? 1 : -1)) : (this.ConvType == ConvToPDFType.PPTToPDF ? 1 : -1)) : (this.ConvType == ConvToPDFType.RtfToPDF ? 1 : -1)) : (this.ConvType == ConvToPDFType.ExcelToPDF ? 1 : -1)) : (this.ConvType == ConvToPDFType.WordToPDF ? 1 : -1)) > 0)
      {
        this.IsFileSelected = new bool?(true);
        this.Status = ToPDFItemStatus.Loaded;
      }
      else
      {
        this.IsFileSelected = new bool?(false);
        this.Status = ToPDFItemStatus.Unsupport;
      }
    })));
  }
}
