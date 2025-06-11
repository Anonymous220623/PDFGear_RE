// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Copilot.ChatPanel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using CommomLib.Controls;
using CommomLib.Views;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf.Net;
using pdfeditor.Models.Copilot;
using pdfeditor.Utils;
using pdfeditor.Utils.Copilot;
using pdfeditor.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.Copilot;

public partial class ChatPanel : UserControl, IComponentConnector, IStyleConnector
{
  private DateTime lastScroll;
  private ObservableCollection<ChatMessageModel> chatMessages;
  private CancellationTokenSource cts;
  public static readonly DependencyProperty CopilotHelperProperty = DependencyProperty.Register(nameof (CopilotHelper), typeof (CopilotHelper), typeof (ChatPanel), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ChatPanel chatPanel2) || a.NewValue == a.OldValue)
      return;
    chatPanel2.UpdateCopilotHelper();
  })));
  private string text = "";
  internal Grid TitleBar;
  internal Button FeedbackButton;
  internal Grid WelcomePage;
  internal Button ChatNowButton;
  internal CommomLib.Controls.ProgressBar AnalysisProgressBar;
  internal Grid ChatPage;
  internal ScrollViewer ScrollViewer;
  internal ItemsControl ChatItemsControl;
  internal Grid InputPanel;
  internal TextBlock ChatTips;
  internal HyperlinkButton ClearHistoryButton;
  internal HyperlinkButton ExportButton;
  internal HyperlinkButton StopButton;
  internal TextBox UserInputTextBox;
  internal ChatBubble ChatEmptytip;
  internal Button SendButton;
  internal Grid Overchance;
  private bool _contentLoaded;

  public ChatPanel()
  {
    this.InitializeComponent();
    this.chatMessages = new ObservableCollection<ChatMessageModel>();
    this.ChatItemsControl.ItemsSource = (IEnumerable) this.chatMessages;
  }

  private void CloseButton_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("ChatPdf", "Close", "Count", 1L);
    EventHandler closeButtonClick = this.CloseButtonClick;
    if (closeButtonClick == null)
      return;
    closeButtonClick((object) this, (EventArgs) e);
  }

  public event EventHandler CloseButtonClick;

  public CopilotHelper CopilotHelper
  {
    get => (CopilotHelper) this.GetValue(ChatPanel.CopilotHelperProperty);
    set => this.SetValue(ChatPanel.CopilotHelperProperty, (object) value);
  }

  public void FocusUserInputTextBox()
  {
    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() =>
    {
      if (!this.IsVisible || this.ChatPage.Visibility != Visibility.Visible)
        return;
      this.ScrollToBottom(true);
      this.UserInputTextBox.Focus();
      Keyboard.Focus((IInputElement) this.UserInputTextBox);
    }));
  }

  private void UpdateCopilotHelper() => this.Reset();

  private void Reset()
  {
    this.ChatNowButton.Content = (object) pdfeditor.Properties.Resources.ResourceManager.GetString("CopilotWelcomePageChatNowButtonText");
    this.AnalysisProgressBar.Visibility = Visibility.Collapsed;
    this.AnalysisProgressBar.Value = 0.0;
    this.AnalysisProgressBar.IsIndeterminate = false;
    this.WelcomePage.Visibility = Visibility.Visible;
    this.ChatPage.Visibility = Visibility.Collapsed;
    this.StopButton.Visibility = Visibility.Collapsed;
    this.chatMessages.Clear();
  }

  public bool Chatting => this.ChatPage.Visibility == Visibility.Visible;

  private async void NavigatedFromWelcome(object sender, RoutedEventArgs e)
  {
    ChatPanel chatPanel = this;
    CommomLib.Commom.GAManager.SendEvent("ChatPdf", "ChatNowButton", "Count", 1L);
    try
    {
      chatPanel.SendButton.IsEnabled = false;
      ((UIElement) sender).IsEnabled = false;
      CopilotHelper helper = chatPanel.CopilotHelper;
      if (helper != null)
      {
        // ISSUE: reference to a compiler-generated method
        Progress<double> progressReporter = new Progress<double>(new Action<double>(chatPanel.\u003CNavigatedFromWelcome\u003Eb__17_0));
        chatPanel.ChatNowButton.Content = (object) pdfeditor.Properties.Resources.ResourceManager.GetString("CopilotWelcomePageAnalysingText");
        chatPanel.AnalysisProgressBar.Visibility = Visibility.Visible;
        await helper.InitializeAsync((IProgress<double>) progressReporter);
        chatPanel.AnalysisProgressBar.IsIndeterminate = true;
        ChatMessageModel assistantModel = ChatMessageModel.CreateAssistantModel(ChatMessageModel.ChatMessageType.Welcome);
        chatPanel.chatMessages.Add(assistantModel);
        int num = await chatPanel.LoadMessageListFromCacheAsync() ? 1 : 0;
        if (helper == chatPanel.CopilotHelper)
        {
          chatPanel.WelcomePage.Visibility = Visibility.Collapsed;
          chatPanel.ChatPage.Visibility = Visibility.Visible;
          chatPanel.AnalysisProgressBar.IsIndeterminate = false;
          chatPanel.FocusUserInputTextBox();
        }
      }
      helper = (CopilotHelper) null;
    }
    finally
    {
      ((UIElement) sender).IsEnabled = true;
      await chatPanel.UpdateCanChatState();
    }
  }

  private async Task<bool> LoadMessageListFromCacheAsync()
  {
    CopilotHelper copilotHelper = this.CopilotHelper;
    if (copilotHelper != null)
    {
      List<CopilotHelper.ChatMessage> messageListAsync = await copilotHelper.GetCachedMessageListAsync();
      if (messageListAsync != null && messageListAsync.Count > 0)
      {
        foreach (CopilotHelper.ChatMessage chatMessage in messageListAsync)
        {
          ChatMessageModel chatMessageModel = ChatMessageModel.Create(chatMessage.Role, ChatMessageModel.ChatMessageType.Chat);
          chatMessageModel.Text = chatMessage.Content;
          chatMessageModel.Pages = chatMessage.Pages;
          chatMessageModel.Like = !(chatMessage.Liked == "Like") ? (!(chatMessage.Liked == "Dislike") ? ChatMessageModel.Liked.None : ChatMessageModel.Liked.Dislike) : ChatMessageModel.Liked.Like;
          this.chatMessages.Add(chatMessageModel);
        }
        return true;
      }
    }
    return false;
  }

  private async void SendButton_Click(object sender, RoutedEventArgs e)
  {
    await this.SendAsync(true);
  }

  public async Task RequestSummaryAsync()
  {
    ChatPanel chatPanel = this;
    CopilotHelper helper;
    if (!chatPanel.SendButton.IsEnabled)
    {
      helper = (CopilotHelper) null;
    }
    else
    {
      helper = chatPanel.CopilotHelper;
      if (helper == null)
      {
        helper = (CopilotHelper) null;
      }
      else
      {
        chatPanel.SendButton.IsEnabled = false;
        try
        {
          ChatMessageModel summaryChatModel = (ChatMessageModel) null;
          string cachedSummaryAsync = await helper.GetCachedSummaryAsync();
          if (!string.IsNullOrEmpty(cachedSummaryAsync))
          {
            summaryChatModel = ChatMessageModel.CreateAssistantModel(ChatMessageModel.ChatMessageType.Summary);
            summaryChatModel.Text = cachedSummaryAsync;
            chatPanel.chatMessages.Add(summaryChatModel);
            await chatPanel.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() =>
            {
              if (this.ChatItemsControl.ItemContainerGenerator.ContainerFromItem((object) summaryChatModel) is FrameworkElement frameworkElement2)
                frameworkElement2.BringIntoView();
              else
                this.ScrollToBottom(true);
            }));
          }
          else
          {
            await chatPanel.UpdateCanChatState();
            chatPanel.SendButton.IsEnabled = false;
            StringBuilder sb = new StringBuilder();
            summaryChatModel = ChatMessageModel.CreateAssistantModel(ChatMessageModel.ChatMessageType.Summary);
            summaryChatModel.Loading = true;
            chatPanel.chatMessages.Add(summaryChatModel);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            chatPanel.cts?.Cancel();
            chatPanel.cts = cancellationTokenSource;
            CopilotHelper.CopilotResult summaryAsync = await helper.GetSummaryAsync((Func<string, CancellationToken, Task>) ((text, ct) =>
            {
              lock (sb)
              {
                this.WelcomePage.Visibility = Visibility.Collapsed;
                this.ChatPage.Visibility = Visibility.Visible;
                this.AnalysisProgressBar.IsIndeterminate = false;
                sb.Append(text);
                summaryChatModel.Text = sb.ToString();
                this.ScrollToBottom(false);
              }
              return Task.CompletedTask;
            }), cancellationTokenSource.Token);
            if (summaryChatModel != null)
            {
              summaryChatModel.Loading = false;
              summaryChatModel.Error = summaryAsync.Error;
              if (summaryChatModel.HasError)
                CommomLib.Commom.GAManager.SendEvent("ChatPdf", "Summary_Failed", "Count", 1L);
            }
          }
        }
        finally
        {
          await chatPanel.UpdateCanChatState();
          if (helper == chatPanel.CopilotHelper)
          {
            chatPanel.SendButton.IsEnabled = true;
            chatPanel.StopButton.Visibility = Visibility.Collapsed;
          }
        }
        helper = (CopilotHelper) null;
      }
    }
  }

  private async Task SendAsync(bool useAppAction)
  {
    string text;
    ChatMessageModel userModel;
    CancellationTokenSource cts2;
    if (!this.SendButton.IsEnabled)
    {
      text = (string) null;
      userModel = (ChatMessageModel) null;
      cts2 = (CancellationTokenSource) null;
    }
    else if (string.IsNullOrEmpty(this.UserInputTextBox.Text.TrimStart().TrimEnd()))
    {
      this.ChatEmptytip.ShowBubble(new TimeSpan?(TimeSpan.FromSeconds(2.0)));
      this.UserInputTextBox.Text = "";
      this.UserInputTextBox.Focus();
      text = (string) null;
      userModel = (ChatMessageModel) null;
      cts2 = (CancellationTokenSource) null;
    }
    else
    {
      CommomLib.Commom.GAManager.SendEvent("ChatPdf", "SendButton", "Count", 1L);
      text = this.UserInputTextBox.Text;
      this.UserInputTextBox.Text = "";
      userModel = ChatMessageModel.CreateUserModel();
      userModel.Text = text;
      this.chatMessages.Add(userModel);
      this.ScrollToBottom(true);
      cts2 = new CancellationTokenSource();
      this.cts?.Cancel();
      this.cts = cts2;
      CopilotHelper.CopilotResult copilotResult1 = (CopilotHelper.CopilotResult) null;
      if (useAppAction)
        copilotResult1 = await this.DirectGetAppActionAsync(text, userModel, cts2.Token);
      if (copilotResult1 == null || copilotResult1.AppAction == null && copilotResult1.MaybeNotAppAction)
      {
        CommomLib.Commom.GAManager.SendEvent("ChatPdf", "Chat_DirectChat", "Count", 1L);
        CopilotHelper.CopilotResult copilotResult2 = await this.DirectChatAsync(text, userModel, cts2.Token);
        text = (string) null;
        userModel = (ChatMessageModel) null;
        cts2 = (CancellationTokenSource) null;
      }
      else
      {
        CommomLib.Commom.GAManager.SendEvent("ChatPdf", "Chat_AppAction", copilotResult1.AppAction.Name ?? "", 1L);
        text = (string) null;
        userModel = (ChatMessageModel) null;
        cts2 = (CancellationTokenSource) null;
      }
    }
  }

  private async Task<CopilotHelper.CopilotResult> DirectGetAppActionAsync(
    string message,
    ChatMessageModel askModel,
    CancellationToken cancellationToken)
  {
    if (!this.SendButton.IsEnabled)
      return CopilotHelper.CopilotResult.EmptyUnknownFailed;
    CopilotHelper helper = this.CopilotHelper;
    if (helper == null || string.IsNullOrEmpty(message?.Trim()))
      return CopilotHelper.CopilotResult.EmptyUnknownFailed;
    object obj = (object) null;
    int num = 0;
    CopilotHelper.CopilotResult appActionAsync;
    try
    {
      await this.UpdateCanChatState();
      this.SendButton.IsEnabled = false;
      this.StopButton.Visibility = Visibility.Visible;
      ChatMessageModel model = ChatMessageModel.CreateAssistantModel(ChatMessageModel.ChatMessageType.Chat);
      model.Loading = true;
      model.AskModel = askModel;
      this.chatMessages.Add(model);
      this.ScrollToBottom(true);
      CopilotHelper.CopilotResult appActionResult = await helper.GetAppActionAsync(message, cancellationToken);
      if (appActionResult != null && appActionResult.Error == CopilotHelper.ChatResultError.None && appActionResult.AppAction != null)
      {
        CommomLib.Commom.GAManager.SendEvent("ChatPdf", "AppAction_DirectGetAppActionAsync", appActionResult.AppAction.Name ?? "", 1L);
        if (appActionResult.Text == null)
        {
          this.chatMessages.Remove(model);
          if (helper != null && this.CopilotHelper == helper)
          {
            int num1 = await helper.ProcessNativeAppAction(appActionResult.AppAction) ? 1 : 0;
          }
        }
        else
        {
          model.MessageType.Value = ChatMessageModel.ChatMessageType.AppAction;
          model.Loading = false;
          model.AskModel = askModel;
          model.Text = appActionResult.Text;
          model.AppAction = appActionResult.AppAction;
          model.MaybeNotAppAction = appActionResult.MaybeNotAppAction;
        }
        this.ScrollToBottom(true);
      }
      else
        this.chatMessages.Remove(model);
      appActionAsync = appActionResult ?? CopilotHelper.CopilotResult.EmptyUnknownFailed;
      num = 1;
    }
    catch (object ex)
    {
      obj = ex;
    }
    await this.UpdateCanChatState();
    if (helper == this.CopilotHelper)
    {
      this.SendButton.IsEnabled = true;
      this.StopButton.Visibility = Visibility.Collapsed;
    }
    object obj1 = obj;
    if (obj1 != null)
    {
      if (!(obj1 is Exception source))
        throw obj1;
      ExceptionDispatchInfo.Capture(source).Throw();
    }
    if (num == 1)
      return appActionAsync;
    obj = (object) null;
    appActionAsync = (CopilotHelper.CopilotResult) null;
    helper = (CopilotHelper) null;
    CopilotHelper.CopilotResult appActionAsync1;
    return appActionAsync1;
  }

  private async Task<CopilotHelper.CopilotResult> DirectChatAsync(
    string message,
    ChatMessageModel askModel,
    CancellationToken cancellationToken)
  {
    if (!this.SendButton.IsEnabled)
      return CopilotHelper.CopilotResult.EmptyUnknownFailed;
    CopilotHelper helper = this.CopilotHelper;
    if (helper == null || string.IsNullOrEmpty(message?.Trim()))
      return CopilotHelper.CopilotResult.EmptyUnknownFailed;
    object obj = (object) null;
    int num = 0;
    CopilotHelper.CopilotResult copilotResult;
    try
    {
      await this.UpdateCanChatState();
      this.SendButton.IsEnabled = false;
      this.StopButton.Visibility = Visibility.Visible;
      ChatMessageModel model = ChatMessageModel.CreateAssistantModel(ChatMessageModel.ChatMessageType.Chat);
      model.Loading = true;
      model.AskModel = askModel;
      this.chatMessages.Add(model);
      this.ScrollToBottom(true);
      StringBuilder sb = new StringBuilder();
      CopilotHelper.CopilotResult copilotResult1 = await helper.ChatAsync(message, (Func<string, CancellationToken, Task>) ((result, ct) =>
      {
        lock (sb)
        {
          sb.Append(result);
          model.Text = sb.ToString();
          this.ScrollToBottom(false, false);
        }
        return Task.CompletedTask;
      }), cancellationToken);
      if (copilotResult1 != null)
      {
        model.Loading = false;
        model.Error = copilotResult1.Error;
        model.Pages = copilotResult1.Pages;
        if (model.HasError)
          CommomLib.Commom.GAManager.SendEvent("ChatPdf", "Chat_Failed", "Count", 1L);
      }
      this.ScrollToBottom(true);
      copilotResult = copilotResult1 ?? CopilotHelper.CopilotResult.EmptyUnknownFailed;
      num = 1;
    }
    catch (object ex)
    {
      obj = ex;
    }
    await this.UpdateCanChatState();
    if (helper == this.CopilotHelper)
    {
      this.SendButton.IsEnabled = true;
      this.StopButton.Visibility = Visibility.Collapsed;
    }
    object obj1 = obj;
    if (obj1 != null)
    {
      if (!(obj1 is Exception source))
        throw obj1;
      ExceptionDispatchInfo.Capture(source).Throw();
    }
    if (num == 1)
      return copilotResult;
    obj = (object) null;
    copilotResult = (CopilotHelper.CopilotResult) null;
    helper = (CopilotHelper) null;
    CopilotHelper.CopilotResult copilotResult2;
    return copilotResult2;
  }

  private async void ChatItemsControl_HyperlinkClick(
    object sender,
    ChatTextHyperlinkClickEventArgs e)
  {
    if (e.Action == ChatTextHyperlinkClickAction.SuggestionAppAction)
    {
      if (string.IsNullOrEmpty(e.Text) || e.AppAction == null)
        return;
      CommomLib.Commom.GAManager.SendEvent("ChatPdf", "SuggestionAppActionBtnClick", e.AppAction.Name ?? "", 1L);
      if (e.AppAction.Confirm == null)
      {
        ChatMessageModel userModel = ChatMessageModel.CreateUserModel();
        userModel.Text = e.Text;
        this.chatMessages.Add(userModel);
        CopilotHelper copilotHelper = this.CopilotHelper;
        if (copilotHelper == null)
          return;
        int num = await copilotHelper.ProcessNativeAppAction(e.AppAction) ? 1 : 0;
      }
      else
      {
        ChatMessageModel userModel = ChatMessageModel.CreateUserModel();
        userModel.Text = e.Text;
        ChatMessageModel assistantModel = ChatMessageModel.CreateAssistantModel(ChatMessageModel.ChatMessageType.AppAction);
        assistantModel.Text = e.AppAction.Confirm;
        assistantModel.AppAction = e.AppAction;
        this.chatMessages.Add(userModel);
        this.chatMessages.Add(assistantModel);
        this.ScrollToBottom(true);
      }
    }
    else if (e.Action == ChatTextHyperlinkClickAction.SummaryQuestion)
    {
      CommomLib.Commom.GAManager.SendEvent("ChatPdf", "HyperlinkClick", "SummaryQuestion", 1L);
      this.SendUserInputMessage(e.Text, false);
    }
    else if (e.Action == ChatTextHyperlinkClickAction.ErrorTryAgain)
    {
      CommomLib.Commom.GAManager.SendEvent("ChatPdf", "HyperlinkClick", "ErrorTryAgain", 1L);
      if (!(e.OriginalSource is ChatTextControl originalSource) || !(originalSource.DataContext is ChatMessageModel dataContext))
        return;
      this.SendUserInputMessage(dataContext.AskModel.Text, true);
    }
    else
    {
      if (e.Action != ChatTextHyperlinkClickAction.GoToPage)
        return;
      CommomLib.Commom.GAManager.SendEvent("ChatPdf", "HyperlinkClick", "GoToPage", 1L);
      int result;
      if (!int.TryParse(e.Text, out result))
        return;
      PdfDocument document = this.CopilotHelper?.Document;
      if (result < 0 || result >= document.Pages.Count)
        return;
      PDFKit.PdfControl.GetPdfControl(document).ScrollToPage(result);
    }
  }

  private void SendUserInputMessage(string text, bool useAppAction)
  {
    if (string.IsNullOrEmpty(text) || !this.SendButton.IsEnabled)
      return;
    this.UserInputTextBox.Text = text;
    this.SendAsync(useAppAction);
  }

  private void ScrollToBottom(bool force, bool animated = true)
  {
    DateTime now = DateTime.Now;
    if (!force && (now - this.lastScroll).TotalSeconds <= 1.0)
      return;
    this.lastScroll = now;
    if (animated)
      this.ScrollToBottomAnimated();
    else
      this.ScrollViewer.ScrollToEnd();
  }

  private void ScrollToBottomAnimated()
  {
    this.ScrollViewer.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() =>
    {
      if (!this.IsLoaded)
        return;
      double scrollableHeight = this.ScrollViewer.ScrollableHeight;
      if (scrollableHeight <= 0.0)
        return;
      this.ScrollViewer.SmoothScrollToVerticalOffset(scrollableHeight);
    }));
  }

  private async void UserInputTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    bool flag1 = (Keyboard.Modifiers & ModifierKeys.Control) != 0;
    bool flag2 = (Keyboard.Modifiers & ModifierKeys.Shift) != 0;
    if (e.Key != Key.Return || !(flag1 | flag2))
      return;
    e.Handled = true;
    await this.SendAsync(true);
  }

  private async Task UpdateCanChatState()
  {
    CopilotHelper copilotHelper = this.CopilotHelper;
    if (copilotHelper == null)
      return;
    int chatRemaining = await copilotHelper.GetChatRemaining();
    if (chatRemaining > 0)
    {
      this.SendButton.IsEnabled = true;
      this.UserInputTextBox.IsEnabled = true;
      this.Overchance.Visibility = Visibility.Collapsed;
      this.InputPanel.Visibility = Visibility.Visible;
    }
    else
    {
      this.InputPanel.Visibility = Visibility.Collapsed;
      this.Overchance.Visibility = Visibility.Visible;
    }
    this.ChatTips.Text = pdfeditor.Properties.Resources.ResourceManager.GetString("CopilotMessagesRemainingTips").Replace("XXX", $"{chatRemaining}/{50}");
  }

  private async void ClearHistoryButton_Click(object sender, RoutedEventArgs e)
  {
    await this.ClearHistoryAsync();
  }

  private void FeedbackButton_Click(object sender, RoutedEventArgs e)
  {
    string documentPath = Ioc.Default.GetRequiredService<MainViewModel>().DocumentWrapper.DocumentPath;
    FeedbackWindow feedbackWindow = new FeedbackWindow();
    feedbackWindow.Owner = App.Current.MainWindow;
    feedbackWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    feedbackWindow.source = "ChatPdf";
    feedbackWindow.HideFaq();
    if (!string.IsNullOrEmpty(documentPath))
    {
      feedbackWindow.flist.Add(documentPath);
      feedbackWindow.showAttachmentCB(true);
    }
    feedbackWindow.ShowDialog();
  }

  private async void ExportButton_Click(object sender, RoutedEventArgs e)
  {
    await this.ExportHistory();
  }

  private void DisLike_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("ChatPdf", "DisLikeBtn", "Count", 1L);
    ChatMessageModel dataContext = ((FrameworkElement) sender).DataContext as ChatMessageModel;
    dataContext.Like = ChatMessageModel.Liked.Dislike;
    this.SetLikedState(dataContext);
    this.CopilotHelper.ShowFeedbackWindow(true);
  }

  private void Like_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("ChatPdf", "LikeBtn", "Count", 1L);
    ChatMessageModel dataContext = ((FrameworkElement) sender).DataContext as ChatMessageModel;
    dataContext.Like = ChatMessageModel.Liked.Like;
    this.SetLikedState(dataContext);
  }

  private void SetLikedState(ChatMessageModel data)
  {
    this.CopilotHelper.LikedAsyne(new CopilotHelper.ChatMessage()
    {
      Role = data.Role,
      Content = data.Text,
      Pages = data.Pages,
      Liked = data.Like.ToString()
    });
  }

  private async Task ClearHistoryAsync()
  {
    if (!this.SendButton.IsEnabled || CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.CopilotClearHistoryTip, UtilManager.GetProductName(), MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
      return;
    CommomLib.Commom.GAManager.SendEvent("ChatPdf", "ClearHistory", "Count", 1L);
    CopilotHelper copilotHelper = this.CopilotHelper;
    if (copilotHelper != null)
    {
      this.chatMessages.Clear();
      await copilotHelper.ClearMessageListAsync();
    }
    this.chatMessages.Add(ChatMessageModel.CreateAssistantModel(ChatMessageModel.ChatMessageType.Welcome));
  }

  private Task ExportHistory()
  {
    ExportChatMessagesDialog chatMessagesDialog = new ExportChatMessagesDialog((IEnumerable<ChatMessageModel>) this.chatMessages);
    chatMessagesDialog.Owner = App.Current.MainWindow;
    chatMessagesDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    chatMessagesDialog.ShowDialog();
    return Task.CompletedTask;
  }

  private void StopButton_Click(object sender, RoutedEventArgs e)
  {
    this.cts?.Cancel();
    this.cts = (CancellationTokenSource) null;
  }

  private async void AppActionCancelButton_Click(object sender, RoutedEventArgs e)
  {
    if (!(((FrameworkElement) sender).DataContext is ChatMessageModel dataContext) || dataContext.AppAction == null)
      return;
    if (dataContext.MaybeNotAppAction && dataContext.AskModel != null && this.chatMessages.LastOrDefault<ChatMessageModel>() == dataContext)
    {
      this.chatMessages.Remove(dataContext);
      CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
      this.cts?.Cancel();
      this.cts = cancellationTokenSource;
      CommomLib.Commom.GAManager.SendEvent("ChatPdf", "AppAction_CancelAndChat", dataContext.AppAction.Name ?? "", 1L);
      CopilotHelper.CopilotResult copilotResult = await this.DirectChatAsync(dataContext.AskModel.Text, dataContext.AskModel, cancellationTokenSource.Token);
    }
    else
    {
      CommomLib.Commom.GAManager.SendEvent("ChatPdf", "AppAction_Cancel", dataContext.AppAction.Name ?? "", 1L);
      dataContext.AppActionState.Value = ChatMessageModel.MessageAppActionState.Canceled;
    }
  }

  private async void AppActionYesButton_Click(object sender, RoutedEventArgs e)
  {
    if (!(((FrameworkElement) sender).DataContext is ChatMessageModel model))
      model = (ChatMessageModel) null;
    else if (model.AppAction == null)
    {
      model = (ChatMessageModel) null;
    }
    else
    {
      model.AppActionState.Value = ChatMessageModel.MessageAppActionState.Processing;
      try
      {
        CommomLib.Commom.GAManager.SendEvent("ChatPdf", "AppAction_Yes", model.AppAction.Name ?? "", 1L);
        int num = await this.CopilotHelper.ProcessNativeAppAction(model.AppAction) ? 1 : 0;
        model = (ChatMessageModel) null;
      }
      finally
      {
        model.AppActionState.Value = ChatMessageModel.MessageAppActionState.Done;
      }
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/copilot/chatpanel.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.TitleBar = (Grid) target;
        break;
      case 2:
        this.FeedbackButton = (Button) target;
        this.FeedbackButton.Click += new RoutedEventHandler(this.FeedbackButton_Click);
        break;
      case 3:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.CloseButton_Click);
        break;
      case 4:
        this.WelcomePage = (Grid) target;
        break;
      case 5:
        this.ChatNowButton = (Button) target;
        this.ChatNowButton.Click += new RoutedEventHandler(this.NavigatedFromWelcome);
        break;
      case 6:
        this.AnalysisProgressBar = (CommomLib.Controls.ProgressBar) target;
        break;
      case 7:
        this.ChatPage = (Grid) target;
        break;
      case 8:
        this.ScrollViewer = (ScrollViewer) target;
        break;
      case 9:
        this.ChatItemsControl = (ItemsControl) target;
        break;
      case 15:
        this.InputPanel = (Grid) target;
        break;
      case 16 /*0x10*/:
        this.ChatTips = (TextBlock) target;
        break;
      case 17:
        this.ClearHistoryButton = (HyperlinkButton) target;
        this.ClearHistoryButton.Click += new RoutedEventHandler(this.ClearHistoryButton_Click);
        break;
      case 18:
        this.ExportButton = (HyperlinkButton) target;
        this.ExportButton.Click += new RoutedEventHandler(this.ExportButton_Click);
        break;
      case 19:
        this.StopButton = (HyperlinkButton) target;
        this.StopButton.Click += new RoutedEventHandler(this.StopButton_Click);
        break;
      case 20:
        this.UserInputTextBox = (TextBox) target;
        this.UserInputTextBox.PreviewKeyDown += new KeyEventHandler(this.UserInputTextBox_PreviewKeyDown);
        break;
      case 21:
        this.ChatEmptytip = (ChatBubble) target;
        break;
      case 22:
        this.SendButton = (Button) target;
        this.SendButton.Click += new RoutedEventHandler(this.SendButton_Click);
        break;
      case 23:
        this.Overchance = (Grid) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IStyleConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 10:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.Like_Click);
        break;
      case 11:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.DisLike_Click);
        break;
      case 12:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.AppActionCancelButton_Click);
        break;
      case 13:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.AppActionCancelButton_Click);
        break;
      case 14:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.AppActionYesButton_Click);
        break;
    }
  }
}
