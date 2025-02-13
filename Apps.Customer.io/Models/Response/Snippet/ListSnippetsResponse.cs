using Apps.Customer.io.Models.Entity;

namespace Apps.Customer.io.Models.Response.Snippet;

public class ListSnippetsResponse
{
    public IEnumerable<SnippetEntity> Snippets { get; set; } = new List<SnippetEntity>();
}