# FPT-OBOMS

# Authentication and User Management API Documentation

This document describes the API endpoints for **Authentication and User Management**. These APIs handle security, account management, and user management. The API follows RESTful principles and communicates using JSON.

---

## Base URL

`http://127.0.0.1:5001/api/`

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

**Response:**

**200 Success**

**401 Unauthorized**

### 2. User management

#### **2.1 Get current user**

**GET** `/user/currentUser`

**Description:**

**Authentication**

Endpoints require an API key passed as a header:

```
Authorization: Bearer <your_api_key>
```

**Response:**

**200 Success:**

```json
{
  "email": "minhnhut9a8@gmail.com",
  "fullName": "ô văn kê",
  "profilePicture": "https://cnbwnwbtafbarsgmcabf.supabase.co/storage/v1/object/public/avatars/92c8cd5b-0498-4722-8c35-adbbebf4b585_3342-gooseknife.png",
  "bio": null,
  "dateOfBirth": "2024-12-22T17:00:00"
}
```

**401 Unauthorized**

---

#### **2.2 Get profile user**

**GET** `/user/{id}`

**Description:**

**Authentication**

Endpoints require an API key passed as a header:

```
Authorization: Bearer <your_api_key>
```

**Response:**

**200 Success:**

```json
{
  "email": "minhnhut9a8@gmail.com",
  "fullName": "ô văn kê",
  "profilePicture": "https://cnbwnwbtafbarsgmcabf.supabase.co/storage/v1/object/public/avatars/92c8cd5b-0498-4722-8c35-adbbebf4b585_3342-gooseknife.png",
  "bio": null,
  "dateOfBirth": "2024-12-22T17:00:00"
}
```

**400 Bad Request:**

```json
{
  "msg": "Profile not found!"
}
```

**401 Unauthorized**

---

#### **2.3 Update profile user**

**PUT** `/user/updateProfile/`

**Description:**

**Authentication**

Endpoints require an API key passed as a header:

```
Authorization: Bearer <your_api_key>
```

**Request Body Form:**

| Field            | Type              | Required |
| ---------------- | ----------------- | -------- |
| `Email`          | string            | No       |
| `FullName`       | string            | No       |
| `ProfilePicture` | file (image)      | No       |
| `Bio`            | string            | No       |
| `DateOfBirth`    | date (YYYY-MM-DD) | No       |

**Response:**

**200 Success:**

```json
{
  "email": "minhnhut9a8@gmail.com",
  "fullName": "ô văn kê",
  "profilePicture": "https://cnbwnwbtafbarsgmcabf.supabase.co/storage/v1/object/public/avatars/92c8cd5b-0498-4722-8c35-adbbebf4b585_3342-gooseknife.png",
  "bio": null,
  "dateOfBirth": "2024-12-22T17:00:00"
}
```

**400 Bad Request:**

```json
{
  "msg": "Profile not found!"
}
```

```json
{
  "msg": "Invalid file type! Only JPG, PNG, GIF, and WEBP are allowed."
}
```

```json
{
  "msg": "File size exceeds the 50MB limit."
}
```

```json
{
  "msg": "Invalid Birthday!"
}
```

**401 Unauthorized**

---

#### **2.4 Get all user**

**GET** `/user`

**Description:**

**Authentication**

Endpoints require an API key passed as a header:

```
Authorization: Bearer <your_api_key>
```

**Request Body Form:**

| Field         | Type              | Required | Example Value |
| ------------- | ----------------- | -------- | ------------- |
| `FullName`    | string            | No       | test          |
| `Role`        | string            | No       | Customer      |
| `DateOfBirth` | date (YYYY-MM-DD) | No       | 2004-04-29    |
| `PageNumber`  | string            | No       | 1             |
| `PageSize`    | string            | No       | 5             |

**Response:**

**200 Success:**

```json
{
  "item1": [
    {
      "id": "bd99768c-d208-4b80-8cde-7e084872c6f1",
      "fullName": "abc",
      "profilePicture": null,
      "bio": null,
      "dateOfBirth": "2024-12-23T00:00:00",
      "createdAt": "2025-02-25T19:26:51",
      "updatedAt": "2025-02-25T19:26:51",
      "role": "Owner",
      "accountId": "d2bb290e-e4a7-4c2c-94df-983845b3fea4"
    },
    {
      "id": "c00f50f4-b504-407c-9585-1b5eb0676c51",
      "fullName": "abc2",
      "profilePicture": null,
      "bio": null,
      "dateOfBirth": "2024-12-23T00:00:00",
      "createdAt": "2025-03-01T09:49:45",
      "updatedAt": "2025-03-01T09:49:45",
      "role": "Customer",
      "accountId": "c29b37d2-5be2-45b0-b39e-b0a6e50ec758"
    },
    {
      "id": "79685a60-6c58-492f-b61b-2a81343851c6",
      "fullName": "ô văn kê",
      "profilePicture": "https://cnbwnwbtafbarsgmcabf.supabase.co/storage/v1/object/public/avatars/861b1f49-a3c4-4387-a0b5-2d3cb3de12d0_b46.gif",
      "bio": null,
      "dateOfBirth": "2024-12-22T17:00:00",
      "createdAt": "2024-12-29T07:54:32",
      "updatedAt": "2025-03-14T19:24:29",
      "role": "Owner",
      "accountId": "8e88715f-a783-4f6e-b350-f9829b9c04fa"
    },
    {
      "id": "bd4241cc-a61a-4c47-baf0-c2b5108e6736",
      "fullName": "test",
      "profilePicture": null,
      "bio": null,
      "dateOfBirth": "2004-04-29T00:00:00",
      "createdAt": "2025-03-16T15:22:31",
      "updatedAt": "2025-03-16T15:22:31",
      "role": "Customer",
      "accountId": "00d115e4-7de2-43ba-9752-025131c627ae"
    },
    {
      "id": "09cf9840-5390-4004-9079-a2b651ca1d2e",
      "fullName": "ô văn kê",
      "profilePicture": "https://cnbwnwbtafbarsgmcabf.supabase.co/storage/v1/object/public/avatars/2fb3486c-1a8e-4ea3-ae1b-a7981b99bd48_b46.gif",
      "bio": null,
      "dateOfBirth": "2024-12-22T17:00:00",
      "createdAt": "2024-12-03T11:15:14",
      "updatedAt": "2025-03-16T16:09:44",
      "role": "Customer",
      "accountId": "61276ff4-527a-42ee-9887-cc3107977ca5"
    }
  ],
  "item2": {
    "totalResults": 5,
    "totalPages": 1,
    "currentPage": 1,
    "pageSize": 5
  }
}
```

**400 Bad Request:**

```json
{
  "msg": "User not found!"
}
```

**401 Unauthorized**

---
