# Universidad de Castilla-La Mancha

# <Test Plan>

## Version <1.0>

Test plan Fecha: < 12 / 05 / 2025 >
Test Plan Document

# Revisions

```
Date Version Description Author
```

< 12 / 05 / 2025 > < 1. 0 > Entire Document Juan Picazo Martínez

## Test plan Fecha: < 12 / 05 / 2025 >

Test Plan Document

- 1. Test plan identifier Index
- 2. Introduction
- 3. Test items
- 4. Features to be tested
- 5. Features not to be tested
- 6. Approach
- 7. Item pass/fail criteria
- 8. Suspension criteria and resumption requirements
- 9. Test deliverables
- 10. Testing tasks
- 11. Environmental needs
- 12. Responsibilities
- 13. Staffing and training needs
- 14. Schedule
- 15. Risks and contingencies
- 16. Approvals

```
Test plan Fecha: < 12 / 05 / 2025 >
Test Plan Document
```

# Test plan

```
To prescribe the scope, approach, resources, and schedule of the testing activities. To identify
the items being tested, the features to be tested, the testing tasks to be performed, the personnel
responsible for each task, and the risks associated with this plan.
```

## 1. Test plan identifier

```
AppforClothes-TestPlan
```

## 2. Introduction

```
The purpose of this document is to define the testing strategy, scope, objectives, schedule
and resources required for our software system AppForClothes.
```

```
AppForClothes enables users to purchase clothes
```

```
Testing is essential to ensure the functionality, performance and security of the software
guaranteeing a good, seamless and satisfactory user experience for customers.
```

## 3. Test items

```
The following test items are identified for evaluation, along with their version/revision level
and transmittal media characteristics:
```

```
Supply references to the following test item documentation, if it exists:
```

```
a) Requirements specification;
b) Design specification;
c) Users guide;
d) Operations guide;
e) Installation guide.
```

```
Reference any incident reports relating to the test items.
```

```
Items that are to be specifically excluded from testing may be identified.
```

## 4. Features to be tested

```
Place an Order
```

```
Filtering items by type and genre.
```

```
Test plan Fecha: < 12 / 05 / 2025 >
Test Plan Document
```

```
Adding item to the purchase cart.
Editing a item quantity inside cart
Removing a item from the purchase cart.
```

```
Completing the order process.
```

_Associated Design Specification:_ Use Case: Place an Order

## 5. Features not to be tested

```
Attempt to purchase an item with quantity 0, but the system automatically hides the
“Select” button when the stock is 0, so it cannot be selected.
```

```
Edit the quantity of an item in the cart and set a value greater than the available
stock, but the system prevents this. The quantity cannot exceed the stock nor go
below 1 — the allowed range is therefore: min = 1, max = stock.
```

## 6. Approach

**6.1 Main testing activities**

```
The test environment must be configured:
```

- Configure the testing environment on Windows 1 0 with Visual Studio 2022.
- Validate that the API is operational before initiating tests.
- Ensure the application’s web view is accessible via Microsoft Edge.

```
The feature testing must be done for:
```

- **Place Order** : Validate filtering, cart management, and checkout workflows.
- **General Application Features** : Assess UI compatibility, application initialization,
  and API connectivity.

```
The testing must be automated and manual :
```

- Use Selenium WebDriver for automated functional testing.
- Perform manual exploratory testing for edge cases and scenarios that automation
  may not cover.
  Error identification and reporting will be conducted as follow:
- Log all identified defects, specifying steps to reproduce, expected versus actual
  behavior, and severity.
- Critical errors (e.g., crashes, data corruption) will be prioritized for resolution.

```
Test plan Fecha: < 12 / 05 / 2025 >
Test Plan Document
```

**6.2 Techniques and tools**

```
API Testing: Validate API endpoints for functionality, manually test endpoints using
Swagger for exploratory and quick validation of edge cases, automate API tests using
unit tests (XUnit with Moq for mocking dependencies) to verify the correctness of
responses.
Functional Testing : Will be automated test written in Visual Studio using Selenium
WebDriver. Manual testing for API, Database creation and update and UI edge cases.
Non-Functional Testing : API validation to ensure connectivity and performance and
compatibility testing to verify smooth operation on Microsoft Edge.
Coverage Assessment : Achieve at least 8 0% coverage using automated test suites
and perform additional tests to cover high-risk scenarios identified during exploratory
testing.
Traceability : Link test cases to use cases and requirements documented in the
specification and use Azure DevOps for tracking test execution and requirements
coverage.
```

**6.3 Completion**

Testing can be considered complete when:

```
All functional test cases pass with no critical or major defects.
Automated tests achieve at least 90% code coverage.
Manual tests confirm the resolution of identified high-priority defects.
Defect error frequency falls below 5% for major workflows.
```

**6.4 Deadlines**

Tests must align with project sprints:

1. Sprint 3: 25/11/2024 - 10 /01/
2. Extra Sprint :13/01/2025 – 20/05/

## 7. Item pass/fail criteria

```
Pass and fail criteria of each use case will be defined in each Test Case
```

## 8. Suspension criteria and resumption requirements

```
Suspension criteria:
Testing will be temporarily suspended if any of the following conditions occur:
```

```
Test plan Fecha: < 12 / 05 / 2025 >
Test Plan Document
```

- A critical defect is detected in a core functionality such as product selection, cart
  handling, or order confirmation, which blocks further execution of test cases.
- The test environment becomes unstable or unavailable (e.g., database corruption,
  deployment failure, server crash).
