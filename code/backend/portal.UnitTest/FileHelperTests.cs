using portal.IO.Common;
using System.IO;
using Xunit;

namespace portal.UnitTest
{
    public class FileHelperTests
    {
        [Theory]
        [InlineData("assets/samplepdf")]
        public void VerifyFormat_PDF_ForPDFFile(string path)
        {
            bool isPDF = false;

            using (var fs = File.Open(path, FileMode.Open))
            {
                isPDF = FileHelper.VerifyFormat.PDF(fs);
            }
            Assert.True(isPDF);
        }

        [Theory]
        [InlineData("assets/sampleimage")]
        [InlineData("assets/sampledocx")]
        [InlineData("assets/sampletext")]
        public void VerifyFormat_PDF_ForOtherFile(string path)
        {
            bool isPDF = false;

            using (var fs = File.Open(path, FileMode.Open))
            {
                isPDF = FileHelper.VerifyFormat.PDF(fs);
            }
            Assert.False(isPDF);
        }
    }
}