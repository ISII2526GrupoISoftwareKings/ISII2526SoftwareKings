# Universidad de Castilla-La Mancha

# <Test summary report>

## Version <1.0>

```
Test summary report Fecha: 03 / 06 / 2025
```

Confidential ©Universidad de Castilla-La
1

# Revisions

```
Date Version Description Author
```

```
29 / 05 / 2025 617af9a AppForClothes/ UC-Place an Order Juan Picazo Martinez
```

## Confidential ©Universidad de Castilla-La

- Test summary report Fecha: 03 / 06 /
  -
- 1. Test summary report identifier Index
- 2. Summary
- 3. Variances
- 4. Comprehensiveness assessment
- 5. Summary of results
- 6. Evaluation
- 7. Summary of activities
- 8. Approvals

```
Test summary report Fecha: 03 / 06 / 2025
```

```
Confidential ©Universidad de Castilla-La
1
```

# Test summary report

```
To summarize the results of the designated testing activities and to provide evaluations
based on these results.
```

## 1. Test summary report identifier

### TSR- 1

## 2. Summary

```
Summarize the evaluation of the test items. Identify the items tested, indicating their
version/revision level. Indicate the environment in which the testing activities took place.
```

```
For each test item, supply references to the following documents if they exist: test plan, test
design specifications, test procedure specifications, test item transmittal reports, test logs,
and test incident reports.
```

```
Test Item Description
```

```
UC1-Place an Order Tested functionalities for Main Flow and
Alternative Flow
```

## 3. Variances

```
Of all the use cases, only UC1 – Place an Order has been tested
```

## 4. Comprehensiveness assessment

```
The comprehensiveness of the testing process was evaluated against the criteria
specified in the Test Plan (section 6.3). The plan required achieving at least 80%
coverage through automated test suites, complemented by manual testing for edge
cases and high-risk scenarios.
```

```
In practice, the functional and unit tests implemented covered all the main workflows
related to ordering clothes, including product search, filtering, adding to the cart,
removing items, and completing the order process. Automated tests (using Selenium
for UI and xUnit for APIs) ensured that the most critical flows worked correctly,
meeting the plan’s coverage target for functional testing.
```

```
Test summary report Fecha: 03 / 06 / 2025
```

```
Confidential ©Universidad de Castilla-La
2
```

```
However, due to the fact that I was the only person executing the tests, some edge
cases and feature combinations that would have been covered by collaborative manual
exploratory testing were not exhaustively verified.
```

```
The reason for these gaps is that the initial plan relied on a collaborative effort from a
four-person team (as described in section 12 of the Test Plan), but ultimately, only I
was available to complete the tests for my part of the project. As a result, while the
functional coverage for individual workflows is high, the overall comprehensiveness of
exploratory and non-functional testing is partial.
```

```
Overall, the comprehensiveness criteria for automated functional testing were met.
Some exploratory scenarios were not fully covered due to the reduced testing team
size.
```

## 5. Summary of results

```
The "Place an Order" Use Case has been tested by executing a total of 25 tests,
covering the Main Flow, the Alternative Flows, and their combinations with no failure.
```

## 6. Evaluation

```
The product can be deployed because it meets all the functionalities required by the
user.
```

```
0
```

```
5
```

```
10
```

```
15
```

```
20
```

```
25
```

```
30
```

```
Categoría 1
```

Tests

```
Pass Fail
```

```
Test summary report Fecha: 03 / 06 / 2025
```

```
Confidential ©Universidad de Castilla-La
3
```

## 7. Summary of activities

```
The time spent creating the tests was 13 hours.
The test-execution time was 3.2 minutes
```

## 8. Approvals

```
Name Role Date
Juan Picazo Martínez Test Plan Author / Developer 30 /05/
```

```
Elena Maria Navarro Martínez Product Owner 03 /0 6 /
```
