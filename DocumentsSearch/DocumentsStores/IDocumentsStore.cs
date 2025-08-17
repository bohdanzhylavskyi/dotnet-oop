using DocumentsSearch.Documents;

namespace DocumentsSearch.DocumentStores
{
    public class DocumentRecord
    {
        public required DocumentType DocumentType { get; init; }
        public required int DocumentNumber { get; init; }
    }

    public interface IDocumentsStore
    {
        public List<DocumentRecord> ListDocumentRecords();

        public Document ReadDocument(DocumentType type, int number);
    }
}
