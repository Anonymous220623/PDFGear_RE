// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Copilot.ChatMessageModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using pdfeditor.Utils.Copilot;

#nullable disable
namespace pdfeditor.Models.Copilot;

internal class ChatMessageModel : ObservableObject
{
  private string text = "";
  private ChatMessageModel askModel;
  private string role = "user";
  private EnumBindingObject<ChatMessageModel.ChatMessageType> messageType;
  private CopilotHelper.AppActionModel appAction;
  private EnumBindingObject<ChatMessageModel.MessageAppActionState> appActionState;
  private bool isEditing;
  private bool loading;
  private string errorText;
  private int[] pages;
  private ChatMessageModel.Liked like;
  private CopilotHelper.ChatResultError error;
  private bool maybeNotAppAction;

  private ChatMessageModel()
  {
  }

  public string Text
  {
    get => this.text;
    set => this.SetProperty<string>(ref this.text, value, nameof (Text));
  }

  public ChatMessageModel.Liked Like
  {
    get => this.like;
    set
    {
      if (!this.SetProperty<ChatMessageModel.Liked>(ref this.like, value, nameof (Like)))
        return;
      this.OnPropertyChanged("IsLiked");
      this.OnPropertyChanged("IsDisliked");
      this.OnPropertyChanged("IsLikeButtonVisible");
      this.OnPropertyChanged("IsDislikeButtonVisible");
    }
  }

  public EnumBindingObject<ChatMessageModel.ChatMessageType> MessageType
  {
    get
    {
      return this.messageType ?? (this.messageType = new EnumBindingObject<ChatMessageModel.ChatMessageType>(ChatMessageModel.ChatMessageType.Chat));
    }
  }

  public CopilotHelper.AppActionModel AppAction
  {
    get => this.appAction;
    set
    {
      this.SetProperty<CopilotHelper.AppActionModel>(ref this.appAction, value, nameof (AppAction));
    }
  }

  public EnumBindingObject<ChatMessageModel.MessageAppActionState> AppActionState
  {
    get
    {
      return this.appActionState ?? (this.appActionState = new EnumBindingObject<ChatMessageModel.MessageAppActionState>(ChatMessageModel.MessageAppActionState.None));
    }
  }

  public ChatMessageModel AskModel
  {
    get => this.askModel;
    set => this.SetProperty<ChatMessageModel>(ref this.askModel, value, nameof (AskModel));
  }

  public CopilotHelper.ChatResultError Error
  {
    get => this.error;
    set
    {
      if (!this.SetProperty<CopilotHelper.ChatResultError>(ref this.error, value, nameof (Error)))
        return;
      this.OnPropertyChanged("HasError");
      this.OnPropertyChanged("NoError");
    }
  }

  public bool MaybeNotAppAction
  {
    get => this.maybeNotAppAction;
    set => this.SetProperty<bool>(ref this.maybeNotAppAction, value, nameof (MaybeNotAppAction));
  }

  public bool Loading
  {
    get => this.loading;
    set => this.SetProperty<bool>(ref this.loading, value, nameof (Loading));
  }

  public bool IsEditing
  {
    get => this.isEditing;
    set => this.SetProperty<bool>(ref this.isEditing, value, nameof (IsEditing));
  }

  public int[] Pages
  {
    get => this.pages;
    set => this.SetProperty<int[]>(ref this.pages, value, nameof (Pages));
  }

  public bool HasError => this.Error != 0;

  public bool NoError => this.Error == CopilotHelper.ChatResultError.None;

  public string Role => this.role;

  public bool IsUser => this.role == "user";

  public bool IsAssistant => this.role == "assistant";

  public bool IsLiked => this.like == ChatMessageModel.Liked.Like;

  public bool IsDisliked => this.like == ChatMessageModel.Liked.Dislike;

  public bool IsLikeButtonVisible
  {
    get
    {
      if (!this.IsAssistant || this.MessageType.Value == ChatMessageModel.ChatMessageType.Summary)
        return false;
      return this.like == ChatMessageModel.Liked.Like || this.like == ChatMessageModel.Liked.None;
    }
  }

  public bool IsDislikeButtonVisible
  {
    get
    {
      if (!this.IsAssistant || this.MessageType.Value == ChatMessageModel.ChatMessageType.Summary)
        return false;
      return this.like == ChatMessageModel.Liked.Dislike || this.like == ChatMessageModel.Liked.None;
    }
  }

  public static ChatMessageModel CreateUserModel()
  {
    return new ChatMessageModel() { role = "user" };
  }

  public static ChatMessageModel CreateAssistantModel(ChatMessageModel.ChatMessageType type)
  {
    return new ChatMessageModel()
    {
      role = "assistant",
      MessageType = {
        Value = type
      }
    };
  }

  public static ChatMessageModel Create(string role, ChatMessageModel.ChatMessageType type)
  {
    return new ChatMessageModel()
    {
      role = role,
      MessageType = {
        Value = type
      }
    };
  }

  public enum Liked
  {
    None,
    Like,
    Dislike,
  }

  public enum ChatMessageType
  {
    Chat,
    Summary,
    Welcome,
    AppAction,
  }

  public enum MessageAppActionState
  {
    None,
    Processing,
    Done,
    Canceled,
  }
}
