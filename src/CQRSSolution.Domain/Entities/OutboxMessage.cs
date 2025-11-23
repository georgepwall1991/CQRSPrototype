using System;

namespace CQRSSolution.Domain.Entities
{
    /// <summary>
    /// Represents a message to be published, stored temporarily in the outbox for guaranteed delivery.
    /// </summary>
    public class OutboxMessage
    {
        /// <summary>
        /// Gets or sets the unique identifier for the outbox message.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets or sets the fully qualified name of the event type.
        /// </summary>
        public string Type { get; private set; } = string.Empty;

        /// <summary>
        /// Gets or sets the serialized event data (e.g., in JSON format).
        /// </summary>
        public string Payload { get; private set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Coordinated Universal Time (UTC) when the event occurred.
        /// </summary>
        public DateTime OccurredOnUtc { get; private set; }

        /// <summary>
        /// Gets or sets the Coordinated Universal Time (UTC) when the message was processed and published.
        /// Null if the message has not yet been processed.
        /// </summary>
        public DateTime? ProcessedOnUtc { get; private set; }

        /// <summary>
        /// Gets or sets an error message if processing failed.
        /// </summary>
        public string? Error { get; private set; }

        /// <summary>
        /// Gets or sets the number of processing attempts made for this message.
        /// </summary>
        public int Attempts { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutboxMessage"/> class.
        /// </summary>
        /// <param name="type">The event type.</param>
        /// <param name="payload">The serialized payload.</param>
        public OutboxMessage(string type, string payload)
        {
            if (string.IsNullOrWhiteSpace(type)) throw new ArgumentException("Type cannot be empty", nameof(type));
            if (string.IsNullOrWhiteSpace(payload)) throw new ArgumentException("Payload cannot be empty", nameof(payload));

            Id = Guid.NewGuid();
            Type = type;
            Payload = payload;
            OccurredOnUtc = DateTime.UtcNow;
            Attempts = 0;
        }

        /// <summary>
        /// Protected constructor for EF Core.
        /// </summary>
        protected OutboxMessage() { }

        /// <summary>
        /// Marks the message as processed.
        /// </summary>
        public void MarkAsProcessed()
        {
            ProcessedOnUtc = DateTime.UtcNow;
            Error = null;
        }

        /// <summary>
        /// Records a processing failure.
        /// </summary>
        /// <param name="error">The error message.</param>
        public void RecordFailure(string error)
        {
            Error = error;
            Attempts++;
        }
    }
} 