using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Models;
using tenantApp.Views;
using tenantApp.Webservices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tenantApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TroubleGasPage : ContentPage
	{
        private ObservableCollection<MessageItem> msgItem { get; set; }
        private string conversationID;
        private HttpClient _httpClient;
        private string token;

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            conversationID = await getConversationId();
        }

        public TroubleGasPage ()
		{
			InitializeComponent ();

            msgItem = new ObservableCollection<MessageItem>();

            msgItem.Add(new MessageItem
            {
                quizcontentsVisibility = true,
                faqcontentsVisibility = false
            });

            listview.ItemsSource = msgItem;

            listview.ItemSelected += DeselectItem;
        }

        public void DeselectItem(object sender, EventArgs e)
        {
            ((Xamarin.Forms.ListView)sender).SelectedItem = null;
        }

        private void btnMenu_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void Btn_post_Clicked(object sender, EventArgs e)
        {
            getResponse(ent_question.Text);
        }

        private void trouble1_Clicked(object sender, EventArgs e)
        {
            getResponseFromSelected(Constants.BotAnswer_2);
        }

        private void trouble2_Clicked(object sender, EventArgs e)
        {
            getResponseFromSelected(Constants.BotAnswer_other);
        }

        private void trouble3_Clicked(object sender, EventArgs e)
        {
            getResponseFromSelected(Constants.BotAnswer_2);
        }

        private void troubleother_Clicked(object sender, EventArgs e)
        {
            getResponseFromSelected(Constants.BotAnswer_other);
        }

        private async Task<string> getConversationId()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://directline.botframework.com/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "DiXEYpVouk8.vEnWFaAmnxqrErqsewOePI73TnlphY5MAuTYFKJkz6c");
            var response = await _httpClient.PostAsync("/api/tokens/conversation", null);
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<string>(result.Result);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await _httpClient.PostAsync("/api/conversations", null);
                if (response.IsSuccessStatusCode)
                {
                    var conversationInfo = await response.Content.ReadAsStringAsync();
                    var conversationId = JsonConvert.DeserializeObject<Conversation>(conversationInfo).ConversationId;

                    return conversationId;
                }
            }

            return null;
        }

        private async void getResponse(string strSender)
        {
            if (!string.IsNullOrWhiteSpace(strSender))
            {
                msgItem.Add(new MessageItem
                {
                    quizcontentsVisibility = false,
                    faqcontentsVisibility = true,
                    imgUser = "imgUser.png",
                    UserQuestion = strSender,
                    botFrameVisibility = false,
                    userFrameVisibility = true,
                    imgUserVisibility = true,
                    imgBotVisibility = false
                });

                listview.ItemsSource = msgItem;

                List<MessageItem> msgList = ((IEnumerable<MessageItem>)this.listview.ItemsSource).ToList();
                listview.ScrollTo(msgList[msgList.Count - 1], ScrollToPosition.End, true);

                var messageToSend = new BotMessage() { From = App.tenant_nickname, Text = ent_question.Text };
                
                var contentPost = new StringContent(JsonConvert.SerializeObject(messageToSend), Encoding.UTF8, "application/json");
                var coid = conversationID;
                var conversationUrl = "https://directline.botframework.com/api/conversations/" + conversationID + "/messages/";

                var response = await _httpClient.PostAsync(conversationUrl, contentPost);

                var messagesReceived = await _httpClient.GetAsync(conversationUrl);
                var messagesReceivedData = await messagesReceived.Content.ReadAsStringAsync();
                var messagesRoot = JsonConvert.DeserializeObject<BotMessageRoot>(messagesReceivedData);
                var messages = messagesRoot.Messages;

                var renewUrl = "https://directline.botframework.com/api/tokens/" + conversationID + "/renew/";
                response = await _httpClient.GetAsync(renewUrl);
                try
                {
                    var botMessage = messages.Last().Text;
                    if (botMessage == "No QnA Maker answers were found.")
                    {
                        botMessage = Constants.BotAnswer_default;

                        var display = await DisplayAlert("", botMessage, "はい", "キャンセル");
                        if (display)
                        {
                            await Navigation.PushAsync(new QuizPage());
                        }
                    }

                    msgItem.Add(new MessageItem
                    {
                        quizcontentsVisibility = false,
                        faqcontentsVisibility = true,
                        imgBot = "imgRobot.png",
                        BotAnswer = botMessage,
                        userFrameVisibility = false,
                        botFrameVisibility = true,
                        imgBotVisibility = true,
                        imgUserVisibility = false
                    });

                    await App.QA_Data.SaveNotiAsync(new QAList
                    {
                        question = "Q : " + ent_question.Text,
                        answer = "A : " + botMessage,
                        c_date = DateTime.Now,
                        img_visibility = true
                    });

                    listview.ItemsSource = msgItem;

                    List<MessageItem> mmlist = ((IEnumerable<MessageItem>)this.listview.ItemsSource).ToList();
                    listview.ScrollTo(mmlist[mmlist.Count - 1], ScrollToPosition.End, true);

                    ent_question.Text = string.Empty;

                }
                catch { }
            }
        }

        private void getResponseFromSelected(string strSelected)
        {
            msgItem.Add(new MessageItem
            {
                quizcontentsVisibility = false,
                faqcontentsVisibility = true,
                imgBot = "imgRobot.png",
                BotAnswer = strSelected,
                userFrameVisibility = false,
                botFrameVisibility = true,
                imgBotVisibility = true,
                imgUserVisibility = false
            });

            listview.ItemsSource = msgItem;

            List<MessageItem> mmlist = ((IEnumerable<MessageItem>)this.listview.ItemsSource).ToList();
            listview.ScrollTo(mmlist[mmlist.Count - 1], ScrollToPosition.End, true);
        }
    }
}