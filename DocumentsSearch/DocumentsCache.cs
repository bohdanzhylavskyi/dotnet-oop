using DocumentsSearch.Documents;

namespace DocumentsSearch
{
   //public class DocumentsCacheOptions
   // {
   //     public void AddLiveLongCachingForType(DocumentType type)
   //     {

   //     }

   //     public void AddCachingForType(DocumentType type, int cacheExpirationInMs)
   //     {

   //     }
   // }

    //public class DocumentsCacheBuilder
    //{
    //    private Dictionary<DocumentType, >

    //    public void AddLiveLongCachingForType(DocumentType type)
    //    {
    //    }

    //    public void AddCachingForType(DocumentType type, int cacheExpirationInMs)
    //    {
    //    }

    //    public DocumentsCache Build()
    //    {
    //        return new DocumentsCache();
    //    }
    //}

    public class DocumentTypeCacheConfiguration
    {
        public required int? ExpirationInMs;
    }

    public class DocumentsCacheConfiguration
    {
        public required Dictionary<DocumentType, DocumentTypeCacheConfiguration> config; // TODO: check
    }

    public class DocumentsCacheRecord
    {
        public required Document Document;
        public required DateTime CachedAt;
    }

    public class DocumentsCache
    {
        // type - cache

        private Dictionary<string, DocumentsCacheRecord> cache = new();
        private DocumentsCacheConfiguration cacheConfiguration;


        public DocumentsCache(DocumentsCacheConfiguration cacheConfiguration)
        {
            this.cacheConfiguration = cacheConfiguration;
        }

        public void Cache(DocumentType type, int documentNumber, Document document)
        {
            if (!this.isCachingForTypeEnabled(type))
            {
                return;
            }

            var cacheKey = this.buildDocumentKey(type, documentNumber);

            this.cache[cacheKey] = new DocumentsCacheRecord()
            {
                Document = document,
                CachedAt = DateTime.Now,

            };
        }

        public Document? TryGet(DocumentType type, int documentNumber)
        {
            var cacheKey = this.buildDocumentKey(type, documentNumber);

            this.cache.TryGetValue(cacheKey, out DocumentsCacheRecord cacheRecord);

            if (cacheRecord != null && !this.isCacheExpired(type, cacheRecord.CachedAt))
            {
                return cacheRecord.Document;
            }

            return null;
        }

        private bool isCachingForTypeEnabled(DocumentType type)
        {
            return this.cacheConfiguration.config.ContainsKey(type);
        }

        private bool isCacheExpired(DocumentType type, DateTime cacheCreatedAt)
        {
            var cacheConfig = this.cacheConfiguration.config[type];

            if (cacheConfig.ExpirationInMs == null)
            {
                return false; // TODO: check
            }

            return (DateTime.Now - cacheCreatedAt).TotalMilliseconds > cacheConfig.ExpirationInMs;
        }

        private string buildDocumentKey(DocumentType type, int documentNumber)
        {
            return $"{type}_{documentNumber}";
        }
    }
}
