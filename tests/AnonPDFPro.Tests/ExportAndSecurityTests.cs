using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;
using NUnit.Framework;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.X509;

namespace AnonPDFPro.Tests;

[TestFixture, NonParallelizable]
public class ExportAndSecurityTests
{
    [Test]
    public void HtmlFormattedObjectRemainsReadableAndRenderable()
    {
        using var output = new MemoryStream();
        iText.Html2pdf.HtmlConverter.ConvertToPdf("<html><body><p><b>HEADER 12345</b></p><p>Second line</p></body></html>", output);
        var pdf = output.ToArray();
        using var document = new PdfDocument(new PdfReader(new MemoryStream(pdf)));
        var text = PdfTextExtractor.GetTextFromPage(document.GetPage(1));
        Assert.That(text, Does.Contain("HEADER 12345").And.Contain("Second line"));
        using var rendered = PdfRegressionTests.Render(pdf);
        var dark = 0;
        for (var y = 0; y < 150; y++)
            for (var x = 0; x < rendered.Width; x++)
                if (rendered.GetPixel(x, y).R < 100) dark++;
        Assert.That(dark, Is.GreaterThan(100));
    }

    [Test]
    public void PdfAExportEmbedsFontAndRetainsOneSearchableTextLayer()
    {
        var windows = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        using var profile = File.OpenRead(Path.Combine(windows, "System32", "spool", "drivers", "color", "sRGB Color Space Profile.icm"));
        using var output = new MemoryStream();
        using (var document = new iText.Pdfa.PdfADocument(new PdfWriter(output), PdfAConformance.PDF_A_2B,
            new PdfOutputIntent("sRGB IEC61966-2.1", "", "http://www.color.org", "sRGB IEC61966-2.1", profile)))
        {
            var font = PdfFontFactory.CreateFont(Path.Combine(windows, "Fonts", "arial.ttf"), PdfEncodings.IDENTITY_H,
                PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
            new PdfCanvas(document.AddNewPage()).BeginText().SetFontAndSize(font, 16)
                .MoveText(50, 700).ShowText("Ziela\u0144ski \u017b\u00f3\u0142\u0107").EndText();
        }
        using var result = new PdfDocument(new PdfReader(new MemoryStream(output.ToArray())));
        Assert.That(PdfTextExtractor.GetTextFromPage(result.GetPage(1)).Trim(), Is.EqualTo("Ziela\u0144ski \u017b\u00f3\u0142\u0107"));
        Assert.That(result.GetCatalog().GetPdfObject().GetAsArray(PdfName.OutputIntents).Size(), Is.EqualTo(1));
        var embedded = false;
        for (var i = 1; i < result.GetNumberOfPdfObjects(); i++)
            if (result.GetPdfObject(i) is PdfDictionary dictionary && dictionary.ContainsKey(PdfName.FontFile2)) embedded = true;
        Assert.That(embedded, Is.True);
        Assert.That(result.GetXmpMetadata().GetPropertyInteger("http://www.aiim.org/pdfa/ns/id/", "part"), Is.EqualTo(2));
    }

    [Test]
    public void CertificateVerificationRejectsATamperedSignature()
    {
        var random = new SecureRandom();
        var generator = new RsaKeyPairGenerator();
        generator.Init(new RsaKeyGenerationParameters(Org.BouncyCastle.Math.BigInteger.ValueOf(65537), random, 2048, 80));
        var keys = generator.GenerateKeyPair();
        var certificate = new X509V3CertificateGenerator();
        certificate.SetSerialNumber(Org.BouncyCastle.Math.BigInteger.One);
        certificate.SetIssuerDN(new X509Name("CN=AnonPDFPro synthetic test"));
        certificate.SetSubjectDN(new X509Name("CN=AnonPDFPro synthetic test"));
        certificate.SetNotBefore(DateTime.UtcNow.AddDays(-1));
        certificate.SetNotAfter(DateTime.UtcNow.AddDays(1));
        certificate.SetPublicKey(keys.Public);
        var bytes = certificate.Generate(new Asn1SignatureFactory("SHA256WITHRSA", keys.Private, random)).GetEncoded();
        var parsed = new X509CertificateParser().ReadCertificate(bytes);
        Assert.DoesNotThrow(() => parsed.Verify(keys.Public));
        bytes[bytes.Length - 1] ^= 1;
        var tampered = new X509CertificateParser().ReadCertificate(bytes);
        Assert.That(() => tampered.Verify(keys.Public), Throws.Exception);
    }

    [Test]
    public void XmlSignatureVerificationRejectsModifiedContent()
    {
        using var key = RSA.Create();
        var xml = new XmlDocument { PreserveWhitespace = true };
        xml.LoadXml("<document><value>original</value></document>");
        var signature = new SignedXml(xml) { SigningKey = key };
        var reference = new Reference("");
        reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
        signature.AddReference(reference);
        signature.ComputeSignature();
        xml.DocumentElement.AppendChild(xml.ImportNode(signature.GetXml(), true));
        var verifier = new SignedXml(xml);
        verifier.LoadXml((XmlElement)xml.GetElementsByTagName("Signature")[0]);
        Assert.That(verifier.CheckSignature(key), Is.True);
        xml.SelectSingleNode("/document/value").InnerText = "modified";
        var tampered = new SignedXml(xml);
        tampered.LoadXml((XmlElement)xml.GetElementsByTagName("Signature")[0]);
        Assert.That(tampered.CheckSignature(key), Is.False);
    }
}
