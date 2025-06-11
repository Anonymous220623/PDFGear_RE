// Decompiled with JetBrains decompiler
// Type: pdfconverter.Models.SplitFileItem
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
using pdfconverter.Properties;
using pdfconverter.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#nullable disable
namespace pdfconverter.Models;

public class SplitFileItem : BindableBase
{
  private string _filePath;
  private int _pageCount;
  private int _pageFrom;
  private int _pageTo;
  private SplitStatus _status;
  private int _pageSplitMode;
  private string _pageSplitModeStr;
  private string _outputPath;
  private bool? _isFileSelected = new bool?(true);
  private string _password;

  public SplitFileItem(string file)
  {
    this._filePath = file;
    this.IsFileSelected = new bool?(true);
    this.PageSplitMode = 0;
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

  public string Password
  {
    get => this._password;
    set => this.SetProperty<string>(ref this._password, value, nameof (Password));
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

  public SplitStatus Status
  {
    get => this._status;
    set => this.SetProperty<SplitStatus>(ref this._status, value, nameof (Status));
  }

  public int PageSplitMode
  {
    get => this._pageSplitMode;
    set
    {
      this.SetProperty<int>(ref this._pageSplitMode, value, nameof (PageSplitMode));
      this.PageSplitModeStr = "";
      this.OnPropertyChanged("PageSplitModePlaceHolder");
    }
  }

  public string PageSplitModeStr
  {
    get => this._pageSplitModeStr;
    set
    {
      string str = value;
      if (!string.IsNullOrWhiteSpace(str))
      {
        if (this.PageSplitMode == 0)
        {
          int[][] pageIndexes;
          if (!PageRangeHelper.TryParsePageRange2(str, out pageIndexes, out int _) || !this.CheckPageRange(pageIndexes, this.PageCount - 1))
          {
            int num = (int) ModernMessageBox.Show(Resources.WinMergeSplitSplitFileCheckpageRangeMsg, UtilManager.GetProductName());
            return;
          }
        }
        else
        {
          if (this.PageSplitMode != 1)
            return;
          if (new Regex("^\\d+$").IsMatch(str))
          {
            int int32 = Convert.ToInt32(str);
            if (int32 <= 0 || int32 >= this.PageCount)
            {
              int num = (int) ModernMessageBox.Show(Resources.WinMergeSplitSplitFileCheckpageRangeMsg, UtilManager.GetProductName());
              return;
            }
          }
          else
          {
            int num = (int) ModernMessageBox.Show(Resources.WinMergeSplitSplitFileCheckpageRangeMsg, UtilManager.GetProductName());
            return;
          }
        }
      }
      this.SetProperty<string>(ref this._pageSplitModeStr, value, nameof (PageSplitModeStr));
    }
  }

  public string PageSplitModePlaceHolder
  {
    get
    {
      if (this.PageSplitMode == 0)
        return Resources.WinMergeSplitSplitModeCustomRangeHelpAsgMsg;
      return this.PageSplitMode == 1 ? Resources.WinMergeSplitSplitModeFixedRangeHelpAsgMsg : "";
    }
  }

  public string OutputPath
  {
    get => this._outputPath;
    set => this.SetProperty<string>(ref this._outputPath, value, nameof (OutputPath));
  }

  public void parseFile(string passsword)
  {
    Task.Run(TaskExceptionHelper.ExceptionBoundary((Action) (() =>
    {
      this.Status = SplitStatus.Loading;
      int result = PdfiumNetHelper.GetPageCountAsync(this._filePath, passsword).GetAwaiter().GetResult();
      if (result > 0)
      {
        this.PageCount = result;
        this.PageTo = result;
        this.PageFrom = 1;
        this.IsFileSelected = new bool?(true);
        this.Status = SplitStatus.Loaded;
        this.Password = passsword;
      }
      else
      {
        this.PageCount = 0;
        this.IsFileSelected = new bool?(false);
        this.Status = SplitStatus.Unsupport;
      }
    })));
  }

  private bool CheckPageRange(int[][] pageIndexes, int max)
  {
    foreach (int[] pageIndex in pageIndexes)
    {
      if (pageIndex.Length != 0)
      {
        int num1 = ((IEnumerable<int>) pageIndex).Max();
        int num2 = ((IEnumerable<int>) pageIndex).Min();
        if (num1 > max || num2 < 0)
          return false;
      }
    }
    return true;
  }
}
