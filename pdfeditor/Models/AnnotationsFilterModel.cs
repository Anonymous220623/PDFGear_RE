// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.AnnotationsFilterModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;

#nullable disable
namespace pdfeditor.Models;

public class AnnotationsFilterModel : ObservableObject
{
  private string text;
  private bool isSelect = true;
  private int count = 1;

  public string Text
  {
    get => this.text;
    set => this.text = value;
  }

  public bool IsSelect
  {
    get => this.isSelect;
    set => this.SetProperty<bool>(ref this.isSelect, value, nameof (IsSelect));
  }

  public int Count
  {
    get => this.count;
    set => this.SetProperty<int>(ref this.count, value, nameof (Count));
  }
}
