function authenticate(helper, paramsValues, credentials) {
  var HttpRequestHeader = Java.type('org.parosproxy.paros.network.HttpRequestHeader');
  var HttpHeader = Java.type('org.parosproxy.paros.network.HttpHeader');
  var URI = Java.type('org.apache.commons.httpclient.URI');
  
  var authEndpoint = paramsValues.get("auth_endpoint");
  var clientId = paramsValues.get("client_id");
  var clientSecret = paramsValues.get("client_secret");
  
  // Build URL with query parameters
  var authUrl = authEndpoint + "?id=" + clientId + "&secret=" + clientSecret;
  
  // Create POST request
  var msg = helper.prepareMessage();
  var requestHeader = new HttpRequestHeader(
    HttpRequestHeader.POST,
    new URI(authUrl, true),
    HttpHeader.HTTP11
  );
  
  msg.setRequestHeader(requestHeader);
  
  // Send the request
  helper.sendAndReceive(msg);
  
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
