var token = null;

function sendingRequest(msg, initiator, helper) {
  // If this is the auth or health request, don't modify it
  var url = msg.getRequestHeader().getURI().toString();
  
  print("[HTTPSENDER] Sending request to: " + url);
  
  if (url.toLowerCase().indexOf("/authorisation") !== -1 || 
      url.toLowerCase().indexOf("/health") !== -1) {
    // These endpoints don't need auth token
    print("[HTTPSENDER] Skipping auth for: " + url);
    return;
  }
  
  // For all other requests, add the token if we have one
  if (token) {
    print("[HTTPSENDER] Adding Authorization header");
    msg.getRequestHeader().setHeader("Authorization", "Bearer " + token);
  } else {
    print("[HTTPSENDER] No token available yet");
  }
}

function responseReceived(msg, initiator, helper) {
  // Check if this is a response from the auth endpoint
  var url = msg.getRequestHeader().getURI().toString();
  
  print("[HTTPSENDER] Response received from: " + url);
  
  if (url.toLowerCase().indexOf("/authorisation") !== -1) {
    print("[HTTPSENDER] Auth endpoint response detected");
    // Extract the token from the response
    var responseBody = msg.getResponseBody().toString();
    if (responseBody && responseBody.length > 0) {
      token = responseBody.trim();
      // Remove quotes if present
      token = token.replace(/^"(.*)"$/, '$1');
      print("[HTTPSENDER] Token extracted: " + token.substring(0, 20) + "...");
    } else {
      print("[HTTPSENDER] Empty response body from auth endpoint");
    }
  }
}

function getRequiredParamsNames() {
  return [];
}

function getOptionalParamsNames() {
  return [];
}
