﻿using System;
using System.Text;
using WebSocket4Net;

namespace MediaBrowser.ApiInteraction.WebSocket
{
    public class WebSocket4NetClientWebSocket : IClientWebSocket
    {
        private WebSocket4Net.WebSocket _socket;

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public Model.Net.WebSocketState State
        {
            get { 
            
                switch (_socket.State)
                {
                    case WebSocketState.Closed:
                        return Model.Net.WebSocketState.Closed;
                    case WebSocketState.Closing:
                        return Model.Net.WebSocketState.Closed;
                    case WebSocketState.Connecting:
                        return Model.Net.WebSocketState.Connecting;
                    case WebSocketState.None:
                        return Model.Net.WebSocketState.None;
                    case WebSocketState.Open:
                        return Model.Net.WebSocketState.Open;
                    default:
                        return Model.Net.WebSocketState.None;
                }
            }
        }

        /// <summary>
        /// Connects.
        /// </summary>
        /// <param name="url">The URL.</param>
        public void Connect(string url)
        {
            _socket = new WebSocket4Net.WebSocket(url);

            _socket.MessageReceived += websocket_MessageReceived;

            _socket.Open();
        }

        /// <summary>
        /// Handles the MessageReceived event of the websocket control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MessageReceivedEventArgs"/> instance containing the event data.</param>
        void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (OnReceiveDelegate != null)
            {
                OnReceiveDelegate(e.Message);
            }
        }

        /// <summary>
        /// Gets or sets the receive action.
        /// </summary>
        /// <value>The receive action.</value>
        public Action<string> OnReceiveDelegate { get; set; }

        /// <summary>
        /// Sends the async.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="type">The type.</param>
        /// <param name="endOfMessage">if set to <c>true</c> [end of message].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        public void Send(byte[] bytes, Model.Net.WebSocketMessageType type, bool endOfMessage)
        {
            _socket.Send(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_socket != null)
            {
                _socket.Close();
                _socket = null;
            }
        }
    }
}
