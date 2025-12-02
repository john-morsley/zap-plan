ZAP by Automation Plan
======================

This document outlines the automation plan for ZAP (Zed Attack Proxy) to perform security testing on a simple API.

By using automation it's easy to run a scan of the API without wasting time and effort following a complex manual process.

Plan Explained
--------------

This particular API is secured with an OAUTH2 token, which is obtained by hitting the 'authorisation' endpoint with a POST and 2 query string parameters, id and secret. The id should be 'ABC' and the secret is '123'.  

Every other endpoint requires the token, except the 'authorization' and 'health' endpoints.

All the other endpoints require authorization and they get that via the 'authentication' script.

Steps
-----

1. Run ZAP.
2. In the 'Information' tabs section (History, Search, Alerts, Output) there is a + symbol, click it and select 'Automation', click it again and select 'Active Scan'.
3. If the API you are testing is running on localhost, like here, make sure your application is running.
4. Go back to the 'Automation' tab and click the 'Load Plan...' button (folder icon) and select the 'zap-automation-plan-api.yaml' file. This should load the automation plan.
5. Click the 'Run Plan' button (play icon) to start the scan.
6. No click the 2nd tab you created, the 'Active Scan' tab, and watch your API being scanned.
7. When the scan is finished, a report (in Markdown format) should be in the 'reports' folder.
