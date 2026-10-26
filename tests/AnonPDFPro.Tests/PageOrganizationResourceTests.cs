using System.Globalization;
using AnonPDF;
using AnonPDF.Properties;
using NUnit.Framework;

namespace AnonPDFPro.Tests;

[TestFixture, NonParallelizable]
public class PageOrganizationResourceTests
{
    [TestCase(96)]
    [TestCase(120)]
    [TestCase(144)]
    [TestCase(168)]
    [TestCase(192)]
    public void ThumbnailMetricsScaleProportionallyAcrossScreenScales(int dpi)
    {
        int fontHeight = (int)System.Math.Ceiling(15 * dpi / 96f);
        var metrics = PageThumbnailMetrics.Calculate(dpi, fontHeight);
        Assert.That(metrics.ItemHeight, Is.EqualTo((int)System.Math.Round(220 * dpi / 96f)));
        Assert.That(metrics.ImageSize.Height, Is.GreaterThan(0).And.LessThan(metrics.ItemHeight));
        Assert.That(metrics.ImageSize.Width, Is.GreaterThan(0).And.LessThan(metrics.ColumnWidth));
        Assert.That(metrics.ItemHeight - metrics.ImageSize.Height, Is.GreaterThanOrEqualTo(fontHeight * 2));
    }

    [TestCase("pl")]
    [TestCase("en")]
    [TestCase("de")]
    public void NewInterfaceTextIsLocalized(string culture)
    {
        var previous = Resources.Culture;
        try
        {
            Resources.Culture = CultureInfo.GetCultureInfo(culture);
            foreach (string key in new[] { "Pages_Title", "Pages_SelectPages", "Pages_AddPdf", "Pages_Before", "Pages_After", "Pages_MoveEarlier",
                "Pages_MoveLater", "Pages_Remove", "Pages_Restore", "Pages_DeletionMarked", "Pages_SelectAll", "Pages_SelectNone", "Pages_Insert", "Pages_Apply", "Pages_Blank",
                "Pages_Original", "Pages_Count", "Pages_Applying", "Pages_Error", "Pages_CurrentFormat", "Pages_CustomFormat", "Pages_Portrait",
                "Pages_Landscape", "Pages_Format", "Pages_Orientation", "Pages_Width", "Pages_Height", "Pages_ImportMismatch", "Pages_SignatureWarning" })
                Assert.That(PageOrganizationText.Get(key), Is.Not.EqualTo(key).And.Not.Empty);
        }
        finally { Resources.Culture = previous; }
    }
}
