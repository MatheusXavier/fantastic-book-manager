using NetArchTest.Rules;

using System.Reflection;

namespace Book.UnitTests.Architecture;

public class ArchitectureTests
{
    private const string DomainNamespace = "Book.Domain";
    private const string ApplicationNamespace = "Book.Application";
    private const string InfrastructureNamespace = "Book.Infrastructure";

    [Fact]
    public void BookDomain_Dependency_ShouldNotHaveDepedencyOnOtherProjects()
    {
        // Arrange
        var assembly = Assembly.Load(DomainNamespace);
        string[] otherProjects = new[]
        {
            ApplicationNamespace,
        };

        // Act
        TestResult result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void BookApplication_Dependency_ShouldNotHaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = Assembly.Load(ApplicationNamespace);
        string[] otherProjects = new[]
        {
            InfrastructureNamespace,
        };

        // Act
        TestResult result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
