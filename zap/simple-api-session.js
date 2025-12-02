var token = null;

function extractWebSession(sessionWrapper) {
  // Extract token from the authentication response
  var msg = sessionWrapper.getHttpMessage();
  var responseBody = msg.getResponseBody().toString();
  
  if (responseBody && responseBody.length > 0) {
    token = responseBody.trim();
    // Remove quotes if present
    token = token.replace(/^"(.*)"$/, '$1');
    sessionWrapper.getSession().setValue("token", token);
  }
}

function processMessageToMatchSession(sessionWrapper) {
  // Add the stored token to outgoing requests
  var msg = sessionWrapper.getHttpMessage();
  var session = sessionWrapper.getSession();
  var storedToken = session.getValue("token");
  
  if (storedToken) {
    msg.getRequestHeader().setHeader("Authorization", "Bearer " + storedToken);
  }
  
  return msg;
}

function getRequiredParamsNames() {
  return [];
}

function getOptionalParamsNames() {
  return [];
}
