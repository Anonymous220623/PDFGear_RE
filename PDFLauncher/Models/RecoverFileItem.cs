// Decompiled with JetBrains decompiler
// Type: PDFLauncher.Models.RecoverFileItem
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;

#nullable disable
namespace PDFLauncher.Models;

public class RecoverFileItem : ObservableObject
{
  private string fileName;
  private string displayName;
  private string lastTime;
  private RecoverStatus status;
  private bool? isFileSelected = new bool?(true);
  private string sourceDir;
  private string recoverDir;
  private string recoverFullFileName;
  public string sourceFullFileName;
  public string fileGuid;
  private string editorSourceFullFileName;

  public string FileName
  {
    get => this.fileName;
    set => this.SetProperty<string>(ref this.fileName, value, nameof (FileName));
  }

  public string DisplayName
  {
    get => this.displayName;
    set => this.SetProperty<string>(ref this.displayName, value, nameof (DisplayName));
  }

  public string LastTime
  {
    get => this.lastTime;
    set => this.SetProperty<string>(ref this.lastTime, value, nameof (LastTime));
  }

  public RecoverStatus Status
  {
    get => this.status;
    set => this.SetProperty<RecoverStatus>(ref this.status, value, nameof (Status));
  }

  public bool? IsFileSelected
  {
    get => this.isFileSelected;
    set => this.SetProperty<bool?>(ref this.isFileSelected, value, nameof (IsFileSelected));
  }

  public string SourceDir
  {
    get => this.sourceDir;
    set => this.SetProperty<string>(ref this.sourceDir, value, nameof (SourceDir));
  }

  public string RecoverDir
  {
    get => this.recoverDir;
    set => this.SetProperty<string>(ref this.recoverDir, value, nameof (RecoverDir));
  }

  public string RecoverFullFileName
  {
    get => this.recoverFullFileName;
    set
    {
      this.SetProperty<string>(ref this.recoverFullFileName, value, nameof (RecoverFullFileName));
    }
  }

  public string SourceFullFileName
  {
    get => this.sourceFullFileName;
    set
    {
      this.SetProperty<string>(ref this.sourceFullFileName, value, nameof (SourceFullFileName));
    }
  }

  public string FileGuid
  {
    get => this.fileGuid;
    set => this.SetProperty<string>(ref this.fileGuid, value, nameof (FileGuid));
  }

  public string EditorSourceFullFileName
  {
    get => this.editorSourceFullFileName;
    set
    {
      this.SetProperty<string>(ref this.editorSourceFullFileName, value, nameof (EditorSourceFullFileName));
    }
  }
}
