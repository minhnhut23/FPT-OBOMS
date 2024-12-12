POST /api/auth/login

Request body:
{
    "email": "user@example.com",
    "password": "password123"
}
Response (200):
{
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTYiLCJ1c2VybmFtZSI6ImpvaG5kb2UiLCJpYXQiOjE1MTYyMzkwMjJ9.4fg63f89e5E7Vt6byGY7YrT5G0Q"
}
Response (400):
{
    "code": 400,
    "error_code": "invalid_credentials",
    "msg": "Invalid login credentials"
}

