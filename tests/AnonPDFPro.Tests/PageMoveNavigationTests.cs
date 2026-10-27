using System;
using System.Drawing;
using AnonPDF;
using NUnit.Framework;

namespace AnonPDFPro.Tests;

[TestFixture]
public class PageMoveNavigationTests
{
    private static void AssertLocation(PageThumbnailLocation location, int page, float offset)
    {
        Assert.That(location.PageIndex, Is.EqualTo(page));
        Assert.That(location.RowOffset, Is.EqualTo(offset).Within(0.001f));
    }

    [Test]
    public void NavigationIsDisabledUntilTwoDifferentLocationsExist()
    {
        var navigation = new PageMoveNavigation();
        Assert.That(navigation.CanToggle, Is.False);
        Assert.Throws<InvalidOperationException>(() => navigation.Toggle());
        navigation.Record(new PageThumbnailLocation(10, 0), new PageThumbnailLocation(10, 0));
        Assert.That(navigation.CanToggle, Is.False);
    }

    [Test]
    public void RepeatedSwitchesAlternateBetweenSourceAndDestination()
    {
        var navigation = new PageMoveNavigation();
        navigation.Record(new PageThumbnailLocation(5, 0.25f), new PageThumbnailLocation(790, 0.5f));
        Assert.That(navigation.AtDestination, Is.True);
        for (int i = 0; i < 50; i++)
        {
            AssertLocation(navigation.Toggle(), 5, 0.25f);
            Assert.That(navigation.AtDestination, Is.False);
            AssertLocation(navigation.Toggle(), 790, 0.5f);
            Assert.That(navigation.AtDestination, Is.True);
        }
    }

    [Test]
    public void AnotherMoveReplacesThePreviousPairRatherThanBuildingAnUnboundedHistory()
    {
        var navigation = new PageMoveNavigation();
        navigation.Record(new PageThumbnailLocation(5, 0), new PageThumbnailLocation(790, 0));
        navigation.Toggle();
        navigation.Record(new PageThumbnailLocation(50, 0.25f), new PageThumbnailLocation(300, 0.5f));
        AssertLocation(navigation.Toggle(), 50, 0.25f);
        AssertLocation(navigation.Toggle(), 300, 0.5f);
    }

    [Test]
    public void MovingWithinTheSameViewDoesNotDiscardTheLastUsefulPair()
    {
        var navigation = new PageMoveNavigation();
        navigation.Record(new PageThumbnailLocation(5, 0), new PageThumbnailLocation(790, 0));
        navigation.Record(new PageThumbnailLocation(790, 0), new PageThumbnailLocation(790, 0));
        Assert.That(navigation.CanToggle, Is.True);
        AssertLocation(navigation.Toggle(), 5, 0);
        AssertLocation(navigation.Toggle(), 790, 0);
    }

    [TestCase(96)]
    [TestCase(120)]
    [TestCase(144)]
    [TestCase(168)]
    [TestCase(192)]
    public void LocationsRoundTripWithoutPixelDriftAtEverySupportedScale(int dpi)
    {
        var metrics = PageThumbnailMetrics.Calculate(dpi, (int)Math.Ceiling(15 * dpi / 96f));
        var layout = new PageThumbnailLayout(800, 900, new Size(metrics.ColumnWidth, metrics.ItemHeight));
        int scroll = metrics.ItemHeight * 15 + metrics.ItemHeight / 3;
        var location = layout.CaptureLocation(scroll);
        Assert.That(layout.ScrollForLocation(location, 580), Is.EqualTo(scroll));
    }

    [Test]
    public void RestoringAfterDpiOrColumnChangesKeepsThePageLocationAndRelativeRowOffset()
    {
        var original = new PageThumbnailLayout(800, 900, new Size(180, 220));
        var location = original.CaptureLocation(15 * 220 + 110);
        AssertLocation(location, 75, 0.5f);
        var resized = new PageThumbnailLayout(800, 700, new Size(360, 440));
        Assert.That(resized.Columns, Is.EqualTo(1));
        Assert.That(resized.ScrollForLocation(location, 580), Is.EqualTo(75 * 440 + 220));
    }

    [Test]
    public void RemovingPagesClampsOldLocationsToTheRemainingDocument()
    {
        var layout = new PageThumbnailLayout(10, 900, new Size(180, 220));
        Assert.That(layout.ScrollForLocation(new PageThumbnailLocation(799, 0.5f), 300), Is.EqualTo(140));
        var empty = new PageThumbnailLayout(0, 900, new Size(180, 220));
        Assert.That(empty.ScrollForLocation(new PageThumbnailLocation(799, 0.5f), 300), Is.Zero);
    }
}
