namespace DocumentsSearch.Documents
{
    public class Patent : Document
    {
        public required string Id { get; init; }

        public required string Title { get; init; }

        public required List<string> Authors { get; init; }

        public required DateOnly PublishDate { get; init; }
        public required DateOnly ExpirationDate { get; init; }

        public override DocumentCardInfo GetInfo()
        {
            var info = new DocumentCardInfo();

            info.AddProperty("Document Type", "Patent");
            info.AddProperty("Title", this.Title);
            info.AddProperty("Authors", string.Join(",", this.Authors.ToArray()));
            info.AddProperty("Date Published", this.PublishDate.ToString());
            info.AddProperty("Expiration Date", this.ExpirationDate.ToString());
            info.AddProperty("Unique Id", this.Id);

            return info;
        }

    }
}
