﻿using YouTubeSessionGenerator.Js;
using YouTubeSessionGenerator.Js.Environments;

namespace YouTubeSessionGenerator.Tests;

public class YouTubeSessionGeneratorTests
{
    [Test]
    public void Should_generate_session()
    {
        // Arrange
        using IJsEnvironment environment = new NodeEnvironment();
        YouTubeSessionGenerator generator = new(new() { JsEnvironment = environment });

        // Act
        string? visitorData = null;
        string? rolloutToken = null;
        string? proofOfOriginToken = null;

        Assert.DoesNotThrowAsync(async () =>
        {
            visitorData = await generator.CreateVisitorDataAsync();
            rolloutToken = await generator.CreateRolloutTokenAsync();
            proofOfOriginToken = await generator.CreateProofOfOriginTokenAsync(visitorData);
        });

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(visitorData, Is.Not.Null.Or.Empty);
            Assert.That(rolloutToken, Is.Not.Null.Or.Empty);
            Assert.That(proofOfOriginToken, Is.Not.Null.Or.Empty);
        });

        // Output
        TestContext.Out.WriteLine("Visitor Data: {0}", visitorData);
        TestContext.Out.WriteLine("Rollout Token: {0}", rolloutToken);
        TestContext.Out.WriteLine("Proof Of Origin Token: {0}", proofOfOriginToken);
    }


    [Test]
    public void Should_generate_visior_data()
    {
        // Arrange
        YouTubeSessionGenerator generator = new(new());

        // Act
        string? visitorData = null;

        Assert.DoesNotThrowAsync(async () =>
        {
            visitorData = await generator.CreateVisitorDataAsync();
        });

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(visitorData, Is.Not.Null.Or.Empty);
        });

        // Output
        TestContext.Out.WriteLine("Visitor Data: {0}", visitorData);
    }

    [Test]
    public void Should_generate_rollout_token()
    {
        // Arrange
        YouTubeSessionGenerator generator = new(new());

        // Act
        string? rolloutToken = null;

        Assert.DoesNotThrowAsync(async () =>
        {
            rolloutToken = await generator.CreateRolloutTokenAsync();
        });

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(rolloutToken, Is.Not.Null.Or.Empty);
        });

        // Output
        TestContext.Out.WriteLine("Rollout Token: {0}", rolloutToken);
    }

    [Test]
    [TestCase("CgszX1.....................A%3D%3D")]
    [Ignore("Valid Visitor Data required!")]
    public void Should_generate_proof_of_origin_token(
        string visitorData)
    {
        // Arrange
        using IJsEnvironment environment = new NodeEnvironment();
        YouTubeSessionGenerator generator = new(new() { JsEnvironment = environment });

        // Act
        string? proofOfOriginToken = null;

        Assert.DoesNotThrowAsync(async () =>
        {
            proofOfOriginToken = await generator.CreateProofOfOriginTokenAsync(visitorData);
        });

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(proofOfOriginToken, Is.Not.Null.Or.Empty);
        });

        // Output
        TestContext.Out.WriteLine("Proof Of Origin Token: {0}", proofOfOriginToken);
    }
}