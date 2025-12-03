var token = null;

function extractWebSession(sessionWrapper) {
  print("[SESSION] Extracting web session...");
  // Extract token from the authentication response
  var msg = sessionWrapper.getHttpMessage();
  var responseBody = msg.getResponseBody().toString();
  
  print("[SESSION] Response body length: " + responseBody.length);
  
  if (responseBody && responseBody.length > 0) {
    token = responseBody.trim();
    // Remove quotes if present
    token = token.replace(/^"(.*)"$/, '$1');
    sessionWrapper.getSession().setValue("token", token);
    print("[SESSION] Token extracted and stored: " + token.substring(0, 20) + "...");
  } else {
    print("[SESSION] No token found in response");
  }
}

function processMessageToMatchSession(sessionWrapper) {
  print("[SESSION] Processing message to match session...");
  // Add the stored token to outgoing requests
  var msg = sessionWrapper.getHttpMessage();
  var session = sessionWrapper.getSession();
  var storedToken = session.getValue("token");
  
  if (storedToken) {
    print("[SESSION] Adding Authorization header with token");
    msg.getRequestHeader().setHeader("Authorization", "Bearer " + storedToken);
  } else {
    print("[SESSION] No token available to add");
  }
  
  return msg;
}

function getRequiredParamsNames() {
  return [];
}

function getOptionalParamsNames() {
  return [];
}
