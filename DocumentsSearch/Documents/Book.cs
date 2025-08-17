namespace DocumentsSearch.Documents
{
    public class Book : Document
    {
        public required string ISBN { get; init; }
        public required string Title { get; init; }
        public required List<string> Authors { get; init; }
        public required int PageCount { get; init; }
        public required string Publisher { get; init; }
        public required DateOnly PublishDate { get; init; }

        public override DocumentCardInfo GetInfo()
        {
            var info = new DocumentCardInfo();

            info.AddProperty("Document Type", "Book");
            info.AddProperty("ISBN", this.ISBN);
            info.AddProperty("Title", this.Title);
            info.AddProperty("Authors", string.Join(",", this.Authors.ToArray()));
            info.AddProperty("Number Of Pages", this.PageCount.ToString());
            info.AddProperty("Publisher", this.Publisher);
            info.AddProperty("Date Published", this.PublishDate.ToString());

            return info;
        }
    }
}
