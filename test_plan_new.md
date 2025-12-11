# Universidad de Castilla-La Mancha

# <Test Plan>

## Version <1.0>

Test Plan Fecha: < 11 / 12 / 2025 >

# Revisions

| **Date**       | **Version** | **Description** | **Author**                 |
| -------------- | ----------- | --------------- | -------------------------- |
| 11 / 12 / 2025 | 1.0         | Entire Document | Samuel Bruno Garcia Picazo |
|                |             |                 |                            |

## Test Plan Fecha: < 11 / 12 / 2025 >

Index

1. Test plan identifier
2. Introduction
3. Test items
4. Features to be tested
5. Features not to be tested
6. Approach
7. Item pass/fail criteria
8. Suspension criteria and resumption requirements
9. Test deliverables
10. Testing tasks
11. Environmental needs
12. Responsibilities
13. Staffing and training needs
14. Schedule
15. Risks and contingencies
16. Approvals

---

# Test plan

To prescribe the scope, approach, resources, and schedule of the testing activities. To identify the items being tested, the features to be tested, the testing tasks to be performed, the personnel responsible for each task, and the risks associated with this plan.

## 1. Test plan identifier

```
AlbaGym-TestPlan
```

## 2. Introduction

The purpose of this document is to define the testing strategy, scope, objectives, schedule and resources required for our software system AlbaGym.

AlbaGym is a fitness and gym management application that enables users to:

- Create and manage fitness plans with gym classes
- Purchase gym-related items (supplements, equipment)
- Manage inventory restocking operations
- Browse available classes and items

Testing is essential to ensure the functionality, performance and security of the software, guaranteeing a good, seamless and satisfactory user experience for customers.

## 3. Test items

The following test items are identified for evaluation, along with their version/revision level and transmittal media characteristics:

**Application Components:**

- AppForSEII2526.API - Backend REST API
- AppForSEII2526.Web - Blazor Web Application
- AppForSEII2526.Maui - Mobile Application

**Documentation References:**
a) Requirements specification: Use Case - AlbaGym.pdf
b) Design specification: API Swagger documentation (swagger_new.json)
c) Users guide: N/A
d) Operations guide: N/A
e) Installation guide: README.md

## 4. Features to be tested

**UC1 - Create Plan**

- Selecting classes for a fitness plan
- Adding classes with valid dates and capacity
- Specifying plan duration (weeks)
- Selecting payment method
- Validating plan creation

**UC2 - Create Purchase**

- Selecting items for purchase
- Adding items to purchase cart
- Specifying delivery address
- Selecting payment method
- Completing the purchase process

**UC3 - Create Restock**

- Selecting items for restocking
- Specifying restock quantities
- Setting expected delivery dates
- Validating restock creation

**Associated Design Specifications:**

- Use Case: Create Plan
- Use Case: Create Purchase
- Use Case: Create Restock

## 5. Features not to be tested

- Attempting to create a plan with classes that have no capacity, as the system automatically prevents this by validation.

- Creating a purchase with items exceeding available stock, as the system enforces stock availability constraints.

- Selecting invalid payment methods, as the system validates payment method registration before allowing transactions.

- User authentication and registration flows (handled by Identity framework).

## 6. Approach

### 6.1 Main testing activities

The test environment must be configured:

- Configure the testing environment on Windows 10/11 with Visual Studio 2022.
- Validate that the API is operational before initiating tests.
- Ensure the application's web view is accessible via Microsoft Edge.

The feature testing must be done for:

- **Create Plan**: Validate class selection, plan creation, and payment workflows.
- **Create Purchase**: Validate item selection, cart management, and checkout workflows.
- **Create Restock**: Validate item selection, quantity specification, and restock creation.
- **General Application Features**: Assess UI compatibility, application initialization, and API connectivity.

The testing must be automated and manual:

- Use Selenium WebDriver for automated functional UI testing.
- Use xUnit with Moq for automated unit testing of API controllers.
- Perform manual exploratory testing for edge cases and scenarios that automation may not cover.

