var token = null;

function sendingRequest(msg, initiator, helper) {
  // If this is the auth or health request, don't modify it
  var url = msg.getRequestHeader().getURI().toString();
  
  if (url.indexOf("/authorisation") !== -1 || 
      url.indexOf("/health") !== -1) {
    // These endpoints don't need auth token
    return;
  }
  
  // For all other requests, add the token if we have one
  if (token) {
    msg.getRequestHeader().setHeader("Authorization", "Bearer " + token);
  }
}

function responseReceived(msg, initiator, helper) {
  // Check if this is a response from the auth endpoint
  var url = msg.getRequestHeader().getURI().toString();
  
  if (url.indexOf("/authorisation") !== -1) {
    // Extract the token from the response
    var responseBody = msg.getResponseBody().toString();
    if (responseBody && responseBody.length > 0) {
      token = responseBody.trim();
      // Remove quotes if present
      token = token.replace(/^"(.*)"$/, '$1');
      print("Token extracted: " + token.substring(0, 20) + "...");
    }
  }
}

function getRequiredParamsNames() {
  return [];
}

function getOptionalParamsNames() {
  return [];
}
