# Simple Auth API

A minimal ASP.NET Core Web API with JWT authentication for testing ZAP security scans.

## Endpoints

### 1. POST /authorisation

Gets an authorization token.

**Parameters** (query string):
- `id` - Client identifier
- `secret` - Client secret

**Example Request:**
```bash
curl -X POST "http://localhost:5000/authorisation?id=testclient&secret=testsecret"
```

**Example Response:**
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### 2. GET /test

Protected endpoint that requires a valid bearer token.

**Headers:**
- `Authorization: Bearer <token>`

**Example Request:**
```bash
curl -X GET "http://localhost:5000/test" -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

**Example Response:**
```
Hello
```

## Running the API

```powershell
cd SimpleAuthApi
dotnet run
```

The API will be available at: <http://localhost:5000>

## Testing with PowerShell

```powershell
# Get token
$response = Invoke-RestMethod -Method Post -Uri "http://localhost:5000/authorisation?id=testclient&secret=testsecret"
$token = $response

# Call protected endpoint
Invoke-RestMethod -Method Get -Uri "http://localhost:5000/test" -Headers @{Authorization="Bearer $token"}
```

## Testing with ZAP

This API is designed to be scanned with OWASP ZAP. The authentication flow:

1. ZAP calls `/authorisation` with `id` and `secret` query parameters
2. API returns a JWT token
3. ZAP uses the token in the `Authorization: Bearer <token>` header for subsequent requests
4. ZAP can access the protected `/test` endpoint

## Security Note

This is a **test API only**. It uses:
- Hardcoded JWT secret key
- No real credential validation
- Simple token generation

**Do not use in production!**
