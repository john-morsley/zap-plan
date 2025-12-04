var LogManager = Java.type('org.apache.logging.log4j.LogManager');
var logger = LogManager.getLogger('auth.js');

function authenticate(helper, paramsValues, credentials) {

  logger.info("[AUTH] \\/ --------------------- AUTHENTICATING --------------------- \\/");

  logger.info("[AUTH] Starting authentication...");
  
  var HttpRequestHeader = Java.type('org.parosproxy.paros.network.HttpRequestHeader');
  var HttpHeader = Java.type('org.parosproxy.paros.network.HttpHeader');
  var URI = Java.type('org.apache.commons.httpclient.URI');
  
  var authEndpoint = paramsValues.get("auth_endpoint");
  var clientId = paramsValues.get("client_id");
  var clientSecret = paramsValues.get("client_secret");
  
  logger.info("[AUTH] Endpoint: " + authEndpoint);
  logger.info("[AUTH] Client ID: " + clientId);
  
  // Build URL with query parameters
  var authUrl = authEndpoint + "?id=" + clientId + "&secret=" + clientSecret;
  logger.info("[AUTH] Full URL: " + authUrl);
  
  // Create POST request
  var msg = helper.prepareMessage();
  var requestHeader = new HttpRequestHeader(
    HttpRequestHeader.POST,
    new URI(authUrl, true),
    HttpHeader.HTTP11
  );
  
  // Set required headers for POST request
  requestHeader.setHeader("Content-Type", "application/x-www-form-urlencoded");
  requestHeader.setHeader("Content-Length", "0");
  
  msg.setRequestHeader(requestHeader);
  
  // Send the request
  logger.info("[AUTH] Sending authentication request...");
  helper.sendAndReceive(msg);
  
  logger.info("[AUTH] Response status: " + msg.getResponseHeader().getStatusCode());
  logger.info("[AUTH] Response body: " + msg.getResponseBody().toString());
  
  logger.info("[AUTH] /\\ -------------------- AUTHENTICATING -------------------- /\\");

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
