using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace ExpressServices.Core.Services
{
    public class WebSocketService
    {
        private MessageWebSocket _webSock;

        public delegate void OnMessageHandler(object s, string message);

        public event EventHandler<string> OnMessage;

        public WebSocketService()
        {
            //Instanciate the websocket
            _webSock = new MessageWebSocket();
        }

        public async Task<bool> Init(Uri uri)
        {
            bool success = true;
            //In this case we will be sending/receiving a string so we need to set the MessageType to Utf8.
            _webSock.Control.MessageType = SocketMessageType.Utf8;

            //Add the MessageReceived event handler.
            _webSock.MessageReceived += MessageReceived;

            //Add the Closed event handler.
            _webSock.Closed += OnClose;

            try
            {
                //Connect to the server.
                await _webSock.ConnectAsync(uri);
            }
            catch
            {
                ShowImpossibleToConnectToast();
                success = false;
            }

            return success;
        }

        private void MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            using (DataReader messageReader = args?.GetDataReader())
            {
                messageReader.UnicodeEncoding = UnicodeEncoding.Utf8;
                var response = messageReader.ReadString(messageReader.UnconsumedBufferLength);

                OnMessage?.Invoke(this, response);
            }
        }

        private void ShowImpossibleToConnectToast()
        {
            Debug.WriteLine("Impossível conectar com o servidor local de serviços. Verifique se ele está aberto.");
            _webSock.Dispose();
        }

        public void Close()
        {
            _webSock.Close(1001, "");
            _webSock.Dispose();
        }

        public async void SendMessage(string message)
        {
            using (DataWriter messageWriter = new DataWriter(_webSock.OutputStream))
            {
                messageWriter.WriteString(message);
                await messageWriter.StoreAsync();
            }
        }

        #region Handlers

        private void OnClose(object sender, WebSocketClosedEventArgs e)
        {
            Console.WriteLine("Conexão perdida com o servidor de sockets.");
        }

        #endregion Handlers
    }
}
