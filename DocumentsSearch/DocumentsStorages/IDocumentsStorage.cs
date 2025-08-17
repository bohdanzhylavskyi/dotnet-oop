using DocumentsSearch.Documents;

namespace DocumentsSearch.DocumentStorages
{
    public class DocumentRecord
    {
        public required DocumentType DocumentType { get; init; }
        public required int DocumentNumber { get; init; }
    }

    public interface IDocumentsStorage
    {
        public List<DocumentRecord> ListDocumentRecords();

        public Document ReadDocument(DocumentType type, int number);
    }
}
