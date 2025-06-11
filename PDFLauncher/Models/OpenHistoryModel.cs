// Decompiled with JetBrains decompiler
// Type: PDFLauncher.Models.OpenHistoryModel
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.IO;

#nullable disable
namespace PDFLauncher.Models;

public class OpenHistoryModel : ObservableObject
{
  private string fileName;
  private string filePath;
  private string fileSize;
  private string fileLastOpen;
  private bool isSelect;

  public string FileName
  {
    get => this.fileName;
    set => this.SetProperty<string>(ref this.fileName, value, nameof (FileName));
  }

  public string Extension
  {
    get
    {
      return !string.IsNullOrEmpty(this.filePath) ? new FileInfo(this.filePath).Extension.ToLower() : string.Empty;
    }
  }

  public string FilePath
  {
    get => this.filePath;
    set => this.SetProperty<string>(ref this.filePath, value, nameof (FilePath));
  }

  public string FileSize
  {
    get => this.fileSize;
    set => this.SetProperty<string>(ref this.fileSize, value, nameof (FileSize));
  }

  public string FileLastOpen
  {
    get => this.fileLastOpen;
    set => this.SetProperty<string>(ref this.fileLastOpen, value, nameof (FileLastOpen));
  }

  public bool IsSelect
  {
    get => this.isSelect;
    set => this.SetProperty<bool>(ref this.isSelect, value, nameof (IsSelect));
  }

  public OpenHistoryModel(string filePath) => this.FilePath = filePath;

  public OpenHistoryModel()
  {
  }
}