Error identification and reporting will be conducted as follows:

- Log all identified defects, specifying steps to reproduce, expected versus actual behavior, and severity.
- Critical errors (e.g., crashes, data corruption) will be prioritized for resolution.

### 6.2 Techniques and tools

**API Testing:** Validate API endpoints for functionality, manually test endpoints using Swagger for exploratory and quick validation of edge cases, automate API tests using unit tests (xUnit with Moq for mocking dependencies) to verify the correctness of responses.

**Functional Testing:** Automated tests written in Visual Studio using Selenium WebDriver and xUnit. Manual testing for API, Database creation and update, and UI edge cases.

**Non-Functional Testing:** API validation to ensure connectivity and performance, and compatibility testing to verify smooth operation on Microsoft Edge, Chrome, and Firefox.

**Coverage Assessment:** Achieve at least 80% coverage using automated test suites and perform additional tests to cover high-risk scenarios identified during exploratory testing.

**Traceability:** Link test cases to use cases and requirements documented in the specification.

### 6.3 Completion

Testing can be considered complete when:

- All functional test cases pass with no critical or major defects.
- Automated tests achieve at least 90% code coverage.
- Manual tests confirm the resolution of identified high-priority defects.
- Defect error frequency falls below 5% for major workflows.

### 6.4 Deadlines

Tests must align with project sprints:

1. Sprint 3: 25/11/2024 - 10/01/2025
2. Extra Sprint: 13/01/2025 – 20/05/2025

## 7. Item pass/fail criteria

Pass and fail criteria of each use case will be defined in each Test Case:

- **Pass**: The test executes successfully with expected results matching actual results.
- **Fail**: The test produces unexpected results, errors, or exceptions.

For unit tests:

- Assert.Equal, Assert.True, Assert.IsType validations must pass.
- No unhandled exceptions during test execution.
- BadRequest responses must contain expected error messages.

## 8. Suspension criteria and resumption requirements

**Suspension criteria:**
Testing will be temporarily suspended if any of the following conditions occur:

- A critical defect is detected in a core functionality such as plan creation, purchase processing, or restock operations, which blocks further execution of test cases.
- The test environment becomes unstable or unavailable (e.g., database corruption, deployment failure, server crash).
- Test data is inconsistent or missing, making validation of expected results unreliable.
- A build fails repeatedly and prevents navigation or login to the system.
- A regression issue causes more than 30% of previously passed tests to fail.

**Resumption criteria:**
Testing will be resumed only when:

- The blocking issue(s) have been resolved and verified by the development team.
- A new stable build is deployed and basic smoke tests pass successfully (login, class listing, item listing).
- All affected components have passed at least one round of revalidation.

**Tests to be repeated upon resumption:**

- All test cases that were blocked or incomplete at the moment of suspension.
- Regression tests related to the fixed issues, especially those touching shared components.
- Smoke tests to ensure that the system is in a stable and testable state before continuing with functional or scenario-based testing.

## 9. Test deliverables

The following documents are generated:

- System Test Plan
- System Test Case Specifications:
  - CreatePlan_test.cs
  - GetPlan_test.cs
  - CreatePurchase_test.cs
  - GetPurchase_test.cs
  - CreateRestock_test.cs
  - GetRestock_test.cs
  - SelectClassesForPlan_test.cs
  - GetItemsForPurchase_test.cs
  - GetItemsForRestock_test.cs
- UI Test Specifications:
  - UC_UIT.cs (Base UI Test class)
  - PageObject.cs

## 10. Testing tasks

The following tasks were performed during the testing phase of AlbaGym:

1. **Environment Setup**: Configure SQLite in-memory database for isolated unit testing using the `AppForSEII25264SqliteUT` base class.

2. **Unit Test Development**: Each team member developed unit tests for their assigned use cases:

   - Samuel: CreatePlan tests (main flow + 7 alternative flows)
   - Hugo: GetPlan and class selection tests
   - Alberto: CreateRestock and CreatePurchase tests

