// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.PdfViewJumpManager
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Utils;

public class PdfViewJumpManager : ObservableObject
{
  private Stack<int> backStack = new Stack<int>();
  private Stack<int> preStack = new Stack<int>();

  public bool IsFirstView => this.backStack.Count == 0;

  public bool IsLastView => this.preStack.Count == 0;

  public int ViewStackOperation(bool isBack, int pageIndex)
  {
    if (isBack)
    {
      this.InsertRecord(ref this.preStack, pageIndex);
      return this.backStack.Pop();
    }
    this.InsertRecord(ref this.backStack, pageIndex);
    return this.preStack.Pop();
  }

  private void InsertRecord(ref Stack<int> stack, int pageIndex)
  {
    if (stack.Count <= 0 || pageIndex != stack.Peek())
    {
      if (stack.Count > 500)
        stack = new Stack<int>(stack.Skip<int>(stack.Count - 250).Take<int>(250));
      stack.Push(pageIndex);
    }
    this.OnPropertyChanged("IsFirstView");
    this.OnPropertyChanged("IsLastView");
  }

  public void NewRecord(int pageIndex) => this.InsertRecord(ref this.backStack, pageIndex);

  public int ViewBackCmd(int pageIndex)
  {
    int num = this.ViewStackOperation(true, pageIndex);
    this.OnPropertyChanged("IsFirstView");
    this.OnPropertyChanged("IsLastView");
    return num;
  }

  public int ViewPreCmd(int pageIndex)
  {
    int num = this.ViewStackOperation(false, pageIndex);
    this.OnPropertyChanged("IsFirstView");
    this.OnPropertyChanged("IsLastView");
    return num;
  }

  public void ClearStack()
  {
    this.backStack.Clear();
    this.preStack.Clear();
    this.OnPropertyChanged("IsFirstView");
    this.OnPropertyChanged("IsLastView");
  }

  public void StackChange()
  {
    this.preStack.Clear();
    this.OnPropertyChanged("IsFirstView");
    this.OnPropertyChanged("IsLastView");
  }
}
