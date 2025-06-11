// Decompiled with JetBrains decompiler
// Type: PDFLauncher.CustomControl.PathTextBox
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace PDFLauncher.CustomControl;

public class PathTextBox : TextBox
{
  private string oldText = string.Empty;
  private Button browserButton;

  static PathTextBox()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (PathTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (PathTextBox)));
  }

  private Button BrowserButton
  {
    get => this.browserButton;
    set
    {
      if (this.browserButton == value)
        return;
      if (this.browserButton != null)
        this.browserButton.Click -= new RoutedEventHandler(this.BrowserButton_Click);
      this.browserButton = value;
      if (this.browserButton == null)
        return;
      this.browserButton.Click += new RoutedEventHandler(this.BrowserButton_Click);
    }
  }

  private void BrowserButton_Click(object sender, RoutedEventArgs e)
  {
    e.Handled = true;
    this.RequestDialog();
  }

  private bool RequestDialog()
  {
    string directory;
    string filename;
    if (!this.TryGetDirectoryAndFile(this.Text, out directory, out filename))
    {
      directory = "";
      filename = "";
    }
    switch (this.CreateDialog(directory, filename))
    {
      case null:
        return false;
      case FileDialog fileDialog:
        bool? nullable1 = new bool?();
        Window window1 = Window.GetWindow((DependencyObject) this);
        bool? nullable2 = !window1.IsVisible ? fileDialog.ShowDialog() : fileDialog.ShowDialog(window1);
        if (nullable2.GetValueOrDefault())
          this.Text = fileDialog.FileName;
        return nullable2.GetValueOrDefault();
      case CommonFileDialog commonFileDialog:
        Window window2 = Window.GetWindow((DependencyObject) this);
        CommonFileDialogResult fileDialogResult = !window2.IsVisible ? commonFileDialog.ShowDialog() : commonFileDialog.ShowDialog(window2);
        if (fileDialogResult == CommonFileDialogResult.Ok)
          this.Text = commonFileDialog.FileName;
        return fileDialogResult == CommonFileDialogResult.Ok;
      default:
        return false;
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.BrowserButton = this.GetTemplateChild("BrowserButton") as Button;
  }

  protected override void OnTextChanged(TextChangedEventArgs e)
  {
    base.OnTextChanged(e);
    if (this.IsFocused)
      return;
    this.TryRaisePathChanged();
  }

  protected override void OnLostFocus(RoutedEventArgs e)
  {
    base.OnLostFocus(e);
    if (Keyboard.FocusedElement == this.BrowserButton)
      return;
    this.TryRaisePathChanged();
  }

  private bool TryGetDirectoryAndFile(string text, out string directory, out string filename)
  {
    directory = "";
    filename = "";
    if (string.IsNullOrEmpty(text))
      return false;
    if ((int) text[text.Length - 1] == (int) Path.DirectorySeparatorChar)
    {
      try
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(text);
        directory = text;
        return true;
      }
      catch
      {
      }
    }
    else
    {
      try
      {
        FileInfo fileInfo = new FileInfo(text);
        directory = fileInfo.DirectoryName;
        filename = fileInfo.Name;
      }
      catch
      {
      }
    }
    return false;
  }

  protected virtual object CreateDialog(string initialDirectory, string filename)
  {
    throw new NotImplementedException();
  }

  private void TryRaisePathChanged()
  {
    string text = this.Text;
    string oldText = this.oldText;
    if (!(text != oldText))
      return;
    this.oldText = text;
    this.OnPathChanged(text, oldText);
  }

  protected void OnPathChanged(string newPath, string oldPath)
  {
    PathTextBoxPathChangedEventArgs e = new PathTextBoxPathChangedEventArgs(newPath, oldPath);
    EventHandler<PathTextBoxPathChangedEventArgs> pathChanged = this.PathChanged;
    if (pathChanged == null)
      return;
    pathChanged((object) this, e);
  }

  public event EventHandler<PathTextBoxPathChangedEventArgs> PathChanged;
}