- Test data is inconsistent or missing, making validation of expected results unreliable.
- A build fails repeatedly and prevents navigation or login to the system.
- A regression issue causes more than 30% of previously passed tests to fail.
-

```
Resumption criteria:
Testing will be resumed only when:
```

- The blocking issue(s) have been resolved and verified by the development team.
- A new stable build is deployed and basic smoke tests pass successfully (login, product
  listing, cart access).
- All affected components have passed at least one round of revalidation.

```
Tests to be repeated upon resumption:
```

- All test cases that were blocked or incomplete at the moment of suspension.
- Regression tests related to the fixed issues, especially those touching shared
  components.
- Smoke tests to ensure that the system is in a stable and testable state before continuing
  with functional or scenario-based testing.

## 9. Test deliverables

```
The following documents are generated :
```

- System Test Plan
- System Test Case Order Specification

## 10. Testing tasks

```
Task ID
```

**Task Description** (^) **Dependencies
Required Skills**

```
Test plan Fecha: < 12 / 05 / 2025 >
Test Plan Document
```

```
T01 Set up and configure
the test
environment
(database, test data,
identity users)
```

```
None Basic DevOps, DB
setup, environment
config
```

```
T0 2 Review
requirements and
use cases
```

```
Completion of
specification docs
```

```
Requirements
analysis,
documentation
reading
```

```
T03 Design test cases
based on use cases
and alternative flows
```

```
T02 Functional analysis,
test design
```

```
T0 4 Implement
automated UI tests
(Selenium + xUnit)
```

```
T03 C#, Selenium,
xUnit, Page Object
Model
```

```
T0 5 Execute smoke tests
to validate
deployment integrity
```

```
T01 Functional testing,
Selenium basics
```

```
T06 Run full test suite T04, T05 Test execution and
result analysis
```

### T

```
Report and track
bugs in issue
tracker (Azure
DevOps)
```

```
T06 Bug reporting,
traceability
```

```
T08 Re-run failed or
blocked tests after
bug fixes
```

```
T07 Regression testing,
defect revalidation
```

### T

```
Generate final test
report and
coverage summary
```

```
T08 Reporting,
documentation
```

- Tasks T01 and T02 can be done in parallel.
- Tasks T06 to T09 are iterative and may repeat depending on bug findings

## 11. Environmental needs

The tests are performed locally on machines with at least 8 GB RAM, a modern processor
(Intel i5 or similar), and SSD storage. The system runs on Windows 10/11 or Ubuntu 22.
with the .NET SDK installed.

```
Test plan Fecha: < 12 / 05 / 2025 >
Test Plan Document
```

The application under test is a Blazor web app executed in stand-alone mode (localhost).
Browsers used include Microsoft Edge, Chrome, and Firefox. EdgeDriver is used for UI
automation.

The environment includes a seeded test database with predefined products using the
SeedTypesAndProducts() method. All required tools—Selenium WebDriver, xUnit, and
browser drivers—are locally available.

No special hardware, real user data, or external APIs are involved. The codebase is private
on Azure. No additional office space or documentation is needed. Testers should know C#,
Selenium, and basic Blazor structure.

## 12. Responsibilities

```
All testing-related activities in this project—including planning, test case design,
environment preparation, test execution, validation, and defect reporting—have been
distributed among team members.
```

```
The responsibilities were divided as follows:
```

- Designing and implementing all functional test cases, and
- Developing Page Objects and automation scripts using Selenium and xUnit:
  Juan Picazo Martínez, Alejandro Gómez Lozano, Héctor Ruiz López, and Alberto Bueno
  Vaquero.
- Executing the full suite of tests and analysing results, and
- Maintaining the test environment and tooling:
  Héctor Ruiz López.
- Generating documentation and reporting:
  Each team member (Juan Picazo Martínez, Alejandro Gómez Lozano, Héctor Ruiz López,
  and Alberto Bueno Vaquero) is responsible for generating their own testUse
  documentation.
  All team members collaborated in the creation of the Test Plan, while Héctor Ruiz López is
  in charge of drafting the Final Report.

## 13. Staffing and training needs

No additional staffing was required.

The tester needed the following skill set:

- Knowledge of software testing principles and test case design
- Experience with Selenium WebDriver for automated UI testing
- Proficiency in C# and xUnit as the test framework
- Familiarity with web technologies used in the project (Blazor, Razor, HTML/CSS)
- Ability to configure and manage the local test environment (database seeding, drivers,
  etc.)

```
Test plan Fecha: < 12 / 05 / 2025 >
Test Plan Document
```

## 14. Schedule

```
Our estimation is that the testing phase will begin by the end of Sprint 3, specifically
between January 1st and January 9th, 2024. All four use cases are expected to be
tested during this period. By that time, the test cases for each use case should also be
```

## written and reviewed

## 15. Risks and contingencies

- **Delayed delivery of test items:** May block progress.
  Plan: Focus on available features, extend testing hours if needed.
- **Unstable test environment:** Could prevent execution.
  Plan: Keep a backup setup and reinstall quickly if required.
- **Browser or driver incompatibility:** May break automation.
  Plan: Use fixed driver versions; fall back to manual tests temporarily.
- **Inconsistent test data:** Could affect test reliability.
  Plan: Reseed database before each run.
- **Single-person testing workload:** Risk of delays.
  Plan: Prioritize critical cases and allocate extra time if necessary.

## 16. Approvals

```
Name Role Date
Juan Picazo Martínez Test Plan Author / Developer 12/05/
```

```
Elena Maria Navarro Martínez Product Owner 13/05/
```
