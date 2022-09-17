namespace Twitcher.API.Models;

/// <param name="Id">An ID that uniquely identifies the charity campaign</param>
/// <param name="BroadcasterId">An ID that uniquely identifies the broadcaster that's running the campaign</param>
/// <param name="BroadcasterLogin">The broadcaster's login name</param>
/// <param name="BroadcasterName">The broadcaster's display name</param>
/// <param name="CharityName">The charity's name</param>
/// <param name="CharityDescription">A description of the charity</param>
/// <param name="CharityLogo">A URL to an image of the charity's logo. The image's type is PNG and its size is 100px X 100px</param>
/// <param name="CharityWebsite">A URL to the charity's website</param>
/// <param name="CurrentAmount">An object that contains the current amount of donations that the campaign has received</param>
/// <param name="TargetAmount">An object that contains the amount of money that the campaign is trying to raise. This field may be null if the broadcaster has not defined a target goal</param>
public record CharityData(string Id, string BroadcasterId, string BroadcasterLogin, string BroadcasterName, string CharityName, string CharityDescription, string CharityLogo, string CharityWebsite, CharityAmount CurrentAmount, CharityAmount TargetAmount);

/// <param name="Value">The monetary amount. The amount is specified in the currency's minor unit. For example, the minor units for USD is cents, so if the amount is $5.50 USD, value is set to 550</param>
/// <param name="DecimalPlaces">The number of decimal places used by the currency. For example, USD uses two decimal places. Use this number to translate value from minor units to major units by using the formula: <paramref name="Value"/> / 10^<paramref name="DecimalPlaces"/></param>
/// <param name="Currency">The ISO-4217 three-letter currency code that identifies the type of currency in <paramref name="Value"/></param>
public record CharityAmount(int Value, int DecimalPlaces, string Currency);
