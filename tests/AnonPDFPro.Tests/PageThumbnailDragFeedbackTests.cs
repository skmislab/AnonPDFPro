using System.Drawing;
using AnonPDF;
using NUnit.Framework;

namespace AnonPDFPro.Tests;

[TestFixture]
public class PageThumbnailDragFeedbackTests
{
    [Test]
    public void RepeatedMovementOverTheSameGapDoesNotRequestRepainting()
    {
        var feedback = new PageThumbnailDragFeedback();
        var marker = new Rectangle(180, 220, 180, 2);
        Assert.That(feedback.MoveMarker(3, marker), Is.EqualTo(new[] { marker }));
        for (int i = 0; i < 100; i++)
            Assert.That(feedback.MoveMarker(3, marker), Is.Empty);
        Assert.That(feedback.Gap, Is.EqualTo(3));
    }

    [Test]
    public void MovingOrClearingTheMarkerOnlyInvalidatesItsOldAndNewStrips()
    {
        var feedback = new PageThumbnailDragFeedback();
        var previous = new Rectangle(180, 220, 180, 2);
        var next = new Rectangle(360, 0, 180, 2);
        feedback.MoveMarker(3, previous);
        Assert.That(feedback.MoveMarker(4, next), Is.EqualTo(new[] { previous, next }));
        Assert.That(feedback.MoveMarker(-1, Rectangle.Empty), Is.EqualTo(new[] { next }));
        Assert.That(feedback.Gap, Is.EqualTo(-1));
        Assert.That(feedback.MoveMarker(-1, Rectangle.Empty), Is.Empty);
    }

    [Test]
    public void ScrollingWithTheSameGapStillInvalidatesChangedMarkerCoordinates()
    {
        var feedback = new PageThumbnailDragFeedback();
        var previous = new Rectangle(180, 220, 180, 2);
        var next = new Rectangle(0, 220, 180, 2);
        feedback.MoveMarker(3, previous);
        Assert.That(feedback.MoveMarker(3, next), Is.EqualTo(new[] { previous, next }));
    }

    [TestCase(96)]
    [TestCase(120)]
    [TestCase(144)]
    [TestCase(168)]
    [TestCase(192)]
    public void InsertionMarkStaysInsideItsItemAcrossScreenScales(int dpi)
    {
        var metrics = PageThumbnailMetrics.Calculate(dpi, (int)System.Math.Ceiling(15 * dpi / 96f));
        var item = new Rectangle(metrics.ColumnWidth, metrics.ItemHeight, metrics.ColumnWidth, metrics.ItemHeight);
        var before = PageThumbnailDragFeedback.GetMarkerBounds(item, false, dpi);
        var after = PageThumbnailDragFeedback.GetMarkerBounds(item, true, dpi);
        Assert.That(item.Contains(before), Is.True);
        Assert.That(item.Contains(after), Is.True);
        Assert.That(before.Left, Is.EqualTo(item.Left));
        Assert.That(after.Right, Is.EqualTo(item.Right));
        Assert.That(before.Width, Is.InRange(2, 4));
        Assert.That(before.Height, Is.GreaterThan(before.Width));
        Assert.That(after.Size, Is.EqualTo(before.Size));
    }

    [Test]
    public void EdgeScrollingIsThrottledRatherThanRepeatedForEveryMouseEvent()
    {
        var feedback = new PageThumbnailDragFeedback();
        Assert.That(feedback.ScrollDirection(790, 800, 24, 0), Is.EqualTo(1));
        for (int elapsed = 1; elapsed < 180; elapsed++)
            Assert.That(feedback.ScrollDirection(790, 800, 24, elapsed), Is.Zero);
        Assert.That(feedback.ScrollDirection(790, 800, 24, 180), Is.EqualTo(1));
        Assert.That(feedback.ScrollDirection(10, 800, 24, 360), Is.EqualTo(-1));
        Assert.That(feedback.ScrollDirection(400, 800, 24, 361), Is.Zero);
        Assert.That(feedback.ScrollDirection(10, 800, 24, 362), Is.EqualTo(-1));
    }

    [Test]
    public void NewDragResetsThePreviousScrollDelay()
    {
        var feedback = new PageThumbnailDragFeedback();
        Assert.That(feedback.ScrollDirection(790, 800, 24, 5000), Is.EqualTo(1));
        feedback.ResetScroll();
        Assert.That(feedback.ScrollDirection(10, 800, 24, 0), Is.EqualTo(-1));
    }
}
