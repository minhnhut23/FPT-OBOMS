# FPT-OBOMS

# Authentication and User Management API Documentation

This document describes the API endpoints for **Authentication and User Management**. These APIs handle security, account management, and user management. The API follows RESTful principles and communicates using JSON.

---

## Base URL

`http://127.0.0.1:5002/api/`

---

## Endpoints

### 1. Authentication

#### **1.1 Login**

**POST** `/auth/login/`

**Description:** Allows users to log in by verifying their credentials. Returns an access token and refresh token upon successful authentication.

**Request Parameters:**

```json
{
  "email": "minhnhut9a8@gmail.com",
  "password": "Pass1234@"
}
```

**Response:**

**200 Success**:

```json
{
  "accessToken": "eyJhbG...",
  "refreshToken": "VGnkRl...",
  "expiresAt": "2025-03-14T21:25:25Z"
}
```

**400 Bad Request**:

```json
{
  "msg": "Invalid login credentials"
}
```

---

#### **1.2 Register**

**POST** `/auth/register`

**Description:** Creates a new user account by accepting email, password, full name, date of birth, and role. The system validates input data before saving the user information.

**Request Body:**

```json
{
  "email": "clone.vyvy.2@gmail.com",
  "password": "Pass123@",
  "ConfirmPassword": "Pass123@",
  "fullName": "abc2",
  "dateOfBirth": "2024-12-23",
  "role": "Customer"
}
```

**Response:**

**200 Success:**

```json
{
  "email": "clone.vyvy.2@gmail.com",
  "fullName": "abc2",
  "profilePicture": null,
  "bio": null,
  "dateOfBirth": "2024-12-23"
}
```

**400 Bad Request:**

```json
{
  "msg": "Password fields cannot be null or empty."
}
```

```json
{
  "msg": "Password is not valid: password and confirm password are not the same."
}
```

```json
{
  "msg": "Password is not valid: password must contain at least one lowercase, uppercase letter, digit and special character."
}
```

```json
{
  "msg": "Invalid Birthday!"
}
```

```json
{
  "msg": "Invalid email format!"
}
```

---

#### **1.3 Forgot Password**

**POST** `/auth/forgotPassword`

**Description:** Initiates the password reset process by sending a verification OTP to the registered email.

**Request Body:**

```json
{
  "email": "minhnhut9a8@gmail.com"
}
```

**Response:**

**200 Success**

**400 Bad Request:**

```json
{
  "msg": "Email does not exist!"
}
```

```json
{
  "msg": "Invalid email format!"
}
```

---

#### **1.4 Recover Password**

**POST** `/auth/recoverPassword`

**Description:** Allows users to reset their password by providing a valid OTP along with a new password.

**Request Body:**

```json
{
  "otp": "788979",
  "email": "minhnhut9a8@gmail.com",
  "NewPassword": "Pass1234@",
  "ConfirmPassword": "Pass1234@"
}
```

**Response:**

**200 Success**

**400 Bad Request:**

```json
{
  "msg": "Password is not valid: password and confirm password are not the same."
}
```

```json
{
  "msg": "Password is not valid: password must contain at least one lowercase, uppercase letter, digit and special character."
}
```

```json
{
  "msg": "Password fields cannot be null or empty."
}
```

---

#### **1.5 Change Password**

**POST** `/auth/changePassword`

**Description:** Enables authenticated users to update their password by providing the current password and a new password.

**Authentication**

Endpoints require an API key passed as a header:

```
Authorization: Bearer <your_api_key>
```

**Request Body:**

```json
{
  "OldPassword": "Pass1234@",
  "NewPassword": "Pass123@",
  "ConfirmPassword": "Pass123@"
}
```

**Response:**

**200 Success:**

```json
{
  "msg": "Password has been change successfully."
}
```

**401 Unauthorized**

**400 Bad Request:**

```json
{
  "msg": "Password is not valid: password and confirm password are not the same."
}
```

```json
{
  "msg": "Invalid current password or email."
}
```

```json
{
  "msg": "Password is not valid: password must contain at least one lowercase, uppercase letter, digit and special character."
}
```

```json
{
  "msg": "Password fields cannot be null or empty."
}
```

---

#### **1.6 Logout**

**POST** `/auth/logout`

**Description:** Logs out the authenticated user by invalidating the access token.

**Authentication**

Endpoints require an API key passed as a header:

```
Authorization: Bearer <your_api_key>
```

**Request**

**Response:**

**200 Success**

**401 Unauthorized**
