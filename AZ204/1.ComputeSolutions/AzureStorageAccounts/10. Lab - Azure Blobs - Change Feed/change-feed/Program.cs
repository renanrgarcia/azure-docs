using Azure; 
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.ChangeFeed;

        string accountName = "stdeveus10";
        string accountKey  = "/A1yiSJt28wfR8okTFQd9TVBhFUk9sFqtc+v5TVh2thZVKKJnGxkj+4DAMK+5DnolVZzUmsP8AxQ+AStTxf6Rw==";

        var endpoint   = new Uri($"https://{accountName}.blob.core.windows.net");
        var credential = new StorageSharedKeyCredential(accountName, accountKey);
        var client     = new BlobServiceClient(endpoint, credential);

string? cursor = null;

(cursor, List<BlobChangeFeedEvent> events) =
                await ChangeFeedResumeWithCursorAsync(client, cursor);

            foreach (var e in events)
            {
                Console.WriteLine($"{e.EventType} | {e.Subject} | {e.EventData?.BlobOperationName}");
            }


static async Task<(string? cursor, List<BlobChangeFeedEvent>)> ChangeFeedResumeWithCursorAsync(
        BlobServiceClient client,
        string? cursor)
    {
        BlobChangeFeedClient changeFeedClient = client.GetChangeFeedClient();
        List<BlobChangeFeedEvent> changeFeedEvents = new List<BlobChangeFeedEvent>();

        IAsyncEnumerator<Page<BlobChangeFeedEvent>> enumerator = changeFeedClient
            .GetChangesAsync(continuationToken: cursor)
            .AsPages(pageSizeHint: 10)
            .GetAsyncEnumerator();

        bool hasPage = await enumerator.MoveNextAsync();

    if (!hasPage || enumerator.Current is null)
    {
        return (cursor, changeFeedEvents);
    }

        foreach (BlobChangeFeedEvent changeFeedEvent in enumerator.Current.Values)
        {
            changeFeedEvents.Add(changeFeedEvent);
        }

        cursor = enumerator.Current.ContinuationToken;
        return (cursor, changeFeedEvents);
    }