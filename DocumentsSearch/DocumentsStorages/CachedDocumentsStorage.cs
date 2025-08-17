using DocumentsSearch.Documents;
using DocumentsSearch.DocumentStorages;

namespace DocumentsSearch.DocumentsStorages
{
    public class CachedDocumentsStorage : IDocumentsStorage
    {
        private IDocumentsStorage documentsStore;
        private DocumentsCache documentsCache;

        public CachedDocumentsStorage(IDocumentsStorage documentsStore, DocumentsCache documentsCache)
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
