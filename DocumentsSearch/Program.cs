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

            var docTypesRegistry = new DocumentTypesRegistry();

            RegisterSupportedDocumentTypes(docTypesRegistry);

            var documentsStorage = ConfigureDocumentsStorage(docTypesRegistry, loggerFactory);
            var documentsService = new DocumentsService(documentsStorage);

            var ui = new ConsoleUI(documentsService);

            ui.Bootstrap();
        }

        static private void RegisterSupportedDocumentTypes(DocumentTypesRegistry registry)
        {
            registry.Register(DocumentType.Patent, typeof(Patent));
            registry.Register(DocumentType.Book, typeof(Book));
            registry.Register(DocumentType.LocalizedBook, typeof(LocalizedBook));
            registry.Register(DocumentType.Magazine, typeof(Magazine));
        }

        static private IDocumentsStorage ConfigureDocumentsStorage(DocumentTypesRegistry registry, ILoggerFactory loggerFactory)
        {
            var documentDeserializer = new DocumentDeserializer(registry);

            var fsDocumentsStorage = new FsDocumentsStorage(documentDeserializer, DocumentsFolderPath, loggerFactory);

            var documentsCache = new DocumentsCache(
                new DocumentsCacheConfiguration(
                    new Dictionary<DocumentType, DocumentTypeCacheConfiguration>()
                    {
                        { DocumentType.Patent, new DocumentTypeCacheConfiguration() { ExpirationInMs = null } },
                        { DocumentType.Book, new DocumentTypeCacheConfiguration() { ExpirationInMs = 30000 } }
                    }
                ),
                loggerFactory
            );

            return new CachedDocumentsStorage(fsDocumentsStorage, documentsCache);
        }
    }
}
