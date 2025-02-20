# FPT-OBOMS

Để chạy hết service:
docker compose-up -build

Khởi chạy lại từng service:
docker compose up --no-deps --build -d "tên service"

# Shop Management API Documentation

This document describes the API endpoints for the **Shop Management System**. These APIs allow users to manage shop details, menu items, reservations, and billing history. The API is RESTful and communicates using JSON.

---

## Base URL
`https://api.oboms.example.com/v1`

---

## Authentication
All endpoints require an API key passed as a header:

```
Authorization: Bearer <your_api_key>
```

---

## Endpoints

### 1. Shop Management
#### **1.1 Get Shop Details**
**GET** `/shops/{shop_id}`

**Description:** Fetch detailed information about a specific shop.

**Request Parameters:**
- `shop_id` (path): ID of the shop to retrieve details for.

**Response:**
```json
{
  "id": "1",
  "name": "Cafe Delight",
  "address": "123 Main St, Springfield",
  "phone": "123-456-7890",
  "rating": 4.5,
  "opening_hours": "8:00 AM - 10:00 PM",
  "payment_methods": ["Cash", "Credit Card", "E-Wallet"]
}
```

#### **1.2 Update Shop Details**
**PUT** `/shops/{shop_id}`

**Description:** Update information about a specific shop.

**Request Parameters:**
- `shop_id` (path): ID of the shop to update.

**Request Body:**
```json
{
  "name": "Cafe Delight",
  "address": "123 Main St, Springfield",
  "phone": "123-456-7890",
  "opening_hours": "7:00 AM - 11:00 PM",
  "payment_methods": ["Cash", "Credit Card"]
}
```

**Response:**
```json
{
  "message": "Shop details updated successfully."
}
```

---

### 2. Menu Management
#### **2.1 Get Menu Items**
**GET** `/shops/{shop_id}/menu`

**Description:** Retrieve all menu items for a shop.

**Response:**
```json
[
  {
    "id": "101",
    "name": "Latte",
    "price": 3.5,
    "category": "Beverages"
  },
  {
    "id": "102",
    "name": "Blueberry Muffin",
    "price": 2.0,
    "category": "Snacks"
  }
]
```

#### **2.2 Add Menu Item**
**POST** `/shops/{shop_id}/menu`

**Description:** Add a new item to the shop's menu.

**Request Body:**
```json
{
  "name": "Cappuccino",
  "price": 4.0,
  "category": "Beverages"
}
```

**Response:**
```json
{
  "message": "Menu item added successfully."
}
```

---

### 3. Reservation Management
#### **3.1 Get Reservations**
**GET** `/shops/{shop_id}/reservations`

**Description:** Retrieve all reservations for a shop.

**Response:**
```json
[
  {
    "id": "301",
    "customer_name": "John Doe",
    "table_number": 5,
    "time": "2025-01-14T18:00:00Z",
    "status": "Confirmed"
  }
]
```

#### **3.2 Create Reservation**
**POST** `/shops/{shop_id}/reservations`

**Description:** Create a new reservation for a shop.

**Request Body:**
```json
{
  "customer_name": "Jane Smith",
  "table_number": 3,
  "time": "2025-01-15T19:00:00Z"
}
```

**Response:**
```json
{
  "message": "Reservation created successfully."
}
```

---

### 4. Billing Management
#### **4.1 Get Billing History**
**GET** `/shops/{shop_id}/billing-history`

**Description:** Retrieve the billing history for a shop.

**Response:**
```json
[
  {
    "id": "501",
    "table_number": 4,
    "total": 25.0,
    "payment_method": "Credit Card",
    "timestamp": "2025-01-13T21:15:00Z"
  }
]
```

#### **4.2 Add Bill**
**POST** `/shops/{shop_id}/billing`

**Description:** Add a new bill to the shop's billing history.

**Request Body:**
```json
{
  "table_number": 4,
  "items": [
    { "name": "Latte", "price": 3.5, "quantity": 2 },
    { "name": "Muffin", "price": 2.0, "quantity": 1 }
  ],
  "payment_method": "Cash",
  "total": 9.0
}
```

**Response:**
```json
{
  "message": "Bill added successfully."
}
```

---

## Error Responses
All errors return a JSON object with an error message:
```json
{
  "error": "Resource not found."
}
```

---

## Notes
- Ensure API keys are kept secure.
- Use HTTPS for secure communication.
- Contact support at `support@oboms.example.com` for issues or feature requests.
