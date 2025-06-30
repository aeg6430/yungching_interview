using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace Yungching.UnitTests
{
    public static class LoggerVerifier
    {
        public static void VerifyLogError<T>(Mock<ILogger<T>> loggerMock, Times times, string? containsMessage = null)
        {
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) =>
                        containsMessage == null || (v != null && v.ToString()!.Contains(containsMessage))),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                times);
        }

        public static void VerifyLogWarning<T>(Mock<ILogger<T>> loggerMock, Times times, string? containsMessage = null)
        {
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) =>
                        containsMessage == null || (v != null && v.ToString()!.Contains(containsMessage))),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                times);
        }

        public static void VerifyLogInformation<T>(Mock<ILogger<T>> loggerMock, Times times, string? containsMessage = null)
        {
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) =>
                        containsMessage == null || (v != null && v.ToString()!.Contains(containsMessage))),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                times);
        }
    }
}
