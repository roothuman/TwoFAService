# TwoFAService
API Description:

The project is a Two-Factor Authentication (2FA) service implemented in ASP.NET Core. It provides two API endpoints:

1. Send Confirmation Code:

Endpoint: POST /api/TwoFactorAuth/send-code  (example: https://localhost:7145/api/TwoFactorAuth/send-code)
Description: Sends a confirmation code to a specified phone number.
Request Body:
{
    "PhoneNumber": "string"
}

Response:
Success (200 OK):
{
    "Message": "Code sent successfully."
}

Error (400 Bad Request):
{
    "Message": "Too many active codes for this phone."
}


2. Check Confirmation Code:

Endpoint: POST /api/TwoFactorAuth/check-code (example: https://localhost:7145/api/TwoFactorAuth/check-code)
Description: Checks if a received confirmation code is valid for the specified phone number.
Request Body:
{
    "PhoneNumber": "string",
    "Code": "string"
}
Response:
Success (200 OK):
{
    "Message": "Code is valid."
}
Error (400 Bad Request):
{
    "Message": "Invalid code."
}


Launch Instructions:

Requirements:
.NET Core SDK

1. Clone the Repository
2. Run the Application
3. Test Application using Swagger or Postman
