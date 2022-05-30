using Moq;

namespace DataProcessor.Library.Tests;

[TestClass]
public class DataParserLoggerTests
{
    [TestMethod]
    public async Task ParseData_WithBadRecord_LoggerIsCalledOnce()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        var parser = new DataParser(mockLogger.Object);

        // Act
        await parser.ParseData(TestData.BadRecord);

        // Assert
        mockLogger.Verify(m =>
            m.LogMessage(It.IsAny<string>(), TestData.BadRecord[0]),
            Times.Once());
    }

    [TestMethod]
    public async Task ParseData_WithGoodRecord_LoggerIsNotCalled()
    {
        var mockLogger = new Mock<ILogger>();
        var parser = new DataParser(mockLogger.Object);

        await parser.ParseData(TestData.GoodRecord);

        mockLogger.Verify(m =>
            m.LogMessage(It.IsAny<string>(), TestData.GoodRecord[0]),
            Times.Never());
    }

    [TestMethod]
    public async Task ParseData_WithBadStartDate_LoggerIsCalledOnce()
    {
        var mockLogger = new Mock<ILogger>();
        var parser = new DataParser(mockLogger.Object);

        await parser .ParseData(TestData.BadStartDate);

        mockLogger.Verify(m =>
            m.LogMessage(It.IsAny<string>(), TestData.BadStartDate[0]),
            Times.Once());
    }

    [TestMethod]
    public async Task ParseData_WithBadRating_LoggerIsCalledOnce()
    {
        var mockLogger = new Mock<ILogger>();
        var parser = new DataParser(mockLogger.Object);

        await parser.ParseData(TestData.BadRating);

        mockLogger.Verify(m =>
            m.LogMessage(It.IsAny<string>(), TestData.BadRating[0]),
            Times.Once());
    }
}
