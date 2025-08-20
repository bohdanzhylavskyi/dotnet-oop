using DocumentsSearch.Documents;
using DocumentsSearch.DocumentStorages;

namespace DocumentsSearch
{
    public class DocumentsService
    {
        private IDocumentsStorage documentsStore;

        public DocumentsService(IDocumentsStorage documentsStore)
        {
            this.documentsStore = documentsStore;
        }

        public List<DocumentCardInfo> SearchDocuments(DocumentNumberQuery documentNumberQuery)
        {
            var records = this.documentsStore.ListDocumentRecords();
            
            var matchedRecords = records.FindAll(
                r => r.DocumentNumber.ToString()
                    .Contains(documentNumberQuery.ToString())
            );

            var documents = new List<Document>();

            foreach (var record in matchedRecords)
            {
                var document = this.documentsStore.ReadDocument(record.DocumentType, record.DocumentNumber);

                documents.Add(document);
            }

            return documents.ConvertAll(d => d.GetInfo());
        }
    }
}
