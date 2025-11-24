using CQRSSolution.Api.Controllers;
using CQRSSolution.Application.Commands.CreateOrder;
using CQRSSolution.Domain.Entities;
using CQRSSolution.Infrastructure.Persistence;
using NetArchTest.Rules;
using Xunit;

namespace CQRSSolution.UnitTests;

public class ArchitectureTests
{
    private const string DomainNamespace = "CQRSSolution.Domain";
    private const string ApplicationNamespace = "CQRSSolution.Application";
    private const string InfrastructureNamespace = "CQRSSolution.Infrastructure";
    private const string ApiNamespace = "CQRSSolution.Api";

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = typeof(Order).Assembly;

        var result = Types.InAssembly(assembly)
            .Should()
            .NotHaveDependencyOn(ApplicationNamespace)
            .And()
            .NotHaveDependencyOn(InfrastructureNamespace)
            .And()
            .NotHaveDependencyOn(ApiNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_Should_Not_HaveDependencyOnInfrastructureOrApi()
    {
        var assembly = typeof(CreateOrderCommand).Assembly;

        var result = Types.InAssembly(assembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureNamespace)
            .And()
            .NotHaveDependencyOn(ApiNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnApi()
    {
        var assembly = typeof(ApplicationDbContext).Assembly;

        var result = Types.InAssembly(assembly)
            .Should()
            .NotHaveDependencyOn(ApiNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
}
