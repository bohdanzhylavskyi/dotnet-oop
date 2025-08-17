using DocumentsSearch.Documents;
using DocumentsSearch.DocumentStores;

namespace DocumentsSearch
{
    public class DocumentsService
    {
        private IDocumentsStore documentsStore;

        public DocumentsService(IDocumentsStore documentsStore)
        {
            this.documentsStore = documentsStore;
        }

        public List<DocumentCardInfo> SearchDocuments(int documentNumber)
        {
            var records = this.documentsStore.ListDocumentRecords();
            var matchedRecords = records.FindAll(r => r.DocumentNumber.ToString().Contains(documentNumber.ToString()));

            var documents = new List<Document>();

            foreach (var record in matchedRecords)
            {
                var document = this.documentsStore.ReadDocument(record.DocumentType, record.DocumentNumber);

                documents.Add(document);
            }

            return documents.Select(d => d.GetInfo()).ToList();
        }
    }
}
