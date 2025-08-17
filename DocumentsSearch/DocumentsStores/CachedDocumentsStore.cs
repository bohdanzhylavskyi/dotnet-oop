using DocumentsSearch.Documents;
using DocumentsSearch.DocumentStores;

namespace DocumentsSearch.DocumentsStores
{
    public class CachedDocumentsStore : IDocumentsStore
    {
        private IDocumentsStore documentsStore;
        private DocumentsCache documentsCache;

        public CachedDocumentsStore(IDocumentsStore documentsStore, DocumentsCache documentsCache)
        {
            this.documentsStore = documentsStore;
            this.documentsCache = documentsCache;
        }

        public List<DocumentRecord> ListDocumentRecords()
        {
            return this.documentsStore.ListDocumentRecords();
        }

        public Document ReadDocument(DocumentType type, int number)
        {
            var cachedDocument = this.documentsCache.TryGet(type, number);

            if (cachedDocument != null)
            {
                return cachedDocument;
            }

            var document = this.documentsStore.ReadDocument(type, number);

            this.documentsCache.Cache(type, number, document);

            return document;
        }
    }
}
