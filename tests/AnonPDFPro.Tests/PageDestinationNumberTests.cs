using AnonPDF;
using NUnit.Framework;

namespace AnonPDFPro.Tests;

[TestFixture]
public class PageDestinationNumberTests
{
    [TestCase("1", 800, 1)]
    [TestCase("800", 800, 800)]
    [TestCase("5", 5, 5)]
    public void AcceptsOnlyPageNumbersWithinTheCurrentDocument(string text, int count, int expected)
    {
        Assert.That(PageDestinationNumber.TryParse(text, count, out int number), Is.True);
        Assert.That(number, Is.EqualTo(expected));
    }

    [TestCase("801", 800)]
    [TestCase("999999999999999999999999999", 800)]
    [TestCase("0", 800)]
    [TestCase("-1", 800)]
    [TestCase("1.5", 800)]
    [TestCase("1,5", 800)]
    [TestCase("1e2", 800)]
    [TestCase(" 10 ", 800)]
    [TestCase("", 800)]
    [TestCase(null, 800)]
    [TestCase("abc", 800)]
    public void RejectsOutOfRangeOrNonIntegerTextIncludingPastedValues(string text, int count)
    {
        Assert.That(PageDestinationNumber.TryParse(text, count, out _), Is.False);
    }

    [Test]
    public void UpperBoundIncludesAddedBlankAndImportedPagesAndShrinksAfterRemoval()
    {
        var plan = PageOrganizationPlan.Open(PageOrganizationTests.Document("A", 3));
        Assert.That(PageDestinationNumber.TryParse("4", plan.Pages.Count, out _), Is.False);
        plan.AddBlank(3, 595, 842);
        Assert.That(PageDestinationNumber.TryParse("4", plan.Pages.Count, out _), Is.True);
        plan.AddPdf(PageOrganizationTests.Document("B", 1), "", new[] { 1 }, 4);
        Assert.That(PageDestinationNumber.TryParse("5", plan.Pages.Count, out _), Is.True);
        Assert.That(PageDestinationNumber.TryParse("6", plan.Pages.Count, out _), Is.False);
        Assert.That(plan.RemoveOrRestore(new[] { 4 }), Is.True);
        Assert.That(PageDestinationNumber.TryParse("5", plan.Pages.Count, out _), Is.False);
        Assert.That(PageDestinationNumber.TryParse("4", plan.Pages.Count, out _), Is.True);
    }
}
