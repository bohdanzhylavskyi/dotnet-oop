using DocumentsSearch.Documents;

namespace DocumentsSearch.DocumentStorages
{
    public class FsDocumentsStorage : IDocumentsStorage
    {
        private const string DocumentTypeNumberDelimiter = "_#";
        private DocumentDeserializer documentDeserializer;
        private string targetFolderPath;

        public FsDocumentsStorage(DocumentDeserializer documentDeserializer, string targetFolderPath)
        {
            this.documentDeserializer = documentDeserializer;
            this.targetFolderPath = targetFolderPath;
        }

        public List<DocumentRecord> ListDocumentRecords()
        {
            string[] files = Directory.GetFiles(this.targetFolderPath);
            List<DocumentRecord> documentRecords = new();

            foreach (var file in files)
            {
                documentRecords.Add(this.ParseDocumentFilename(file));
            }

            return documentRecords;

        }

        public Document ReadDocument(DocumentType type, int documentNumber)
        {
            var filename = BuildDocumentRecordFilename(type, documentNumber);

            string json = File.ReadAllText(Path.Combine(this.targetFolderPath, filename));

            return this.documentDeserializer.DeserializeFromJson(type, json);
        }

        private DocumentRecord ParseDocumentFilename(string filename)
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
        }

        private string BuildDocumentRecordFilename(DocumentType type, int documentNumber)
        {
            return $"{type.ToString().ToLower()}{DocumentTypeNumberDelimiter}{documentNumber}.json";
        }
    }
}
