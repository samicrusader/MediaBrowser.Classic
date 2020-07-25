﻿using MediaBrowser.Model.Logging;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace MediaBrowser.ApiInteraction.WebSocket
{
    /// <summary>
    /// Class NativeClientWebSocket
    /// </summary>
    public class NativeClientWebSocket : IClientWebSocket
    {
        /// <summary>
        /// The _client
        /// </summary>
        private readonly ClientWebSocket _client;
        /// <summary>
        /// The _logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeClientWebSocket" /> class.
        /// </summary>
        /// <param name="logManager">The log manager.</param>
        public NativeClientWebSocket(ILogManager logManager)
        {
            _logger = logManager.GetLogger("NativeClientWebSocket");
            _client = new ClientWebSocket();
        }

        /// <summary>
        /// Gets or sets the receive action.
        /// </summary>
        /// <value>The receive action.</value>
        public Action<byte[]> OnReceiveDelegate { get; set; }

        /// <summary>
        /// Connects the async.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        public async Task ConnectAsync(string url, CancellationToken cancellationToken)
        {
            try
            {
                await _client.ConnectAsync(new Uri(url), cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Error connecting to {0}", ex, url);

                throw;
            }

            Receive();
        }

        /// <summary>
        /// Receives this instance.
        /// </summary>
        private async void Receive()
        {
            while (true)
            {
                byte[] bytes;

                try
                {
                    bytes = await ReceiveBytesAsync(CancellationToken.None).ConfigureAwait(false);
                }
                catch (WebSocketException ex)
                {
                    _logger.ErrorException("Error receiving web socket message", ex);

                    break;
                }

                if (OnReceiveDelegate != null)
                {
                    OnReceiveDelegate(bytes);
                }
            }
        }

        /// <summary>
        /// Receives the async.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task{WebSocketMessageInfo}.</returns>
        /// <exception cref="System.Net.WebSockets.WebSocketException">Connection closed</exception>
        private async Task<byte[]> ReceiveBytesAsync(CancellationToken cancellationToken)
        {
            var bytes = new byte[4096];
            var buffer = new ArraySegment<byte>(bytes);

            var result = await _client.ReceiveAsync(buffer, cancellationToken).ConfigureAwait(false);

            if (result.CloseStatus.HasValue)
            {
                throw new WebSocketException("Connection closed");
            }

            return buffer.Array;
        }

        /// <summary>
        /// Sends the async.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="type">The type.</param>
        /// <param name="endOfMessage">if set to <c>true</c> [end of message].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        public Task SendAsync(byte[] bytes, Model.Net.WebSocketMessageType type, bool endOfMessage, CancellationToken cancellationToken)
        {
            WebSocketMessageType nativeType;

            if (!Enum.TryParse(type.ToString(), true, out nativeType))
            {
                _logger.Warn("Unrecognized WebSocketMessageType: {0}", type.ToString());
            }

            return _client.SendAsync(new ArraySegment<byte>(bytes), nativeType, true, cancellationToken);
        }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public Model.Net.WebSocketState State
        {
            get
            {
                Model.Net.WebSocketState commonState;

                if (!Enum.TryParse(_client.State.ToString(), true, out commonState))
                {
                    _logger.Warn("Unrecognized WebSocketState: {0}", _client.State.ToString());
                }

                return commonState;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="dispose"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                _client.Dispose();
            }
        }
    }
}
