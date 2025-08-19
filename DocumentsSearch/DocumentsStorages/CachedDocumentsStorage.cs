using DocumentsSearch.Documents;
using DocumentsSearch.DocumentStorages;

namespace DocumentsSearch.DocumentsStorages
{
    public class CachedDocumentsStorage : IDocumentsStorage
    {
        private IDocumentsStorage documentsStorage;
        private DocumentsCache documentsCache;

        public CachedDocumentsStorage(IDocumentsStorage documentsStorage, DocumentsCache documentsCache)
        {
            this.documentsStorage = documentsStorage;
            this.documentsCache = documentsCache;
        }

        public List<DocumentRecord> ListDocumentRecords()
        {
            return this.documentsStorage.ListDocumentRecords();
        }

        public Document ReadDocument(DocumentType type, int documentNumber)
        {
            var cachedDocument = this.documentsCache.TryGet(type, documentNumber);

            if (cachedDocument != null)
            {
                return cachedDocument;
            }

            var document = this.documentsStorage.ReadDocument(type, documentNumber);

            this.documentsCache.Cache(type, documentNumber, document);

            return document;
        }
    }
}