3. **Test Execution**: Run all xUnit tests using Visual Studio Test Explorer or the command `dotnet test`.

4. **UI Test Configuration**: Set up Selenium WebDriver with Edge/Chrome/Firefox drivers for browser automation testing.

5. **Documentation**: Generate test plan and test summary report documents.

## 11. Environmental needs

The tests are performed locally on machines with at least 8 GB RAM, a modern processor (Intel i5 or similar), and SSD storage. The system runs on Windows 10/11 with the .NET SDK installed.

The application under test is a Blazor web app executed in stand-alone mode (localhost:7083). Browsers used include Microsoft Edge, Chrome, and Firefox. EdgeDriver, ChromeDriver, and GeckoDriver are used for UI automation.

The environment includes a seeded test database using SQLite in-memory for unit tests. All required tools—Selenium WebDriver, xUnit, Moq, and browser drivers—are locally available.

No special hardware, real user data, or external APIs are involved. The codebase is private on GitHub. No additional office space or documentation is needed. Testers should know C#, Selenium, xUnit, and basic Blazor structure.

## 12. Responsibilities

All testing-related activities in this project—including planning, test case design, environment preparation, test execution, validation, and defect reporting—have been distributed among team members.

The responsibilities were divided as follows:

- **Designing and implementing all functional test cases:**
  Samuel Bruno Garcia Picazo, Hugo Rodriguez Villalobos, Alberto Bueno Baquero

- **Developing Page Objects and automation scripts using Selenium and xUnit:**
  Samuel Bruno Garcia Picazo, Hugo Rodriguez Villalobos, Alberto Bueno Baquero

- **Executing the full suite of tests and analyzing results:**
  Samuel Bruno Garcia Picazo

- **Maintaining the test environment and tooling:**
  Hugo Rodriguez Villalobos

- **Generating documentation and reporting:**
  Each team member is responsible for generating their own test documentation.
  All team members collaborated in the creation of the Test Plan.
  Samuel Bruno Garcia Picazo is in charge of drafting the Final Report.

## 13. Staffing and training needs

No additional staffing was required.

The testers needed the following skill set:

- Knowledge of software testing principles and test case design
- Experience with Selenium WebDriver for automated UI testing
- Proficiency in C# and xUnit as the test framework
- Experience with Moq for mocking dependencies
- Familiarity with web technologies used in the project (Blazor, Razor, HTML/CSS)
- Ability to configure and manage the local test environment (database seeding, drivers, etc.)

## 14. Schedule

Our estimation is that the testing phase will begin by the end of Sprint 3, specifically between January 1st and January 9th, 2025. All use cases are expected to be tested during this period. By that time, the test cases for each use case should also be written and reviewed.

Additional testing will continue through the Extra Sprint (January - May 2025) for regression testing and any new features.

## 15. Risks and contingencies

- **Delayed delivery of test items:** May block progress.
  Plan: Focus on available features, extend testing hours if needed.

- **Unstable test environment:** Could prevent execution.
  Plan: Keep a backup setup and reinstall quickly if required.

- **Browser or driver incompatibility:** May break automation.
  Plan: Use fixed driver versions; fall back to manual tests temporarily.

- **Inconsistent test data:** Could affect test reliability.
  Plan: Reseed database before each run using SQLite in-memory.

- **Limited testing team size:** Risk of delays with only three team members.
  Plan: Prioritize critical cases and allocate extra time if necessary.

## 16. Approvals

| Name                         | Role                         | Date       |
| ---------------------------- | ---------------------------- | ---------- |
| Samuel Bruno Garcia Picazo   | Test Plan Author / Developer | 11/12/2025 |
| Hugo Rodriguez Villalobos    | Developer / Reviewer         | 11/12/2025 |
| Alberto Bueno Baquero        | Developer / Reviewer         | 11/12/2025 |
| Elena Maria Navarro Martínez | Product Owner                |            |
