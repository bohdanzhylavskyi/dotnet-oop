using DocumentsSearch.Documents;
using DocumentsSearch.DocumentsStorages;
using DocumentsSearch.DocumentStorages;
using DocumentsSearch.UIs;
using Microsoft.Extensions.Logging;

namespace DocumentsSearch
{
    internal class Program
    {
        static string DocumentsFolderPath = "./DOCUMENTS_STORAGE";

        static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddDebug()
                    .SetMinimumLevel(LogLevel.Debug);
            });

            var documentTypesRegistry = new DocumentTypesRegistry();

            RegisterSupportedDocumentTypes(documentTypesRegistry);

            var documentsStorage = ConfigureDocumentsStorage(documentTypesRegistry, loggerFactory);
            var documentsService = new DocumentsService(documentsStorage);

            var ui = new ConsoleUI(documentsService);

            ui.Bootstrap();
        }

        static private void RegisterSupportedDocumentTypes(DocumentTypesRegistry documentTypesRegistry)
        {
            documentTypesRegistry.Register(DocumentType.Patent, typeof(Patent));
            documentTypesRegistry.Register(DocumentType.Book, typeof(Book));
            documentTypesRegistry.Register(DocumentType.LocalizedBook, typeof(LocalizedBook));
            documentTypesRegistry.Register(DocumentType.Magazine, typeof(Magazine));
        }

        static private IDocumentsStorage ConfigureDocumentsStorage(DocumentTypesRegistry documentTypesRegistry, ILoggerFactory loggerFactory)
        {
            var documentDeserializer = new DocumentDeserializer(documentTypesRegistry);

            var fsDocumentsStorage = new FsDocumentsStorage(documentDeserializer, DocumentsFolderPath, loggerFactory);

            var documentsCacheConfiguration = new DocumentsCacheConfiguration(
                new Dictionary<DocumentType, DocumentTypeCacheConfiguration>()
                {
                    { DocumentType.Patent, new DocumentTypeCacheConfiguration() { ExpirationInMs = null } },
                    { DocumentType.Book, new DocumentTypeCacheConfiguration() { ExpirationInMs = 30000 } }
                }
            );

            var documentsCache = new DocumentsCache(
                documentsCacheConfiguration,
                loggerFactory
            );

            return new CachedDocumentsStorage(fsDocumentsStorage, documentsCache);
        }
    }
}
