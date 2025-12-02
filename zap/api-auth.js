function authenticate(helper, paramsValues, credentials) {
  print("[AUTH] Starting authentication...");
  
  var HttpRequestHeader = Java.type('org.parosproxy.paros.network.HttpRequestHeader');
  var HttpHeader = Java.type('org.parosproxy.paros.network.HttpHeader');
  var URI = Java.type('org.apache.commons.httpclient.URI');
  
  var authEndpoint = paramsValues.get("auth_endpoint");
  var clientId = paramsValues.get("client_id");
  var clientSecret = paramsValues.get("client_secret");
  
  print("[AUTH] Endpoint: " + authEndpoint);
  print("[AUTH] Client ID: " + clientId);
  
  // Build URL with query parameters
  var authUrl = authEndpoint + "?id=" + clientId + "&secret=" + clientSecret;
  print("[AUTH] Full URL: " + authUrl);
  
  // Create POST request
  var msg = helper.prepareMessage();
  var requestHeader = new HttpRequestHeader(
    HttpRequestHeader.POST,
    new URI(authUrl, true),
    HttpHeader.HTTP11
  );
  
  msg.setRequestHeader(requestHeader);
  
  // Send the request
  print("[AUTH] Sending authentication request...");
  helper.sendAndReceive(msg);
  
  print("[AUTH] Response status: " + msg.getResponseHeader().getStatusCode());
  print("[AUTH] Response body: " + msg.getResponseBody().toString());
  
  return msg;
}

function getRequiredParamsNames() {
  return ["auth_endpoint", "client_id", "client_secret"];
}

function getOptionalParamsNames() {
  return [];
}

function getCredentialsParamsNames() {
  return [];
}
