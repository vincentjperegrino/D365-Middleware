using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.App.Queue.EmailNotification;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace TestCyware.EmailNotification
{
    public class ForExTest
    {
        private Mock<IQueueService> _queueServiceMock;
        private Mock<IEmailNotification> _emailNotificationMock;
        private Mock<ILogger<ForEx>> _loggerMock;

        private ForEx _forEx;

        public ForExTest()
        {
            _queueServiceMock = new Mock<IQueueService>();
            _emailNotificationMock = new Mock<IEmailNotification>();
            _loggerMock = new Mock<ILogger<ForEx>>();

            _forEx = new ForEx(_queueServiceMock.Object, _emailNotificationMock.Object);
        }

        [Fact]
        public void Run_WithRetrievedMessages_SendsEmailNotification()
        {
            // Arrange
            var messages = new List<Dictionary<string, string>>
        {
            new Dictionary<string, string> { { "Key1", "Value1" } },
            new Dictionary<string, string> { { "Key2", "Value2" } },
            new Dictionary<string, string> { { "Key3", "Value3" } }
        };
            Mock.Get(_queueServiceMock.Object).Setup(q => q.RetrieveAndPopMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<ILogger>()))
                .Returns(messages);

            // Act
            _forEx.Run(new TimerInfo(null, new ScheduleStatus()), _loggerMock.Object);

            // Assert
            Mock.Get(_emailNotificationMock.Object).Verify(e => e.Notify(
                It.IsAny<string>(),
                It.IsAny<string>(),
                messages,
                It.IsAny<string>(),
                _loggerMock.Object
            ), Times.Once);
        }
    }

}
