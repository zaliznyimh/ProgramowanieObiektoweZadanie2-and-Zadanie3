using Microsoft.VisualStudio.TestPlatform.Utilities;
using SampleHierarchies.Enums;

namespace SampleHierarchies.Services.Tests;

[TestClass]
public class ScreenDefinitionServiceTests
{
    private readonly string notNullJsonFileNameLoad = "NotNullJsonFileNameTestLoad.json";
    private readonly string isNullJsonFileNameLoad = "IsNullJsonFileNameTest.json";
    private readonly string normalJsonFileNameShowLines = "NormalJsonFileNameShowLines.json";
    private readonly string notNormalJsonFileNameShowLines = "NotNormalJsonFileNameShowLines.json";

    [TestMethod]
    public void Load_JsonFileIsNotNull_ReturnsScreenLineHelper()
    {
        // Arrange
        var screenDefinitionService = new ScreenDefinitionService();

        // Act
        var result = ScreenDefinitionService.Load(notNullJsonFileNameLoad);

        // Assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void Load_JsonFileIsNull_ReturnsNull()
    {
        // Arrange
        var screenDefinitionService = new ScreenDefinitionService();

        // Act
        var result = ScreenDefinitionService.Load(isNullJsonFileNameLoad);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void ShowLines_NormalJsonFile_OutputToConsoleIsSuccessfull()
    {
        // Arrange
        var screenDefinitionService = new ScreenDefinitionService();

        // Act
        var result = screenDefinitionService.ShowLines(normalJsonFileNameShowLines, ScreenLineEnum.Test, 0);

        // Assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ShowLines_NotNormalJsonFile_OutputToConsoleIsNotSuccessfull()
    {

        // Arrange
        var screenDefinitionService = new ScreenDefinitionService();

        // Act
        var result = screenDefinitionService.ShowLines(notNormalJsonFileNameShowLines, ScreenLineEnum.Test, 0);

        // Assert
        Assert.IsNull(result);
    }
}