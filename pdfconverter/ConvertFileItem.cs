// Decompiled with JetBrains decompiler
// Type: pdfconverter.ConvertFileItem
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
using pdfconverter.Properties;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable disable
namespace pdfconverter;

internal class ConvertFileItem : INotifyPropertyChanged
{
  private bool? _isFileSelected = new bool?(true);
  private string _filePath;
  private FileCovertStatus _status;
  private int _pageCount;
  private int _pageFrom;
  private int _pageTo;
  private bool? _withOCR = new bool?(false);
  private bool? _singleSheet = new bool?(true);
  private string _password;

  public ConvertFileItem(string file)
  {
    this._filePath = file;
    this._pageCount = -1;
    this.ConvertStatus = FileCovertStatus.ConvertInit;
  }

  public void parseFile(string password)
  {
    Task.Run(TaskExceptionHelper.ExceptionBoundary((Action) (() =>
    {
      this.ConvertStatus = FileCovertStatus.ConvertLoading;
      int result = PdfiumNetHelper.GetPageCountAsync(this._filePath, password).GetAwaiter().GetResult();
      if (result > 0)
      {
        GAManager.SendEvent("PDFConvert", "Loaded", "Count", 1L);
        this.PageCount = result;
        this.PageTo = result;
        this.PageFrom = 1;
        this.FileSelected = new bool?(true);
        this.WithOCR = new bool?(true);
        this.ConvertStatus = FileCovertStatus.ConvertLoaded;
        this.PassWord = password;
      }
      else
      {
        GAManager.SendEvent("PDFConvert", "Unsupport", "Count", 1L);
        this.PageCount = 0;
        this.FileSelected = new bool?(false);
        this.ConvertStatus = FileCovertStatus.ConvertUnsupport;
      }
    })));
  }

  public string convertFile => this._filePath;

  public string PassWord
  {
    get => this._password;
    set
    {
      this._password = value;
      this.RaisePropertyChanged(nameof (PassWord));
    }
  }

  public FileCovertStatus ConvertStatus
  {
    get => this._status;
    set
    {
      this._status = value;
      this.RaisePropertyChanged(nameof (ConvertStatus));
    }
  }

  public string FileName
  {
    get => string.IsNullOrWhiteSpace(this._filePath) ? "" : Path.GetFileName(this._filePath);
  }

  public int PageCount
  {
    get => this._pageCount > 0 ? this._pageCount : 0;
    set
    {
      this._pageCount = value;
      this.RaisePropertyChanged(nameof (PageCount));
    }
  }

  public int PageFrom
  {
    get => this._pageFrom;
    set
    {
      int num1 = value;
      if (num1 < 0 || num1 > this._pageCount || num1 > this._pageTo)
      {
        int num2 = (int) ModernMessageBox.Show(Resources.FileConvertMsgInvaildPageNum, UtilManager.GetProductName());
        this.RaisePropertyChanged(nameof (PageFrom));
      }
      else
      {
        this._pageFrom = num1;
        this.RaisePropertyChanged(nameof (PageFrom));
      }
    }
  }

  public int PageTo
  {
    get => this._pageTo;
    set
    {
      int num1 = value;
      if (num1 < 0 || num1 > this._pageCount || num1 < this._pageFrom)
      {
        int num2 = (int) ModernMessageBox.Show(Resources.FileConvertMsgInvaildPageNum, UtilManager.GetProductName());
        this.RaisePropertyChanged(nameof (PageTo));
      }
      else
      {
        this._pageTo = num1;
        this.RaisePropertyChanged(nameof (PageTo));
      }
    }
  }

  public bool? FileSelected
  {
    get => this._isFileSelected;
    set
    {
      this._isFileSelected = value;
      this.RaisePropertyChanged(nameof (FileSelected));
    }
  }

  public bool? WithOCR
  {
    get => this._withOCR;
    set
    {
      this._withOCR = value;
      this.RaisePropertyChanged(nameof (WithOCR));
    }
  }

  public bool? SingleSheet
  {
    get => this._singleSheet;
    set
    {
      this._singleSheet = value;
      this.RaisePropertyChanged(nameof (SingleSheet));
    }
  }

  public string outputFile { get; set; }

  public bool outputFileIsDir { get; set; }

  public event PropertyChangedEventHandler PropertyChanged;

  protected void RaisePropertyChanged([CallerMemberName] string name = "")
  {
    try
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(name));
    }
    catch
    {
    }
  }
}
