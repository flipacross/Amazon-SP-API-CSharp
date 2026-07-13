# Finances v2024-06-19 Gap Fixes Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.
>
> **Session deviation note:** This session runs autonomously (non-interactive). Tasks are executed inline in-session. Per session policy, NO git commits are made — the user commits after review. Commit steps below are therefore replaced by a single suggested commit at the end.

**Goal:** Bring the fork's existing Finances v2024-06-19 `listTransactions` support up to Amazon's current model and unblock flipacross `IFinancesSpApiClient` (Task 5b): ORDER_ID-only lookups, single-page token-in/token-out sweeps, `contextType` round-trip fidelity, and unknown-enum resilience.

**Architecture:** The fork already has `FinancialService.ListFinancialTransactions20240619` + `Models/Financesv20240619/*` (namespace `FikaAmazonAPI.AmazonSpApiSDK.Models.Finances.Model`) + `FinanceV20240619ApiUrls.Transactions` + `RateLimitType.FinancialV20240619_Transactions` (0.5 rps / burst 10 — matches spec). This plan patches the gaps in place; no new API surface on `AmazonConnection` is needed.

**Tech Stack:** netstandard2.0 library, Newtonsoft.Json (DataContract/DataMember opt-in), NUnit tests (net6.0).

**Audit findings this plan fixes (vs. finances_2024-06-19.json swagger + flipacross contract):**

| # | Gap | Impact |
|---|-----|--------|
| 1 | `ParameterListFinancialTransactions20240619.postedAfter` is non-nullable `DateTime` | Unset ⇒ `getParameters()` emits `postedAfter=0001-01-01T00:00:00.000Z`; with the API's 180-day `postedAfter`/`postedBefore` window rule this yields **empty responses** for ORDER_ID-only queries. Spec: `postedAfter` optional when a related identifier is given. Blocks flipacross `ListTransactionsByOrderIdAsync`. |
| 2 | Single-page fetch (`GetFinancialTransactions20240619ByNextToken*`) is `private`; auto-pager loses the unconsumed `nextToken` and leaves the last **consumed** token on the parameter | Blocks flipacross `ListTransactionsAsync` (single page; caller loops on NextToken). Parameter reuse after a call silently re-fetches only the last page. |
| 3 | `Context` model lacks `contextType` (required discriminator in spec; present in every response) | Consumers can't tell ProductContext/DeferredContext/etc. apart; SDK→JSON round-trip (flipacross `TransactionSnapshotMapper.MapPageFromJson`) silently drops it. |
| 4 | `BusinessContext` model file missing (spec defines it; siblings AmazonPay/Product/Payments/Deferred/TimeRange exist) | Model parity. Its only field `storeName` already exists on the merged `Context`, so this is a standalone-class addition only. |
| 5 | `RelatedIdentifierNameEnum` / `ItemRelatedIdentifierNameEnum` use strict `StringEnumConverter` | Amazon ships undocumented values (fork history: `SAFET_CLAIM_ID`, `DEAL_ID`, `INVOICE_ID` were added after the fact) ⇒ one new value fails whole-response deserialization. |
| 6 | Sample code lacks ORDER_ID and page-by-page examples; stale doc comment says only FINANCIAL_EVENT_GROUP_ID is filterable (spec: ORDER_ID too) | Usage documentation. |

**Explicitly NOT changed (checked, already correct):** `Transaction` has all 12 spec fields; transaction-level `breakdowns` is an array per the schema (the docs *example* showing an object wrapper is a known docs bug — 14 months of production use of the array shape confirms the schema); `RelatedIdentifierNameEnum` covers all 10 spec values (+3 observed extras); rate limits registered 0.5/10; the pager re-sends all original arguments with `nextToken` exactly as the spec requires; `MaxNumberOfPages` stays attribute-less (repo-wide convention).

---

### Task 1: Make `postedAfter` optional + fix stale docs

**Files:**
- Modify: `Source/FikaAmazonAPI/Parameter/Finance/ParameterListFinancialTransactions20240619.cs`

- [ ] **Step 1: Change property + docs**

```csharp
/// <summary>
/// The response includes financial events posted on or after this date (ISO 8601).
/// Required if you do not specify <see cref="relatedIdentifierName"/>/<see cref="relatedIdentifierValue"/>;
/// leave null for identifier-only lookups (e.g. ORDER_ID). If postedAfter and postedBefore
/// are more than 180 days apart, the response is empty.
/// </summary>
public DateTime? postedAfter { get; set; }
```

and update the `relatedIdentifierName` comment to:

```csharp
/// <summary>
/// The identifier name to filter by. FINANCIAL_EVENT_GROUP_ID and ORDER_ID have filtering
/// capability; other values appear in response payloads but cannot be used as query filters.
/// </summary>
```

- [ ] **Step 2: Verify sample still compiles later with the full build (Task 8)** — `postedAfter = DateTime.UtcNow.AddDays(-30)` assignments remain valid against `DateTime?`.

