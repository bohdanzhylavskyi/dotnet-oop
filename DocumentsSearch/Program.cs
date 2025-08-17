using DocumentsSearch.Documents;
using DocumentsSearch.DocumentsStores;
using DocumentsSearch.DocumentStores;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace DocumentsSearch
{
    internal class Program
    {
        static string DocumentsFolderPath = "./documents-store";

        static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddDebug()
                    .SetMinimumLevel(LogLevel.Information);
            });



            var docTypesRegistry = new DocumentTypesRegistry();

            docTypesRegistry.Register(DocumentType.Book, typeof(Book));
            docTypesRegistry.Register(DocumentType.Patent, typeof(Patent));
            docTypesRegistry.Register(DocumentType.LocalizedBook, typeof(LocalizedBook));
            docTypesRegistry.Register(DocumentType.Magazine, typeof(Magazine));

            var documentDeserializer = new DocumentDeserializer(docTypesRegistry);

            var fsDocumentsStore = new FSDocumentsStore(documentDeserializer, DocumentsFolderPath);
            var documentsCache = new DocumentsCache(
                new DocumentsCacheConfiguration()
                {
                    config = new Dictionary<DocumentType, DocumentTypeCacheConfiguration>()
                    {
                        { DocumentType.Book, new DocumentTypeCacheConfiguration() { ExpirationInMs = 60000 } }
                    }
                },
                loggerFactory
            );

            var cachedFsDocumentsStore = new CachedDocumentsStore(fsDocumentsStore, documentsCache); // TODO check

            var documentsService = new DocumentsService(cachedFsDocumentsStore);

            while (true)
            {
                var input = Console.ReadLine();

                if (input == "exit")
                {
                    break;
                }

                var documentNumber = int.Parse(input);
                var searchResult = documentsService.SearchDocuments(documentNumber);

                PrintSearchResult(searchResult);
            }
        }

        private static void PrintSearchResult(List<DocumentCardInfo> infos)
        {
            Console.WriteLine("\nSearch Results:\n");

            foreach (var info in infos)
            {
                foreach (var property in info.GetProperties())
                {
                    Console.WriteLine($"{property.Name}: {property.Value}");
                }

                Console.WriteLine("-------------------------");
            }

        }
    }
}
