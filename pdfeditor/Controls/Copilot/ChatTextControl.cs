// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Copilot.ChatTextControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Models.Copilot;
using pdfeditor.Utils;
using pdfeditor.Utils.Copilot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls.Copilot;

internal class ChatTextControl : ContentControl
{
  private RichTextBox rtb;
  public static readonly DependencyProperty CopilotHelperProperty = DependencyProperty.Register(nameof (CopilotHelper), typeof (CopilotHelper), typeof (ChatTextControl), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (ChatTextControl), new PropertyMetadata((object) "", (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ChatTextControl chatTextControl2) || object.Equals(a.NewValue, a.OldValue))
      return;
    chatTextControl2.UpdateDocument();
  })));
  public static readonly DependencyProperty MessageTypeProperty = DependencyProperty.Register(nameof (MessageType), typeof (ChatMessageModel.ChatMessageType), typeof (ChatTextControl), new PropertyMetadata((object) ChatMessageModel.ChatMessageType.Chat, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ChatTextControl chatTextControl4) || object.Equals(a.NewValue, a.OldValue))
      return;
    chatTextControl4.UpdateDocument();
  })));
  public static readonly DependencyProperty ErrorProperty = DependencyProperty.Register(nameof (Error), typeof (CopilotHelper.ChatResultError), typeof (ChatTextControl), new PropertyMetadata((object) CopilotHelper.ChatResultError.None, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ChatTextControl chatTextControl6) || object.Equals(a.NewValue, a.OldValue))
      return;
    chatTextControl6.UpdateDocument();
  })));
  public static readonly DependencyProperty LoadingProperty = DependencyProperty.Register(nameof (Loading), typeof (bool), typeof (ChatTextControl), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ChatTextControl chatTextControl8) || object.Equals(a.NewValue, a.OldValue))
      return;
    chatTextControl8.UpdateDocument();
  })));
  public static readonly DependencyProperty RoleProperty = DependencyProperty.Register(nameof (Role), typeof (string), typeof (ChatTextControl), new PropertyMetadata((object) "", (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ChatTextControl chatTextControl10) || object.Equals(a.NewValue, a.OldValue))
      return;
    chatTextControl10.UpdateDocument();
  })));
  public static readonly DependencyProperty PagesProperty = DependencyProperty.Register(nameof (Pages), typeof (IEnumerable<int>), typeof (ChatTextControl), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ChatTextControl chatTextControl12) || object.Equals(a.NewValue, a.OldValue))
      return;
    chatTextControl12.UpdateDocument();
  })));
  private const int MaxWelcomeContentButtonCount = 5;
  private static string[] WelcomeSampleActionNames = new string[6]
  {
    "summary",
    "convert-from-pdf",
    "compress",
    "protect-pdf",
    "slide-show",
    "page-zoom"
  };
  private static Random rnd = new Random();
  private static Brush WelcomeContentButtonBorderBrush = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 216, (byte) 216, (byte) 216));
  public static readonly RoutedEvent HyperlinkClickEvent = EventManager.RegisterRoutedEvent("HyperlinkClick", RoutingStrategy.Bubble, typeof (ChatTextHyperlinkClickEventHandler), typeof (ChatTextControl));

  public ChatTextControl()
  {
    RichTextBox richTextBox = new RichTextBox();
    richTextBox.IsReadOnly = true;
    richTextBox.IsDocumentEnabled = true;
    richTextBox.Padding = new Thickness();
    richTextBox.BorderThickness = new Thickness();
    richTextBox.Background = (Brush) null;
    this.rtb = richTextBox;
    this.rtb.SetResourceReference(Control.ForegroundProperty, (object) "TextBrushWhiteAndBlack");
    this.Content = (object) this.rtb;
  }

  public CopilotHelper CopilotHelper
  {
    get => (CopilotHelper) this.GetValue(ChatTextControl.CopilotHelperProperty);
    set => this.SetValue(ChatTextControl.CopilotHelperProperty, (object) value);
  }

  public string Text
  {
    get => (string) this.GetValue(ChatTextControl.TextProperty);
    set => this.SetValue(ChatTextControl.TextProperty, (object) value);
  }

  public ChatMessageModel.ChatMessageType MessageType
  {
    get => (ChatMessageModel.ChatMessageType) this.GetValue(ChatTextControl.MessageTypeProperty);
    set => this.SetValue(ChatTextControl.MessageTypeProperty, (object) value);
  }

  public CopilotHelper.ChatResultError Error
  {
    get => (CopilotHelper.ChatResultError) this.GetValue(ChatTextControl.ErrorProperty);
    set => this.SetValue(ChatTextControl.ErrorProperty, (object) value);
  }

  public bool Loading
  {
    get => (bool) this.GetValue(ChatTextControl.LoadingProperty);
    set => this.SetValue(ChatTextControl.LoadingProperty, (object) value);
  }

  public string Role
  {
    get => (string) this.GetValue(ChatTextControl.RoleProperty);
    set => this.SetValue(ChatTextControl.RoleProperty, (object) value);
  }

  public IEnumerable<int> Pages
  {
    get => (IEnumerable<int>) this.GetValue(ChatTextControl.PagesProperty);
    set => this.SetValue(ChatTextControl.PagesProperty, (object) value);
  }

  public string ErrorText
  {
    get
    {
      switch (this.Error)
      {
        case CopilotHelper.ChatResultError.ContentFiltered:
          return pdfeditor.Properties.Resources.ResourceManager.GetString("ChatPanelMessageError_ContentFiltered");
        case CopilotHelper.ChatResultError.UserCanceled:
          return pdfeditor.Properties.Resources.ResourceManager.GetString("ChatPanelMessageError_UserCanceled");
        default:
          return pdfeditor.Properties.Resources.ResourceManager.GetString("ChatPanelMessageErrorText");
      }
    }
  }

  private void UpdateDocument()
  {
    string text = this.Text;
    FlowDocument document = this.rtb.Document;
    document.Blocks.Clear();
    if (this.MessageType == ChatMessageModel.ChatMessageType.Welcome)
      this.ProcessWelcomeDocument(document);
    else if (this.Error != CopilotHelper.ChatResultError.None)
      this.ProcessErrorDocument(document);
    else if (string.IsNullOrEmpty(text))
    {
      if (!this.Loading)
        return;
      Paragraph paragraph1 = new Paragraph();
      paragraph1.LineHeight = 18.0;
      paragraph1.Inlines.Add((Inline) new LoadingEllipsis(false));
      Paragraph paragraph2 = paragraph1;
      document.Blocks.Add((Block) paragraph2);
    }
    else
    {
      this.ProcessTextDocument(document);
      if (this.MessageType == ChatMessageModel.ChatMessageType.Summary || this.Loading || this.Error != CopilotHelper.ChatResultError.None || this.Pages == null || !(this.Role == "assistant"))
        return;
      int[] array = this.Pages.Distinct<int>().ToArray<int>();
      if (array.Length == 0)
        return;
      Paragraph paragraph3 = new Paragraph();
      paragraph3.LineHeight = 18.0;
      Paragraph paragraph4 = paragraph3;
      paragraph4.Inlines.Add(pdfeditor.Properties.Resources.CopilotViewboxPages);
      foreach (int num in array)
      {
        int page = num;
        Hyperlink hyperlink1 = new Hyperlink((Inline) new Run($"{page + 1}"));
        hyperlink1.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 41, (byte) 143, (byte) 238));
        Hyperlink hyperlink2 = hyperlink1;
        hyperlink2.Click += (RoutedEventHandler) ((s, a) => this.OnHyperlinkClick(ChatTextHyperlinkClickAction.GoToPage, $"{page}"));
        paragraph4.Inlines.Add((Inline) hyperlink2);
        paragraph4.Inlines.Add((Inline) new Run("   "));
      }
      document.Blocks.Add((Block) paragraph4);
    }
  }

  private void ProcessWelcomeDocument(FlowDocument document)
  {
    Run run = new Run(pdfeditor.Properties.Resources.ChatPanelMessageWelcomeText1);
    BlockCollection blocks1 = document.Blocks;
    Paragraph paragraph1 = new Paragraph((Inline) run);
    paragraph1.LineHeight = 18.0;
    paragraph1.Margin = new Thickness(0.0, 0.0, 0.0, 4.0);
    blocks1.Add((Block) paragraph1);
    Paragraph paragraph2 = new Paragraph();
    paragraph2.LineHeight = 18.0;
    paragraph2.Margin = new Thickness(0.0, 0.0, 0.0, 8.0);
    paragraph2.Inlines.Add((Inline) new InlineUIContainer((UIElement) CreateWelcomeContentButton(pdfeditor.Properties.Resources.ChatPanelMessageWelcomeAction_summary, CreateAction("summary", Array.Empty<(string, string)>()))));
    paragraph2.Inlines.Add((Inline) new LineBreak());
    Paragraph paragraph3 = paragraph2;
    foreach (string _name in ((IEnumerable<string>) ChatTextControl.WelcomeSampleActionNames).Where<string>((Func<string, bool>) (c => c != "summary")).OrderBy<string, int>((Func<string, int>) (c => ChatTextControl.rnd.Next())).Take<string>(4).ToArray<string>())
    {
      string _content = pdfeditor.Properties.Resources.ResourceManager.GetString("ChatPanelMessageWelcomeAction_" + _name.Replace("-", "_"));
      switch (_name)
      {
        case "convert-from-pdf":
          paragraph3.Inlines.Add((Inline) new InlineUIContainer((UIElement) CreateWelcomeContentButton(_content, CreateAction(_name, new (string, string)[1]
          {
            ("mode", "word")
          }))));
          break;
        case "compress":
          paragraph3.Inlines.Add((Inline) new InlineUIContainer((UIElement) CreateWelcomeContentButton(_content, CreateAction(_name, new (string, string)[1]
          {
            ("mode", "high")
          }))));
          break;
        case "protect-pdf":
          paragraph3.Inlines.Add((Inline) new InlineUIContainer((UIElement) CreateWelcomeContentButton(_content, CreateAction(_name, Array.Empty<(string, string)>()))));
          break;
        case "slide-show":
          paragraph3.Inlines.Add((Inline) new InlineUIContainer((UIElement) CreateWelcomeContentButton(_content, CreateAction(_name, Array.Empty<(string, string)>()))));
          break;
        case "page-zoom":
          paragraph3.Inlines.Add((Inline) new InlineUIContainer((UIElement) CreateWelcomeContentButton(_content, CreateAction(_name, new (string, string)[1]
          {
            ("mode", "zoom-in")
          }))));
          break;
      }
      if (paragraph3.Inlines.OfType<InlineUIContainer>().Count<InlineUIContainer>() != 5)
        paragraph3.Inlines.Add((Inline) new LineBreak());
    }
    document.Blocks.Add((Block) paragraph3);
    BlockCollection blocks2 = document.Blocks;
    Paragraph paragraph4 = new Paragraph((Inline) new Run(pdfeditor.Properties.Resources.ChatPanelMessageWelcomeText2));
    paragraph4.LineHeight = 18.0;
    paragraph4.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
    blocks2.Add((Block) paragraph4);

    static CopilotHelper.AppActionModel CreateAction(
      string _name,
      (string key, string value)[] parameters)
    {
      return new CopilotHelper.AppActionModel()
      {
        Name = _name,
        Parameters = ((IEnumerable<(string, string)>) parameters).GroupBy<(string, string), string>((Func<(string, string), string>) (c => c.key)).ToDictionary<IGrouping<string, (string, string)>, string, string>((Func<IGrouping<string, (string, string)>, string>) (c => c.Key), (Func<IGrouping<string, (string, string)>, string>) (c => c.FirstOrDefault<(string, string)>().Item2))
      };
    }

    Button CreateWelcomeContentButton(string _content, CopilotHelper.AppActionModel _appAction)
    {
      Button button1 = new Button();
      button1.Tag = (object) new object[2]
      {
        (object) _content,
        (object) _appAction
      };
      Button button2 = button1;
      TextBlock textBlock = new TextBlock();
      textBlock.Text = _content;
      textBlock.TextWrapping = TextWrapping.Wrap;
      textBlock.FontSize = 12.0;
      textBlock.HorizontalAlignment = HorizontalAlignment.Left;
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      textBlock.TextAlignment = TextAlignment.Center;
      button2.Content = (object) textBlock;
      button1.Margin = new Thickness(-4.0, 4.0, 0.0, 4.0);
      button1.Padding = new Thickness(8.0, 6.0, 8.0, 3.0);
      button1.Background = (Brush) Brushes.White;
      button1.BorderBrush = ChatTextControl.WelcomeContentButtonBorderBrush;
      button1.BorderThickness = new Thickness(1.0);
      button1.MinWidth = 0.0;
      button1.MinHeight = 0.0;
      button1.Style = (Style) App.Current.Resources[(object) "DialogButtonStyle"];
      Button welcomeContentButton = button1;
      welcomeContentButton.SetResourceReference(Control.BackgroundProperty, (object) "SignaturePickerBackground");
      UIElementExtension.SetCornerRadius((DependencyObject) welcomeContentButton, new CornerRadius(4.0));
      // ISSUE: method pointer
      welcomeContentButton.Click += new RoutedEventHandler((object) this, __methodptr(\u003CProcessWelcomeDocument\u003Eg___button_Click\u007C37_4));
      return welcomeContentButton;
    }
  }

  private void ProcessErrorDocument(FlowDocument document)
  {
    if (this.Error == CopilotHelper.ChatResultError.UserCanceled && !string.IsNullOrEmpty(this.Text))
      this.ProcessTextDocument(document);
    Paragraph paragraph1 = new Paragraph();
    paragraph1.LineHeight = 18.0;
    Paragraph paragraph2 = paragraph1;
    paragraph2.Inlines.Add(this.ErrorText);
    if (this.MessageType != ChatMessageModel.ChatMessageType.Summary && this.Error != CopilotHelper.ChatResultError.ContentFiltered && this.Error != CopilotHelper.ChatResultError.CountLimit && this.Error != CopilotHelper.ChatResultError.AccountBaned)
    {
      Hyperlink hyperlink1 = new Hyperlink();
      hyperlink1.Inlines.Add((Inline) new Run(pdfeditor.Properties.Resources.ChatPanelMessageErrorRetryText));
      hyperlink1.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 41, (byte) 143, (byte) 238));
      Hyperlink hyperlink2 = hyperlink1;
      hyperlink2.Click += (RoutedEventHandler) ((s, a) => this.OnHyperlinkClick(ChatTextHyperlinkClickAction.ErrorTryAgain, "Try again"));
      paragraph2.Inlines.Add((Inline) new Run(" "));
      paragraph2.Inlines.Add((Inline) hyperlink2);
    }
    if (this.Error != CopilotHelper.ChatResultError.UserCanceled)
    {
      Hyperlink hyperlink3 = new Hyperlink();
      hyperlink3.Inlines.Add((Inline) new Run(pdfeditor.Properties.Resources.MenuHelpContactUsContent));
      hyperlink3.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 41, (byte) 143, (byte) 238));
      Hyperlink hyperlink4 = hyperlink3;
      hyperlink4.Click += (RoutedEventHandler) ((s, a) => this.CopilotHelper?.ShowFeedbackWindow(false));
      paragraph2.Inlines.Add((Inline) new Run(" "));
      paragraph2.Inlines.Add((Inline) hyperlink4);
    }
    document.Blocks.Add((Block) paragraph2);
  }

  private void ProcessTextDocument(FlowDocument document)
  {
    string text = this.Text;
    if (string.IsNullOrEmpty(text))
      return;
    foreach (string str in ((IEnumerable<string>) text.Replace("\r", "").Split('\n')).Where<string>((Func<string, bool>) (c => !string.IsNullOrEmpty(c) && c != "\n")))
    {
      Paragraph paragraph1 = new Paragraph();
      paragraph1.LineHeight = 18.0;
      Paragraph paragraph2 = paragraph1;
      if (this.MessageType == ChatMessageModel.ChatMessageType.Summary && (str.StartsWith("1.") || str.StartsWith("2.") || str.StartsWith("3.")))
      {
        Geometry geometry = (Geometry) new GeometryConverter().ConvertFromString("M-0.0271606 -0.0274658L35.9728 17.7589V19.3509L-0.0271606 36.9725V-0.0274658ZM3.55655 20.3116V31.2084L26.6063 20.3116H3.55655ZM3.55655 16.6884H26.1176L3.55655 5.81897V16.6884Z");
        new Path().Data = geometry;
        InlineUIContainer inlineUiContainer1 = new InlineUIContainer();
        Border border = new Border();
        Viewbox viewbox = new Viewbox();
        viewbox.Stretch = Stretch.Uniform;
        Path path = new Path();
        path.Data = geometry;
        path.Fill = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 41, (byte) 143, (byte) 238));
        viewbox.Child = (UIElement) path;
        viewbox.Width = 12.0;
        viewbox.Height = 12.0;
        viewbox.Margin = new Thickness(0.0, 0.0, 4.0, -2.0);
        border.Child = (UIElement) viewbox;
        inlineUiContainer1.Child = (UIElement) border;
        InlineUIContainer inlineUiContainer2 = inlineUiContainer1;
        string inlineText = str.Substring(2);
        Hyperlink hyperlink1 = new Hyperlink();
        hyperlink1.Inlines.Add((Inline) inlineUiContainer2);
        hyperlink1.Inlines.Add((Inline) new Run()
        {
          Text = inlineText
        });
        hyperlink1.TextDecorations = (TextDecorationCollection) null;
        hyperlink1.Foreground = (Brush) Brushes.Black;
        Hyperlink hyperlink2 = hyperlink1;
        hyperlink2.SetResourceReference(TextElement.ForegroundProperty, (object) "TextBrushWhiteAndBlack");
        hyperlink2.Click += (RoutedEventHandler) ((s, a) => this.OnHyperlinkClick(ChatTextHyperlinkClickAction.SummaryQuestion, inlineText));
        paragraph2.Inlines.Add((Inline) hyperlink2);
      }
      else
        paragraph2.Inlines.Add((Inline) new Run()
        {
          Text = str.Trim()
        });
      document.Blocks.Add((Block) paragraph2);
    }
    if (!this.Loading || !(document.Blocks.LastOrDefault<Block>() is Paragraph paragraph))
      return;
    paragraph.Inlines.Add((Inline) new LoadingEllipsis(true));
  }

  private void OnHyperlinkClick(ChatTextHyperlinkClickAction action, string text)
  {
    this.RaiseEvent((RoutedEventArgs) new ChatTextHyperlinkClickEventArgs(ChatTextControl.HyperlinkClickEvent, (object) this, action, text, (CopilotHelper.AppActionModel) null));
  }

  private void OnSuggestionAppActionClick(string text, CopilotHelper.AppActionModel action)
  {
    this.RaiseEvent((RoutedEventArgs) new ChatTextHyperlinkClickEventArgs(ChatTextControl.HyperlinkClickEvent, (object) this, ChatTextHyperlinkClickAction.SuggestionAppAction, text, action));
  }

  public event ChatTextHyperlinkClickEventHandler HyperlinkClick
  {
    add => this.AddHandler(ChatTextControl.HyperlinkClickEvent, (Delegate) value);
    remove => this.RemoveHandler(ChatTextControl.HyperlinkClickEvent, (Delegate) value);
  }
}