### Task 2: Public single-page access + nextToken write-back in the auto-pager

**Files:**
- Modify: `Source/FikaAmazonAPI/Services/FinancialService.cs:169-194`

- [ ] **Step 1: After the paging `while` loop in `ListFinancialTransactions20240619Async`, write the unconsumed token back:**

```csharp
// Surface continuation state on the parameter: null when all pages were consumed
// (safe re-use for a fresh query), or the next unconsumed token when
// MaxNumberOfPages stopped the loop early (re-call with the same parameter to resume).
parameterListFinancialTransactions.nextToken = string.IsNullOrEmpty(nextToken) ? null : nextToken;
```

- [ ] **Step 2: Make the single-page pair public with XML docs** (rename-free; matches v0's public `...ByNextToken` convention):

```csharp
/// <summary>
/// Fetches a single page of transactions. Set <c>parameter.nextToken</c> (with the same
/// arguments that produced the token) to continue; the response carries
/// <c>Payload.NextToken</c> for the next page.
/// </summary>
public ListTransactionsResponse GetFinancialTransactions20240619ByNextToken(ParameterListFinancialTransactions20240619 parameterListFinancialTransactions) => ...
public async Task<ListTransactionsResponse> GetFinancialTransactions20240619ByNextTokenAsync(ParameterListFinancialTransactions20240619 parameterListFinancialTransactions, CancellationToken cancellationToken = default) { ... }
```

(only the access modifiers + doc comments change; bodies stay as-is)

### Task 3: Add `ContextType` to `Context`

**Files:**
- Modify: `Source/FikaAmazonAPI/AmazonSpApiSDK/Models/Financesv20240619/Context.cs`

- [ ] **Step 1: Append ctor param `string contextType = default(string)` (end of signature), assign it, and add the property** (codegen style):

```csharp
/// <summary>
/// The type of context. Examples: ProductContext, AmazonPayContext, PaymentsContext,
/// DeferredContext, BusinessContext, TimeRangeContext.
/// </summary>
[DataMember(Name="contextType", EmitDefaultValue=false)]
public string ContextType { get; set; }
```

- [ ] **Step 2: Extend `ToString()`, `Equals(Context)`, `GetHashCode()` with the new member**, same pattern as existing members.

### Task 4: Add `BusinessContext` model

**Files:**
- Create: `Source/FikaAmazonAPI/AmazonSpApiSDK/Models/Financesv20240619/BusinessContext.cs`

- [ ] **Step 1: Full file, mirroring `AmazonPayContext.cs` structure** — single property:

```csharp
/// <summary>
/// The store name associated with the transaction. Example: AMAZON_HAUL
/// </summary>
[DataMember(Name="storeName", EmitDefaultValue=false)]
public string StoreName { get; set; }
```

(class `BusinessContext : IEquatable<BusinessContext>, IValidatableObject`, namespace `FikaAmazonAPI.AmazonSpApiSDK.Models.Finances.Model`, standard ToString/ToJson/Equals/GetHashCode/Validate.)

### Task 5: Unknown-enum-tolerant deserialization

**Files:**
- Create: `Source/FikaAmazonAPI/Utils/SafeStringEnumConverter.cs`
- Modify: `Source/FikaAmazonAPI/AmazonSpApiSDK/Models/Financesv20240619/RelatedIdentifier.cs:31`
- Modify: `Source/FikaAmazonAPI/AmazonSpApiSDK/Models/Financesv20240619/ItemRelatedIdentifier.cs:31`

- [ ] **Step 1: Converter (public, like `DoubleJsonConverter`):**

```csharp
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace FikaAmazonAPI.Utils
{
    /// <summary>
    /// A StringEnumConverter that does not fail the whole response when Amazon returns an
    /// enum value this SDK version does not know yet (e.g. Finances v2024-06-19 shipped
    /// SAFET_CLAIM_ID before it was documented). Unknown values map to null for nullable
    /// enum members instead of throwing JsonSerializationException.
    /// </summary>
    public class SafeStringEnumConverter : StringEnumConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
            catch (JsonSerializationException)
            {
                if (Nullable.GetUnderlyingType(objectType) != null)
                    return null;
                return Activator.CreateInstance(objectType);
            }
        }
    }
}
```

- [ ] **Step 2: Swap the enum-level attribute in both model files:**
`[JsonConverter(typeof(StringEnumConverter))]` → `[JsonConverter(typeof(SafeStringEnumConverter))]` + `using FikaAmazonAPI.Utils;`.
Write path is unchanged (inherits `StringEnumConverter.WriteJson`, `[EnumMember]` respected); `ParameterBased.getParameters()` is unaffected for values it can serialize.

### Task 6: Sample code — ORDER_ID + page-by-page usage

**Files:**
- Modify: `Source/FikaAmazonAPI.SampleCode/FinancialSample.cs`

- [ ] **Step 1: Append two methods:**

```csharp
public IList<Transaction> ListFinancialTransactions20240619_ByOrderId(string amazonOrderId)
{
    // postedAfter intentionally omitted: with a related identifier filter the date window
    // is not required (sending one narrows/empties the result).
    return amazonConnection.Financial.ListFinancialTransactions20240619(
        new Parameter.Finance.ParameterListFinancialTransactions20240619()
        {
            relatedIdentifierName = RelatedIdentifier.RelatedIdentifierNameEnum.ORDERID,
            relatedIdentifierValue = amazonOrderId,
        });
}

public List<Transaction> ListFinancialTransactions20240619_PageByPage(string marketplaceId)
{
    var all = new List<Transaction>();
    var parameter = new Parameter.Finance.ParameterListFinancialTransactions20240619()
    {
        postedAfter = DateTime.UtcNow.AddDays(-7),
        marketplaceId = marketplaceId,
    };
    string nextToken = null;
    do
    {
        parameter.nextToken = nextToken;
        var response = amazonConnection.Financial.GetFinancialTransactions20240619ByNextToken(parameter);
        if (response?.Payload?.Transactions != null)
            all.AddRange(response.Payload.Transactions);
        nextToken = response?.Payload?.NextToken;
    } while (!string.IsNullOrEmpty(nextToken));
    return all;
}
```

### Task 7: Deserialization + parameter unit tests

**Files:**
- Create: `Source/Tests/FinancesV20240619Tests.cs`

- [ ] **Step 1: Write the tests** (NUnit; `global using NUnit.Framework;` already present):
  - `Deserialize_FullPayload_MapsAllFields` — schema-shaped payload (transaction-level `breakdowns` as ARRAY) covering all 12 Transaction fields, nested item breakdowns, contexts with `contextType` (Product/AmazonPay/Deferred). Asserts NextToken, ids, enum identifiers, amounts (decimal), `ContextType`, nested breakdown depth, `MaturityDate`.
  - `Deserialize_UnknownIdentifierEnums_DoesNotThrow` — `relatedIdentifierName: "FUTURE_UNKNOWN_ID"` and `itemRelatedIdentifierName: "FUTURE_ITEM_ID"` ⇒ no exception, name properties null, values preserved.
  - `Serialize_Roundtrip_PreservesContextTypeAndEnumWireValues` — re-serialize and assert `"contextType":"ProductContext"` and `"ORDER_ID"` appear (guards flipacross raw-JSON round-trip).
  - `GetParameters_IdentifierOnly_OmitsPostedAfter` — ORDER_ID-only parameter ⇒ no `postedAfter` key; `relatedIdentifierName` serialized as `ORDER_ID`.
  - `GetParameters_WithPostedAfter_UsesIso8601` — `postedAfter` set ⇒ `postedAfter=2026-06-01T00:00:00.000Z` (Constants.DateISO8601Format).

- [ ] **Step 2: Run** `dotnet test ./Source/Tests/Tests.csproj --filter "FullyQualifiedName~FinancesV20240619"` — expect all pass (and expect `GetParameters_IdentifierOnly_OmitsPostedAfter` to FAIL if run before Task 1, confirming the test bites).

### Task 8: Version bump + full verification

**Files:**
- Modify: `Source/FikaAmazonAPI/FikaAmazonAPI.csproj:10-12`

- [ ] **Step 1: Bump `<Version>`, `<AssemblyVersion>`, `<FileVersion>` 1.9.30 → 1.9.31** (release convention from commit 698a347).
- [ ] **Step 2: `dotnet build`** (solution) — expect Build succeeded, 0 errors.
- [ ] **Step 3: `dotnet test ./Source/Tests/Tests.csproj`** — expect full suite green (RateLimits + new Finances tests).
- [ ] **Step 4: Suggested single commit (user executes after review):**

```
feat(finances): close v2024-06-19 listTransactions gaps for flipacross Task 5b

- postedAfter nullable (spec: optional with relatedIdentifier filter) + doc fixes
- expose single-page GetFinancialTransactions20240619ByNextToken publicly;
  auto-pager writes continuation token back to parameter.nextToken
- add Context.contextType discriminator + BusinessContext model (AMAZON_HAUL)
- SafeStringEnumConverter: unknown identifier enum values no longer fail deserialization
- samples for ORDER_ID lookup + page-by-page sweep; deserialization/parameter tests
- bump 1.9.30 -> 1.9.31
```

---

**Post-plan self-review:** spec coverage ✓ (all 6 audit gaps have tasks; non-gaps documented); no placeholders (Task 4's "standard ToString/..." refers to the concrete sibling file `AmazonPayContext.cs` as the template — implementer copies it); type consistency ✓ (`ListTransactionsResponse.Payload.NextToken`/`Transactions : List<Transaction>` verified in repo).
