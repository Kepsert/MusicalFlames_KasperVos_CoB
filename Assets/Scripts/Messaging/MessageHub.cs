using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Messaging
{
    public class MessageClass
    {
        public int Token { get; private set; }
        public Action<MessageBase> Message;

        public MessageClass(Action<MessageBase> message, int token)
        {
            Message = message;
            Token = token;
        }
    }
    public static class MessageHub
    {
        private static readonly Dictionary<Type, List<MessageClass>> _registeredListeners =
            new Dictionary<Type, List<MessageClass>>();

        public static void ClearListeners()
        {
            _registeredListeners.Clear();
        }

        public static void Unsubscribe<T>(Object token)
        {
            var type = typeof(T);
            if (_registeredListeners.TryGetValue(type, out var listener))
            {
                listener.RemoveAll(x => x.Token == token.GetInstanceID());
            }
        }

        public static void Unsubscribe<T>(int instanceId)
        {
            var type = typeof(T);
            if (_registeredListeners.TryGetValue(type, out var listener))
            {
                listener.RemoveAll(x => x.Token == instanceId);
            }
        }

        public static void Subscribe<T>(Object monoBehaviour, Action<T> listener) where T : MessageBase
        {
            Subscribe(monoBehaviour.GetInstanceID(), listener);
        }

        public static void Subscribe<T>(int instanceId, Action<T> listener) where T : MessageBase
        {
            var type = typeof(T);
            if (!_registeredListeners.ContainsKey(type))
            {
                _registeredListeners.Add(type, new List<MessageClass>());
            }

            var messageClass = new MessageClass(m => listener(m as T), instanceId);
            _registeredListeners[type].Add(messageClass);

        }



        public static async Task PublishAsync<T>(T message) where T : MessageBase
        {
            await Task.Run(() => Publish(message));
        }

        public static void Publish<T>(T message) where T : MessageBase
        {
            PublishInternal(message);
        }

        private static void PublishInternal<T>(T message) where T : MessageBase
        {
            var type = message.GetType();
            if (_registeredListeners.TryGetValue(type, out var listener))
            {
                foreach (var kv in listener)
                {
                    try
                    {
                        kv.Message?.Invoke(message);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"{e}\n\n{e.StackTrace}");
                    }
                }
            }
        }
    }
}
