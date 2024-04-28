namespace Phi.Model.Api;

/*

{
"title": "A uBlock Origin update was rejected from the Chrome Web Store",
"uri": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
"postedBy": "ismaildonmez",
"time": "2019-10-12T13:43:01+00:00",
"score": 1716,
"commentCount": 572
}

*/

public class Story
{
    public required string Title { get; set; }
    public required string Uri { get; set; }
    public required string PostedBy { get; set; }
    public DateTime Time { get; set; }
    public int Score { get; set; }
    public int CommentCount { get; set; }
}