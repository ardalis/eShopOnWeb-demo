# EndToEndTests

This project contains browser-based end-to-end tests using Playwright for .NET.

## What this covers

- Run browser tests against the real app URL (`https://localhost:5001`) started by the fixture.
- Keep tests readable with role/text/test-id locators and page helpers.
- Use Playwright codegen as a draft, then refactor for maintainability.

## First-time setup

1. Build the project to generate the Playwright install script:

   dotnet build tests/EndToEndTests/EndToEndTests.csproj

2. Install browser binaries:

   pwsh tests/EndToEndTests/bin/Debug/net10.0/playwright.ps1 install

## Run tests

dotnet test tests/EndToEndTests/EndToEndTests.csproj

The smoke test starts the Web app automatically using its normal launch profile (<https://localhost:5001>), loads the home page, and validates seeded catalog data is rendered.

## Track 1: Codegen to cleaned test workflow

1. Generate a first draft test:

   pwsh tests/EndToEndTests/bin/Debug/net10.0/playwright.ps1 codegen <https://localhost:5001>

2. Paste the generated code into a test file as a draft.

3. Refactor the draft into maintainable tests:

   - Prefer `GetByRole`, `GetByText`, and `GetByTestId` locators.
   - Remove brittle CSS/XPath selectors when possible.
   - Keep assertions focused on user-visible behavior.
   - Extract page helpers only when they improve readability.

4. Compare examples in this repo:

   - `Playwright/SmokeTests.cs` for a minimal baseline scenario.
   - `Playwright/CatalogTests.cs` and `Playwright/Pages/CatalogPage.cs` for intent-revealing helper usage.

## Level 1 demo script: codegen draft to artifact-producing run

Use this walkthrough when teaching the full authoring loop. It intentionally ends with a failing run so Level 2 can inspect real artifacts.

1. Build and install browsers (first run only):

   dotnet build tests/EndToEndTests/EndToEndTests.csproj
   pwsh tests/EndToEndTests/bin/Debug/net10.0/playwright.ps1 install

2. Generate a draft with Playwright codegen:

   pwsh tests/EndToEndTests/bin/Debug/net10.0/playwright.ps1 codegen <https://localhost:5001>

3. In the codegen browser, perform this short flow:

   - Open home page.
   - Click the first `Add to Basket` button.
   - Click `Basket`.
   - Confirm a basket line item is visible.

4. Save generated output as a draft and clean it:

   - Keep a copy of generated draft code for comparison.
   - Refactor selectors toward `GetByRole`/`GetByText`/`GetByTestId`.
   - Keep assertions user-visible and intent-based.

5. Enable the intentional failure demo test in `Playwright/BasketTests.cs`:

   - Remove the `Skip` value from `Cart_AddItem_ShowsExpectedTotal`.

6. Run only the intentional failure test:

   dotnet test tests/EndToEndTests/EndToEndTests.csproj --filter FullyQualifiedName~BasketTests.Cart_AddItem_ShowsExpectedTotal

7. Verify artifacts were produced under:

   - `TestResults/PlaywrightArtifacts/<test-name>-<timestamp>/failure.png`
   - `TestResults/PlaywrightArtifacts/<test-name>-<timestamp>/trace.zip`
   - `TestResults/PlaywrightArtifacts/<test-name>-<timestamp>/video/`

8. Reset the failure test to keep default suite green:

   - Restore the `Skip` value after the demo.

This completes Level 1 and provides concrete artifacts for Level 2 analysis/refactor/diagnostics practice.

## Track 2: Debugging with traces, screenshots, and video

This repo includes an intentionally failing basket test scenario in `Playwright/BasketTests.cs`.

- It is skip-enabled by default to keep normal test runs green.
- Remove the test `Skip` value when you want to run the failure demo.

### Artifact output behavior

- Artifacts are written to `TestResults/PlaywrightArtifacts/<test-name>-<timestamp>/`.
- Failure screenshot path: `failure.png`
- Failure trace path: `trace.zip`
- Video output path: `video/`

### Run and inspect

1. Run tests:

   dotnet test tests/EndToEndTests/EndToEndTests.csproj

2. Optional: produce structured test result output:

   dotnet test tests/EndToEndTests/EndToEndTests.csproj --logger "trx;LogFileName=endtoend.trx"

3. Open trace locally with Playwright:

   pwsh tests/EndToEndTests/bin/Debug/net10.0/playwright.ps1 show-trace TestResults/PlaywrightArtifacts/<folder>/trace.zip

4. Or open trace in hosted viewer:

   - Go to <https://trace.playwright.dev>
   - Upload `trace.zip`

Trace Viewer helps diagnose failures by showing action timeline, DOM snapshots, console/network details, and screenshots.

### Important note on visual comparisons

Screenshots/videos here are artifacts from the failed execution. For true visual comparison against a previous baseline, add screenshot assertions and baselines or integrate a dedicated visual testing tool/service.
