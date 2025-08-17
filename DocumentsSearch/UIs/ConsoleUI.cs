using DocumentsSearch.Documents;

namespace DocumentsSearch.UIs
{
    public class ConsoleUI
    {
        private DocumentsService documentsService;
        public ConsoleUI(DocumentsService documentsService)
        {
            this.documentsService = documentsService;
        }
        public void Bootstrap()
        {
            while (true)
            {
                Console.WriteLine("Enter document number query (or 'exit' to close the app):");

                var input = Console.ReadLine();

                if (input == "exit")
                {
                    break;
                }

                if (!this.TryParseDocumentNumberQuery(input, out int documentNumber))
                {
                    Console.WriteLine("\nInvalid search query was provided, document number is expected\n");

                    continue;
                }

                var searchResult = this.documentsService.SearchDocuments(documentNumber);

                PrintSearchResult(searchResult);
            }
        }

        private bool TryParseDocumentNumberQuery(string searchQuery, out int documentNumber)
        {
            return int.TryParse(searchQuery, out documentNumber);
        }

        private static void PrintSearchResult(List<DocumentCardInfo> infos)
        {
            if (infos.Count == 0)
            {
                Console.WriteLine("\nNo matches found for your query\n");

                return;
            }


            Console.WriteLine("\nSearch Results:\n");

            foreach (var info in infos)
            {
                foreach (var property in info.GetProperties())
                {
                    Console.WriteLine($"  {property.Name}: {property.Value}");
                }

                Console.WriteLine("\n-------------------------\n");
            }

        }
    }
}
