// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.BookmarkRenameDialog
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.DependencyInjection;
using pdfeditor.Models.Bookmarks;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls;

public partial class BookmarkRenameDialog : Window, IComponentConnector
{
  private readonly BookmarkModel bookmarkModel;
  private readonly string originalTitle;
  private bool canBeEmpty;
  internal TextBlock RenameTips;
  internal TextBlock CreateNewTips;
  internal TextBox TitleTextBox;
  internal Button OkBtn;
  private bool _contentLoaded;

  private BookmarkRenameDialog(BookmarkModel bookmarkModel, string title)
  {
    this.InitializeComponent();
    this.bookmarkModel = bookmarkModel;
    this.originalTitle = bookmarkModel?.Title ?? title ?? string.Empty;
    this.TitleTextBox.Text = this.originalTitle;
    if (bookmarkModel == null)
    {
      this.CreateNewTips.Visibility = Visibility.Visible;
      this.RenameTips.Visibility = Visibility.Collapsed;
    }
    else
      this.canBeEmpty = string.IsNullOrEmpty(this.originalTitle);
    this.TitleTextBox.SelectAll();
    this.UpdateButtonState();
  }

  public string BookmarkTitle { get; private set; }

  private async void OKButton_Click(object sender, RoutedEventArgs e)
  {
    BookmarkRenameDialog bookmarkRenameDialog = this;
    bookmarkRenameDialog.IsEnabled = false;
    try
    {
      string title = bookmarkRenameDialog.TitleTextBox.Text;
      MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
      int num = await requiredService.OperationManager.UpdateBookmarkTitleAsync(requiredService.Document, bookmarkRenameDialog.bookmarkModel, title) ? 1 : 0;
      bookmarkRenameDialog.BookmarkTitle = title;
      bookmarkRenameDialog.DialogResult = new bool?(true);
      title = (string) null;
    }
    finally
    {
      bookmarkRenameDialog.IsEnabled = false;
    }
  }

  private void TitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
  {
    this.UpdateButtonState();
  }

  private void UpdateButtonState()
  {
    this.OkBtn.IsEnabled = this.canBeEmpty || !string.IsNullOrEmpty(this.TitleTextBox.Text);
  }

  public static BookmarkRenameDialog Create(BookmarkModel bookmarkModel)
  {
    BookmarkRenameDialog bookmarkRenameDialog = new BookmarkRenameDialog(bookmarkModel, (string) null);
    bookmarkRenameDialog.Owner = Application.Current.MainWindow;
    bookmarkRenameDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    return bookmarkRenameDialog;
  }

  public static BookmarkRenameDialog Create(string title)
  {
    BookmarkRenameDialog bookmarkRenameDialog = new BookmarkRenameDialog((BookmarkModel) null, title);
    bookmarkRenameDialog.Owner = Application.Current.MainWindow;
    bookmarkRenameDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    return bookmarkRenameDialog;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/bookmarkrenamedialog.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.RenameTips = (TextBlock) target;
        break;
      case 2:
        this.CreateNewTips = (TextBlock) target;
        break;
      case 3:
        this.TitleTextBox = (TextBox) target;
        this.TitleTextBox.TextChanged += new TextChangedEventHandler(this.TitleTextBox_TextChanged);
        break;
      case 4:
        this.OkBtn = (Button) target;
        this.OkBtn.Click += new RoutedEventHandler(this.OKButton_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
