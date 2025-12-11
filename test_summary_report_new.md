# Universidad de Castilla-La Mancha

# <Test Summary Report>

## Version <1.0>

Test summary report Fecha: 11 / 12 / 2025

Confidential ©Universidad de Castilla-La Mancha

# Revisions

| **Date**       | **Version** | **Description**             | **Author**                 |
| -------------- | ----------- | --------------------------- | -------------------------- |
| 11 / 12 / 2025 | 1.0         | AlbaGym Test Summary Report | Samuel Bruno Garcia Picazo |

## Test summary report Fecha: 11 / 12 / 2025

Index

1. Test summary report identifier
2. Summary
3. Variances
4. Comprehensiveness assessment
5. Summary of results
6. Evaluation
7. Summary of activities
8. Approvals

---

# Test summary report

To summarize the results of the designated testing activities and to provide evaluations based on these results.

## 1. Test summary report identifier

### TSR-AlbaGym-1

## 2. Summary

Summarize the evaluation of the test items. Identify the items tested, indicating their version/revision level. Indicate the environment in which the testing activities took place.

| Test Item               | Description                                                                                         |
| ----------------------- | --------------------------------------------------------------------------------------------------- |
| UC1 - Create Plan       | Tested functionalities for Main Flow and Alternative Flows (7 error scenarios + 1 success scenario) |
| UC2 - Create Purchase   | Tested functionalities for Main Flow and Alternative Flows (4 error scenarios + 1 success scenario) |
| UC3 - Create Restock    | Tested functionalities for Main Flow and Alternative Flows (8 error scenarios + 1 success scenario) |
| Get Plan                | Tested retrieval functionality                                                                      |
| Get Purchase            | Tested retrieval functionality                                                                      |
| Get Restock             | Tested retrieval functionality                                                                      |
| Select Classes For Plan | Tested class selection functionality                                                                |
| Get Items For Purchase  | Tested item listing for purchases                                                                   |
| Get Items For Restock   | Tested item listing for restocking                                                                  |

**Test Environment:**

- Operating System: Windows 10/11
- IDE: Visual Studio 2022
- Framework: .NET with xUnit
- Database: SQLite in-memory for unit tests
- Browsers: Microsoft Edge (primary), Chrome, Firefox
- UI Automation: Selenium WebDriver

**Documentation References:**

- Test Plan: AlbaGym-TestPlan
- Requirements: Use Case - AlbaGym.pdf
- API Specification: swagger_new.json

## 3. Variances

All planned use cases have been tested according to the Test Plan:

- **UC1 - Create Plan**: Full coverage of main flow and alternative flows
- **UC2 - Create Purchase**: Full coverage of main flow and alternative flows
- **UC3 - Create Restock**: Full coverage of main flow and alternative flows

The following Get operations were also tested:

- GetPlan, GetPurchase, GetRestock
- SelectClassesForPlan, GetItemsForPurchase, GetItemsForRestock

## 4. Comprehensiveness assessment

The comprehensiveness of the testing process was evaluated against the criteria specified in the Test Plan (section 6.3). The plan required achieving at least 80% coverage through automated test suites, complemented by manual testing for edge cases and high-risk scenarios.

In practice, the functional and unit tests implemented covered all the main workflows related to:

- Creating fitness plans with gym classes
- Purchasing gym items (supplements, equipment)
- Restocking inventory items

Automated tests (using xUnit with Moq for API testing) ensured that the most critical flows worked correctly, meeting the plan's coverage target for functional testing.

**Test Coverage by Use Case:**

| Use Case        | Test Cases                    | Coverage              |
| --------------- | ----------------------------- | --------------------- |
| Create Plan     | 8 tests (7 error + 1 success) | 100% of defined flows |
| Create Purchase | 5 tests (4 error + 1 success) | 100% of defined flows |
| Create Restock  | 9 tests (8 error + 1 success) | 100% of defined flows |

The team of three members (Samuel Bruno Garcia Picazo, Hugo Rodriguez Villalobos, Alberto Bueno Baquero) successfully completed all test execution activities.

Overall, the comprehensiveness criteria for automated functional testing were met. The test cases cover both successful scenarios and error handling for all alternative flows.

## 5. Summary of results

The testing activities have been completed with the following results:

| Category        | Total Tests | Passed | Failed |
| --------------- | ----------- | ------ | ------ |
| Create Plan     | 8           | 8      | 0      |
| Create Purchase | 5           | 5      | 0      |
| Create Restock  | 9           | 9      | 0      |
| **Total**       | **22**      | **22** | **0**  |

**Test Breakdown by Type:**

| Test Type       | Count      | Status              |
| --------------- | ---------- | ------------------- |
| Unit Tests (UT) | 22         | ✓ All Passed        |
| UI Tests (UIT)  | Configured | Ready for execution |

All unit tests covering the Main Flow and Alternative Flows have passed with no failures.

## 6. Evaluation

The product can be deployed because it meets all the functionalities required by the user.

**Quality Assessment:**

| Criteria                       | Status |
| ------------------------------ | ------ |
| All functional test cases pass | ✓      |
| No critical defects            | ✓      |
| No major defects               | ✓      |
| Error handling implemented     | ✓      |
| API responses validated        | ✓      |

**Test Results Summary:**

```
Tests Executed: 22
Tests Passed: 22
Tests Failed: 0
Pass Rate: 100%
```

The application demonstrates robust error handling with clear error messages for:

- Missing mandatory fields (payment method, address, items, title)
- Invalid data (past dates, zero quantities, unregistered users)
- Business rule violations (exceeding stock, no capacity)

## 7. Summary of activities

**Time Investment:**

| Activity            | Time Spent |
| ------------------- | ---------- |
| Test case design    | ~8 hours   |
| Test implementation | ~12 hours  |
| Test execution      | ~2 minutes |
| Total               | ~20 hours  |

**Team Contributions:**

- **Samuel Bruno Garcia Picazo**: CreatePlan tests, test infrastructure, documentation
- **Hugo Rodriguez Villalobos**: GetPlan tests, environment setup
- **Alberto Bueno Baquero**: CreateRestock tests, CreatePurchase tests

## 8. Approvals

| Name                         | Role                            | Date       |
| ---------------------------- | ------------------------------- | ---------- |
| Samuel Bruno Garcia Picazo   | Test Summary Author / Developer | 11/12/2025 |
| Hugo Rodriguez Villalobos    | Developer / Reviewer            | 11/12/2025 |
| Alberto Bueno Baquero        | Developer / Reviewer            | 11/12/2025 |
| Elena Maria Navarro Martínez | Product Owner                   |            |
