﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace HfUtils
{
    public class HandlerThread
    {

        Thread mThread;

        public delegate void DoMessageHandler(Message msg);

        private Queue<Message> mQueue = new Queue<Message>();

        public event DoMessageHandler MessageHandler;

        ManualResetEvent mEmptyLock = new ManualResetEvent(false);
        bool mQuiting = false;

        public HandlerThread()
        {
            mThread = new Thread(this.DoLoop);
        }

        private void DoLoop()
        {
            while (!mQuiting)
            {
                Message msg = next();
                if (msg == null)
                    return;
                MessageHandler.Invoke(msg);
                msg.reset();
            }
        }

        // This function may call in mutil thread
        bool enqueueMessage(Message msg, long when)
        {
            lock (mQueue)
            {
                mQueue.Enqueue(msg);
                mEmptyLock.Set();
                return true;
            }
        }

        public void Start()
        {
            mThread.Start();
        }

        public void Stop()
        {
            mQuiting = true;
            mEmptyLock.Set();
        }

        // 取出下一个消息, 如果消息为空, 则阻塞, 直到有消息
        Message next()
        {
            while (!mQuiting)
            {
                if (mQueue.Count == 0)
                {
                    mEmptyLock.WaitOne();
                }
                else
                {
                    lock (mQueue)
                    {
                        return mQueue.Dequeue();
                    }
                }
            }
            return null;
        }
        public bool hasMessage(int what, object obj)
        {
            lock (mQueue)
            {
                foreach (Message msg in mQueue)
                {
                    if (msg.what == what && (obj == null || msg.obj == obj))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void sendMessage(Message msg)
        {
            enqueueMessage(msg, DateTime.Now.Ticks);
        }

        public void sendMessage(int what, int arg1, int arg2, object obj)
        {
            Message msg = new Message(what, arg1, arg2, obj);
            enqueueMessage(msg, DateTime.Now.Ticks);
        }

        public void sendMessage(int what)
        {
            Message msg = new Message(what);
            enqueueMessage(msg, DateTime.Now.Ticks);
        }
        public void sendMessage(int what, int arg1, int arg2)
        {
            Message msg = new Message(what, arg1, arg2);
            enqueueMessage(msg, DateTime.Now.Ticks);
        }

        public void sendMessage(int what, object obj)
        {
            Message msg = new Message(what, obj);
            enqueueMessage(msg, DateTime.Now.Ticks);
        }
    }

    public class Message
    {
        public int what;
        public int arg1;
        public int arg2;
        public object obj;
        public long when;

        public Message()
        {

        }

        public Message(int what) : this(what, 0, 0, null)
        {

        }

        public Message(int what, int arg1, int arg2) : this(what, arg1, arg2, null)
        {

        }
        public Message(int what, object obj) : this(what, 0, 0, obj)
        {

        }

        public Message(int what, int arg1, int arg2, object obj)
        {
            this.what = what;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.obj = obj;
        }

        internal void reset()
        {
            what = 0;
            arg1 = 0;
            arg2 = 0;
            obj = null;
            when = 0;
        }
    }

}
