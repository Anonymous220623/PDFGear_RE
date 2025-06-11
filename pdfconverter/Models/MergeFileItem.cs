// Decompiled with JetBrains decompiler
// Type: pdfconverter.Models.MergeFileItem
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
using pdfconverter.Properties;
using pdfconverter.Utils;
using System;
using System.IO;
using System.Threading.Tasks;

#nullable disable
namespace pdfconverter.Models;

public class MergeFileItem : BindableBase
{
  private string _filePath;
  private int _pageCount;
  private int _pageFrom;
  private int _pageTo;
  private MergeStatus _status;
  private bool? _isFileSelected = new bool?(true);
  private string _password;

  public MergeFileItem(string file)
  {
    this._filePath = file;
    this.IsFileSelected = new bool?(true);
  }

  public string FilePath => this._filePath;

  public string FileName
  {
    get => string.IsNullOrWhiteSpace(this._filePath) ? "" : Path.GetFileName(this._filePath);
  }

  public bool? IsFileSelected
  {
    get => this._isFileSelected;
    set => this.SetProperty<bool?>(ref this._isFileSelected, value, nameof (IsFileSelected));
  }

  public string Passwrod
  {
    get => this._password;
    set => this.SetProperty<string>(ref this._password, value, nameof (Passwrod));
  }

  public int PageCount
  {
    get => this._pageCount > 0 ? this._pageCount : 0;
    set => this.SetProperty<int>(ref this._pageCount, value, nameof (PageCount));
  }

  public int PageFrom
  {
    get => this._pageFrom;
    set
    {
      int num1 = value;
      if (num1 <= 0 || num1 > this._pageCount || num1 > this._pageTo)
      {
        int num2 = (int) ModernMessageBox.Show(Resources.FileConvertMsgInvaildPageNum, UtilManager.GetProductName());
      }
      else
        this.SetProperty<int>(ref this._pageFrom, num1, nameof (PageFrom));
    }
  }

  public int PageTo
  {
    get => this._pageTo;
    set
    {
      int num1 = value;
      if (num1 <= 0 || num1 > this._pageCount || num1 < this._pageFrom)
      {
        int num2 = (int) ModernMessageBox.Show(Resources.FileConvertMsgInvaildPageNum, UtilManager.GetProductName());
      }
      else
        this.SetProperty<int>(ref this._pageTo, num1, nameof (PageTo));
    }
  }

  public MergeStatus Status
  {
    get => this._status;
    set => this.SetProperty<MergeStatus>(ref this._status, value, nameof (Status));
  }

  public void parseFile(string password)
  {
    Task.Run(TaskExceptionHelper.ExceptionBoundary((Action) (() =>
    {
      this.Status = MergeStatus.Loading;
      int result = PdfiumNetHelper.GetPageCountAsync(this._filePath, password).GetAwaiter().GetResult();
      if (result > 0)
      {
        this.PageCount = result;
        this.PageTo = result;
        this.PageFrom = 1;
        this.IsFileSelected = new bool?(true);
        this.Status = MergeStatus.Loaded;
        this.Passwrod = password;
      }
      else
      {
        this.PageCount = 0;
        this.IsFileSelected = new bool?(false);
        this.Status = MergeStatus.Unsupport;
      }
    })));
  }
}
