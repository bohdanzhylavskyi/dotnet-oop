using DocumentsSearch.Documents;
using Microsoft.Extensions.Logging;

namespace DocumentsSearch
{
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
        private Dictionary<string, DocumentsCacheRecord> cache = new();
        private DocumentsCacheConfiguration cacheConfiguration;
        private ILogger<DocumentsCache> logger;

        public DocumentsCache(DocumentsCacheConfiguration cacheConfiguration, ILoggerFactory loggerFactory)
        {
            this.cacheConfiguration = cacheConfiguration;
            this.logger = loggerFactory.CreateLogger<DocumentsCache>();
        }

        public void Cache(DocumentType type, int documentNumber, Document document)
        {
            this.logger.LogInformation($"Request to cache document with type={type} and documentNumber={documentNumber}");

            if (!this.isCachingForTypeEnabled(type))
            {
                this.logger.LogInformation($"Caching for documentType={type} is disabled, document with type={type} and documentNumber={documentNumber} is not added to cache as cache for such document type is not enabled");

                return;
            }

            var cacheKey = this.buildDocumentKey(type, documentNumber);
            var cacheRecord = new DocumentsCacheRecord()
            {
                Document = document,
                CachedAt = DateTime.Now,
            };


            this.cache[cacheKey] = cacheRecord;

            this.logger.LogInformation($"Document with type={type} and documentNumber={documentNumber} is cached, cachedAt={cacheRecord.CachedAt}, cacheKey={cacheKey}");
        }

        public Document? TryGet(DocumentType type, int documentNumber)
        {
            var cacheKey = this.buildDocumentKey(type, documentNumber);

            this.logger.LogInformation($"Request to retrieve document with type={type} and documentNumber={documentNumber} from cache using cacheKey={cacheKey}");

            this.cache.TryGetValue(cacheKey, out DocumentsCacheRecord cacheRecord);

            if (cacheRecord == null)
            {
                this.logger.LogInformation($"Document with type={type} and documentNumber={documentNumber} was not found in cache");

                return null;
            }

            if (this.isCacheExpired(type, cacheRecord.CachedAt))
            {
                this.logger.LogInformation($"Document with type={type} and documentNumber={documentNumber} was found in cache, but cache is expired");

                return null;
            }

            var cacheExpiration = this.getCacheExpirationInMs(type);

            if (cacheExpiration == null)
            {
                this.logger.LogInformation($"Document with type={type} and documentNumber={documentNumber} was retrieved from live-long cache");
            }
            else
            {
                var expireInMs = (cacheExpiration - (DateTime.Now - cacheRecord.CachedAt).TotalMilliseconds);
                this.logger.LogInformation($"Document with type={type} and documentNumber={documentNumber} was retrieved from cache. Cache will expire in {expireInMs} miliseconds");
            }

            return cacheRecord.Document;
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

        private int? getCacheExpirationInMs(DocumentType type)
        {
            return this.cacheConfiguration.config[type].ExpirationInMs;
        }

        private string buildDocumentKey(DocumentType type, int documentNumber)
        {
            return $"{type}_{documentNumber}";
        }
    }
}
