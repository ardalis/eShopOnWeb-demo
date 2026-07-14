# Issue #1 reproduction artifacts

Playwright artifacts supporting https://github.com/ardalis/eShopOnWeb-demo/issues/1

- basket-quantity-2.png — basket page after a single ADD TO BASKET click (Quantity = 2, total $39.00)
- trace.zip — Playwright trace (view with: npx playwright show-trace trace.zip)
- repro-video.webm — video of the full test run

Regenerate: dotnet test tests/EndToEndTests/EndToEndTests.csproj --filter FullyQualifiedName~Basket_AddSingleItemFromCatalog_ShowsQuantityOne
