namespace Twitcher.API.Models;

/// <param name="Data">Response data</param>
/// <param name="Total">The total number of users that subscribe to this broadcaster</param>
/// <param name="Points">The current number of subscriber points earned by this broadcaster. Points are based on the subscription tier of each user that subscribes to this broadcaster. For example, a Tier 1 subscription is worth 1 point, Tier 2 is worth 2 points, and Tier 3 is worth 6 points. The number of points determines the number of emote slots that are unlocked for the broadcaster</param>
/// <param name="Pagination">A cursor value, to be used in a subsequent request to specify the starting point of the next set of results</param>
public record BroadcasterSubscriptionResponseBody(BroadcasterSubscription[] Data, int Total, int Points, Pagination Pagination) : DataPaginationResponse<BroadcasterSubscription[]>(Data, Pagination);

/// <param name="BroadcasterId">User ID of the broadcaster</param>
/// <param name="BroadcasterLogin">Login of the broadcaster</param>
/// <param name="BroadcasterName">Display name of the broadcaster</param>
/// <param name="GifterId">If the subscription was gifted, this is the user ID of the gifter. <see cref="string.Empty"/> otherwise</param>
/// <param name="GifterLogin">If the subscription was gifted, this is the login of the gifter. <see cref="string.Empty"/> otherwise</param>
/// <param name="GifterName">If the subscription was gifted, this is the display name of the gifter. <see cref="string.Empty"/> otherwise</param>
/// <param name="IsGift">Is <see langword="true"/> if the subscription is a gift subscription</param>
/// <param name="PlanName">Name of the subscription</param>
/// <param name="Tier">Type of subscription: 1000 = Tier 1, 2000 = Tier 2, 3000 = Tier 3 subscriptions</param>
/// <param name="UserId">ID of the subscribed user</param>
/// <param name="UserName">Display name of the subscribed user</param>
/// <param name="UserLogin">Login of the subscribed user</param>
public record BroadcasterSubscription(string BroadcasterId, string BroadcasterLogin, string BroadcasterName, string GifterId, string GifterLogin, string GifterName, bool IsGift, string PlanName, string Tier, string UserId, string UserName, string UserLogin);

/// <param name="BroadcasterId">User ID of the broadcaster</param>
/// <param name="BroadcasterLogin">Login of the broadcaster</param>
/// <param name="BroadcasterName">Display name of the broadcaster</param>
/// <param name="IsGift">Indicates if the subscription is a gift</param>
/// <param name="GifterLogin">Login of the gifter (if is_gift is true)</param>
/// <param name="GifterName">Display name of the gifter (if is_gift is true)</param>
/// <param name="Tier">Subscription tier. 1000 is tier 1, 2000 is tier 2, and 3000 is tier 3</param>
public record UserSubscription(string BroadcasterId, string BroadcasterLogin, string BroadcasterName, bool IsGift, string GifterLogin, string GifterName, string Tier);
