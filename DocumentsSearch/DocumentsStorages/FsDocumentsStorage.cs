using DocumentsSearch.Documents;
using Microsoft.Extensions.Logging;

namespace DocumentsSearch.DocumentStorages
{
    public class FsDocumentsStorage : IDocumentsStorage
    {
        private const string DocumentTypeNumberDelimiter = "_#";
        private DocumentDeserializer documentDeserializer;
        private string targetFolderPath;
        private ILogger<FsDocumentsStorage> logger;

        public FsDocumentsStorage(
            DocumentDeserializer documentDeserializer,
            string targetFolderPath,
            ILoggerFactory loggerFactory)
        {
            this.documentDeserializer = documentDeserializer;
            this.targetFolderPath = targetFolderPath;
            this.logger = loggerFactory.CreateLogger<FsDocumentsStorage>();
        }

        public List<DocumentRecord> ListDocumentRecords()
        {
            string[] files = Directory.GetFiles(this.targetFolderPath);
            List<DocumentRecord> documentRecords = new();

            foreach (var filename in files)
            {
                var parsedDocumentRecord = this.TryParseDocumentDocumentRecord(filename);

                if (parsedDocumentRecord != null)
                {
                    documentRecords.Add(parsedDocumentRecord);
                } else {
                    this.logger.LogWarning($"Skip file with filename={filename} as it can't be parsed to valid document record");
                }
            }

            return documentRecords;

        }

        public Document ReadDocument(DocumentType type, int documentNumber)
        {
            var filename = BuildDocumentRecordFilename(type, documentNumber);
            var filePath = Path.Combine(this.targetFolderPath, filename);

            string json = File.ReadAllText(filePath);

            return this.documentDeserializer.DeserializeFromJson(type, json);
        }

        private DocumentRecord? TryParseDocumentDocumentRecord(string filename)
        {
            try
            {
                string nameOnly = Path.GetFileNameWithoutExtension(filename);
                string[] parts = nameOnly.Split(DocumentTypeNumberDelimiter);

                string type = parts[0];
                int number = int.Parse(parts[1]);

                return new DocumentRecord()
                {
                    DocumentType = Enum.Parse<DocumentType>(type, ignoreCase: true),
                    DocumentNumber = number
                };
            } catch (Exception e)
            {
                this.logger.LogWarning(e, $"Failed to parse document record from file with filename={filename}");

                return null;
            }
        }

        private string BuildDocumentRecordFilename(DocumentType type, int documentNumber)
        {
            return $"{type.ToString().ToLower()}{DocumentTypeNumberDelimiter}{documentNumber}.json";
        }
    }
}
