using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komodo.Core.Model
{
    public partial class QueueMessage
    {
        public QueueMessage(string messageBody)
        {
            AsString = messageBody;
        }
        // Summary:
        //     Gets the content of the message as a byte array.
        public int Id { get; set; }
        // Summary:
        //     Gets the content of the message as a byte array.
        public byte[] AsBytes { get; set; }
        //
        // Summary:
        //     Gets the content of the message, as a string.
        public string AsString { get; set; }
        //
        // Summary:
        //     Gets the number of times this message has been dequeued.
        public int DequeueCount { get; set; }
        //
        // Summary:
        //     Gets the time that the message expires.
        public DateTimeOffset? ExpirationTime { get; set; }
        //
        // Summary:
        //     Gets the message ID.
        public string MessageId { get; set; }
        //
        // Summary:
        //     Gets the time that the message was added to the queue.
        public DateTimeOffset? InsertionTime { get; set; }
        //
        // Summary:
        //     Gets the maximum message size in bytes.
        public static long MaxMessageSize { get; set; }
        //
        // Summary:
        //     Gets the maximum number of messages that can be peeked at a time.
        public static int MaxNumberOfMessagesToPeek { get; set; }
        //
        // Summary:
        //     Gets the maximum amount of time a message is kept in the queue.
        public static TimeSpan MaxTimeToLive { get; set; }
        //
        // Summary:
        //     Gets the time that the message will next be visible.
        public DateTimeOffset? NextVisibleTime { get; set; }
        //
        // Summary:
        //     Gets the message's pop receipt.
        public string PopReceipt { get; set; }

    }

}

