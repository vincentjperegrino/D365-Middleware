using Microsoft.Extensions.Logging;

namespace KTI.Moo.Base.Domain.Queue
{
    public interface IEmailNotification
    {
        bool Notify(string Subject, string MessageBody, IEnumerable<Dictionary<string, object>> AttachmentData, string AttachmentFileName, ILogger log);
        bool NotifyWithoutAttachment(string Subject, string MessageBody, ILogger log);
        bool NotifyWithAttachments(string Subject, string MessageBody, IEnumerable<IEnumerable<Dictionary<string, object>>> AttachmentDataCollection, IEnumerable<string> AttachmentFileNames, ILogger log);
    }
}
