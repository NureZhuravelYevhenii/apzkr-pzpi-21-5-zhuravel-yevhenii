using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ExcelSpreadsheet
{
    public class ExcelSpreadsheetDocument<T> : IDisposable
    {
        private readonly SpreadsheetDocument _document;
        private readonly Stream _documentStream;
        private readonly string _filePath;
        private bool _disposed = false;

        public ExcelSpreadsheetDocument(string file, bool createNew)
        {
            _filePath = file;
            if (!createNew)
            {
                if (!File.Exists(file))
                {
                    throw new FileNotFoundException();
                }

                _documentStream = File.Open(file, FileMode.Open);

                _document = SpreadsheetDocument.Open(_documentStream, true);
            }
            else
            {
                _documentStream = new MemoryStream();

                _document = SpreadsheetDocument.Create(_documentStream, SpreadsheetDocumentType.Workbook);
            }
        }

        ~ExcelSpreadsheetDocument()
        {
            Dispose(true);
        }

        public bool Loading { get; private set; } = false;

        public void Dispose()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _document.Dispose();
                if (!File.Exists(_filePath))
                {
                    using var newFile = File.Create(_filePath);
                    _documentStream.CopyTo(newFile);
                }
                if (disposing)
                {
                    //managed resources
                }
                _disposed = true;
            }
        }

        public Task<IEnumerable<T>> GetEntities()
        {
            Loading = true;

            if (_document.WorkbookPart is null)
            {
                var workbookPart = _document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
            }

            return null;
        }
    }
}
