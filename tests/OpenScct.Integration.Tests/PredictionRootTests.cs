using OpenScct.Net;
using OpenScct.Server;
using Xunit;

namespace OpenScct.Integration.Tests;

public sealed class PredictionRootTests
{
    [Fact]
    public void FenceRootHandoffKeepsTheCompletedCommandAligned()
    {
        RubberBandRun run = Play("moveset/spy-fence-climb", SimLinkProfile.Ping200);
        Assert.True(run.LargestCorrection() <= 32f, run.Describe());
        Assert.Empty(run.Reinits);
        Assert.True(run.FinalError < 2f, run.Describe());
    }

    [Theory]
    [InlineData("150")]
    [InlineData("200")]
    public void WallJumpHandsRootMotionBackToFalling(string profile)
    {
        RubberBandRun run = Play("moveset/spy-wall-jump", SimLinkProfile.Get(profile));
        Assert.True(run.LargestCorrection() <= 32f, run.Describe());
        Assert.Empty(run.Reinits);
        Assert.True(run.FinalError < 2f, run.Describe());
    }

    [Fact]
    public void LadderEntryDoesNotAccumulateRootCorrections()
    {
        RubberBandRun run = Play("moveset/spy-ladder-down", SimLinkProfile.C);
        Assert.True(run.LargestCorrection() <= 32f, run.Describe());
        Assert.Empty(run.Reinits);
        Assert.True(run.FinalError < 2f, run.Describe());
    }

    private static RubberBandRun Play(string scenario, SimLinkProfile link)
    {
        string map = RubberBandRun.MapOf(scenario);
        Assert.SkipWhen(MapCatalog.CreateDefault().FindFolder(map) is null, $"{map} has not been converted into .content/svmap.");
        return RubberBandRun.Play(scenario, link);
    }
}
