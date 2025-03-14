# FPT-OBOMS

# Authentication and User Management API Documentation

This document describes the API endpoints for **Authentication and User Management**. These APIs handle security, account management, and user management. The API follows RESTful principles and communicates using JSON.

---

## Base URL

`http://127.0.0.1:5003/api/`

---

## Authentication

All endpoints require an API key passed as a header:

```
Authorization: Bearer <your_api_key>
```

---

## Endpoints

### 1. Authentication

#### **1.1 Login**

**POST** `/auth/login/`

**Description:** Authenticate a user by verifying credentials and returning access tokens.

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
  "refreshToken": "VGnkRl7WZKxGIsu47r__Dg",
  "expiresAt": "2025-03-14T21:25:25Z"
}
```

**400 Bad Request**:

```json
{
  "msg": "Invalid login credentials"
}
```

#### **1.2 Register**

**POST** `/auth/register`

**Description:** Register a new user by providing account details, including email, password, full name, date of birth, and role. The system validates the input and returns the created user details upon success.

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
**200 Success**:

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

#### **1.3 Forgot Password**

**POST** `/auth/forgotPassword`

**Description:** Update information about a specific shop.

**Request Body:**

```json
{
  "email": "minhnhut9a8@gmail.com"
}
```

**Response:**
**200 Success**:

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
