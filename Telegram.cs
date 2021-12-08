﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PainTrainStation
{
    static class Telegram
    {
        static string APIKey;
        static string APIPath = "https://api.telegram.org/bot{0}/";
        static JsonSerializer serializer;
        public static string lastError;
        public const string tag = "PainTrainStation@telegram";

        public static void SetAPIKey(string aik)
        {
            APIKey = aik;
            serializer = JsonSerializer.Create();
            Helpers.writeOut(tag, "API Key was updated.");

        }

        public static TGUpdate[] getUpdates(long offset)
        {
            var b = new NameValueCollection();
            b["offset"] = offset.ToString();

            TGResponse resp = apiGetRequest("getUpdates", b);
            if (resp.ok == true)
            {
                var rede = resp.result.CreateReader();
                var ret = serializer.Deserialize<TGUpdate[]>(rede);
                rede.Close();
                return ret;
            }
            return null;
        }

        public static TGUpdate[] getUpdates(long offset, short timeout)
        {
            var b = new NameValueCollection();
            b["offset"] = offset.ToString();
            b["timeout"] = timeout.ToString();
            TGResponse resp = apiGetRequest("getUpdates", b);
            if (resp.ok == true)
            {
                var rede = resp.result.CreateReader();
                var ret = serializer.Deserialize<TGUpdate[]>(rede);
                rede.Close();
                return ret;
            }
            return null;
        }

        public static TGChatMember getChatMember(TGChat chat, TGUser user)
        {
            var b = new NameValueCollection();
            b["chat_id"] = chat.id.ToString();
            b["user_id"] = user.id.ToString();

            TGResponse resp = apiGetRequest("getChatMember", b);
            if (resp.ok == true)
            {
                var rede = resp.result.CreateReader();
                var ret = serializer.Deserialize<TGChatMember>(rede);
                rede.Close();
                return ret;
            }
            return null;

        }

        public static TGChat getChat(long chat)
        {
            var b = new NameValueCollection();
            b["chat_id"] = chat.ToString();

            TGResponse resp = apiGetRequest("getChat", b);
            if (resp.ok == true)
            {
                var rede = resp.result.CreateReader();
                var ret = serializer.Deserialize<TGChat>(rede);
                rede.Close();
                return ret;
            }
            return null;

        }
        public static TGChat getChat(string chat)
        {
            var b = new NameValueCollection();
            b["chat_id"] = chat;
            Console.WriteLine(chat);
            TGResponse resp = apiGetRequest("getChat", b);
            if (resp.ok == true)
            {
                var rede = resp.result.CreateReader();
                var ret = serializer.Deserialize<TGChat>(rede);
                rede.Close();
                return ret;
            }
            return null;

        }

        public static int getNumProfilePhotos(TGUser user)
        {
            var b = new NameValueCollection();

            b["user_id"] = user.id.ToString();

            TGResponse resp = apiGetRequest("getUserProfilePhotos", b);
            if (resp.ok == true)
            {
                var rede = resp.result.CreateReader();
                var ret = serializer.Deserialize<TGProfilePhotos>(rede);
                rede.Close();
                return ret.total_count;
            }
            return -1;

        }



        public static bool restrictChatMember(TGChat chat, TGUser who, int secondsDuration, bool canSendmessages, bool canSendMedia, bool canSendMisc, bool generateLinkPreviews)
        {
            if (secondsDuration < 30 && secondsDuration != 0)
            {
                secondsDuration = 35;  // Prevent accidental permaban.
            }

            var b = new NameValueCollection();
            b["chat_id"] = chat.id.ToString();
            b["user_id"] = who.id.ToString();
            b["until_date"] = (Helpers.getUnixTime() + secondsDuration).ToString();
            b["can_send_messages"] = canSendmessages.ToString();
            b["can_send_media_messages"] = canSendMedia.ToString();
            b["can_send_other_messages"] = canSendMisc.ToString();
            b["can_add_web_page_previews"] = generateLinkPreviews.ToString();
            TGResponse resp = apiGetRequest("restrictChatMember", b);

            if (resp.ok == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static bool restrictChatMemberMute(TGChat chat, TGUser who, bool canSendMedia, bool canSendMisc, bool generateLinkPreviews)
        {

            var b = new NameValueCollection();
            b["chat_id"] = chat.id.ToString();
            b["user_id"] = who.id.ToString();
            b["until_date"] = "0";
            b["can_send_media_messages"] = canSendMedia.ToString();
            b["can_send_other_messages"] = canSendMisc.ToString();
            b["can_add_web_page_previews"] = generateLinkPreviews.ToString();
            TGResponse resp = apiGetRequest("restrictChatMember", b);

            if (resp.ok == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool kickChatMember(TGChat chat, TGUser who, int secondsDuration)
        {
            if (secondsDuration < 30 && secondsDuration != 0)
            {
                secondsDuration = 35;  // Prevent accidental permaban.
            }
            var b = new NameValueCollection();
            b["chat_id"] = chat.id.ToString();
            b["user_id"] = who.id.ToString();
            b["until_date"] = (Helpers.getUnixTime() + secondsDuration).ToString();

            TGResponse resp = apiGetRequest("kickChatMember", b);

            if (resp.ok == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool unbanChatMember(TGChat chat, TGUser who, int secondsDuration)
        {
            if (secondsDuration < 30 && secondsDuration != 0)
            {
                secondsDuration = 35;  // Prevent accidental permaban.
            }

            var b = new NameValueCollection();
            b["chat_id"] = chat.id.ToString();
            b["user_id"] = who.id.ToString();

            TGResponse resp = apiGetRequest("unbanChatMember", b);
            if (resp.ok == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static TGMessage sendMessage(TGChat chat, string message)
        {
            var b = new NameValueCollection();
            b["chat_id"] = chat.id.ToString();
            b["text"] = message;

            TGResponse resp = apiGetRequest("sendMessage", b);

            if (resp.ok == true)
            {
                return serializer.Deserialize<TGMessage>(resp.result.CreateReader());
            }
            return null;
        }


        public static TGUser getMe()
        {
            var b = new NameValueCollection();
            TGResponse resp = apiGetRequest("getMe", b);

            if (resp.ok == true)
            {
                return serializer.Deserialize<TGUser>(resp.result.CreateReader());
            }

            return null;
        }

        public static bool deleteMessage(TGChat chat, long MessageID)
        {
            var b = new NameValueCollection();
            b["chat_id"] = chat.id.ToString();
            b["message_id"] = MessageID.ToString();

            TGResponse resp = apiGetRequest("deleteMessage", b);

            if (resp.ok == true)
            {
                return true;
            }

            return false;
        }

        public static bool deleteMessage(TGChat chat, TGMessage message)
        {
            var b = new NameValueCollection();
            b["chat_id"] = chat.id.ToString();
            b["message_id"] = message.message_id.ToString();

            TGResponse resp = apiGetRequest("deleteMessage", b);

            if (resp.ok == true)
            {
                return true;
            }

            return false;
        }
        public static async Task<bool> deleteMessageAsync(TGChat chat, TGMessage message)
        {
            var b = new NameValueCollection();
            b["chat_id"] = chat.id.ToString();
            b["message_id"] = message.message_id.ToString();

            TGResponse resp = await apiGetRequestAsync("deleteMessage", b);

            if (resp.ok == true)
            {
                return true;
            }

            return false;
        }


        public static async Task<bool> banSenderChatAsync(TGChat chat, TGChat banChat)
        {
            var b = new NameValueCollection();
            b["chat_id"] = chat.id.ToString();
            b["sender_chat_id"] = banChat.id.ToString();

            TGResponse resp = await apiGetRequestAsync("banChatSenderChat", b);

            if (resp.ok == true)
            {
                return true;
            }

            return false;
        }


        public static TGMessage sendMessage(TGChat chat, string message, string parse_mode)
        {

            var b = new NameValueCollection();

            b["chat_id"] = chat.id.ToString();
            b["text"] = message;
            b["parse_mode"] = parse_mode.ToString();

            TGResponse resp = apiGetRequest("sendMessage", b);
            var rede = resp.result.CreateReader();
            if (resp.ok == true)
            {
                return serializer.Deserialize<TGMessage>(rede);
            }
            else
            {
                lastError = serializer.Deserialize<string>(rede);
            }

            return null;

        }

        public static TGResponse apiGetRequest(string req, NameValueCollection para)
        {

            var fullPath = string.Format(APIPath, APIKey);
            var endpoint = fullPath + req;

            using (WebClient client = new WebClient())
            {
                try
                {
                    byte[] response = client.UploadValues(endpoint, para);
                    string result = System.Text.Encoding.UTF8.GetString(response);
                    JObject tree = JObject.Parse(result);

                    return new TGResponse
                    {
                        ok = (bool)tree["ok"],
                        result = tree["result"],
                    };

                }
                catch (WebException F)
                {
                    lastError = F.ToString();
                    return new TGResponse
                    {
                        ok = false,
                    };
                }

            }
        }

        public static async Task<TGResponse> apiGetRequestAsync(string req, NameValueCollection para)
        {

            var fullPath = string.Format(APIPath, APIKey);
            var endpoint = fullPath + req;

            using (WebClient client = new WebClient())
            {
                try
                {
                    byte[] response = await client.UploadValuesTaskAsync(endpoint, para);
                    string result = System.Text.Encoding.UTF8.GetString(response);
                    JObject tree = JObject.Parse(result);

                    return new TGResponse
                    {
                        ok = (bool)tree["ok"],
                        result = tree["result"],
                    };

                }
                catch (WebException F)
                {
                    lastError = F.ToString();
                    return new TGResponse
                    {
                        ok = false,
                    };
                }

            }
        }
    }
}