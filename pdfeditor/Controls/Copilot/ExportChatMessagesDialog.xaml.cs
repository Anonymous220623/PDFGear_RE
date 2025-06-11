// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Copilot.ExportChatMessagesDialog
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Win32;
using pdfeditor.Models.Copilot;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace pdfeditor.Controls.Copilot;

public partial class ExportChatMessagesDialog : Window, IComponentConnector
{
  private const string SummaryTitle = "PDFgear: ";
  private const string UserMessageTitle = "Me: ";
  private const string AssistantMessageTitle = "PDFgear: ";
  private Storyboard showToastAnimation;
  internal Grid LayoutRoot;
  internal RichTextBox ContentTextBox;
  internal Button CloseBtn;
  internal Button CopyBtn;
  internal Button DownloadBtn;
  internal Grid Toast;
  internal TranslateTransform ToastTrans;
  internal TextBlock ToastContent;
  private bool _contentLoaded;

  internal ExportChatMessagesDialog(IEnumerable<ChatMessageModel> chatMessages)
  {
    this.InitializeComponent();
    this.showToastAnimation = this.LayoutRoot.Resources[(object) "ShowToastAnimation"] as Storyboard;
    ExportChatMessagesDialog.BuildContentText(this.ContentTextBox.Document, chatMessages);
    this.Loaded += (RoutedEventHandler) ((s, a) =>
    {
      this.ContentTextBox.Document.PagePadding = new Thickness(8.0, 4.0, 8.0, 4.0);
      this.ContentTextBox.Focus();
      Keyboard.Focus((IInputElement) this.ContentTextBox);
      this.ContentTextBox.SelectAll();
    });
  }

  private static void BuildContentText(FlowDocument doc, IEnumerable<ChatMessageModel> chatMessages)
  {
    doc.Blocks.Clear();
    foreach (ChatMessageModel chatMessage in chatMessages)
    {
      if (chatMessage != null && chatMessage.NoError && !string.IsNullOrEmpty(chatMessage.Text))
      {
        string text = "";
        if (chatMessage.MessageType.Value == ChatMessageModel.ChatMessageType.Summary)
          text = "PDFgear: ";
        else if (chatMessage.IsAssistant)
          text = "PDFgear: ";
        else if (chatMessage.IsUser)
          text = "Me: ";
        Paragraph paragraph1 = new Paragraph();
        paragraph1.LineHeight = 16.0;
        paragraph1.FontSize = 12.0;
        paragraph1.TextIndent = -20.0;
        paragraph1.Margin = new Thickness(20.0, 0.0, 0.0, 10.0);
        InlineCollection inlines = paragraph1.Inlines;
        Run run = new Run(text);
        run.FontWeight = FontWeights.Bold;
        inlines.Add((Inline) run);
        paragraph1.Inlines.Add((Inline) new Run(chatMessage.Text));
        Paragraph paragraph2 = paragraph1;
        if (chatMessage.Pages != null && chatMessage.Pages.Length != 0)
        {
          paragraph2.Inlines.Add((Inline) new LineBreak());
          paragraph2.Inlines.Add(pdfeditor.Properties.Resources.CopilotViewboxPages);
          paragraph2.Inlines.Add(((IEnumerable<int>) chatMessage.Pages).Aggregate<int, StringBuilder>(new StringBuilder(), (Func<StringBuilder, int, StringBuilder>) ((sb, c) => sb.Append(c + 1).Append(' '))).ToString());
        }
        doc.Blocks.Add((Block) paragraph2);
      }
    }
  }

  private string GetContentText()
  {
    try
    {
      return new TextRange(this.ContentTextBox.Document.ContentStart, this.ContentTextBox.Document.ContentEnd).Text;
    }
    catch
    {
      return string.Empty;
    }
  }

  private async void CopyBtn_Click(object sender, RoutedEventArgs e)
  {
    string contentText = this.GetContentText();
    if (string.IsNullOrEmpty(contentText))
      return;
    ((UIElement) sender).IsEnabled = false;
    try
    {
      Clipboard.SetDataObject((object) contentText);
      this.showToastAnimation.SkipToFill();
      this.showToastAnimation.Begin();
      await Task.Delay(300);
    }
    catch
    {
      this.showToastAnimation.SkipToFill();
    }
    ((UIElement) sender).IsEnabled = true;
  }

  private async void DownloadBtn_Click(object sender, RoutedEventArgs e)
  {
    string contentText = this.GetContentText();
    if (string.IsNullOrEmpty(contentText))
      return;
    ((UIElement) sender).IsEnabled = false;
    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
    saveFileDialog1.AddExtension = true;
    saveFileDialog1.Filter = "txt|*.txt";
    saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    saveFileDialog1.FileName = "Export.txt";
    SaveFileDialog saveFileDialog2 = saveFileDialog1;
    if (saveFileDialog2.ShowDialog().GetValueOrDefault())
    {
      string fileName = saveFileDialog2.FileName;
      try
      {
        File.WriteAllText(fileName, contentText, Encoding.UTF8);
        Mouse.OverrideCursor = Cursors.AppStarting;
        int num = await ExplorerUtils.SelectItemInExplorerAsync(fileName, new CancellationToken()) ? 1 : 0;
        Mouse.OverrideCursor = (Cursor) null;
      }
      catch
      {
      }
    }
    ((UIElement) sender).IsEnabled = true;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/copilot/exportchatmessagesdialog.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.LayoutRoot = (Grid) target;
        break;
      case 2:
        this.ContentTextBox = (RichTextBox) target;
        break;
      case 3:
        this.CloseBtn = (Button) target;
        break;
      case 4:
        this.CopyBtn = (Button) target;
        this.CopyBtn.Click += new RoutedEventHandler(this.CopyBtn_Click);
        break;
      case 5:
        this.DownloadBtn = (Button) target;
        this.DownloadBtn.Click += new RoutedEventHandler(this.DownloadBtn_Click);
        break;
      case 6:
        this.Toast = (Grid) target;
        break;
      case 7:
        this.ToastTrans = (TranslateTransform) target;
        break;
      case 8:
        this.ToastContent = (TextBlock) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
