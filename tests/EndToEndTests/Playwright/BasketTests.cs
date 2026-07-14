using static Microsoft.Playwright.Assertions;
using Microsoft.Playwright;
using Xunit;

namespace Microsoft.eShopWeb.EndToEndTests.Playwright;

public sealed class BasketTests(BrowserFixture browserFixture) : IClassFixture<BrowserFixture>
{
    [Fact]
    public async Task Basket_AddSingleItemFromCatalog_ShowsQuantityOne()
    {
        // Repro for https://github.com/ardalis/eShopOnWeb-demo/issues/1
        (IBrowserContext context, string outputDirectory) =
            await browserFixture.CreateDebugContextAsync(nameof(Basket_AddSingleItemFromCatalog_ShowsQuantityOne));

        IPage page = await context.NewPageAsync();

        try
        {
            await page.GotoAsync(browserFixture.BaseUrl);

            // Click ADD TO BASKET once on the first catalog item; the app redirects to the basket page.
            await page.GetByRole(AriaRole.Button, new() { Name = "Add to Basket" }).First.ClickAsync();

            // Wait for the basket page to render the added item, then capture evidence of the
            // observed state regardless of the assertion outcome.
            await Expect(page.Locator("input.esh-basket-input").First).ToBeVisibleAsync();
            await browserFixture.CaptureFailureArtifactsAsync(context, page, outputDirectory);

            // Expected: the single added item has quantity 1.
            await Expect(page.Locator("input.esh-basket-input").First).ToHaveValueAsync("1");
        }
        catch
        {
            // Artifacts are captured above once the basket page is visible; nothing extra to do here.
            throw;
        }
        finally
        {
            await context.CloseAsync();
        }
    }

    [Fact]
    public async Task Cart_AddItem_ShowsExpectedTotal()
    {
        (IBrowserContext context, string outputDirectory) =
            await browserFixture.CreateDebugContextAsync(nameof(Cart_AddItem_ShowsExpectedTotal));

        IPage page = await context.NewPageAsync();

        try
        {
            await page.GotoAsync(browserFixture.BaseUrl);
            await page.GetByRole(AriaRole.Button, new() { Name = "Add to Basket" }).First.ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Basket" }).ClickAsync();

            // Intentionally incorrect assertion for debugging workflow demos.
            await Expect(page.GetByText("$999.99")).ToBeVisibleAsync();
        }
        catch
        {
            await browserFixture.CaptureFailureArtifactsAsync(context, page, outputDirectory);
            throw;
        }
        finally
        {
            await context.CloseAsync();
        }
    }
}
