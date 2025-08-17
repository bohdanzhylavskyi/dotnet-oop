using DocumentsSearch.Documents;
using System.IO;

namespace DocumentsSearch.DocumentStores
{
    public class FSDocumentsStore : IDocumentsStore
    {
        private DocumentDeserializer documentDeserializer;
        private string targetFolderPath;

        public FSDocumentsStore(DocumentDeserializer documentDeserializer, string targetFolderPath)
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
                documentRecords.Add(this.parseDocumentFilename(file));
            }

            return documentRecords;

        }

        public Document ReadDocument(DocumentType type, int number)
        {
            var filename = $"{type.ToString().ToLower()}_#{number}.json";

            string json = File.ReadAllText(Path.Combine(this.targetFolderPath, filename));

            return this.documentDeserializer.DeserializeFromJson(type, json);
        }

        private DocumentRecord parseDocumentFilename(string filename)
        {
            string nameOnly = Path.GetFileNameWithoutExtension(filename);
            string[] parts = nameOnly.Split("_#");

            string type = parts[0];
            int number = int.Parse(parts[1]);

            return new DocumentRecord()
            {
                DocumentType = Enum.Parse<DocumentType>(type, ignoreCase: true),
                DocumentNumber = number
            };
        }
    }
}
